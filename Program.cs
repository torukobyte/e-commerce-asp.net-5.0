using System;
using ETicaret.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ETicaret
{
public class Program
{
public static void Main(string[] args)
{
        var host = CreateHostBuilder(args).Build();
        Console.WriteLine("Seeding..");
        using (var scope = host.Services.CreateScope())
        {
                var services = scope.ServiceProvider;
                SeedData.Initialize(services);
        }
        //seed data
        host.Run();
}
public static IHostBuilder CreateHostBuilder(string[] args) =>
Host.CreateDefaultBuilder(args)
.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}
}
