using System.ComponentModel.DataAnnotations;

namespace BookNest.DTOs.Categories
{
    public class UpdateCategoryRequest
    {
        [Required(ErrorMessage = "Category title is required.")]
        [StringLength(100, ErrorMessage = "Category title cannot exceed 100 characters.")]
        public string Title { get; set; } = null!;
    }
}
