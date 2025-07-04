﻿namespace CinemaApp.Services.Core
{
    using System.Globalization;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Interfaces;
    using Web.ViewModels.Movie;
    using static GCommon.ApplicationConstants;

    public class MovieService : IMovieService
    {
        private readonly CinemaAppDbContext dbContext;

        public MovieService(CinemaAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync()
        {
            IEnumerable<AllMoviesIndexViewModel> allMovies = await this.dbContext
                .Movies
                .AsNoTracking()
                .Select(m => new AllMoviesIndexViewModel()
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

        public async Task AddMovieAsync(MovieFormInputModel inputModel)
        {
            Movie newMovie = new Movie()
            {
                Title = inputModel.Title,
                Genre = inputModel.Genre,
                Director = inputModel.Director,
                Description = inputModel.Description,
                Duration = inputModel.Duration,
                ImageUrl = inputModel.ImageUrl,
                ReleaseDate = DateOnly
                    .ParseExact(inputModel.ReleaseDate, AppDateFormat,
                        CultureInfo.InvariantCulture, DateTimeStyles.None),
            };

            await this.dbContext.Movies.AddAsync(newMovie);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(string? id)
        {
            MovieDetailsViewModel? movieDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid movieId);
            if (isIdValidGuid)
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

        public async Task<MovieFormInputModel?> GetEditableMovieByIdAsync(string? id)
        {
            MovieFormInputModel? editableMovie = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid movieId);
            if (isIdValidGuid)
            {
                editableMovie = await this.dbContext
                    .Movies
                    .AsNoTracking()
                    .Where(m => m.Id == movieId)
                    .Select(m => new MovieFormInputModel()
                    {
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

            return editableMovie;
        }

        public async Task<bool> EditMovieAsync(MovieFormInputModel inputModel)
        {
            Movie? editableMovie = await this.FindMovieByStringId(inputModel.Id);
            if (editableMovie == null)
            {
                return false;
            }

            DateOnly movieReleaseDate = DateOnly
                .ParseExact(inputModel.ReleaseDate, AppDateFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None);
            editableMovie.Title = inputModel.Title;
            editableMovie.Description = inputModel.Description;
            editableMovie.Director = inputModel.Director;
            editableMovie.Duration = inputModel.Duration;
            editableMovie.Genre = inputModel.Genre;
            editableMovie.ImageUrl = inputModel.ImageUrl ?? $"/images/{NoImageUrl}";
            editableMovie.ReleaseDate = movieReleaseDate;

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<DeleteMovieViewModel?> GetMovieDeleteDetailsByIdAsync(string? id)
        {
            DeleteMovieViewModel? deleteMovieViewModel = null;

            Movie? movieToBeDeleted = await this.FindMovieByStringId(id);
            if (movieToBeDeleted != null)
            {
                deleteMovieViewModel = new DeleteMovieViewModel()
                {
                    Id = movieToBeDeleted.Id.ToString(),
                    Title = movieToBeDeleted.Title,
                    ImageUrl = movieToBeDeleted.ImageUrl ?? $"/images/{NoImageUrl}",
                };
            }

            return deleteMovieViewModel;
        }

        public async Task<bool> SoftDeleteMovieAsync(string? id)
        {
            Movie? movieToDelete = await this.FindMovieByStringId(id);
            if (movieToDelete == null)
            {
                return false;
            }

            movieToDelete.IsDeleted = true;

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteMovieAsync(string? id)
        {
            Movie? movieToDelete = await this.FindMovieByStringId(id);
            if (movieToDelete == null)
            {
                return false;
            }

            this.dbContext.Movies.Remove(movieToDelete);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<Movie?> FindMovieByStringId(string? id)
        {
            Movie? movie = null;

            if (!string.IsNullOrWhiteSpace(id))
            {
                bool isGuidValid = Guid.TryParse(id, out Guid movieGuid);
                if (isGuidValid)
                {
                    movie = await this.dbContext
                        .Movies
                        .FindAsync(movieGuid);
                }
            }

            return movie;
        }
    }
}