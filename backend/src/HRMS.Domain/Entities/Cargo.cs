namespace HRMS.Domain.Entities;

using HRMS.Domain.Common;

public class Cargo : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public int Nivel { get; private set; }
    public bool Ativo { get; private set; }

    private readonly List<Colaborador> _colaboradores = new();
    public IReadOnlyCollection<Colaborador> Colaboradores => _colaboradores.AsReadOnly();

    private Cargo() { }

    public static Cargo Criar(string nome, int nivel, string? descricao = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do cargo e obrigatorio.", nameof(nome));

        if (nivel < 1 || nivel > 10)
            throw new ArgumentException("Nivel deve estar entre 1 e 10.", nameof(nivel));

        return new Cargo
        {
            Nome = nome.Trim(),
            Nivel = nivel,
            Descricao = descricao?.Trim(),
            Ativo = true
        };
    }

    public void Atualizar(string nome, int nivel, string? descricao)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do cargo e obrigatorio.", nameof(nome));

        if (nivel < 1 || nivel > 10)
            throw new ArgumentException("Nivel deve estar entre 1 e 10.", nameof(nivel));

        Nome = nome.Trim();
        Nivel = nivel;
        Descricao = descricao?.Trim();
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Desativar()
    {
        Ativo = false;
        AtualizadoEm = DateTime.UtcNow;
    }
}
