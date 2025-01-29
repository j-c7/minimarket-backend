using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Minimarket.Entity;
using Minimarket.DAL.Context;
using Minimarket.DAL.Repository.Interfaces;

namespace Minimarket.DAL.Repository;

public class GenericRepo<T>(MinimarketdbContext context) : IGenericRepo<T> where T : class
{
    public async Task<Result<T>> Create(T entity)
    {
        context.Set<T>().Add(entity);
        if (await context.SaveChangesAsync() == 0)
        {
            return Result<T>.Failure([$"Imposible crear {entity}"]);
        }
        return Result<T>.Success(entity);
    }

    public async Task<Result<T>> Delete(T entity)
    {
        context.Set<T>().Remove(entity);
        if (await context.SaveChangesAsync() == 0)
        {
            return Result<T>.Failure([$"Error al borrar {entity}"]);
        }
        return Result<T>.Success(entity);
    }

    public async Task<Result<T>> Edit(T entity)
    {
        context.Set<T>().Update(entity);
        if (await context.SaveChangesAsync() == 0)
        {
            return Result<T>.Failure([$"Error al editar {entity}"]);
        }
        return Result<T>.Success(entity);
    }

    public IQueryable<T> Query(Expression<Func<T, bool>>? filter = null) => 
        (filter is not null) ? context.Set<T>().Where(filter) : context.Set<T>();
    
    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}