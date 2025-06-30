namespace CinemaApp.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Identity.Client;
    using CinemaApp.Data.Models;
    using System.Reflection;
    using System.Reflection.Emit;
    public class CinemaAppDbContext : IdentityDbContext
    {
        public CinemaAppDbContext(DbContextOptions<CinemaAppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Movie> Movies { get; set; } = null!;
        public virtual DbSet<UserMovie> UserMovies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());

            builder.Entity<Movie>().HasQueryFilter(m => !m.IsDeleted);
            builder.Entity<UserMovie>()
                .HasKey(um => new { um.UserId, um.MovieId });
        }
    }
}
