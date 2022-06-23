using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieApp.Application.Interfaces;
using MovieApp.Application.Wrappers;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Settings;
using MovieApp.Infrastructure.Persistence.Context;
using MovieApp.Infrastructure.Services;
using Newtonsoft.Json;
using System.Text;

namespace MovieApp.Infrastructure
{
    public static class ServiceExtensions
    {
        public static async Task AddSeedsAsync(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                using (ServiceProvider serviceProvider = services.BuildServiceProvider())
                {
                    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    await Persistence.Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
                    await Persistence.Seeds.DefaultSuperAdmin.SeedAsync(userManager, roleManager);
                }
            }catch (Exception ex)
            {
                //TODO: log
                Console.WriteLine();
            }
            

        }
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(DataContext).Assembly.FullName)));

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
            #region Services
            services.AddTransient<IIdentityService, IdentityService>();
            #endregion
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
        }
    }
}
