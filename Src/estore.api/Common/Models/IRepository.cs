namespace estore.api.Common.Models;

using System.Linq.Expressions;

public interface IRepository<TEntity> where TEntity : notnull
{
    void Add(TEntity entity);

    void Update(TEntity entity);

    IQueryable<TEntity> GetAll();

    Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression);
}
