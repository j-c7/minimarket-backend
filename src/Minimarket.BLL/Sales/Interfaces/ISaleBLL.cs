using Minimarket.DTO.Sale;
using Minimarket.Entity;

namespace Minimarket.BLL.Sales.Interfaces;

public interface ISaleBLL
{
    Task<Result<SaleDTO>> Register(SaleDTO entity);

    Task<Result<List<SaleDTO>>> History(string seach, string saleNumber, string startDate, string endDate);
}