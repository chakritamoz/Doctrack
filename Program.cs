using Doctrack.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

//Add DbContext
builder.Services.AddDbContext<DoctrackContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DoctrackContext")));

//Add Session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(option => 
{
    option.IdleTimeout = TimeSpan.FromSeconds(10);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Documents}/{action=Index}/{id?}");

app.Run();
