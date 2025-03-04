using System.ComponentModel.DataAnnotations;

namespace Shopping_Tutorial.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(4,ErrorMessage ="Enter type of the category")]
        public string Name { get; set; }
        [Required, MinLength(4, ErrorMessage = "Enter description of the category")]
        public String Description { get; set; }
        [Required]
        public string Slug { get; set; }
        public int Status { get; set; }
    }
}
