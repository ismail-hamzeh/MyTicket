using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using MyTicket.Data.Repositories;
using MyTicket.ViewModel;
using Microsoft.AspNetCore.Authorization;
using MyTicket.Static;

namespace MyTicket.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class MovieController : Controller
    {
        private readonly IMovieRepo movieRepo;
        private readonly ILogger<MovieController> _logger;

        public MovieController(ILogger<MovieController> logger, IMovieRepo movieRepo)
        {
            _logger = logger;
            this.movieRepo = movieRepo;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var movies = await movieRepo.GetAllAsync(m => m.Cinema);
            return View(movies);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string term)
        {
            var allMovies = await movieRepo.GetAllAsync(m => m.Cinema);

            if (term.IsNullOrEmpty())
            {
                return View("Index", allMovies);
            }

            var searchResult = allMovies.Where(m => m.Name.ToLower().Contains(term.ToLower()) || m.Description.ToLower().Contains(term.ToLower())).ToList(); 

            return View("Index", searchResult);
        }

        public async Task<IActionResult> Add()
        {
            var movieDropdownsData = await movieRepo.GetMovieDropdownsValues();

            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(MovieViewModel movieViewModel)
        {
            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await movieRepo.GetMovieDropdownsValues();
                                                  
                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
                return View(movieViewModel);
            }

            await movieRepo.AddMovieAsync(movieViewModel);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var movieDetails = await movieRepo.GetMovieByIdAsync(id);
            return View(movieDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var movie = await movieRepo.GetMovieByIdAsync(id);
            if (movie == null) return View("NotFound");

            var response = new MovieViewModel()
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                Price = movie.Price,
                StartDate = movie.StartDate,
                EndDate = movie.EndDate,
                ImgUrl = movie.ImageUrl,
                MovieCategory = movie.MovieCategory,
                CinemaId = movie.CinemaId,
                ProducerId = movie.ProducerId,
                ActorIds = movie.Actor_Movies.Select(n => n.ActorId).ToList(),
            };

            var movieDropdownsData = await movieRepo.GetMovieDropdownsValues();

            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MovieViewModel movie)
        {
            if (id != movie.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await movieRepo.GetMovieDropdownsValues();

                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

                return View(movie);
            }

            await movieRepo.UpdateMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }

       /* [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}