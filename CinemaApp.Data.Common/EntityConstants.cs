using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Common
{
    public class EntityConstants
    {
        public const int TitleMinLength = 2;
        public const int TitleMaxLength = 100;
        public const int GenreMinLength = 3;
        public const int GenreMaxLength = 50;
        public const int DirectorNameMinLength = 2;
        public const int DirectorNameMaxLength = 100;
        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 1000;
        public const int DurationMin = 1;
        public const int DurationMax = 300;
        public const int ImageUrlMaxLength = 2048;
    }
}
