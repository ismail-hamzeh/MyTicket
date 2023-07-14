using MyTicket.Data.Repositories.Base;
using MyTicket.Models;
using MyTicket.ViewModel;
using NPOI.SS.Formula.Functions;
using System.Linq.Expressions;

namespace MyTicket.Data.Repositories
{
    public interface IMovieRepo : IEntityBaseRepo<Movie>
    {
        Task<Movie> GetMovieByIdAsync(int id);
        Task AddMovieAsync(MovieViewModel movieViewModel);
        Task UpdateMovieAsync(MovieViewModel movieViewModel);
        Task<MovieDropdownsVM> GetMovieDropdownsValues();
    }
}
