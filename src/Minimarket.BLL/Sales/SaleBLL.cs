using AutoMapper;
using Minimarket.BLL.Sales.Interfaces;
using Minimarket.DAL.Repository.Interfaces;
using Minimarket.Entity;
using Minimarket.DTO.Sale;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Minimarket.BLL.Sales;

public class SaleBLL(ISaleRepo repo, IGenericRepo<SaleDetail> saleDRepo, IMapper mapper) : ISaleBLL
{
    public async Task<Result<SaleDTO>> Register(SaleDTO entity)
    {
        var sale = await repo.Register(mapper.Map<Sale>(entity));
        if (!sale.IsSucess)
            return Result<SaleDTO>.Failure(sale.Errors);

        return Result<SaleDTO>.Success(mapper.Map<SaleDTO>(sale.Value));
    }

    public async Task<Result<List<SaleDTO>>> History(string seach, string saleNumber, string startDate, string endDate)
    {
        saleNumber = saleNumber == "NA" ? "" : saleNumber;
        startDate = startDate == "NA" ? "" : startDate;
        endDate = endDate == "NA" ? "" : endDate;

        var sales = repo.Query(null);
        if (sales == null)
            return Result<List<SaleDTO>>.Failure(["Historial no encontrado"]);

        List<Sale> list = [];
        if (seach.ToLower() == "fecha")
        {
            DateTime start = DateTime.ParseExact(startDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "d/M/yyyy", CultureInfo.InvariantCulture);

            list = await sales.Where(
                s => s.RegisterDate!.Value.Date >= start && s.RegisterDate.Value.Date <= end.Date)
                .Include(s => s.SaleDetails)
                .ThenInclude(s => s.IdProductNavigation)
                .ToListAsync();
        }
        else
        {
            list = await sales.Where(s => s.DocumentNumber == saleNumber)
            .Include(p => p.SaleDetails)
            .ThenInclude(p => p.IdProductNavigation)
            .ToListAsync();
        }
        if (list.Count == 0)
            return Result<List<SaleDTO>>.Failure(["Venta no encontrada"]);
        return Result<List<SaleDTO>>.Success(mapper.Map<List<SaleDTO>>(list));
    }

    public async Task<Result<List<ReportDTO>>> Report(string startDate, string endDate)
    {
        var sd = saleDRepo.Query(null);
        if (sd == null)
            return Result<List<ReportDTO>>.Failure(["Detalles de venta no encontrados"]);

        DateTime start = DateTime.ParseExact(startDate, "d/M/yyyy", CultureInfo.InvariantCulture);
        DateTime end = DateTime.ParseExact(endDate, "d/M/yyyy", CultureInfo.InvariantCulture);

        List<SaleDetail> list = await sd
        .Include(p => p.IdProductNavigation)
        .Include(s => s.IdSaleNavigation)
        .Where(sd =>
            sd.IdSaleNavigation!.RegisterDate!.Value.Date >= start.Date &&
            sd.IdSaleNavigation.RegisterDate.Value.Date <= end.Date)
        .ToListAsync();
        if (list.Count == 0)
            return Result<List<ReportDTO>>.Failure(["Detalle de venta no encontrado"]);

        return Result<List<ReportDTO>>.Success(mapper.Map<List<ReportDTO>>(list));
    }
}