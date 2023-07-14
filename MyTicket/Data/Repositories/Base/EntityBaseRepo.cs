using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace MyTicket.Data.Repositories.Base
{
    public class EntityBaseRepo<T> : IEntityBaseRepo<T> where T : class, IEntityBase, new()
    {

        private readonly AppDbContext db;

        public EntityBaseRepo(AppDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(T entity)
        {
            await db.Set<T>().AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await db.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                EntityEntry entry = db.Entry(entity);
                entry.State = EntityState.Deleted;
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await db.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = db.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id) => await db.Set<T>().FirstOrDefaultAsync(n => n.Id == id);

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = db.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task UpdateAsync(int id, T entity)
        {
            EntityEntry entry = db.Entry<T>(entity);
            entry.State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}