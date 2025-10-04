using GameEngine.Models;

namespace GameEngine.Services
{
    public interface IOrdersAndItemsService
    {
        IEnumerable<Order> GetAll();

        IEnumerable<OrderItem> GetItemsByOrderId(int orderId);

        void Create(List<ShoppingCart> items, decimal totalPrice, string userId);
    }

}
