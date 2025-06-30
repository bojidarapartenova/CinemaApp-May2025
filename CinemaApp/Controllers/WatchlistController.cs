using CinemaApp.Web.ViewModels.Watchlist;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Controllers
{
    public class WatchlistController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<WatchlistViewModel> emptyWatchlist= new List<WatchlistViewModel>();
            return View(emptyWatchlist);
        }
    }
}
