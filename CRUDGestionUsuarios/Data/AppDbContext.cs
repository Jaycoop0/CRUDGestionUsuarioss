using Microsoft.EntityFrameworkCore;
using CRUDGestionUsuarios.Models;

namespace CRUDGestionUsuarios.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}