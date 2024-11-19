using Inventory.Business.App;
using Inventory.Data.App;
using Inventory.Model.APP;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace InventoryManagementSystem.Presentation.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IRepositry<Customer>, CustomerRepository>();

            builder.Services.AddScoped<IRepositry<Supplier>, SupplierRepository>();


            builder.Services.AddScoped<ReportRepository>();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            builder.Services.AddScoped<IRepositry<Product>, ProductRepository>();

            builder.Services.AddScoped<IRepositry<Order>, OrderRepository>();


            builder.Services.AddDbContext<InventoryContext>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
