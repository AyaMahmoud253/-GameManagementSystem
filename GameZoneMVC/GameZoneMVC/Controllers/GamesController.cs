using GameZoneMVC.Data;
using GameZoneMVC.Sevices;
using GameZoneMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class GamesController : Controller
{
    private readonly ApplicationDBContext _context;
    private readonly IGamesService _gamesService;

    public GamesController(ApplicationDBContext context, IGamesService gamesService)
    {
        _context = context;
        _gamesService = gamesService;
    }

    // Display the form to create a new game (GET)
    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new CreateGameFormViewModel
        {
            Categories = _gamesService.GetSelectListCat(),
            Devices = _gamesService.GetSelectListDev()
        };

        return View(viewModel);
    }

    // Handle the form submission to create a new game (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateGameFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = _gamesService.GetSelectListCat();
            model.Devices = _gamesService.GetSelectListDev();
            return View(model);
        }

        await _gamesService.Create(model);

        return RedirectToAction(nameof(Index));
    }

    // Index: Display all games
    public async Task<IActionResult> Index()
    {
        var games = await _gamesService.GetAll(); // Await the Task
        return View(games);
    }

    // Display the form to edit an existing game (GET)
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var game = await _gamesService.GetAll()
                                      .ContinueWith(t => t.Result.FirstOrDefault(g => g.Id == id));

        if (game == null)
        {
            return NotFound();
        }

        var viewModel = new UpdateGameFormViewModel
        {
            Id = game.Id,
            Name = game.Name,
            Description = game.Description,
            CategoryId = game.CategoryId,
            SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList()
        };

        ViewBag.Categories = _gamesService.GetSelectListCat();
        ViewBag.Devices = _gamesService.GetSelectListDev();

        return View(viewModel);
    }


   

    // POST: Games/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateGameFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _gamesService.GetSelectListCat();
            ViewBag.Devices = _gamesService.GetSelectListDev();
            return View(model);
        }

        try
        {
            await _gamesService.Update(model);
            return RedirectToAction(nameof(Index)); // Redirect to the Index action
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            ViewBag.Categories = _gamesService.GetSelectListCat();
            ViewBag.Devices = _gamesService.GetSelectListDev();
            return View(model);
        }
    }
    // Handle the form submission to delete an existing game (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var game = await _context.Games.FindAsync(id);

        if (game == null)
        {
            return NotFound(); // Return 404 if the game is not found
        }

        // Delete the game from the database
        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index)); // Redirect to the game index page after deletion
    }
}
