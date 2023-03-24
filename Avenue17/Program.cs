using Avenue17;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("cadenaLibreria");
builder.Services.AddDbContext<BooksContext>(options => options
    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
        .UseSqlServer(connString));
builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddScoped<WebCrawler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Uncommment to use the crawler to feed the initial database

using (var scope = app.Services.CreateScope())
{
    var crawler = scope.ServiceProvider.GetRequiredService<WebCrawler>();
    await crawler.SaveBooks();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.Run();

// remote: ConnectionStrings:cadenaLibreria = Server=tcp:avenue17-felipe-cardozo.database.windows.net,1433;Initial Catalog=servicio-avenue17;Persist Security Info=False;User ID=felipe-cardozo;Password=JuanDavid2009;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;