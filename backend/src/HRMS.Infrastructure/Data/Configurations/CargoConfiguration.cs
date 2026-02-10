namespace HRMS.Infrastructure.Data.Configurations;

using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CargoConfiguration : IEntityTypeConfiguration<Cargo>
{
    public void Configure(EntityTypeBuilder<Cargo> builder)
    {
        builder.ToTable("cargos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Descricao)
            .HasMaxLength(500);

        builder.Property(c => c.Nivel)
            .IsRequired();

        builder.Property(c => c.Ativo)
            .IsRequired();

        builder.Property(c => c.CriadoEm)
            .IsRequired();

        builder.Property(c => c.AtualizadoEm);

        builder.HasIndex(c => c.Nome)
            .IsUnique();
    }
}
