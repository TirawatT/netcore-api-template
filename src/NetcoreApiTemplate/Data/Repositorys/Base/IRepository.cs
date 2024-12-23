namespace NetcoreApiTemplate.Data.Repositorys.Base
{
    public interface IRepository<T> where T : class
    {
        //IEnumerable<T> GetAll();
        IQueryable<T> GetAll();
        T? Get(params object?[]? keyValues);
        T? Add(T entity);
        T? Update(T entity);
        T? Delete(params object?[]? keyValues);
    }
}
