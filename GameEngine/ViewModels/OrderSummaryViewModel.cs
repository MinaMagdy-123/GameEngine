using GameEngine.Models;

namespace GameEngine.ViewModels
{
    public class OrderSummaryViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public List<ShoppingCart> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}
