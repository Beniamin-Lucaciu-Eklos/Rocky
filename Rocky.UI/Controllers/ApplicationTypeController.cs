using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocky.UI.Data;
using Rocky.UI.Models;
using System.Threading.Tasks;

namespace Rocky.UI.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ApplicationTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ApplicationType> applicationTypes = await _db.ApplicationTypes.ToListAsync();
            return View(applicationTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationType applicationType)
        {
            if (!ModelState.IsValid)
                return View(applicationType);

            await _db.ApplicationTypes.AddAsync(applicationType);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var applicationType = await _db.ApplicationTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (applicationType is null)
                return NotFound();

            return View(applicationType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationType applicationType)
        {
            if (!ModelState.IsValid)
                return View(applicationType);

            _db.ApplicationTypes.Update(applicationType);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var applicationType = await _db.ApplicationTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (applicationType is null)
                return NotFound();

            return View(applicationType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var applicationType = await _db.ApplicationTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (applicationType is null)
                return NotFound();

            _db.ApplicationTypes.Remove(applicationType);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
