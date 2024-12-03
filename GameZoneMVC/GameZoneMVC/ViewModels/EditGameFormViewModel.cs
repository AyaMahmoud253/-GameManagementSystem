using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZoneMVC.ViewModels
{
    public class UpdateGameFormViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public List<int> SelectedDevices { get; set; } = new();
        public IFormFile? Cover { get; set; }
    }


}
