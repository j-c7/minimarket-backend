using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Minimarket.DAL.Context;
using Minimarket.DAL.Repository.Interfaces;
using Minimarket.DAL.Repository;
using Minimarket.DAL.Automapper;
using Minimarket.BLL.User.Interfaces;
using Minimarket.BLL.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Minimarket.BLL.Products.Interfaces;
using Minimarket.BLL.Products;
using Minimarket.BLL.Sales.Interfaces;
using Minimarket.BLL.Sales;

namespace Minimarket.IOC;

public static class MinimarketIOC
{
    public static void AddMinimarketIOC(this IServiceCollection service, IConfiguration config)
    {
        // Conección
        service.AddDbContext<MinimarketdbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DatabaseStr")!)
        );

        // Inyección de dependencias.
        service.AddTransient(typeof(IGenericRepo<>), typeof(GenericRepo<>));
        service.AddScoped<ISaleRepo, SaleRepo>();
        service.AddScoped<IUserBLL, UserBLL>();
        service.AddScoped<IProductCategoryBLL, ProductCategoryBLL>();
        service.AddScoped<IProductBLL, ProductBLL>();
        service.AddScoped<ISaleBLL, SaleBLL>();

        //Automapper
        service.AddAutoMapper(typeof(AutomapperProfile));

        // Cors
        // Ya que este proyecto es de práctica, permitimos cualquier origen.
        service.AddCors(options =>
        {
            options.AddPolicy(name: "CorsPolicy", app =>
                app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });

        // JWT
        service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config["JWT:Key"]!)
                )
            };
        });
    }
}

