using System.ComponentModel.DataAnnotations;

namespace Rocky.UI.Models
{
    public class ApplicationType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }
    }
}
