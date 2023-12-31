using Library.Interface;
using Library.Models;
using Library.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LibraryDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddScoped<ILibraryAsset, LibraryAssetService>();       // LibraryAssetService is going to be injected into CatalogController anytime it is requesting out ILibraryAsset Interface
builder.Services.AddScoped<ICheckout, CheckoutService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
