namespace HRMS.Domain.Entities;

using HRMS.Domain.Common;
using HRMS.Domain.Enums;

public class Usuario : Entity
{
    public string Email { get; private set; } = string.Empty;
    public string SenhaHash { get; private set; } = string.Empty;
    public Role Role { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime? UltimoLogin { get; private set; }

    public Guid? ColaboradorId { get; private set; }
    public Colaborador? Colaborador { get; private set; }

    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private Usuario() { }

    public static Usuario Criar(string email, string senhaHash, Role role, Guid? colaboradorId = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email e obrigatorio.", nameof(email));

        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new ArgumentException("Senha e obrigatoria.", nameof(senhaHash));

        return new Usuario
        {
            Email = email.Trim().ToLowerInvariant(),
            SenhaHash = senhaHash,
            Role = role,
            ColaboradorId = colaboradorId,
            Ativo = true
        };
    }

    public void RegistrarLogin()
    {
        UltimoLogin = DateTime.UtcNow;
    }

    public void AlterarSenha(string novaSenhaHash)
    {
        if (string.IsNullOrWhiteSpace(novaSenhaHash))
            throw new ArgumentException("Nova senha e obrigatoria.", nameof(novaSenhaHash));

        SenhaHash = novaSenhaHash;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Desativar()
    {
        Ativo = false;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Ativar()
    {
        Ativo = true;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void VincularColaborador(Guid colaboradorId)
    {
        ColaboradorId = colaboradorId;
        AtualizadoEm = DateTime.UtcNow;
    }
}
