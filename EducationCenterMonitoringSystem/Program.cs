using MonitoringSystem.Infrustructure;
using MonitoringSystem.Application;
using EducationCenterMonitoringSystem.RataLimiters;
using Serilog;
using MonitoringSystem.Infrustructure.Persistence;
using Microsoft.AspNetCore.Identity;
using EducationCenterMonitoringSystem.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// Add services to the container.
ConfigurationServices.AddRateLimiters(builder);
// Add services to the container.


//LoggingConfigurations.UseLogging(builder.Configuration);

builder.Services.AddAuthentication().AddGoogle(x =>
    {
        x.ClientId = builder.Configuration["Web:client_id"];
        x.ClientSecret = builder.Configuration["Web:client_secret"];
    }
);

builder.Host.UseSerilog();
builder.Services.AddLazyCache();
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService();
//builder.Services.AddCors(option => option.AddPolicy(
//    "PolicyForPDP",
//    x =>
//    {
//        x.AllowCredentials();
//        x.WithOrigins("https://online.pdp.uz/");
//    }
//    ));

//builder.Services.AddCors(option => option.AddPolicy(
//    "PolicyForMicrosoft",
//    x =>
//    {
//        x.AllowCredentials();
//        x.WithOrigins("https://www.microsoft.com/");
//        x.WithMethods("Get");
//        x.WithHeaders("Microsoft");  
//    }
//    ));


var app = builder.Build();
app.UseCustomMiddleware();
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
//app.UseCors(
//    x =>
//    {
//        x.AllowCredentials();
//        x.WithOrigins("https://online.pdp.uz/");
//        x.WithMethods("Create");
//    });
//app.UseAuthentication();

app.UseAuthorization();
app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(name: "login",
              pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");
    endpoints.MapControllerRoute(name: "default",
              pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapRazorPages();

app.Run();
