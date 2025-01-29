using Microsoft.EntityFrameworkCore;
using Minimarket.DAL.Context;
using Minimarket.DAL.Repository.Interfaces;
using Minimarket.Entity;

namespace Minimarket.DAL.Repository;

public class SaleRepo(MinimarketdbContext context) : GenericRepo<Sale>(context), ISaleRepo
{
    public async Task<Result<Sale>> Register(Sale entity)
    {
        Sale newSale = new();
        using (var transaction = context.Database.BeginTransaction())
        {
            // Stock.
            foreach (SaleDetail sd in entity.SaleDetails)
            {
                var product = await context.Products
                    .Where(p => p.Id == sd.IdProduct).FirstOrDefaultAsync();

                if (product == null)
                {
                    transaction.Rollback();
                    return Result<Sale>.Failure(["Producto no encontrado"]);
                }

                product.Stock -= sd.Amount;
                context.Products.Update(product);
            }

            if (await context.SaveChangesAsync() < 1)
                return Result<Sale>.Failure(["No se pudieron guardar cambios en el stock"]);

            // Correlativo en Document Number.
            DocumentNumber? correlative = await context.DocumentNumbers.FirstOrDefaultAsync();
            if (correlative == null)
            {
                correlative = new()
                {
                    LastNumber = 0000,
                    RegisterDate = DateTime.Now
                };
                await context.SaveChangesAsync();
            }
            correlative.LastNumber += 1;
            correlative.RegisterDate = DateTime.Now;
            context.DocumentNumbers.Update(correlative);
            if (await context.SaveChangesAsync() < 1)
                return Result<Sale>.Failure(["No se pudieron guardar cambios en correlativo"]);

            // Número de documento en Ventas.
            int characters = 4;
            string zeros = string.Concat(Enumerable.Repeat("0", characters));
            string saleNumber = zeros + correlative.LastNumber.ToString();
            saleNumber = saleNumber.Substring(saleNumber.Length - characters, characters);

            entity.DocumentNumber = saleNumber;
            await context.Sales.AddAsync(entity);

            if (await context.SaveChangesAsync() < 1)
                return Result<Sale>.Failure(["No se pudieron guardar cambios en el número de documento"]);

            newSale = entity;
            transaction.Commit();
        }
        return Result<Sale>.Success(newSale);
    }
}