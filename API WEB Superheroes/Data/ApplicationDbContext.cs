using API_WEB_Superheroes.Models;
using Microsoft.EntityFrameworkCore;

namespace API_WEB_Superheroes.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> Options) : base(Options)
        {
        }

        public DbSet<SuperHeroe> SuperHeroe { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

    }
}
