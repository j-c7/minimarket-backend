using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Minimarket.DAL.Context;
using Minimarket.DAL.Repository.Interfaces;
using Minimarket.DAL.Repository;
using Minimarket.DAL.Automapper;
using Minimarket.BLL.User.Interfaces;
using Minimarket.BLL.User;

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
        service.AddScoped<IUserBLL, UserBLL>();

        //Automapper
        service.AddAutoMapper(typeof(AutomapperProfile));

        // Cors
        // Ya que este proyecto es de práctica, permitimos cualquier origen.
        service.AddCors(options =>
        {
            options.AddPolicy(name: "CorsPolicy", app =>
                app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });
    }
}

