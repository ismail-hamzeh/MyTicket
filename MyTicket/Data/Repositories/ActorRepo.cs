using MyTicket.Data.Repositories.Base;
using MyTicket.Models;

namespace MyTicket.Data.Repositories
{
    public class ActorRepo : EntityBaseRepo<Actor>, IActorRepo
    {

        public ActorRepo(AppDbContext context): base(context)
        {
        }

        /*private readonly AppDbContext db;

        public ActorRepo(AppDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(Actor actor)
        {

            await db.Actors.AddAsync(actor);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var result = await db.Actors.FirstOrDefaultAsync(a => a.Id == id);
            db.Actors.Remove(result);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Actor>> GetActorsAsync()
        {
            return await db.Actors.ToListAsync();
        }

        public async Task<Actor> GetByIdAsync(int id)
        {
            var result = await db.Actors.FirstOrDefaultAsync(a => a.Id == id);
            return result;
        }

        public async Task<Actor> UpdateAsync(int id, Actor actor)
        {
            db.Update(actor);
            await db.SaveChangesAsync();
            return actor;
    }*/
    }
}
