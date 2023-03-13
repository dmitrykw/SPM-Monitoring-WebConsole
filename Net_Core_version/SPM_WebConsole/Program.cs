var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var startup = new Startup(builder.Configuration); //Add startup class
startup.ConfigureServices(builder.Services); // calling ConfigureServices method
var app = builder.Build();
startup.Configure(app, builder.Environment); // calling Configure method

// Add services to the container.


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
    pattern: "{controller=Monitoring}/{action=Index}/{id?}");

app.Run();
