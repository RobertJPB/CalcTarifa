using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CalcTarifa.Domain.Entities;

namespace CalcTarifa.Infrastructure.Configurations
{
    public class TarifaRegionConfiguration : IEntityTypeConfiguration<TarifaRegion>
    {
        public void Configure(EntityTypeBuilder<TarifaRegion> b)
        {
            b.ToTable("Tarifas");
            b.HasKey(t => t.Id);
            b.Property(t => t.NombrePais).IsRequired().HasMaxLength(100);
            b.Property(t => t.TarifaPorKg).HasColumnType("decimal(10,2)");
            b.Property(t => t.Region).HasConversion<int>();
            b.HasIndex(t => t.Region).IsUnique();
        }
    }

    public class RegistroCalculoConfiguration : IEntityTypeConfiguration<RegistroCalculo>
    {
        public void Configure(EntityTypeBuilder<RegistroCalculo> b)
        {
            b.ToTable("RegistrosCalculo");
            b.HasKey(r => r.Id);
            b.Property(r => r.NombreCliente).IsRequired().HasMaxLength(200);
            b.Property(r => r.EmailCliente).IsRequired().HasMaxLength(320);
            b.Property(r => r.PesoKg).HasColumnType("decimal(10,3)");
            b.Property(r => r.TarifaAplicada).HasColumnType("decimal(10,2)");
            b.Property(r => r.CostoTotal).HasColumnType("decimal(12,2)");
            b.Property(r => r.Region).HasConversion<int>();
            b.HasIndex(r => r.EmailCliente);
            b.HasIndex(r => r.FechaCalculo);
        }
    }
}
