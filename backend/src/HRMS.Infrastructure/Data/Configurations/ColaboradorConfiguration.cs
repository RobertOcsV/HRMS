namespace HRMS.Infrastructure.Data.Configurations;

using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ColaboradorConfiguration : IEntityTypeConfiguration<Colaborador>
{
    public void Configure(EntityTypeBuilder<Colaborador> builder)
    {
        builder.ToTable("colaboradores");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.CPF)
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.DataNascimento)
            .IsRequired();

        builder.Property(c => c.DataAdmissao)
            .IsRequired();

        builder.Property(c => c.DataDesligamento);

        builder.Property(c => c.Ativo)
            .IsRequired();

        builder.Property(c => c.CriadoEm)
            .IsRequired();

        builder.Property(c => c.AtualizadoEm);

        builder.HasIndex(c => c.CPF)
            .IsUnique();

        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.HasOne(c => c.Cargo)
            .WithMany(ca => ca.Colaboradores)
            .HasForeignKey(c => c.CargoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Departamento)
            .WithMany(d => d.Colaboradores)
            .HasForeignKey(c => c.DepartamentoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Gestor)
            .WithMany(g => g.Subordinados)
            .HasForeignKey(c => c.GestorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
