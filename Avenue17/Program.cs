using Avenue17;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("cadenaLibreria");
Console.WriteLine($"Cadena de conexion desde la aplicación: {connString}");
using var db = new BooksContext();


// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BooksContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;




app.Run();