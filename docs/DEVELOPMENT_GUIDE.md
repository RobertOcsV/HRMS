# Guia de Desenvolvimento HRMS

## Estado Atual do Projeto

### O que ja existe

**Domain Layer:**
- `Entity` (base) com Id, CriadoEm, AtualizadoEm
- `Colaborador` - entidade completa com validacoes
- `Cargo` - entidade completa
- `Departamento` - entidade completa
- `IRepository<T>` e `IUnitOfWork` - interfaces base

**Infrastructure Layer:**
- `HrmsDbContext` configurado com PostgreSQL
- `Repository<T>` generico
- Configurations para EF Core (Cargo, Colaborador, Departamento)
- Migration inicial criada

**API Layer:**
- Program.cs com Swagger, CORS, Health Checks, Serilog
- Controllers basicos (Cargos, Departamentos)

### O que falta implementar

Baseado no README, ainda precisamos:
1. Sistema de autenticacao (Usuario, JWT)
2. Gestao de Ferias e Ausencias
3. Folha de Ponto
4. Avaliacao de Desempenho
5. Dashboard e Relatorios
6. MediatR com CQRS
7. FluentValidation
8. Testes

---

## Fases de Desenvolvimento

### FASE 1: Fundacao (Voce esta aqui)
> Objetivo: Ter a base solida para construir as features

#### 1.1 Remover dependencia de Redis temporariamente
O Program.cs atual tem health check do Redis. Precisamos remover ate implementar cache.

**Arquivo:** `backend/src/HRMS.API/Program.cs`
```csharp
// REMOVER esta linha por enquanto:
// .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

// MANTER apenas:
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!);
```

#### 1.2 Criar entidade Usuario para autenticacao

**Arquivo:** `backend/src/HRMS.Domain/Entities/Usuario.cs`
```csharp
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

    private Usuario() { }

    public static Usuario Criar(string email, string senhaHash, Role role, Guid? colaboradorId = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email e obrigatorio.", nameof(email));

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
        SenhaHash = novaSenhaHash;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Desativar()
    {
        Ativo = false;
        AtualizadoEm = DateTime.UtcNow;
    }
}
```

#### 1.3 Criar Enum de Roles

**Arquivo:** `backend/src/HRMS.Domain/Enums/Role.cs`
```csharp
namespace HRMS.Domain.Enums;

public enum Role
{
    Colaborador = 1,
    Gestor = 2,
    RH = 3,
    Admin = 4
}
```

#### 1.4 Configurar FluentValidation + MediatR

**Instalar pacotes no HRMS.Application:**
```bash
cd backend/src/HRMS.Application
dotnet add package MediatR
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions
```

#### 1.5 Criar estrutura CQRS

Estrutura de pastas recomendada para Application:
```
HRMS.Application/
├── Common/
│   ├── Behaviors/
│   │   ├── ValidationBehavior.cs
│   │   └── LoggingBehavior.cs
│   ├── Interfaces/
│   │   └── ICurrentUser.cs
│   └── Mappings/
├── Features/
│   ├── Auth/
│   │   ├── Commands/
│   │   │   ├── Login/
│   │   │   │   ├── LoginCommand.cs
│   │   │   │   ├── LoginCommandHandler.cs
│   │   │   │   └── LoginCommandValidator.cs
│   │   │   └── RefreshToken/
│   │   └── Queries/
│   ├── Colaboradores/
│   │   ├── Commands/
│   │   │   ├── Create/
│   │   │   ├── Update/
│   │   │   └── Delete/
│   │   └── Queries/
│   │       ├── GetById/
│   │       └── GetAll/
│   └── ... (outras features)
└── DTOs/
```

---

### FASE 2: Autenticacao e Autorizacao
> Objetivo: Sistema de login funcional com JWT

#### 2.1 Entidades e configuracoes
- [ ] Criar `Usuario` entity
- [ ] Criar `RefreshToken` entity
- [ ] Criar configurations do EF Core
- [ ] Criar migration

#### 2.2 Servicos de autenticacao
- [ ] Criar `IJwtService` e implementacao
- [ ] Criar `IPasswordHasher` e implementacao (BCrypt)
- [ ] Configurar JWT no Program.cs

#### 2.3 Commands e Queries
- [ ] LoginCommand + Handler + Validator
- [ ] RefreshTokenCommand
- [ ] LogoutCommand

#### 2.4 Controller
- [ ] AuthController com endpoints

**Entidades necessarias:**

```csharp
// RefreshToken.cs
public class RefreshToken : Entity
{
    public string Token { get; private set; } = string.Empty;
    public DateTime Expiracao { get; private set; }
    public bool Revogado { get; private set; }
    public DateTime? RevogadoEm { get; private set; }

    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
}
```

---

### FASE 3: CRUD Colaboradores Completo
> Objetivo: Gestao completa de colaboradores com CQRS

#### 3.1 Commands
- [ ] CreateColaboradorCommand
- [ ] UpdateColaboradorCommand
- [ ] DesligarColaboradorCommand

