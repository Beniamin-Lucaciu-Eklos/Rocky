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
                .Include(p => p.Category)
                .Include(p => p.ApplicationType)
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Save(int? id)
        {
            ProductViewModel vmProduct = new ProductViewModel();
            vmProduct.Product = new Product();

            if (id is not null)
            {
                vmProduct.Product = _db.Products
                    .Include(p => p.Category)
                    .Include(p => p.ApplicationType)
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
            ModelState.Remove($"{nameof(Product)}.{nameof(Product.Category)}");
            ModelState.Remove($"{nameof(Product)}.{nameof(Product.ApplicationType)}");

            await PopulateDropdowns(vmProduct);

            if (ModelState.IsValid)
            {
                if (vmProduct.IsEditMode)
                    await Update(vmProduct);
                else
                    await Insert(vmProduct);

                return RedirectToAction(nameof(Index));
            }

            return View(vmProduct);
        }

        private async Task PopulateDropdowns(ProductViewModel vmProduct)
        {
            vmProduct.Categories = await _db.Categories.Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToListAsync();

            vmProduct.ApplicationTypes = await _db.ApplicationTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()))
              .ToListAsync();
        }

        private async Task Insert(ProductViewModel vmProduct)
        {
            SaveImage(vmProduct);

            _db.Products.Add(vmProduct.Product);
            await _db.SaveChangesAsync();
        }

        private (string FileName, string FullPath) CreateDestinationPath(IFormFile? imageFile)
        {
            var fileName = GetFormFileName(imageFile);
            var fullPath = CreateImageFullPath(fileName);
            return (fileName, fullPath);
        }

        private string CreateImageFullPath(string fileName)
        {
            var webRoot = _webHostEnvironment.WebRootPath;
            var upload = webRoot + WWWRootConstants.ProductImagesPath;
            return Path.Combine(upload, fileName);
        }

        private string GetFormFileName(IFormFile imageFile)
        {
            if (imageFile is null)
                return null;

            var fileName = Guid.NewGuid().ToString("N");
            var extension = Path.GetExtension(imageFile.FileName);
            return fileName + extension;
        }

        private void SaveImage(ProductViewModel vmProduct)
        {
            var imageFile = vmProduct.ImageFile;
            if (imageFile is { Length: > 0 })
            {
                var destinationPath = CreateDestinationPath(imageFile);

                using (var fileStream = new FileStream(destinationPath.FullPath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }
                vmProduct.Product.Image = destinationPath.FileName;
            }
        }

        private async Task Update(ProductViewModel vmProduct)
        {
            var productFromDb = await _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == vmProduct.Product.Id);

            var imageFile = vmProduct.ImageFile;
            if (imageFile is not null && productFromDb.Image is not null)
            {
                DeleteImage(productFromDb.Image);
            }
            else
                vmProduct.Product.Image = productFromDb.Image;

            SaveImage(vmProduct);

            _db.Products.Update(vmProduct.Product);
            await _db.SaveChangesAsync();
        }

        private void DeleteImage(string? imageFile)
        {
            if (imageFile is (null or ""))
                return;

            var imageFullPath = CreateImageFullPath(imageFile);
            if (System.IO.File.Exists(imageFullPath))
                System.IO.File.Delete(imageFullPath);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var product = await _db.Products
                .Include(c => c.Category)
                .Include(c => c.ApplicationType)
                .FirstOrDefaultAsync(x => x.Id == id);
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

            DeleteImage(product.Image);

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
