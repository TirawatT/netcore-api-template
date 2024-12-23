using Microsoft.EntityFrameworkCore;

namespace NetcoreApiTemplate.Data.Repositorys.Base
{
    public abstract class Repository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext context;
        public readonly DbSet<TEntity> DbSet;
        public Repository(TContext context)
        {
            this.context = context;
            DbSet = context.Set<TEntity>();
        }
        public IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }
        /// <summary>
        /// Finds an entity with the given primary key values <br/> sequence by HasKey mapping
        /// </summary>
        /// <param name="keyValues">The values of the primary key</param>
        /// <returns></returns>
        public TEntity? Get(params object?[]? keyValues)
        {
            return DbSet.Find(keyValues);
        }

        public TEntity? Add(TEntity entity)
        {
            DbSet.Add(entity);
            context.SaveChanges();
            return entity;
        }
        public TEntity? Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
            return entity;
        }
        public TEntity? Delete(params object?[]? keyValues)
        {
            var entity = Get(keyValues);
            if (entity == null)
            {
                return entity;
            }

            DbSet.Remove(entity);
            context.SaveChanges();

            return entity;
        }




    }
}
