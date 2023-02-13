using EFCoreTutorial.Context;
using EFCoreTutorial.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTutorial.Repositories
{
    public abstract class Repository<TEntity> :  IRepository<TEntity> where TEntity : class
    {
        protected readonly MusicContext context;
        public Repository(MusicContext dBcontext)
        {
            context = dBcontext;
        }

        protected DbContext Context { get { return context; } }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(int id)
        {
            TEntity? entity = await GetByIdAsync(id);
            if (entity != null)
            {
                Context.Remove(entity);
                await Context.SaveChangesAsync();
            }
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            TEntity? entity = await Context.Set<TEntity>()
                                 .Where(w => EF.Property<int>(w, "Id") == id)
                                 .FirstOrDefaultAsync();
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            Context.Entry<TEntity>(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return entity;
        }
    }
}
