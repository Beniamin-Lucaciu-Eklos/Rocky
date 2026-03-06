using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky.UI.Data;
using Rocky.UI.Models;
using System.Threading.Tasks;

namespace Rocky.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _db.Products
                .Include(nameof(Category))
                .ToListAsync();

            return View(products);
        }

        public IActionResult Save(int? id)
        {
            IEnumerable<SelectListItem> categoryDropdown = _db.Categories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            ViewBag.CategoryDropdown = categoryDropdown;

            Product product = new();
            if (id is not null)
            {
                product = _db.Products.FirstOrDefault(product => product.Id == id);
                if (product is null)
                    return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
                return NotFound();

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
