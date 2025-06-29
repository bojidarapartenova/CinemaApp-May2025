using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using static CinemaApp.Web.ViewModels.ValidationMessages.Movie;

namespace CinemaApp.Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService movieService;
        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllMoviesIndexViewModel> allMovies = await this.movieService
                .GetAllMoviesAsync();

            return View(allMovies);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(MovieFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.movieService.AddMovieAsync(inputModel);
                return this.RedirectToAction(nameof(Index));
            }
            catch(Exception e) 
            {
                Console.WriteLine(e.Message);

                this.ModelState.AddModelError(string.Empty, ServiceCreateError);
                return this.View(inputModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                MovieDetailsViewModel? movieDetails = await movieService
                    .GetMovieDetailsByIdAsync(id);

                if(movieDetails == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(movieDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                MovieFormInputModel? editableMovie= await this.movieService
                    .GetEditableMovieByIdAsync(id);

                if(editableMovie == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(editableMovie);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MovieFormInputModel inputModel)
        {
            if(!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                bool result = await movieService.EditMovieAsync(inputModel);

                if(!result)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Details), new {id=inputModel.Id});
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            try
            {
                DeleteMovieViewModel? movieToBeDeleted = await this.movieService
                    .GetMovieDeleteDetailsByIdAsync(id);
                if (movieToBeDeleted == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(movieToBeDeleted);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteMovieViewModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                bool deleteResult = await this.movieService
                    .SoftDeleteMovieAsync(inputModel.Id);
                if (deleteResult == false)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
