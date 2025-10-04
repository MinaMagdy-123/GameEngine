using GameEngine.Models;

namespace GameEngine.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCart> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}
