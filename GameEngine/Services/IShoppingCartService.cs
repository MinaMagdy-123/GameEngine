using GameEngine.Models;

namespace GameEngine.Services
{
    public interface IShoppingCartService
    {
        void AddToCart(string userId, int gameId, int deviceId, int quantity);
        List<ShoppingCart> GetCartItems(string userId);
        void RemoveFromCart(int cartId);
        void ClearCart(string userId);
        decimal GetCartTotal(string userId);
    }
}
