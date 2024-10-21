using Serilog;
using Rotativa.AspNetCore;
using Movie_Rating.Repositories.DataAccess;
using Movie_Rating.RepositoryContracts;
using Movie_Rating.Repositories;
using Movie_Rating.ServiceContracts;
using Movie_Rating.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

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

builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddScoped<IMoviesRepository, MoviesRepository>();
builder.Services.AddScoped<IMoviesGetterServices, MoviesGetterService>();
builder.Services.AddScoped<IMoviesUpdaterServices, MoviesUpdaterService>();
builder.Services.AddScoped<IMoviesDeleterServices, MoviesDeleterService>();
builder.Services.AddScoped<ISignInService, SignInService>();

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
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
