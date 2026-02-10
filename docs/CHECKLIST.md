# Checklist de Desenvolvimento HRMS

Use este arquivo para marcar seu progresso. Atualize os [ ] para [x] conforme for completando.

---

## FASE 1: Fundacao

### 1.1 Configuracao Inicial
- [ ] Remover health check do Redis (Program.cs)
- [ ] Testar se a API sobe sem erros
- [ ] Verificar se Swagger esta acessivel

### 1.2 Estrutura Domain
- [ ] Criar pasta `Enums` em HRMS.Domain
- [ ] Criar `Role.cs` (Colaborador, Gestor, RH, Admin)
- [ ] Criar `StatusSolicitacao.cs` (para ferias)
- [ ] Criar pasta `ValueObjects` em HRMS.Domain
- [ ] Criar `CPF.cs` como Value Object (opcional)

### 1.3 Entidades de Autenticacao
- [ ] Criar `Usuario.cs`
- [ ] Criar `RefreshToken.cs`
- [ ] Adicionar DbSets no HrmsDbContext
- [ ] Criar `UsuarioConfiguration.cs`
- [ ] Criar `RefreshTokenConfiguration.cs`
- [ ] Criar migration: `AddUsuarioAndRefreshToken`
- [ ] Aplicar migration no banco

### 1.4 Pacotes NuGet
- [ ] Instalar MediatR no HRMS.Application
- [ ] Instalar FluentValidation no HRMS.Application
- [ ] Instalar FluentValidation.DependencyInjectionExtensions
- [ ] Instalar BCrypt.Net-Next no HRMS.Infrastructure
- [ ] Instalar Microsoft.AspNetCore.Authentication.JwtBearer na API

### 1.5 Estrutura CQRS
- [ ] Criar pasta `Common/Behaviors` em Application
- [ ] Criar `ValidationBehavior.cs`
- [ ] Criar `LoggingBehavior.cs`
- [ ] Criar pasta `Common/Interfaces`
- [ ] Criar `ICurrentUser.cs`
- [ ] Atualizar `DependencyInjection.cs` do Application

---

## FASE 2: Autenticacao

### 2.1 Servicos
- [ ] Criar `IJwtService.cs` em Application/Common/Interfaces
- [ ] Criar `IPasswordHasher.cs` em Application/Common/Interfaces
- [ ] Criar `JwtService.cs` em Infrastructure/Services
- [ ] Criar `PasswordHasher.cs` em Infrastructure/Services
- [ ] Criar `JwtSettings.cs` para configuracoes

### 2.2 Login Command
- [ ] Criar pasta `Features/Auth/Commands/Login`
- [ ] Criar `LoginCommand.cs`
- [ ] Criar `LoginCommandHandler.cs`
- [ ] Criar `LoginCommandValidator.cs`
- [ ] Criar `LoginResponse.cs` (DTO)

### 2.3 Refresh Token Command
- [ ] Criar pasta `Features/Auth/Commands/RefreshToken`
- [ ] Criar `RefreshTokenCommand.cs`
- [ ] Criar `RefreshTokenCommandHandler.cs`

### 2.4 Controller
- [ ] Criar `AuthController.cs`
- [ ] Endpoint POST /api/auth/login
- [ ] Endpoint POST /api/auth/refresh
- [ ] Endpoint POST /api/auth/logout

### 2.5 Configuracao JWT
- [ ] Adicionar JwtSettings no appsettings.json
- [ ] Configurar autenticacao no Program.cs
- [ ] Adicionar [Authorize] nos controllers protegidos
- [ ] Testar login via Swagger

### 2.6 Seed do Usuario Admin
- [ ] Criar seed de usuario admin
- [ ] Email: admin@hrms.com
- [ ] Senha: Admin@123

---

## FASE 3: Colaboradores CRUD

### 3.1 Queries
- [ ] Criar `GetColaboradorByIdQuery`
- [ ] Criar `GetColaboradorByIdQueryHandler`
- [ ] Criar `GetAllColaboradoresQuery` (com paginacao)
- [ ] Criar `GetAllColaboradoresQueryHandler`

### 3.2 Commands
- [ ] Criar `CreateColaboradorCommand`
- [ ] Criar `CreateColaboradorCommandHandler`
- [ ] Criar `CreateColaboradorCommandValidator`
- [ ] Criar `UpdateColaboradorCommand` + Handler + Validator
- [ ] Criar `DesligarColaboradorCommand` + Handler

### 3.3 Controller
- [ ] Atualizar `ColaboradoresController` com MediatR
- [ ] GET /api/colaboradores (paginado)
- [ ] GET /api/colaboradores/{id}
- [ ] POST /api/colaboradores
- [ ] PUT /api/colaboradores/{id}
- [ ] DELETE /api/colaboradores/{id}

