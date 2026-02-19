# Guia de Desenvolvimento — HRMS

Documento de referência para orientar o desenvolvimento das primeiras features do sistema.

---

## Estado Atual do Projeto

| Camada | O que existe |
|--------|-------------|
| Domain | Entidades (Colaborador, Cargo, Departamento, Usuario, RefreshToken), IRepository, IUnitOfWork |
| Application | Login command/handler/validator (com bug), interfaces (IJwtService, IPasswordHasher, ICurrentUser), DTOs |
| Infrastructure | EF Core context, Repository genérico, migrations, JwtService, PasswordHasher |
| API | AuthController, CargosController, DepartamentosController |
| Frontend | **Vazio** — Angular ainda não foi iniciado |

---

## Bug Crítico: EF Core na Application Layer

### O Problema

O `LoginCommandHandler.cs` usa `.Include()` e `.FirstOrDefaultAsync()` que são métodos de extensão do EF Core.
A Application layer não tem (e **não deve ter**) referência ao EF Core — isso quebra a Clean Architecture.

Há também um segundo bug no mesmo arquivo: chama `.AddAsync()` mas o `IRepository<T>` define `AdicionarAsync()`.

### Por que NÃO instalar EF Core na Application?

A Application layer só pode depender do Domain. EF Core é um detalhe de infraestrutura.
Adicionar EF Core na Application criaria um acoplamento errado, quebrando os princípios da Clean Architecture.

### A Correção Correta

**Passo 1** — Criar interface específica em `HRMS.Domain/Interfaces/IUsuarioRepository.cs`:

```csharp
namespace HRMS.Domain.Interfaces;

using HRMS.Domain.Entities;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> ObterPorEmailComColaboradorAsync(
        string email,
        CancellationToken cancellationToken = default);
}
```

**Passo 2** — Criar implementação em `HRMS.Infrastructure/Data/Repositories/UsuarioRepository.cs`:

```csharp
namespace HRMS.Infrastructure.Data.Repositories;

using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(HrmsDbContext context) : base(context) { }

    public async Task<Usuario?> ObterPorEmailComColaboradorAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(u => u.Colaborador)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}
```

**Passo 3** — Registrar no `HRMS.Infrastructure/DependencyInjection.cs`:

```csharp
// Adicionar junto ao registro do Repository genérico
services.AddScoped<IUsuarioRepository, UsuarioRepository>();
```

**Passo 4** — Corrigir `LoginCommandHandler.cs`:

```csharp
// Trocar:
private readonly IRepository<Usuario> _usuarioRepository;

// Por:
private readonly IUsuarioRepository _usuarioRepository;

// Trocar o uso do Query().Include().FirstOrDefaultAsync() por:
var usuario = await _usuarioRepository
    .ObterPorEmailComColaboradorAsync(request.Email.ToLowerInvariant(), cancellationToken);

// Trocar:
await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

// Por:
await _refreshTokenRepository.AdicionarAsync(refreshToken, cancellationToken);
```

---

## Roadmap das Primeiras Features

### Fase 1 — Auth (base de tudo)

- [x] Login command/handler/validator — *existe, mas tem bug (corrigir acima)*
- [ ] Refresh token — `POST /api/auth/refresh`
- [ ] Logout — `POST /api/auth/logout`

> Para Refresh e Logout, seguir o mesmo padrão: criar `IRefreshTokenRepository` com métodos específicos
> (`ObterPorTokenAsync`, `RevogarAsync`) no Domain, implementar na Infrastructure.

---

### Fase 2 — Colaboradores CRUD (feature central)

Seguir o padrão CQRS com MediatR. Estrutura de pastas:

```
HRMS.Application/Features/Colaboradores/
  Commands/
    CriarColaborador/
      CriarColaboradorCommand.cs
      CriarColaboradorCommandHandler.cs
      CriarColaboradorCommandValidator.cs
    AtualizarColaborador/
      ...
    DesligarColaborador/
      ...
  Queries/
    GetColaboradores/
      GetColaboradoresQuery.cs
      GetColaboradoresQueryHandler.cs
    GetColaboradorById/
      ...
```

