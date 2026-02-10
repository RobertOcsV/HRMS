namespace HRMS.Domain.Entities;

using HRMS.Domain.Common;

public class Departamento : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public bool Ativo { get; private set; }

    private readonly List<Colaborador> _colaboradores = new();
    public IReadOnlyCollection<Colaborador> Colaboradores => _colaboradores.AsReadOnly();

    private Departamento() { }

    public static Departamento Criar(string nome, string? descricao = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do departamento e obrigatorio.", nameof(nome));

        return new Departamento
        {
            Nome = nome.Trim(),
            Descricao = descricao?.Trim(),
            Ativo = true
        };
    }

    public void Atualizar(string nome, string? descricao)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do departamento e obrigatorio.", nameof(nome));

        Nome = nome.Trim();
        Descricao = descricao?.Trim();
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
}
