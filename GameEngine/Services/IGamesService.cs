using GameEngine.Models;
using GameEngine.ViewModels;

namespace GameEngine.Services
{
    public interface IGamesService
    {
        IEnumerable<Game> GetAll();
        IEnumerable<Game> GetByName(string gameName);
        IEnumerable<Game> GetByCategoryId(int id);
        Task Create(CreateGameFormViewModel model);

        Task Update(EditGameFormViewModel model);

        void Remove(int id);

        Game? GetbyId(int id);
    }
}
