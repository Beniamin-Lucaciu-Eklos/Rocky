using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.UI.Models;
using System.ComponentModel.DataAnnotations;

namespace Rocky.UI.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; } = new();

        //[Required]
        public IFormFile? ImageFile { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = [];

        public IEnumerable<SelectListItem> ApplicationTypes { get; set; } = [];

        public bool IsEditMode => Product is { Id: > 0 };
    }
}
