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

        public async Task<IActionResult> Save(int? id)
        {
            ProductViewModel vmProduct = new ProductViewModel();
            vmProduct.Product = new Product();

            if (id is not null)
            {
                vmProduct.Product = _db.Products.Include(p => p.Category)
                                .FirstOrDefault(product => product.Id == id);

                if (vmProduct.Product is null)
                    return NotFound();
            }

            await PopulateDropdowns(vmProduct);
            return View(vmProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ProductViewModel vmProduct)
        {
            ModelState.Remove($"{nameof(Product)}.{nameof(Product.Image)}");

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(vmProduct);
                return View(vmProduct);
            }

            if (vmProduct.IsEditMode)
                await Update(vmProduct);
            else
                await Insert(vmProduct);

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdowns(ProductViewModel vmProduct)
        {
            vmProduct.Categories = await _db.Categories.Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToListAsync();
        }

        private async Task Insert(ProductViewModel vmProduct)
        {
            var files = HttpContext.Request.Form.Files;
            var webRoot = _webHostEnvironment.WebRootPath;

            string upload = webRoot + WWWRootConstants.ProductImagesPath;
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

        private async Task Update(ProductViewModel vmProduct)
        {
            //var files = HttpContext.Request.Form.Files;
            //var webRoot = _webHostEnvironment.WebRootPath;

            //string upload = _webHostEnvironment + WWWRootConstants.ProductImagesPath;
            //string fileName = Guid.NewGuid().ToString();
            //string extension = Path.GetExtension(files[0].FileName);

            //string targetFileName = fileName + extension;
            //using (var fileStream = new FileStream(Path.Combine(upload, targetFileName), FileMode.Create))
            //{
            //    files[0].CopyTo(fileStream);
            //}
            //vmProduct.Product.Image = targetFileName;

            //_db.Products.Add(vmProduct.Product);
            //await _db.SaveChangesAsync();
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
