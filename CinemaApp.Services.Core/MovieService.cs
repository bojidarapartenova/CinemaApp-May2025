using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.EntityFrameworkCore;
using static CinemaApp.GCommon.ApplicationConstants;

namespace CinemaApp.Services.Core
{
    public class MovieService : IMovieService
    {
        private readonly CinemaAppDbContext dbContext;
        public MovieService(CinemaAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddMovieAsync(MovieFormInputModel inputModel)
        {
            var movie = new Movie
            {
                Title = inputModel.Title,
                Genre = inputModel.Genre,
                Director = inputModel.Director,
                Description = inputModel.Description,
                Duration = inputModel.Duration,
                ReleaseDate = DateOnly
                    .ParseExact(inputModel.ReleaseDate, AppDateFormat,
                        CultureInfo.InvariantCulture, DateTimeStyles.None),
                ImageUrl = inputModel.ImageUrl
            };

            await dbContext.Movies.AddAsync(movie);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> EditMovieAsync(MovieFormInputModel inputModel)
        {
            Movie? editableMovie = await dbContext
                .Movies
                .SingleOrDefaultAsync(m => m.Id.ToString() == inputModel.Id);

            if(editableMovie == null)
            {
                return false;
            }

            DateOnly movieReleaseDate = DateOnly
                .ParseExact(inputModel.ReleaseDate, AppDateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None);

            editableMovie.Title = inputModel.Title;
            editableMovie.Description=inputModel.Description;
            editableMovie.Director = inputModel.Director;
            editableMovie.Duration=inputModel.Duration;
            editableMovie.Genre=inputModel.Genre;
            editableMovie.ImageUrl=inputModel.ImageUrl ?? $"/images/{NoImageUrl}";
            editableMovie.ReleaseDate=movieReleaseDate;

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync()
        {
            IEnumerable<AllMoviesIndexViewModel> allMovies = await this.dbContext
                .Movies
                .Select(m => new AllMoviesIndexViewModel
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    Genre = m.Genre,
                    ReleaseDate = m.ReleaseDate.ToString(AppDateFormat),
                    Director = m.Director,
                    ImageUrl = m.ImageUrl,
                })
                .ToListAsync();

            foreach (AllMoviesIndexViewModel movie in allMovies)
            {
                if (String.IsNullOrEmpty(movie.ImageUrl))
                {
                    movie.ImageUrl = $"/images/{NoImageUrl}";
                }
            }

            return allMovies;
        }

        public async Task<MovieFormInputModel?> GetEditableMovieByIdAsync(string? id)
        {
            MovieFormInputModel? movieDetails = null;

            bool isValidId = Guid.TryParse(id, out Guid movieId);

            if (isValidId)
            {
                movieDetails = await this.dbContext
                    .Movies
                    .AsNoTracking()
                    .Where(m => m.Id == movieId)
                    .Select(m => new MovieFormInputModel()
                    {
                        Id=m.Id.ToString(),
                        Description = m.Description,
                        Director = m.Director,
                        Duration = m.Duration,
                        Genre = m.Genre,
                        ImageUrl = m.ImageUrl ?? $"/images/{NoImageUrl}",
                        ReleaseDate = m.ReleaseDate.ToString(AppDateFormat),
                        Title = m.Title
                    })
                    .SingleOrDefaultAsync();
            }

            return movieDetails;
        }

        public async Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(string? id)
        {
            MovieDetailsViewModel? movieDetails = null;

            bool isValidId= Guid.TryParse(id, out Guid movieId);

            if (isValidId)
            {
                movieDetails = await this.dbContext
                    .Movies
                    .AsNoTracking()
                    .Where(m => m.Id == movieId)
                    .Select(m => new MovieDetailsViewModel()
                    {
                        Id = m.Id.ToString(),
                        Description = m.Description,
                        Director = m.Director,
                        Duration = m.Duration,
                        Genre = m.Genre,
                        ImageUrl = m.ImageUrl ?? $"/images/{NoImageUrl}",
                        ReleaseDate = m.ReleaseDate.ToString(AppDateFormat),
                        Title = m.Title
                    })
                    .SingleOrDefaultAsync();
            }

            return movieDetails;
        }
    }
}
