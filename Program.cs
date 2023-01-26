using System.Text.Json;
using dotnetEtsyApp.Data;
using dotnetEtsyApp.Helpers.Abstract;
using dotnetEtsyApp.Helpers.Concrete;
using dotnetEtsyApp.Middleware;
using dotnetEtsyApp.Models;
using dotnetEtsyApp.Models.Cache;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var apiEndPoints = System.IO.File.ReadAllText("ApiEndPoints.json");
// add dependency injection
builder.Services.AddSingleton<ICodeGenerator, CodeGenerator>();
// read the connection string from the appsettings.json file
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<CacheData>(x => new CacheData(connectionString, x.GetService<ICodeGenerator>()));
builder.Services.AddSingleton<ApiEndPointsModel>(x=> JsonSerializer.Deserialize<ApiEndPointsModel>(apiEndPoints));
builder.Services.AddScoped<UserService>();
// add database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/GrantAccess";
    });

builder.Services.AddHttpContextAccessor();

// read ApiEndPoints.json file


var app = builder.Build();




// add HttpContextAccessor

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.Use(async (context, next) => await RequestHandler.Handle(context, next));



app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
