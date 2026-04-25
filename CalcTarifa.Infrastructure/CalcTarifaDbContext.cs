using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CalcTarifa.Domain.Entities;
using CalcTarifa.Domain.Enums;

namespace CalcTarifa.Infrastructure
{
    public class CalcTarifaDbContext : IdentityDbContext<ApplicationUser>
    {
        public CalcTarifaDbContext(DbContextOptions<CalcTarifaDbContext> options) : base(options) { }

        public DbSet<TarifaRegion>    Tarifas   { get; set; }
        public DbSet<RegistroCalculo> Registros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CalcTarifaDbContext).Assembly);

            // Seed de tarifas según reglas de negocio definidas
            modelBuilder.Entity<TarifaRegion>().HasData(
                new { Id = 1, Region = RegionEnvio.India, NombrePais = "India",
                      TarifaPorKg = 5m,  Activo = true },
                new { Id = 2, Region = RegionEnvio.US,    NombrePais = "Estados Unidos",
                      TarifaPorKg = 8m,  Activo = true },
                new { Id = 3, Region = RegionEnvio.UK,    NombrePais = "Reino Unido",
                      TarifaPorKg = 10m, Activo = true }
            );
        }
    }
}
