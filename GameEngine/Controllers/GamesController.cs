using GameEngine.Services;
using GameEngine.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameEngine.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GamesController : Controller
    {

        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesService _devicesService;
        private readonly IGamesService _gamesService;

        public GamesController(ICategoriesService categoriesService,
            IDevicesService devicesService,
            IGamesService gamesService)
        {
            _categoriesService = categoriesService;
            _devicesService = devicesService;
            _gamesService = gamesService;
        }

        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }


        [HttpGet]
        public IActionResult Search(string gameName)
        {
            var games = _gamesService.GetByName(gameName);
            return View("Index", games);
        }


        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var game = _gamesService.GetbyId(id);
            if (game is null)
                return NotFound();

            var viewModel = new GameDetailsViewModel
            {
                Game = game,
                Devices = _devicesService.GetSelectListByGameId(id)
            };

            return View(viewModel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel viewModel = new()
            {
                Categories = _categoriesService.GetSelectList(),

                Devices = _devicesService.GetSelectList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();

                model.Devices = _devicesService.GetSelectList();

                return View(model);
            }

            //save game to database
            //save cover to server
            await _gamesService.Create(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var game = _gamesService.GetbyId(id);
            if (game is null)
                return NotFound();

            EditGameFormViewModel viewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                Price = game.Price,
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectList(),
                CurrentCover = game.Cover
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();

                model.Devices = _devicesService.GetSelectList();

                return View(model);
            }

            await _gamesService.Update(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _gamesService.Remove(id);

            return Ok();
        }

    }
}
