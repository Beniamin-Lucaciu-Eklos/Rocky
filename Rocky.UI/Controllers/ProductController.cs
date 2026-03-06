using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky.UI.Data;
using Rocky.UI.Models;
using Rocky.UI.ViewModels;
using System.Threading.Tasks;

namespace Rocky.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db,
            IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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
            //IEnumerable<SelectListItem> categoryDropdown = _db.Categories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            //ViewBag.CategoryDropdown = categoryDropdown;

            //Product product = new();

            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.Product = new Product();
            productViewModel.Categories = _db.Categories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            if (id is not null)
            {
                productViewModel.Product = _db.Products.FirstOrDefault(product => product.Id == id);

                if (productViewModel.Product is null)
                    return NotFound();
            }

            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ProductViewModel vmProduct)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                var webRoot = _webHostEnvironment.WebRootPath;


                if (vmProduct is { Product: { Id: 0 } })
                {
                    string upload = _webHostEnvironment + WWWRootConstants.ProductImagesPath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    string targetFileName = fileName + extension;
                    using (var fileStream = new FileStream(Path.Combine(upload, targetFileName), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    vmProduct.Product.Image = targetFileName;

                    _db.Products.Add(vmProduct.Product);
                    await _db.SaveChangesAsync();
                }
                else
                {
                }
            }

            return View();
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
