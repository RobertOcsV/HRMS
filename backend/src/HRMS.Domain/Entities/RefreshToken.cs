namespace HRMS.Domain.Entities;

using HRMS.Domain.Common;

public class RefreshToken : Entity
{
    public string Token { get; private set; } = string.Empty;
    public DateTime Expiracao { get; private set; }
    public bool Revogado { get; private set; }
    public DateTime? RevogadoEm { get; private set; }
    public string? SubstituidoPor { get; private set; }

    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;

    private RefreshToken() { }

    public static RefreshToken Criar(Guid usuarioId, int diasValidade = 7)
    {
        return new RefreshToken
        {
            Token = GerarToken(),
            UsuarioId = usuarioId,
            Expiracao = DateTime.UtcNow.AddDays(diasValidade),
            Revogado = false
        };
    }

    public bool EstaAtivo => !Revogado && DateTime.UtcNow < Expiracao;

    public void Revogar(string? substituidoPor = null)
    {
        Revogado = true;
        RevogadoEm = DateTime.UtcNow;
        SubstituidoPor = substituidoPor;
    }

    private static string GerarToken()
    {
        var randomBytes = new byte[64];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
