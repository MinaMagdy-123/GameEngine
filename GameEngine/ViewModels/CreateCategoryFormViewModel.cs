using System.ComponentModel.DataAnnotations;

namespace GameEngine.ViewModels
{
    public class CreateCategoryFormViewModel
    {
        [Required]
        [MaxLength(20)]
        public required string Name { get; set; }
    }
}
