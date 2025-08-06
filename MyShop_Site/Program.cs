using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MyShop_Site.Data;
using MyShopSite.Startup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCoresPolicies();
builder.Services.AddApplicationServices();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<MyShop_Site.Services.ProductService>();
builder.Services.AddScoped<MyShop_Site.Services.PlanService>();

var app = builder.Build();



app.UseGlobalMiddleware();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
