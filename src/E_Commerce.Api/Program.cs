
using E_Commerce.Api.Errors;
using E_Commerce.Api.Extensions;
using E_Commerce.Api.MiddleWare;
using E_Commerce.Core.Interfaces;
using E_Commerce.Infrastructure;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace E_Commerce.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddApiRegisteration();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.InfrastructureConfiguration(builder.Configuration);

            //string currentDirectory = Directory.GetCurrentDirectory();
            //string wwwrootPath = Path.Combine(currentDirectory, "wwwroot");

            //if (!string.IsNullOrEmpty(currentDirectory) && Directory.Exists(wwwrootPath))
            //{
            //    builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(wwwrootPath));
            //}
            //else
            //{
            //    Console.WriteLine("Null Exception shoud be handeld");
            //}
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleWare>();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("corsPolicy");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