#### 3.2 Queries
- [ ] GetColaboradorByIdQuery
- [ ] GetAllColaboradoresQuery (com paginacao)
- [ ] GetColaboradoresByDepartamentoQuery

#### 3.3 Melhorias na entidade
- [ ] Adicionar HistoricoCargo para rastrear mudancas

**Entidade HistoricoCargo:**
```csharp
public class HistoricoCargo : Entity
{
    public Guid ColaboradorId { get; private set; }
    public Colaborador Colaborador { get; private set; } = null!;

    public Guid CargoAnteriorId { get; private set; }
    public Cargo CargoAnterior { get; private set; } = null!;

    public Guid CargoNovoId { get; private set; }
    public Cargo CargoNovo { get; private set; } = null!;

    public DateTime DataAlteracao { get; private set; }
    public string? Motivo { get; private set; }
}
```

---

### FASE 4: Gestao de Ferias
> Objetivo: Solicitacao e aprovacao de ferias

#### 4.1 Entidades
```csharp
// SolicitacaoFerias.cs
public class SolicitacaoFerias : Entity
{
    public Guid ColaboradorId { get; private set; }
    public DateTime DataInicio { get; private set; }
    public DateTime DataFim { get; private set; }
    public StatusSolicitacao Status { get; private set; }
    public string? Observacao { get; private set; }

    public Guid? AprovadorGestorId { get; private set; }
    public DateTime? DataAprovacaoGestor { get; private set; }

    public Guid? AprovadorRHId { get; private set; }
    public DateTime? DataAprovacaoRH { get; private set; }

    public string? MotivoRejeicao { get; private set; }
}

// Enum
public enum StatusSolicitacao
{
    Pendente = 1,
    AprovadoGestor = 2,
    AprovadoRH = 3,
    Rejeitado = 4,
    Cancelado = 5
}
```

#### 4.2 Regras de negocio
- Colaborador tem direito a 30 dias apos 12 meses
- Pode dividir em ate 3 periodos (minimo 14 dias no primeiro)
- Workflow: Colaborador -> Gestor -> RH

---

### FASE 5: Folha de Ponto
> Objetivo: Registro de entrada/saida

#### 5.1 Entidades
```csharp
// RegistroPonto.cs
public class RegistroPonto : Entity
{
    public Guid ColaboradorId { get; private set; }
    public DateTime DataHora { get; private set; }
    public TipoPonto Tipo { get; private set; }
    public string? Localizacao { get; private set; }
    public string? Justificativa { get; private set; }
    public bool Ajustado { get; private set; }
}

public enum TipoPonto
{
    Entrada = 1,
    SaidaAlmoco = 2,
    RetornoAlmoco = 3,
    Saida = 4
}

// BancoHoras.cs
public class BancoHoras : Entity
{
    public Guid ColaboradorId { get; private set; }
    public int Ano { get; private set; }
    public int Mes { get; private set; }
    public TimeSpan SaldoHoras { get; private set; }
}
```

---

### FASE 6: Avaliacao de Desempenho
> Objetivo: Ciclos de avaliacao

#### 6.1 Entidades
```csharp
// CicloAvaliacao.cs
public class CicloAvaliacao : Entity
{
    public int Ano { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public DateTime DataInicio { get; private set; }
    public DateTime DataFim { get; private set; }
    public StatusCiclo Status { get; private set; }
}

// Avaliacao.cs
public class Avaliacao : Entity
{
    public Guid CicloId { get; private set; }
    public Guid AvaliadoId { get; private set; }
    public Guid AvaliadorId { get; private set; }
    public TipoAvaliacao Tipo { get; private set; }
    public decimal NotaFinal { get; private set; }
    public string? Feedback { get; private set; }
    public StatusAvaliacao Status { get; private set; }
}

// CriterioAvaliacao.cs - configuravel por cargo
public class CriterioAvaliacao : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public decimal Peso { get; private set; }
    public Guid? CargoId { get; private set; } // null = todos os cargos
}
```

---

## Boas Praticas e Padroes

### 1. Result Pattern (evitar exceptions para fluxo de negocio)

**Criar em Domain/Common:**
```csharp
// Result.cs
public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, string.Empty);
    public static Result Failure(string error) => new(false, error);
    public static Result<T> Success<T>(T value) => new(value, true, string.Empty);
    public static Result<T> Failure<T>(string error) => new(default!, false, error);
}

public class Result<T> : Result
{
    public T Value { get; }

    protected internal Result(T value, bool isSuccess, string error)
        : base(isSuccess, error)
    {
        Value = value;
    }
}
```

### 2. Domain Events (para desacoplamento)

```csharp
// IDomainEvent.cs
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

// Adicionar na Entity base:
private readonly List<IDomainEvent> _domainEvents = new();
public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

public void AddDomainEvent(IDomainEvent domainEvent)
{
    _domainEvents.Add(domainEvent);
}

public void ClearDomainEvents()
{
    _domainEvents.Clear();
}
```

