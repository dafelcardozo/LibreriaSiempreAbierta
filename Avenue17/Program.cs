using Avenue17;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("cadenaLibreria");
builder.Services.AddDbContext<BooksContext>(options => options
#if DEBUG
        .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
#endif
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

// Query and load a Google Books dataset by defining this variable
#if FIRST_LOAD
using (var scope = app.Services.CreateScope())
{
    var crawler = scope.ServiceProvider.GetRequiredService<WebCrawler>();
    await crawler.SaveBooks();
}
#endif

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.Run();
