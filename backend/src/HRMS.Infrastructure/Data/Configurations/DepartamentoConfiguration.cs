namespace HRMS.Infrastructure.Data.Configurations;

using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>
{
    public void Configure(EntityTypeBuilder<Departamento> builder)
    {
        builder.ToTable("departamentos");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Descricao)
            .HasMaxLength(500);

        builder.Property(d => d.Ativo)
            .IsRequired();

        builder.Property(d => d.CriadoEm)
            .IsRequired();

        builder.Property(d => d.AtualizadoEm);

        builder.HasIndex(d => d.Nome)
            .IsUnique();
    }
}
