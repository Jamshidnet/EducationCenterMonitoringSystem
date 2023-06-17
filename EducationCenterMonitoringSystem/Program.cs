using MonitoringSystem.Infrustructure;
using MonitoringSystem.Application;
using EducationCenterMonitoringSystem.RataLimiters;
using Serilog;
using MonitoringSystem.Infrustructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// Add services to the container.
ConfigurationServices.AddRateLimiters(builder);
// Add services to the container.
//LoggingConfigurations.UseLogging(builder.Configuration);
builder.Host.UseSerilog();
//builder.Services.AddDefaultIdentity<IdentityUser>(
//    options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddLazyCache();
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService();

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
