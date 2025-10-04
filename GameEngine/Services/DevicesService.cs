using GameEngine.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameEngine.Services
{
    public class DevicesService : IDevicesService
    {
        private readonly ApplicationDbContext _context;

        public DevicesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _context.Devices
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                .OrderBy(d => d.Text)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<SelectListItem> GetSelectListByGameId(int Id)
        {
            return _context.GameDevices
                .Where(gd => gd.GameId == Id)
                .Include(gd => gd.Device)
                .Select(gd => new SelectListItem
                {
                    Value = gd.DeviceId.ToString(),
                    Text = gd.Device.Name
                }).ToList();
        }
    }
}
