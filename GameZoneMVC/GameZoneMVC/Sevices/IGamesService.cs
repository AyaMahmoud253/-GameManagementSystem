using GameZoneMVC.Models;
using GameZoneMVC.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZoneMVC.Sevices
{
    public interface IGamesService
    {
        Task<IEnumerable<Game>> GetAll();
        Task Create(CreateGameFormViewModel model);
        Task Update(UpdateGameFormViewModel model);
        IEnumerable<SelectListItem> GetSelectListCat();
        IEnumerable<SelectListItem> GetSelectListDev();
    }
}
