using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocky.UI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MinLength(10, ErrorMessage = "At least 10 characters")]
        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        public double Price { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        [Required(ErrorMessage = "Please select a ApplicationType")]
        public int ApplicationTypeId { get; set; }
        [ForeignKey(nameof(ApplicationTypeId))]
        public virtual ApplicationType ApplicationType { get; set; }

    }
}
