using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.UI.Models;

namespace Rocky.UI.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
