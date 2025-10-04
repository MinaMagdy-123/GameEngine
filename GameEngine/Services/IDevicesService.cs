using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameEngine.Services
{
    public interface IDevicesService
    {
        IEnumerable<SelectListItem> GetSelectList();
        IEnumerable<SelectListItem> GetSelectListByGameId(int Id);
    }
}
