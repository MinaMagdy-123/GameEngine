using GameEngine.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GameEngine.ViewModels
{
    public class GameDetailsViewModel
    {
        public Game Game { get; set; } = default!;

        [Required(ErrorMessage = "Device is required")]
        [Display(Name = "Select Device")]
        public int DeviceId { get; set; }
        public IEnumerable<SelectListItem> Devices { get; set; } = Enumerable.Empty<SelectListItem>();

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 10, ErrorMessage = "Quantity must be between 1 and 10")]
        public int Quantity { get; set; } = 1;
    }
}
