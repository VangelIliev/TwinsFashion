using Serilog;
using TwinsFashion.Models;
using TwinsFashion.Services;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using Domain.Interfaces;
using Domain.Implementation;
using TwinsFashion.Models.Mappings;
using Domain.Mappers;
using Microsoft.AspNetCore.Authentication.Cookies;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
//serilog
builder.Host.UseSerilog();

//Services
builder.Services.AddControllersWithViews();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddSession();

// Add authentication and cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/LogIn";
        options.LogoutPath = "/Admin/LogOut";
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IDomainMapper, DomainMapper>();
builder.Services.AddTransient<IViewMapper, ViewMapper>();   
builder.Services.AddTransient<IAdminService, AdminService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
