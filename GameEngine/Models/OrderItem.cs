namespace GameEngine.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; } = default!;

        public int DeviceId { get; set; }
        public Device Device { get; set; } = default!;

        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;
    }
}
