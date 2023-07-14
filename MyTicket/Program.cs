using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyTicket.Data;
using MyTicket.Data.Cart;
using MyTicket.Data.Repositories;
using MyTicket.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
   builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddDataProtection();

builder.Services.AddScoped<IActorRepo, ActorRepo>();
builder.Services.AddScoped<IProducerRepo, ProducerRepo>();
builder.Services.AddScoped<ICinemaRepo, CinemaRepo>();
builder.Services.AddScoped<IMovieRepo, MovieRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(s => Cart.GetShoppingCart(s));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;           
    options.Password.RequireLowercase = true;       
    options.Password.RequireUppercase = true;       
    options.Password.RequireNonAlphanumeric = true; 
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(option => option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme);

builder.Services.ConfigureApplicationCookie(config => config.LoginPath = "/Identity/Login");

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
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movie}/{action=Index}/{id?}");

//Seed database
AppDbInitializer.Seed(app);
await AppDbInitializer.SeedUsersAndRolesAsync(app);

app.Run();