| Ação | Tipo | Endpoint |
|------|------|----------|
| Listar | Query | `GET /api/colaboradores` |
| Buscar por ID | Query | `GET /api/colaboradores/{id}` |
| Criar | Command | `POST /api/colaboradores` |
| Atualizar | Command | `PUT /api/colaboradores/{id}` |
| Desligar | Command | `DELETE /api/colaboradores/{id}` |

> Cargos e Departamentos têm controllers simples sem CQRS — pode manter assim, são entidades de suporte/lookup.

---

### Fase 3 — Frontend Angular

O diretório `frontend/` está vazio. Para iniciar:

```bash
cd frontend
ng new hrms-web --style=scss --routing=true --standalone=false
cd hrms-web
ng add @angular/material
```

Estrutura recomendada de pastas:

```
src/app/
  core/
    services/
      auth.service.ts
    interceptors/
      jwt.interceptor.ts
    guards/
      auth.guard.ts
  features/
    auth/
      login/
    colaboradores/
      colaboradores-list/
      colaborador-form/
  layout/
    sidebar/
    header/
  shared/
```

Ordem de implementação:

1. `AuthService` — métodos `login()`, `logout()`, `getToken()`
2. `JwtInterceptor` — adiciona `Authorization: Bearer {token}` em toda requisição
3. `AuthGuard` — redireciona para login se não autenticado
4. Página de Login — formulário, chama a API, salva token no `localStorage`
5. Layout (sidebar + header)
6. Listagem de Colaboradores
7. Formulário de cadastro de Colaborador

---

## Calendário Sugerido

```
Semana 1
  - Corrigir LoginCommandHandler (IUsuarioRepository)
  - Testar Login via Swagger
  - Implementar Refresh Token + Logout

Semana 2
  - Colaboradores: Queries (listar + buscar por ID)
  - Colaboradores: Commands (criar + atualizar)
  - ColaboradoresController

Semana 3
  - Criar projeto Angular
  - Auth: interceptor + guard + login page
  - Colaboradores: listagem no frontend

Semana 4
  - Colaboradores: formulário de cadastro
  - Dashboard básico
```

---

## Convenções do Projeto

### Backend
- Padrão CQRS com MediatR para features (Commands e Queries separados)
- Repositórios específicos para entidades que precisam de queries complexas (ex: `IUsuarioRepository`)
- Repositório genérico `IRepository<T>` para operações simples (Cargo, Departamento)
- Entidades com factory method estático (`Cargo.Criar(...)`, `Colaborador.Criar(...)`)
- Propriedades com `private set` — mutação apenas via métodos de domínio

### Frontend
- Smart/Dumb components — componentes de página gerenciam estado, componentes de UI são "burros"
- Signals para gerenciamento de estado local
- `environment.ts` para configuração da URL da API

### Commits
Seguir Conventional Commits:
- `feat:` nova funcionalidade
- `fix:` correção de bug
- `refactor:` refatoração sem mudança de comportamento
- `test:` testes

---

## Variáveis de Ambiente Necessárias

### Backend — `appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=hrms;Username=postgres;Password=sua_senha"
  },
  "JwtSettings": {
    "Secret": "chave_secreta_minimo_32_caracteres_aqui",
    "Issuer": "HRMS",
    "Audience": "HRMS.Users",
    "ExpirationInMinutes": 60,
    "RefreshExpirationInDays": 7
  }
}
```

### Frontend — `src/environments/environment.ts`

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api',
  tokenKey: 'hrms_token',
  refreshTokenKey: 'hrms_refresh_token'
};
```

---

## Comandos Úteis

```bash
# Rodar a API
cd backend/src/HRMS.API
dotnet run

# Criar nova migration
dotnet ef migrations add NomeDaMigration --project ../HRMS.Infrastructure --startup-project .

# Aplicar migrations
dotnet ef database update --project ../HRMS.Infrastructure --startup-project .

# Rodar frontend
cd frontend/hrms-web
ng serve

# Swagger (com API rodando)
# https://localhost:5001/swagger
```
