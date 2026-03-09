using Rocky.UI.Models;

namespace Rocky.UI.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; } = [];

        public IEnumerable<Category> Categories { get; set; } = [];
    }
}
