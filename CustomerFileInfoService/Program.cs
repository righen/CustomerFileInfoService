using CustomerFileInfoService.Repository;
using CustomerFileInfoService.Services.EmailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerFileInfoService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseSystemd()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    var optionsBuilder = new DbContextOptionsBuilder<CustomerFileInfoDbContext>();

                    services.AddSingleton<IEmailService>(new EmailService(configuration));
                   
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                    services.AddScoped(s => new CustomerFileInfoDbContext(optionsBuilder.Options));

                    services.AddSingleton<IUserNotificationPathRepository>(s => new UserNotificationPathRepository(s));
                    services.AddHostedService<CustomerFileInfoService>();
                });
    }
}
