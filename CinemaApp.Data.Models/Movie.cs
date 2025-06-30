using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Data.Models
{
    public class Movie
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public DateOnly ReleaseDate { get; set; }
        public string Director { get; set; } = null!;
        public int Duration { get; set; }
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<UserMovie> UserWatchlists { get; set; }=
            new HashSet<UserMovie>();
    }
}
