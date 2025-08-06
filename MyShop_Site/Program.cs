
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using MyShop_Site.Data;
using MyShop_Site.Services;
using MyShopSite.Startup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add Entity Framework
builder.Services.AddDbContext<MyShopDbContext>(options =>
    options.UseInMemoryDatabase("MyShopDb"));

// Add custom services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<SubscriptionService>();
builder.Services.AddScoped<OrderService>();

// Add authentication services
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// Add application services
builder.Services.AddApplicationServices();
builder.Services.AddCoresPolicies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Use global middleware
app.UseGlobalMiddleware();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

//// Initialize database
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<MyShopDbContext>();
//    context.Database.EnsureCreated();
//}

app.Run();
