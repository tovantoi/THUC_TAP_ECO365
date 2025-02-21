using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace _365Architect.Demo.Persistence
{
    /// <summary>
    /// Application database context 
    /// </summary>
    /// <param name="options">Options for database context</param>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}