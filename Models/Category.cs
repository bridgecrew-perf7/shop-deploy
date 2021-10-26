using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is mandatory")]
        [MaxLength(60, ErrorMessage = "This field is mandatory and must have between 3 and 60 characters")]
        [MinLength(3, ErrorMessage = "This field is mandatory and must have between 3 and 60 characters")]
        public string Title { get; set; }
    }

}