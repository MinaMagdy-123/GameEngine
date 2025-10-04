using GameEngine.Data;
using GameEngine.Models;
using GameEngine.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameEngine.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext _context;

        public CategoriesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .OrderBy(c => c.Text)
                .ToList();
        }


        public IEnumerable<Category> GetAll()
        {
            return _context.Categories
                .OrderByDescending(c => c.Id)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<Category> GetByName(string categoryName)
        {
            return _context.Categories.Where(g => EF.Functions.Like(g.Name, categoryName + "%")).ToList();
        }

        public void Create(CreateCategoryFormViewModel model)
        {
            Category category = new()
            {
                Name = model.Name
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(EditCategoryFormViewModel model)
        {
            var category = _context.Categories.SingleOrDefault(c => c.Id == model.CategoryId);

            if (category is not null)
            {
                category.Name = model.Name;
                _context.Categories.Update(category);
                _context.SaveChanges();
            }
        }
    }
}
