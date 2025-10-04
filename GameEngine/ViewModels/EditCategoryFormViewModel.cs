using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GameEngine.ViewModels
{
    public class EditCategoryFormViewModel
    {
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();

        [Required]
        [MaxLength(20)]
        [DisplayName("New category name")]
        public string Name { get; set; } = string.Empty;

    }
}
