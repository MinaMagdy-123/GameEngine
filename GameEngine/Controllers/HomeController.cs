using GameEngine.Models;
using GameEngine.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GameEngine.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGamesService _gamesService;

        public HomeController(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }

        [HttpGet]
        public IActionResult IndexByCategory(int id)
        {
            var games = _gamesService.GetByCategoryId(id);
            return View("Index", games);
        }

        [HttpGet]
        public IActionResult Search(string gameName)
        {
            var games = _gamesService.GetByName(gameName);
            return View("Index", games);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
