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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<SessionCheckBackgroundService>(); // Register the background service

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options =>
			{
				options.LoginPath = "/Auth/Login";
				options.LogoutPath = "/Auth/Logout";
			});
builder.Services.AddDistributedMemoryCache(); // In-memory cache for session
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromHours(1); // Adjust as necessary
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true; // Ensure session cookies are essential for GDPR compliance
});


//builder.Services.ConfigureApplicationCookie(options =>
//{
//	options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Only send cookies over HTTPS
//});

//builder.Services.AddAntiforgery(options =>
//{
//	options.Cookie.SameSite = SameSiteMode.None;  // Allow cookie across schemes in dev only
//});

builder.Services.AddScoped<SessionCheckFilter>();
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddScoped<IMoviesRepository, MoviesRepository>();
builder.Services.AddScoped<IMoviesGetterServices, MoviesGetterService>();
builder.Services.AddScoped<IMoviesUpdaterServices, MoviesUpdaterService>();
builder.Services.AddScoped<IMoviesDeleterServices, MoviesDeleterService>();
builder.Services.AddScoped<ISignInService, SignInService>(); 
builder.Services.AddScoped<ISessionChecker, SessionChecker>();


//Serilog
builder.Host.UseSerilog((HostBuilderContext context,
	IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
	loggerConfiguration
	.ReadFrom.Configuration(context.Configuration) //read configuration settings from built-in IConfiguration
	.ReadFrom.Services(services); //read out current app's services and make them available to serilog
});

builder.Services.AddHttpLogging(options =>
{
	options.LoggingFields =
	Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties |
	Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders;
});

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
