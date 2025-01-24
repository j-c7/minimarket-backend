using System.Linq.Expressions;
using Minimarket.Entity;

namespace Minimarket.DAL.Repository.Interfaces;

public interface IGenericRepo<T> where T : class
{
    Task<Result<T>> Create(T entity);

    Task<Result<T>> Edit(T entity);

    Task<Result<T>> Delete(T entity);

    IQueryable<T> Query(Expression<Func<T, bool>>? filter);

    Task SaveChangesAsync();
}