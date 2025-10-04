using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameEngine.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        [Range(0, 100)]
        public int Quantity { get; set; }

        [Required]
        public int GameId { get; set; }

        [ForeignKey("GameId")]
        public Game Game { get; set; } = default!;

        [Required]
        public int DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public Device Device { get; set; } = default!;

        [Required]
        public String ApplicationUserId { get; set; } = string.Empty;

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; } = default!;


    }
}
