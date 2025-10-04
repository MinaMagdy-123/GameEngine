using GameEngine.Data;
using GameEngine.Models;
using Microsoft.EntityFrameworkCore;

namespace GameEngine.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddToCart(string userId, int gameId, int deviceId, int quantity)
        {
            var existingItem = _context.ShoppingCarts
                .FirstOrDefault(i =>
                    i.ApplicationUserId == userId &&
                    i.GameId == gameId &&
                    i.DeviceId == deviceId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new ShoppingCart
                {
                    ApplicationUserId = userId,
                    GameId = gameId,
                    DeviceId = deviceId,
                    Quantity = quantity
                };

                _context.ShoppingCarts.Add(newItem);
            }

            _context.SaveChanges();
        }

        public List<ShoppingCart> GetCartItems(string userId)
        {
            return _context.ShoppingCarts
                .Where(i => i.ApplicationUserId == userId)
                .Include(i => i.Game)
                .Include(i => i.Device)
                .ToList();
        }

        public void RemoveFromCart(int cartId)
        {
            var item = _context.ShoppingCarts
                .SingleOrDefault(c => c.Id == cartId);

            if (item != null)
            {
                _context.ShoppingCarts.Remove(item);
                _context.SaveChanges();
            }
        }

        public void ClearCart(string userId)
        {
            var items = _context.ShoppingCarts
                .Where(i => i.ApplicationUserId == userId);

            _context.ShoppingCarts.RemoveRange(items);
            _context.SaveChanges();
        }

        public decimal GetCartTotal(string userId)
        {
            var items = _context.ShoppingCarts
                .Where(i => i.ApplicationUserId == userId)
                .Include(i => i.Game)
                .ToList();

            return items.Sum(i => i.Game.Price * i.Quantity);
        }
    }
}
