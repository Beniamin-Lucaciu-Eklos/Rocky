using System.ComponentModel.DataAnnotations;

namespace Rocky.UI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "display order must be greater than 0")]
        public int DisplayOrder { get; set; }
    }
}
