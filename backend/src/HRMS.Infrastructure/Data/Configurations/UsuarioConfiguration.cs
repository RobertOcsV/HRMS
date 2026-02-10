namespace HRMS.Infrastructure.Data.Configurations;

using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id");

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.SenhaHash)
            .HasColumnName("senha_hash")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .IsRequired();

        builder.Property(u => u.Ativo)
            .HasColumnName("ativo")
            .IsRequired();

        builder.Property(u => u.UltimoLogin)
            .HasColumnName("ultimo_login");

        builder.Property(u => u.ColaboradorId)
            .HasColumnName("colaborador_id");

        builder.Property(u => u.CriadoEm)
            .HasColumnName("criado_em")
            .IsRequired();

        builder.Property(u => u.AtualizadoEm)
            .HasColumnName("atualizado_em");

        builder.HasOne(u => u.Colaborador)
            .WithMany()
            .HasForeignKey(u => u.ColaboradorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Metadata
            .FindNavigation(nameof(Usuario.RefreshTokens))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
