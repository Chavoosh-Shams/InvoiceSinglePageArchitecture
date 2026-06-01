using InvoiceApp.Models;
using Microsoft.EntityFrameworkCore;
using InvoiceApp.Models.Services.Contracts;
using InvoiceApp.Models.Services.Repositories;
using InvoiceApp.ApplicationServices.Services;
using InvoiceApp.ApplicationServices.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

#region [- AddDbContext() -]
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ProjectDbContext>(options => options.UseSqlServer(connectionString));
#endregion

#region [- AddScoped() -]
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerApplicationService, CustomerApplicationService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
