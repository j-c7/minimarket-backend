using System.Globalization;
using Minimarket.BLL.Dashboard.Interfaces;
using Minimarket.DAL.Repository.Interfaces;
using Minimarket.DTO.Dashboard;
using Minimarket.Entity;

namespace Minimarket.BLL.Dashboard;

public class DashboardBLL(ISaleRepo saleRepo, IGenericRepo<Product> productRepo) : IDashboardBLL
{

    private static IQueryable<Sale> GetSales(IQueryable<Sale> saleTable, int subsDays)
    {
        DateTime? last = saleTable
        .OrderByDescending(s => s.RegisterDate)
        .Select(s => s.RegisterDate)
        .First();

        last = last!.Value.AddDays(subsDays);
        return saleTable.Where(s => s.RegisterDate!.Value.Date >= last.Value.Date);
    }

    private int TotalSalesLastWeek()
    {
        int total = 0;
        var sales = saleRepo.Query(null);
        if (sales.Count() > 0)
        {
            var saleTable = GetSales(sales, -7);
            total = saleTable.Count();
        }
        return total;
    }

    private string TotalRenueveLastWeek()
    {
        decimal res = 0;
        var sales = saleRepo.Query(null);
        if (sales.Count() > 0)
        {
            var saleTable = GetSales(sales, -7);
            res = saleTable.Select(s => s.Total).Sum(s => s!.Value);
        }
        return Convert.ToString(res, CultureInfo.InstalledUICulture);
    }

    private int TotalProducts() => productRepo.Query(null).Count();

    private Dictionary<string, int> SalesLastWeek()
    {
        Dictionary<string, int> res = [];
        var sales = saleRepo.Query(null);

        if (sales.Count() > 0)
        {
            var saleTable = GetSales(sales, -7);
            res = saleTable
            .GroupBy(s => s.RegisterDate!.Value.Date)
            .OrderBy(g => g.Key)
            .Select(ds => new { date = ds.Key.ToString("dd/MM/yyyy"), total = ds.Count() })
            .ToDictionary(keySelector: s => s.date, elementSelector: s => s.total);
        }
        return res;
    }

    public Result<DashboardDTO> Summary()
    {
        DashboardDTO dboard = new();
        dboard.TotalSales = TotalSalesLastWeek();
        dboard.TotalRevenue = TotalRenueveLastWeek();
        dboard.TotalProducts = TotalProducts();
        List<SaleWeekDTO> salesWeek = [];
        foreach (KeyValuePair<string, int> item in SalesLastWeek())
        {
            salesWeek.Add(new SaleWeekDTO()
            {
                Date = item.Key,
                Total = item.Value
            });
        }
        dboard.LastWeekSales = salesWeek;
        return Result<DashboardDTO>.Success(dboard);
    }
}