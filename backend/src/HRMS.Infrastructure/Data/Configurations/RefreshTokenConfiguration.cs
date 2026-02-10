namespace HRMS.Infrastructure.Data.Configurations;

using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Id)
            .HasColumnName("id");

        builder.Property(rt => rt.Token)
            .HasColumnName("token")
            .HasMaxLength(500)
            .IsRequired();

        builder.HasIndex(rt => rt.Token)
            .IsUnique();

        builder.Property(rt => rt.Expiracao)
            .HasColumnName("expiracao")
            .IsRequired();

        builder.Property(rt => rt.Revogado)
            .HasColumnName("revogado")
            .IsRequired();

        builder.Property(rt => rt.RevogadoEm)
            .HasColumnName("revogado_em");

        builder.Property(rt => rt.SubstituidoPor)
            .HasColumnName("substituido_por")
            .HasMaxLength(500);

        builder.Property(rt => rt.UsuarioId)
            .HasColumnName("usuario_id")
            .IsRequired();

        builder.Property(rt => rt.CriadoEm)
            .HasColumnName("criado_em")
            .IsRequired();

        builder.Property(rt => rt.AtualizadoEm)
            .HasColumnName("atualizado_em");

        builder.HasOne(rt => rt.Usuario)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
