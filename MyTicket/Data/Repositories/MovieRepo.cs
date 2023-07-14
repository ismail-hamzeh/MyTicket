using Microsoft.EntityFrameworkCore;
using MyTicket.Data.Repositories.Base;
using MyTicket.Models;
using MyTicket.ViewModel;

namespace MyTicket.Data.Repositories
{
    public class MovieRepo : EntityBaseRepo<Movie>, IMovieRepo
    {
        private readonly AppDbContext db;

        public MovieRepo(AppDbContext context) : base(context)
        {
            this.db = context;
        }

        public async Task AddMovieAsync(MovieViewModel movieViewModel)
        {
            var movie = new Movie
            {
                ImageUrl = movieViewModel.ImgUrl,
                Name = movieViewModel.Name,
                Description = movieViewModel.Description,
                Price = movieViewModel.Price,
                StartDate = movieViewModel.StartDate,
                EndDate = movieViewModel.EndDate,
                MovieCategory = movieViewModel.MovieCategory,
                CinemaId = movieViewModel.CinemaId,
                ProducerId = movieViewModel.ProducerId
            };

            await db.Movies.AddAsync(movie);
            await db.SaveChangesAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movie = await db.Movies.Include(c => c.Cinema)
                .Include(p => p.Producer)
                .Include(ac => ac.Actor_Movies)
                .ThenInclude(a => a.Actor).FirstOrDefaultAsync<Movie>(m => m.Id == id);
            return movie;
        }

        public async Task<MovieDropdownsVM> GetMovieDropdownsValues()
        {
            var response = new MovieDropdownsVM()
            {
                Actors = await db.Actors.OrderBy(n => n.FullName).ToListAsync(),
                Cinemas = await db.Cinemas.OrderBy(n => n.Name).ToListAsync(),
                Producers = await db.Producers.OrderBy(n => n.FullName).ToListAsync()
            };

            return response;
        }

        public async Task UpdateMovieAsync(MovieViewModel movieViewModel)
        {
            var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == movieViewModel.Id);

            if (movie != null)
            {
                movie.Name= movieViewModel.Name;
                movie.Description= movieViewModel.Description;
                movie.Price = movieViewModel.Price;
                movie.ImageUrl = movieViewModel.ImgUrl;
                movie.StartDate = movieViewModel.StartDate;
                movie.EndDate = movieViewModel.EndDate;
                movie.MovieCategory = movieViewModel.MovieCategory;
                movie.CinemaId = movieViewModel.CinemaId;
                movie.ProducerId = movieViewModel.ProducerId;
                await db.SaveChangesAsync();
            }

            var existingActors = db.Actors_Movies.Where(m => m.MovieId == movieViewModel.Id);
            db.Actors_Movies.RemoveRange(existingActors);
            await db.SaveChangesAsync();

            foreach (var actorId in movieViewModel.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = movieViewModel.Id,
                    ActorId = actorId
                };
                await db.Actors_Movies.AddAsync(newActorMovie);
            }
            await db.SaveChangesAsync();

        }
    }
}
