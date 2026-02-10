namespace HRMS.Domain.Entities;

using HRMS.Domain.Common;

public class Colaborador : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string CPF { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime DataNascimento { get; private set; }
    public DateTime DataAdmissao { get; private set; }
    public DateTime? DataDesligamento { get; private set; }
    public bool Ativo { get; private set; }

    public Guid CargoId { get; private set; }
    public Cargo Cargo { get; private set; } = null!;

    public Guid DepartamentoId { get; private set; }
    public Departamento Departamento { get; private set; } = null!;

    public Guid? GestorId { get; private set; }
    public Colaborador? Gestor { get; private set; }

    private readonly List<Colaborador> _subordinados = new();
    public IReadOnlyCollection<Colaborador> Subordinados => _subordinados.AsReadOnly();

    private Colaborador() { }

    public static Colaborador Criar(
        string nome,
        string cpf,
        string email,
        DateTime dataNascimento,
        DateTime dataAdmissao,
        Guid cargoId,
        Guid departamentoId,
        Guid? gestorId = null)
    {
        ValidarDados(nome, cpf, email, dataNascimento, dataAdmissao);

        return new Colaborador
        {
            Nome = nome.Trim(),
            CPF = LimparCPF(cpf),
            Email = email.Trim().ToLowerInvariant(),
            DataNascimento = dataNascimento.Date,
            DataAdmissao = dataAdmissao.Date,
            CargoId = cargoId,
            DepartamentoId = departamentoId,
            GestorId = gestorId,
            Ativo = true
        };
    }

    public void Atualizar(
        string nome,
        string email,
        DateTime dataNascimento,
        Guid cargoId,
        Guid departamentoId,
        Guid? gestorId)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome e obrigatorio.", nameof(nome));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email e obrigatorio.", nameof(email));

        Nome = nome.Trim();
        Email = email.Trim().ToLowerInvariant();
        DataNascimento = dataNascimento.Date;
        CargoId = cargoId;
        DepartamentoId = departamentoId;
        GestorId = gestorId;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Desligar(DateTime dataDesligamento)
    {
        if (dataDesligamento < DataAdmissao)
            throw new ArgumentException("Data de desligamento nao pode ser anterior a admissao.");

        DataDesligamento = dataDesligamento.Date;
        Ativo = false;
        AtualizadoEm = DateTime.UtcNow;
    }

    private static void ValidarDados(
        string nome,
        string cpf,
        string email,
        DateTime dataNascimento,
        DateTime dataAdmissao)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome e obrigatorio.", nameof(nome));

        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF e obrigatorio.", nameof(cpf));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email e obrigatorio.", nameof(email));

        if (dataNascimento >= DateTime.Today)
            throw new ArgumentException("Data de nascimento invalida.", nameof(dataNascimento));

        if (dataAdmissao > DateTime.Today)
            throw new ArgumentException("Data de admissao nao pode ser futura.", nameof(dataAdmissao));
    }

    private static string LimparCPF(string cpf)
    {
        return new string(cpf.Where(char.IsDigit).ToArray());
    }
}
