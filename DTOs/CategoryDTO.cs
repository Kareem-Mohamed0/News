using System.ComponentModel.DataAnnotations;

namespace News.DTOs
{
    public class CategoryDTO
    {

        [Required(ErrorMessage ="the name is required")]
        [MinLength(2,ErrorMessage ="the name must be more than 2 characters")]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
