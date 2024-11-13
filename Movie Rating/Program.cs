using Serilog;
using Rotativa.AspNetCore;
using Movie_Rating.Repositories.DataAccess;
using Movie_Rating.RepositoryContracts;
using Movie_Rating.Repositories;
using Movie_Rating.ServiceContracts;
using Movie_Rating.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie_Rating.StartUpExtention;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<SessionCheckBackgroundService>(); // Register the background service

//Serilog
builder.Host.UseSerilog((HostBuilderContext context,
    IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) //read configuration settings from built-in IConfiguration
    .ReadFrom.Services(services); //read out current app's services and make them available to serilog
});

builder.Services.ConfigureServices(builder.Configuration);

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

var app = builder.Build();

app.UseExceptionHandler("/Auth/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Auth/Error");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
