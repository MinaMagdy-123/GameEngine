using GameEngine.Models;
using GameEngine.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameEngine.Services
{
    public interface ICategoriesService
    {
        IEnumerable<SelectListItem> GetSelectList();
        IEnumerable<Category> GetAll();
        IEnumerable<Category> GetByName(string categoryName);
        void Create(CreateCategoryFormViewModel model);
        void Update(EditCategoryFormViewModel model);
    }
}
