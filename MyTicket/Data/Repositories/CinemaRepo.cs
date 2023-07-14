using MyTicket.Data.Repositories.Base;
using MyTicket.Models;

namespace MyTicket.Data.Repositories
{
    public class CinemaRepo : EntityBaseRepo<Cinema>, ICinemaRepo
    {
        public CinemaRepo(AppDbContext context): base(context)
        {

        }
    }
}