### 3.4 Historico de Cargos
- [ ] Criar entidade `HistoricoCargo`
- [ ] Criar configuration EF Core
- [ ] Criar migration
- [ ] Implementar registro automatico de mudancas

---

## FASE 4: Gestao de Ferias

### 4.1 Entidades
- [ ] Criar enum `StatusSolicitacao`
- [ ] Criar entidade `SolicitacaoFerias`
- [ ] Criar entidade `SaldoFerias`
- [ ] Criar configurations EF Core
- [ ] Criar migration

### 4.2 Commands
- [ ] `SolicitarFeriasCommand`
- [ ] `AprovarFeriasGestorCommand`
- [ ] `AprovarFeriasRHCommand`
- [ ] `RejeitarFeriasCommand`
- [ ] `CancelarFeriasCommand`

### 4.3 Queries
- [ ] `GetSolicitacoesFeriasQuery`
- [ ] `GetSaldoFeriasQuery`
- [ ] `GetCalendarioFeriasQuery`

### 4.4 Controller
- [ ] Criar `FeriasController`
- [ ] Implementar todos os endpoints

### 4.5 Regras de Negocio
- [ ] Validar saldo disponivel
- [ ] Validar periodo minimo (14 dias no primeiro)
- [ ] Validar maximo 3 periodos por ano
- [ ] Workflow de aprovacao (Gestor -> RH)

---

## FASE 5: Folha de Ponto

### 5.1 Entidades
- [ ] Criar enum `TipoPonto`
- [ ] Criar entidade `RegistroPonto`
- [ ] Criar entidade `BancoHoras`
- [ ] Criar entidade `SolicitacaoAjuste`
- [ ] Criar configurations EF Core
- [ ] Criar migration

### 5.2 Commands
- [ ] `RegistrarPontoCommand`
- [ ] `SolicitarAjusteCommand`
- [ ] `AprovarAjusteCommand`
- [ ] `FecharFolhaMensalCommand`

### 5.3 Queries
- [ ] `GetRegistrosPontoHojeQuery`
- [ ] `GetFolhaMensalQuery`
- [ ] `GetBancoHorasQuery`

### 5.4 Controller
- [ ] Criar `PontoController`
- [ ] Implementar todos os endpoints

### 5.5 Calculos
- [ ] Calcular horas trabalhadas
- [ ] Identificar horas extras
- [ ] Atualizar banco de horas

---

## FASE 6: Avaliacao de Desempenho

### 6.1 Entidades
- [ ] Criar enum `StatusCiclo`
- [ ] Criar enum `TipoAvaliacao`
- [ ] Criar enum `StatusAvaliacao`
- [ ] Criar entidade `CicloAvaliacao`
- [ ] Criar entidade `CriterioAvaliacao`
- [ ] Criar entidade `Avaliacao`
- [ ] Criar entidade `NotaCriterio`
- [ ] Criar configurations EF Core
- [ ] Criar migration

### 6.2 Commands
- [ ] `CriarCicloAvaliacaoCommand`
- [ ] `IniciarCicloCommand`
- [ ] `SubmeterAutoavaliacaoCommand`
- [ ] `SubmeterAvaliacaoGestorCommand`
- [ ] `CalibrarNotasCommand`
- [ ] `FinalizarCicloCommand`

### 6.3 Queries
- [ ] `GetCiclosAvaliacaoQuery`
- [ ] `GetAvaliacoesPendentesQuery`
- [ ] `GetHistoricoAvaliacoesColaboradorQuery`

### 6.4 Controller
- [ ] Criar `AvaliacoesController`
- [ ] Implementar todos os endpoints

---

## Extras (Pos-MVP)

### Testes
- [ ] Configurar projeto HRMS.Domain.Tests
- [ ] Configurar projeto HRMS.Application.Tests
- [ ] Configurar projeto HRMS.Infrastructure.Tests
- [ ] Configurar projeto HRMS.API.Tests
- [ ] Escrever testes para entidades
- [ ] Escrever testes para handlers
- [ ] Escrever testes de integracao

### CI/CD
- [ ] Criar workflow backend-ci.yml
- [ ] Build automatico
- [ ] Rodar testes automaticamente
- [ ] Deploy automatico para Railway

### Frontend Angular
- [ ] Criar projeto Angular 19
- [ ] Configurar Angular Material
- [ ] Implementar autenticacao
- [ ] Implementar modulo de colaboradores
- [ ] Implementar modulo de ferias
- [ ] Implementar modulo de ponto
- [ ] Implementar modulo de avaliacoes
- [ ] Implementar dashboard

---

## Anotacoes Pessoais

Use este espaco para suas anotacoes:

```
-
-
-
```

---

Ultima atualizacao: 2026-02-10
