using Microsoft.EntityFrameworkCore;
using Rocky.UI.Models;

namespace Rocky.UI.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Category> Categories { get; set; }
    }
}