### 3. Specification Pattern (para queries complexas)

```csharp
// ISpecification.cs
public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
}
```

### 4. Estrutura de Validators (FluentValidation)

```csharp
// CreateColaboradorCommandValidator.cs
public class CreateColaboradorCommandValidator : AbstractValidator<CreateColaboradorCommand>
{
    public CreateColaboradorCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome e obrigatorio")
            .MaximumLength(200).WithMessage("Nome deve ter no maximo 200 caracteres");

        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("CPF e obrigatorio")
            .Must(ValidarCPF).WithMessage("CPF invalido");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email e obrigatorio")
            .EmailAddress().WithMessage("Email invalido");
    }

    private bool ValidarCPF(string cpf)
    {
        // Implementar validacao de CPF
        return true;
    }
}
```

### 5. Paginacao padronizada

```csharp
// PagedResult.cs
public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public int TotalCount { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PagedResult(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
```

---

## Proximos Passos Imediatos

### Checklist para comecar AGORA:

1. [ ] **Remover Redis do health check** (Program.cs linha 30)

2. [ ] **Criar pasta Enums em Domain**
   ```
   backend/src/HRMS.Domain/Enums/
   ```

3. [ ] **Criar enum Role**
   ```
   Role.cs (Colaborador, Gestor, RH, Admin)
   ```

4. [ ] **Criar entidade Usuario**
   ```
   backend/src/HRMS.Domain/Entities/Usuario.cs
   ```

5. [ ] **Criar entidade RefreshToken**
   ```
   backend/src/HRMS.Domain/Entities/RefreshToken.cs
   ```

6. [ ] **Adicionar DbSets no HrmsDbContext**
   ```csharp
   public DbSet<Usuario> Usuarios => Set<Usuario>();
   public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
   ```

7. [ ] **Criar Configurations do EF Core**
   ```
   UsuarioConfiguration.cs
   RefreshTokenConfiguration.cs
   ```

8. [ ] **Criar nova migration**
   ```bash
   dotnet ef migrations add AddUsuarioAndRefreshToken --project src/HRMS.Infrastructure --startup-project src/HRMS.API
   ```

9. [ ] **Instalar pacotes MediatR e FluentValidation**

10. [ ] **Configurar estrutura CQRS**

---

## Dicas para Portfolio

### O que impressiona recrutadores:

1. **README bem documentado** (voce ja tem!)
2. **Commits semanticos** (feat:, fix:, docs:)
3. **Testes automatizados** (unitarios + integracao)
4. **CI/CD funcionando** (GitHub Actions)
5. **Docker Compose** para subir tudo facilmente
6. **Swagger documentado** com exemplos
7. **Clean Architecture real** (nao so teoria)
8. **Tratamento de erros consistente**
9. **Logs estruturados** (Serilog)
10. **Seed data** para demonstracao

### Sugestoes de melhorias futuras:

1. **API Versioning** - suportar v1, v2
2. **Rate Limiting** - proteger contra abuso
3. **Response Caching** - performance
4. **Background Jobs** - Hangfire para emails
5. **Audit Trail** - rastrear todas mudancas
6. **Multi-tenancy** - se quiser escalar para SaaS
7. **Testes de carga** - k6 ou Artillery
8. **Observabilidade** - OpenTelemetry

---

## Ordem de Implementacao Sugerida

```
Semana 1-2: FASE 1 + FASE 2 (Base + Auth)
   ↓
Semana 3-4: FASE 3 (Colaboradores CRUD completo)
   ↓
Semana 5-6: FASE 4 (Ferias)
   ↓
Semana 7-8: FASE 5 (Ponto)
   ↓
Semana 9-10: FASE 6 (Avaliacoes)
   ↓
Semana 11+: Frontend Angular
```

---

## Comandos Uteis

```bash
# Rodar a API
cd backend/src/HRMS.API
dotnet run

# Criar migration
dotnet ef migrations add NomeDaMigration --project src/HRMS.Infrastructure --startup-project src/HRMS.API

# Aplicar migrations
dotnet ef database update --project src/HRMS.Infrastructure --startup-project src/HRMS.API

# Rodar testes
dotnet test

# Build de producao
dotnet publish -c Release
```

---

## Perguntas Frequentes

**P: Devo criar um repository especifico para cada entidade?**
R: Nao necessariamente. O Repository generico atende na maioria dos casos. Crie especificos apenas quando tiver queries muito complexas.

**P: Onde colocar validacao de CPF?**
R: Crie um Value Object `CPF` em Domain/ValueObjects ou use FluentValidation nos Commands.

**P: Como testar sem banco de dados?**
R: Use InMemory database do EF Core para testes unitarios, e Testcontainers com PostgreSQL para integracao.

**P: MediatR e necessario?**
R: Nao e obrigatorio, mas facilita muito o CQRS e desacopla Controllers dos Handlers.

---

Atualizado em: 2026-02-10
