using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CalcTarifa.BusinessApplication.Interfaces.Persistence;
using CalcTarifa.Infrastructure.Configurations;
using CalcTarifa.Infrastructure;

namespace CalcTarifa.Infrastructure.IoC
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration          configuration)
        {
            // ── Opciones de base de datos ───────────────────────────────────
            services.Configure<DatabaseOptions>(
                configuration.GetSection(DatabaseOptions.ConnectionStrings));

            services.AddDbContext<CalcTarifaDbContext>((sp, options) =>
            {
                var dbOpts = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
                options.UseSqlServer(
                    dbOpts.DefaultConnection,
                    sql => sql.EnableRetryOnFailure());
            });

            // ── ASP.NET Core Identity ───────────────────────────────────────
            services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength         = 6;
                opts.Password.RequireDigit           = false;
                opts.Password.RequireUppercase       = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.User.RequireUniqueEmail         = true;
            })
            .AddEntityFrameworkStores<CalcTarifaDbContext>()
            .AddDefaultTokenProviders();

            // ── JWT Bearer ──────────────────────────────────────────────────
            var jwtKey = configuration["Jwt:Key"]
                         ?? throw new InvalidOperationException("Jwt:Key no configurada.");

            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = configuration["Jwt:Issuer"],
                    ValidAudience            = configuration["Jwt:Audience"],
                    IssuerSigningKey         = new SymmetricSecurityKey(
                                                  Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<CalcTarifa.BusinessApplication.Interfaces.Services.ITokenService, CalcTarifa.Infrastructure.Services.TokenService>();
            return services;
        }
    }
}
