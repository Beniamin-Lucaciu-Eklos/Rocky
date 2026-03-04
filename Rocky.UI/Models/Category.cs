using System.ComponentModel.DataAnnotations;

namespace Rocky.UI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }
    }
}
