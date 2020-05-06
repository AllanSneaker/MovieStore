using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace MovieStore.WebUI
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var identity = services.GetRequiredService<IIdentityService>();
                    var context = services.GetRequiredService<MovieStoreContext>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    context.Database.Migrate();

                    await MovieStoreContextSeeder.SeedAllAsync(context);
                    await MovieStoreContextSeeder.SeedRolesAsync(roleManager);
                    //await MovieStoreContextSeeder.SeedDefaultUserAsync(identity);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
