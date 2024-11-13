using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie_Rating.Repositories.DataAccess;
using Movie_Rating.Repositories;
using Movie_Rating.RepositoryContracts;
using Movie_Rating.ServiceContracts;
using Movie_Rating.Services;
using System;
using System.Configuration;
using System.Text;

namespace Movie_Rating.StartUpExtention
{
    public static class ConfigureServicesExtention
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policyBuilder =>
                {
                    policyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie(options =>
                        {
                            options.LoginPath = "/Auth/Login";
                            options.LogoutPath = "/Auth/Logout";
                        });
            services.AddDistributedMemoryCache(); // In-memory cache for session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1); // Adjust as necessary
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // Ensure session cookies are essential for GDPR compliance
            });


            //services.ConfigureApplicationCookie(options =>
            //{
            //	options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Only send cookies over HTTPS
            //});

            //services.AddAntiforgery(options =>
            //{
            //	options.Cookie.SameSite = SameSiteMode.None;  // Allow cookie across schemes in dev only
            //});
            services.AddSingleton(new AES(configuration["AesGcm:Key"]));

            services.AddScoped<SessionCheckFilter>();
            services.AddScoped<IDataRepository, DataRepository>();
            services.AddScoped<IMoviesRepository, MoviesRepository>();
            services.AddScoped<IMoviesGetterServices, MoviesGetterService>();
            services.AddScoped<IMoviesUpdaterServices, MoviesUpdaterService>();
            services.AddScoped<IMoviesDeleterServices, MoviesDeleterService>();
            services.AddScoped<ISignInService, SignInService>();
            services.AddScoped<ISessionChecker, SessionChecker>();

            services.AddHttpLogging(options =>
            {
                options.LoggingFields =
                Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties |
                Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders;
            });

            return services;
        }
    }
}
