using System;
using System.Collections.Generic;
using BLL;
using ConsoleUI;
using DAL;
using Domain;
using MenuSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp
{
    class Program
    {
        // Inspired by
        // https://medium.com/swlh/how-to-take-advantage-of-dependency-injection-in-net-core-2-2-console-applications-274e50a6c350
        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<AppDbContext>(
                options => options.UseMySql(
                    "Server=alpha.akaver.com;Database=student2018_jaroot_aspnetbook_akaver;User=student2018;Password=student2018;"
                ))
                .AddTransient<ConsoleApplication>()
                .AddScoped<IStateRepository, StateRepository>()
                .AddScoped<IEngine, BLL.Engine>()
                .BuildServiceProvider();
            
            
            serviceProvider.GetService<ConsoleApplication>().Run();
        }

    }
}
