using CalcTarifa.API.Middleware;
using CalcTarifa.BusinessApplication.IoC;
using CalcTarifa.Infrastructure;
using CalcTarifa.Infrastructure.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ── Capas de negocio e infraestructura (incluye Identity + JWT) ────────────
builder.Services.AddBusinessApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger con soporte para JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CalcTarifa API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.ApiKey,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Ingresa: Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            []
        }
    });
});

builder.Services.AddCors(opt =>
    opt.AddPolicy("WebPolicy", p =>
        p.WithOrigins(builder.Configuration["AllowedOrigins:Web"]?.Split(';') ?? new[] { "http://localhost:5002" })
         .AllowAnyHeader()
         .AllowAnyMethod()));

var app = builder.Build();

// ── Migrar / sembrar BD ────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<CalcTarifaDbContext>();
    db.Database.Migrate();

    // Sembrar Admin
    await DbInitializer.SeedRolesAndAdminAsync(scope.ServiceProvider);

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("WebPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
