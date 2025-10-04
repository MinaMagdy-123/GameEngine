using GameEngine.Services;
using GameEngine.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameEngine.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var categories = _categoriesService.GetAll();
            return View(categories);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Search(string categoryName)
        {
            var categories = _categoriesService.GetByName(categoryName);
            return View("Index", categories);
        }


        [HttpGet]
        public IActionResult Create()
        {
            CreateCategoryFormViewModel viewModel = new()
            {
                Name = string.Empty
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _categoriesService.Create(model);
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Edit()
        {
            EditCategoryFormViewModel viewModel = new()
            {
                Categories = _categoriesService.GetSelectList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditCategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _categoriesService.Update(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
