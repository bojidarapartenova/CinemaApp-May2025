using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CinemaApp.Data.Models
{
    public class UserMovie
    {
        public string UserId { get; set; } = null!;
        public virtual IdentityUser IdentityUser { get; set; } = null!;

        public Guid MovieId { get; set; }
        public virtual Movie Movie { get; set; } = null!;

        public bool IsDeleted {  get; set; }
    }
}
