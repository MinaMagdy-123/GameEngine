using GameEngine.Data;
using GameEngine.Models;
using GameEngine.Settings;
using GameEngine.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GameEngine.Services
{
    public class GamesService : IGamesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagePath;

        public GamesService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagePath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
        }

        public IEnumerable<Game> GetAll()
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(d => d.Device)
                .OrderByDescending(g => g.Id)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<Game> GetByName(string gameName)
        {
            return _context.Games
                .Where(g => EF.Functions.Like(g.Name, gameName + "%"))
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(d => d.Device)
                .OrderBy(g => g.Name)
                .AsNoTracking()
                .ToList();
        }

        public Game? GetbyId(int id)
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(d => d.Device)
                .OrderBy(g => g.Name)
                .AsNoTracking()
                .SingleOrDefault(g => g.Id == id);
        }

        public IEnumerable<Game> GetByCategoryId(int id)
        {
            return _context.Games.Where(g => g.CategoryId == id)
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(d => d.Device)
                .OrderBy(g => g.Name)
                .AsNoTracking()
                .ToList();
        }

        public async Task Create(CreateGameFormViewModel model)
        {
            var coverName = await SaveCover(model.Cover);
            //stream.Dispose();
            Game game = new()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                Cover = coverName,
                Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
            };

            _context.Games.Add(game);
            _context.SaveChanges();
        }

        public async Task Update(EditGameFormViewModel model)
        {
            var game = _context.Games
                .Include(g => g.Devices)
                .SingleOrDefault(g => g.Id == model.Id);

            if (game is not null)
            {
                game.Name = model.Name;
                game.Description = model.Description;
                game.Price = model.Price;
                game.CategoryId = model.CategoryId;
                game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();

                var hasNewCover = model.Cover is not null;

                if (hasNewCover)
                {
                    File.Delete(Path.Combine(_imagePath, game.Cover));
                    game.Cover = await SaveCover(model.Cover!);
                }
                _context.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            var game = _context.Games.Find(id);

            if (game is not null)
            {
                File.Delete(Path.Combine(_imagePath, game.Cover));
                _context.Remove(game);
                _context.SaveChanges();
            }
        }

        private async Task<string> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
            var path = Path.Combine(_imagePath, coverName);

            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);

            return coverName;
        }


    }
}
