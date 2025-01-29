using Minimarket.Entity;

namespace Minimarket.DAL.Repository.Interfaces;

public interface ISaleRepo : IGenericRepo<Sale>
{
    Task<Result<Sale>> Register(Sale entity);
}