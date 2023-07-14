using MyTicket.Data.Repositories.Base;
using MyTicket.Models;

namespace MyTicket.Data.Repositories
{
    public class ProducerRepo : EntityBaseRepo<Producer>, IProducerRepo
    {
        public ProducerRepo(AppDbContext context) : base(context)
        {
            
        }
    }
}
