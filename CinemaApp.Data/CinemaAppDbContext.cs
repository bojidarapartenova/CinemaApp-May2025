namespace CinemaApp.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Identity.Client;
    using CinemaApp.Data.Models;
    using System.Reflection;

    public class CinemaAppDbContext : IdentityDbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public CinemaAppDbContext(DbContextOptions<CinemaAppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
        }
    }
}
