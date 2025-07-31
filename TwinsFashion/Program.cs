using Serilog;
using TwinsFashion.Models;
using TwinsFashion.Services;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using Domain.Interfaces;
using Domain.Implementation;
using TwinsFashion.Models.Mappings;

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
builder.Services.AddAutoMapper(typeof(MappingProfile));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<IProductService, ProductService>();
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
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Ensure database and run seeding logic
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // This triggers seeding logic
}

app.Run();
