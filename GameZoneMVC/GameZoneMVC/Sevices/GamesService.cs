using GameZoneMVC.Data;
using GameZoneMVC.Models;
using GameZoneMVC.Sevices;
using GameZoneMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace GameZoneMVC.Services
{
    public class GamesService : IGamesService
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _coverImagesPath;

        public GamesService(ApplicationDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            // Use WebRootPath for dynamic path handling
            _coverImagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "games");
        }

        // Save the cover image file
        public async Task<string> SaveCover(IFormFile cover)
        {
            // Ensure the directory exists
            if (!Directory.Exists(_coverImagesPath))
            {
                Directory.CreateDirectory(_coverImagesPath);
            }

            // Generate a unique file name for the cover image
            var coverName = Guid.NewGuid().ToString() + Path.GetExtension(cover.FileName);
            var coverPath = Path.Combine(_coverImagesPath, coverName);

            // Save the file
            using (var stream = new FileStream(coverPath, FileMode.Create))
            {
                await cover.CopyToAsync(stream);
            }

            // Return the relative URL of the saved image (for storing in the database)
            return $"/assets/images/games/{coverName}";
        }

        // Create a new game record
        public async Task Create(CreateGameFormViewModel model)
        {
            var coverName = await SaveCover(model.Cover);

            Game game = new()
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Cover = coverName,
                Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
            };

            _context.Add(game);
            await _context.SaveChangesAsync(); // Use async method to save changes
        }

 
        // Get categories as select list
        public IEnumerable<SelectListItem> GetSelectListCat()
        {
            return _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .OrderBy(c => c.Text)
                    .AsNoTracking()
                    .ToList();
        }

        // Get devices as select list
        public IEnumerable<SelectListItem> GetSelectListDev()
        {
            return _context.Devices
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                    .OrderBy(d => d.Text)
                    .AsNoTracking()
                    .ToList();
        }

        // Update an existing game record
        public async Task Update(UpdateGameFormViewModel model)
        {
            // Fetch the game from the database
            var game = await _context.Games
                .Include(g => g.Devices) // Include related devices for proper update
                .FirstOrDefaultAsync(g => g.Id == model.Id);

            if (game == null)
            {
                throw new KeyNotFoundException("Game not found.");
            }

            // Update properties
            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;

            // Handle cover update if a new file is uploaded
            if (model.Cover != null)
            {
                // Delete the old cover file, if exists
                if (!string.IsNullOrEmpty(game.Cover))
                {
                    var oldCoverPath = Path.Combine(_webHostEnvironment.WebRootPath, game.Cover.TrimStart('/'));
                    if (File.Exists(oldCoverPath))
                    {
                        File.Delete(oldCoverPath);
                    }
                }

                // Save the new cover file
                game.Cover = await SaveCover(model.Cover);
            }

            // Update devices
            game.Devices.Clear();
            game.Devices = model.SelectedDevices
                .Select(d => new GameDevice { DeviceId = d, GameId = game.Id })
                .ToList();

            // Save changes to the database
            _context.Update(game);
            await _context.SaveChangesAsync();
        }


        // Get all games with their related category and devices
        public async Task<IEnumerable<Game>> GetAll()
        {
            if (_context == null)
            {
                throw new InvalidOperationException("Context is not initialized.");
            }

            return await _context.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .ToListAsync(); // Use async ToList
        }
    }
}
