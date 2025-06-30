using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApp.Data.Configuration
{
    public class ApplicationUserMovieConfiguration : IEntityTypeConfiguration<UserMovie>
    {
        public void Configure(EntityTypeBuilder<UserMovie> entity)
        {
            entity
                .HasKey(e => new { e.UserId, e.MovieId });

            entity
                .Property(e => e.UserId)
                .IsRequired();

            entity
                .Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasOne(e => e.IdentityUser)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(e => e.Movie)
                .WithMany(m => m.UserWatchlists)
                .HasForeignKey(e => e.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasQueryFilter(e => e.Movie.IsDeleted == false);

            entity
                .HasQueryFilter(e => e.IsDeleted == false);
        }
    }
}
