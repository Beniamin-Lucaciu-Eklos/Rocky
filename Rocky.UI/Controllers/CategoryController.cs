using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocky.UI.Data;
using Rocky.UI.Models;
using System.Threading.Tasks;

namespace Rocky.UI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categories = await _db.Categories.ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
