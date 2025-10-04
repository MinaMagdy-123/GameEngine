using GameEngine.Data;
using GameEngine.Models;
using Microsoft.EntityFrameworkCore;

namespace GameEngine.Services
{
    public class OrdersAndItemsService : IOrdersAndItemsService
    {
        private readonly ApplicationDbContext _context;

        public OrdersAndItemsService(ApplicationDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Order> GetAll()
        {
            return _context.Orders
                .Include(o => o.ApplicationUser)
                .OrderByDescending(g => g.Id)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<OrderItem> GetItemsByOrderId(int orderId)
        {
            return _context.OrderItems
                .Where(o => o.OrderId == orderId)
                .Include(o => o.Game)
                .Include(o => o.Device)
                .OrderByDescending(g => g.Id)
                .AsNoTracking()
                .ToList();
        }

        public void Create(List<ShoppingCart> items, decimal totalPrice, string userId)
        {
            var order = new Order
            {
                ApplicationUserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                Items = items.Select(i => new OrderItem
                {
                    GameId = i.GameId,
                    DeviceId = i.DeviceId,
                    Quantity = i.Quantity,
                    Price = i.Game.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
        }

    }
}
