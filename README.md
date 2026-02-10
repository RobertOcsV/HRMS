# HRMS - Sistema de Gestão de Recursos Humanos

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Angular](https://img.shields.io/badge/Angular-19-DD0031?style=for-the-badge&logo=angular&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-7-DC382D?style=for-the-badge&logo=redis&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

Sistema completo para gestão de colaboradores, férias, ponto e avaliações de desempenho.

---

## Sobre o Projeto

O HRMS é uma solução web moderna para departamentos de Recursos Humanos, desenvolvida com foco em boas práticas de arquitetura de software, código limpo e experiência do usuário.

Este projeto foi construído como demonstração de competências em desenvolvimento fullstack, aplicando padrões de projeto consolidados e tecnologias atuais do mercado.

### Problema que resolve

Empresas de médio porte frequentemente gerenciam processos de RH em planilhas dispersas, emails e sistemas legados sem integração. O HRMS centraliza:

- Cadastro e histórico de colaboradores
- Solicitações de férias com workflow de aprovação
- Controle de ponto com cálculo automático de horas
- Ciclos de avaliação de desempenho

---

## Funcionalidades

### Gestão de Colaboradores

- Cadastro completo com dados pessoais e profissionais
- Estrutura hierárquica com gestor direto
- Histórico de mudanças de cargo e departamento
- Upload de documentos e foto
- Visualização de organograma
- Processo de desligamento com registro de motivo

### Gestão de Férias e Ausências

- Solicitação de férias com validação de saldo
- Workflow de aprovação multinível (Gestor -> RH)
- Diferentes tipos de ausência: atestado, falta justificada, licenças
- Calendário visual de ausências da equipe
- Notificações por email nas mudanças de status
- Relatórios de saldo por departamento

### Folha de Ponto

- Registro de entrada e saída
- Captura de geolocalização (versão mobile)
- Cálculo automático de horas trabalhadas
- Identificação de horas extras
- Solicitação de ajustes com justificativa
- Fechamento mensal com aprovação do gestor
- Banco de horas

### Avaliação de Desempenho

- Criação de ciclos de avaliação
- Critérios configuráveis por cargo
- Autoavaliação e avaliação pelo gestor
- Calibração de notas pelo RH
- Feedback estruturado obrigatório
- Histórico completo do colaborador

### Dashboard e Relatórios

- Indicadores de headcount e turnover
- Distribuição por departamento e cargo
- Status de férias e ausências
- Métricas de avaliação de desempenho
- Exportação para Excel e PDF

---

## Tecnologias

### Backend

| Tecnologia | Versão | Propósito |
|------------|--------|-----------|
| .NET | 9.0 | Runtime e SDK |
| C# | 13 | Linguagem principal |
| ASP.NET Core | 9.0 | Framework Web API |
| Entity Framework Core | 9.0 | ORM |
| PostgreSQL | 16+ | Banco de dados relacional |
| Redis | 7.x | Cache distribuído |
| MediatR | 12.x | CQRS e Mediator pattern |
| FluentValidation | 11.x | Validação de dados |
| Serilog | 4.x | Logging estruturado |
| BCrypt.Net-Next | 4.x | Hash de senhas |

### Frontend

| Tecnologia | Versão | Propósito |
|------------|--------|-----------|
| Angular | 19.x | Framework SPA |
| TypeScript | 5.6+ | Linguagem principal |
| Angular Material | 19.x | Componentes UI |
| Angular Signals | Nativo | Gerenciamento de estado |
| RxJS | 7.x | Programação reativa |
| ngx-charts | 21.x | Visualização de dados |

### Infraestrutura

| Serviço | Propósito |
|---------|-----------|
| Railway | Hospedagem do backend e PostgreSQL |
| Cloudflare Pages | Hospedagem do frontend |
| Upstash | Redis serverless |
| Cloudflare R2 | Armazenamento de arquivos |
| GitHub Actions | CI/CD |

---

## Arquitetura

### Visão Geral

```
┌─────────────────┐         ┌─────────────────┐         ┌─────────────────┐
│                 │         │                 │         │                 │
│  Angular SPA    │◄───────►│  ASP.NET Core   │◄───────►│   PostgreSQL    │
│  (CF Pages)     │  HTTPS  │  Web API        │         │   (Railway)     │
│                 │         │  (Railway)      │         │                 │
└─────────────────┘         └────────┬────────┘         └─────────────────┘
                                     │
                            ┌────────┴────────┐
                            │                 │
                            ▼                 ▼
                   ┌─────────────────┐ ┌─────────────────┐
                   │  Redis Cache    │ │  Cloudflare R2  │
                   │  (Upstash)      │ │  (Storage)      │
                   └─────────────────┘ └─────────────────┘
```

### Estrutura de Diretórios

```
hrms/
├── backend/
│   ├── src/
│   │   ├── HRMS.Domain/           # Entidades, Value Objects, Domain Events
│   │   ├── HRMS.Application/      # Use Cases, Commands, Queries, DTOs
│   │   ├── HRMS.Infrastructure/   # EF Core, Repositórios, Serviços externos
│   │   └── HRMS.API/              # Controllers, Middlewares, Configuração
│   ├── tests/
│   │   ├── HRMS.Domain.Tests/
│   │   ├── HRMS.Application.Tests/
│   │   ├── HRMS.Infrastructure.Tests/
│   │   └── HRMS.API.Tests/
│   ├── HRMS.sln
│   └── docker-compose.yml
│
├── frontend/
│   └── hrms-web/
│       ├── src/
│       │   └── app/
│       │       ├── core/          # Serviços singleton, guards, interceptors
│       │       ├── shared/        # Componentes reutilizáveis, pipes, directives
│       │       ├── features/      # Módulos de funcionalidades
│       │       │   ├── colaboradores/
│       │       │   ├── ferias/
│       │       │   ├── ponto/
│       │       │   ├── avaliacoes/
│       │       │   └── dashboard/
│       │       └── layout/        # Estrutura visual (header, sidebar)
│       ├── angular.json
│       └── package.json
│
├── docs/
│   ├── API.md
│   ├── ARCHITECTURE.md
│   └── DEPLOYMENT.md
│
├── .github/
│   └── workflows/
│       ├── backend-ci.yml
│       └── frontend-ci.yml
│
├── README.md
└── LICENSE
```

### Backend - Clean Architecture

O backend segue os princípios da Clean Architecture, separando responsabilidades em camadas bem definidas.

**Fluxo de uma requisição:**

```
HTTP Request
     │
     ▼
┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│ Controller  │───►│  MediatR    │───►│  Handler    │───►│ Repository  │
└─────────────┘    └─────────────┘    └─────────────┘    └─────────────┘
                          │
                          ▼
                   ┌─────────────┐
                   │  Behaviors  │
                   │ (Validation,│
                   │  Logging)   │
                   └─────────────┘
```

### Design Patterns Utilizados

**Backend:**

| Pattern | Aplicação |
|---------|-----------|
| Repository | Abstração de acesso a dados |
| Unit of Work | Transações atômicas via DbContext |
| CQRS | Separação de Commands e Queries |
| Mediator | Desacoplamento via MediatR |
| Specification | Regras de consulta encapsuladas |
| Domain Events | Comunicação entre agregados |
| State | Máquina de estados para workflow de férias |
| Result | Retorno explícito de sucesso/falha |

**Frontend:**

| Pattern | Aplicação |
|---------|-----------|
| Smart/Dumb Components | Separação de lógica e apresentação |
| Facade | Encapsulamento de estado por feature |
| Interceptor | Injeção de token e tratamento de erros |
| Guard | Proteção de rotas |
| Adapter | Transformação de dados da API |

---

## Modelagem de Dados

### Diagrama ER Simplificado

```
┌─────────────────┐       ┌─────────────────┐       ┌─────────────────┐
│   Colaborador   │       │ SolicitacaoFeria│       │  RegistroPonto  │
├─────────────────┤       ├─────────────────┤       ├─────────────────┤
│ Id (PK)         │       │ Id (PK)         │       │ Id (PK)         │
│ Nome            │──────<│ ColaboradorId   │       │ ColaboradorId   │>──────│
│ CPF             │       │ DataInicio      │       │ DataHora        │
│ Email           │       │ DataFim         │       │ Tipo            │
│ DataAdmissao    │       │ Status          │       │ Localizacao     │
│ CargoId (FK)    │       │ AprovadorId     │       └─────────────────┘
│ DepartamentoId  │       └─────────────────┘
│ GestorId (FK)   │
└─────────────────┘       ┌─────────────────┐       ┌─────────────────┐
                          │ CicloAvaliacao  │       │    Avaliacao    │
┌─────────────────┐       ├─────────────────┤       ├─────────────────┤
│      Cargo      │       │ Id (PK)         │──────<│ Id (PK)         │
├─────────────────┤       │ Ano             │       │ CicloId (FK)    │
│ Id (PK)         │       │ Status          │       │ AvaliadorId     │
│ Nome            │       └─────────────────┘       │ AvaliadoId      │
│ Nivel           │                                 │ Nota            │
└─────────────────┘       ┌─────────────────┐       └─────────────────┘
                          │  Departamento   │
                          ├─────────────────┤
                          │ Id (PK)         │
                          │ Nome            │
                          │ GestorId (FK)   │
                          └─────────────────┘
```

### Seed Data Inicial

O sistema inclui dados de seed para desenvolvimento:

- 3 departamentos (TI, RH, Financeiro)
- 5 cargos (Desenvolvedor, Analista, Gerente, Diretor, Estagiário)
- 1 usuário admin (admin@hrms.com / Admin@123)
- 10 colaboradores de exemplo

---

## Instalação

### Pré-requisitos

- .NET 9 SDK (https://dotnet.microsoft.com/download/dotnet/9.0)
- Node.js 20+ (https://nodejs.org/)
- PostgreSQL 16+ (https://www.postgresql.org/download/)
- Redis 7+ (https://redis.io/download/) - opcional para desenvolvimento local
- Docker (https://www.docker.com/) - alternativa recomendada

### Opção 1: Docker Compose (Recomendado)

```bash
# Clonar o repositório
git clone https://github.com/seu-usuario/hrms.git
cd hrms

# Subir todos os serviços
docker-compose up -d

# A aplicação estará disponível em:
# Frontend: http://localhost:4200
# Backend:  http://localhost:5000
# Swagger:  http://localhost:5000/swagger
```

### Opção 2: Instalação Manual

#### Backend

```bash
# Navegar para o diretório do backend
cd backend/src/HRMS.API

# Restaurar dependências
dotnet restore

# Configurar variáveis de ambiente (ou editar appsettings.Development.json)
export ConnectionStrings__DefaultConnection="Host=localhost;Database=hrms;Username=postgres;Password=sua_senha"
export ConnectionStrings__Redis="localhost:6379"
export JwtSettings__Secret="sua_chave_secreta_com_pelo_menos_32_caracteres"

# Aplicar migrations
dotnet ef database update --project ../HRMS.Infrastructure

# Executar a API
dotnet run

# A API estará disponível em https://localhost:5001
```

#### Frontend

```bash
# Navegar para o diretório do frontend
cd frontend/hrms-web

# Instalar dependências
npm install

# Configurar ambiente (editar src/environments/environment.ts)
# apiUrl: 'https://localhost:5001/api'

# Executar em modo desenvolvimento
ng serve

# A aplicação estará disponível em http://localhost:4200
```

### Variáveis de Ambiente

#### Backend (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=hrms;Username=postgres;Password=senha",
    "Redis": "localhost:6379"
  },
  "JwtSettings": {
    "Secret": "chave_secreta_minimo_32_caracteres",
    "Issuer": "HRMS",
    "Audience": "HRMS.Users",
    "ExpirationInMinutes": 60,
    "RefreshExpirationInDays": 7
  },
  "StorageSettings": {
    "Provider": "Local",
    "BasePath": "./uploads"
  }
}
```

#### Frontend (environment.ts)

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api',
  tokenKey: 'hrms_token',
  refreshTokenKey: 'hrms_refresh_token'
};
```

---

## Documentação da API

### Autenticação

A API utiliza JWT Bearer tokens. Após login, inclua o token no header:

```
Authorization: Bearer {seu_token}
```

### Endpoints Principais

#### Autenticação

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/auth/login` | Autenticar usuário |
| POST | `/api/auth/refresh` | Renovar token |
| POST | `/api/auth/logout` | Invalidar sessão |

#### Colaboradores

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/colaboradores` | Listar colaboradores |
| GET | `/api/colaboradores/{id}` | Obter colaborador |
| POST | `/api/colaboradores` | Criar colaborador |
| PUT | `/api/colaboradores/{id}` | Atualizar colaborador |
| DELETE | `/api/colaboradores/{id}` | Desligar colaborador |
| GET | `/api/colaboradores/{id}/historico` | Histórico de cargos |

#### Férias

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/ferias` | Listar solicitações |
| GET | `/api/ferias/{id}` | Obter solicitação |
| POST | `/api/ferias` | Criar solicitação |
| POST | `/api/ferias/{id}/aprovar` | Aprovar solicitação |
| POST | `/api/ferias/{id}/rejeitar` | Rejeitar solicitação |
| GET | `/api/ferias/saldo/{colaboradorId}` | Consultar saldo |

#### Ponto

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/ponto/registrar` | Registrar ponto |
| GET | `/api/ponto/hoje` | Registros de hoje |
| GET | `/api/ponto/mensal/{ano}/{mes}` | Folha mensal |
| POST | `/api/ponto/ajuste` | Solicitar ajuste |

#### Avaliações

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/avaliacoes/ciclos` | Listar ciclos |
| POST | `/api/avaliacoes/ciclos` | Criar ciclo |
| GET | `/api/avaliacoes/{cicloId}` | Listar avaliações do ciclo |
| POST | `/api/avaliacoes` | Submeter avaliação |

### Exemplos de Requisição

#### Login

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@hrms.com",
    "senha": "Admin@123"
  }'
```

Resposta:

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4...",
  "expiresIn": 3600,
  "usuario": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "nome": "Administrador",
    "email": "admin@hrms.com",
    "role": "Admin"
  }
}
```

#### Criar Colaborador

```bash
curl -X POST https://localhost:5001/api/colaboradores \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "nome": "João Silva",
    "cpf": "123.456.789-00",
    "email": "joao.silva@empresa.com",
    "dataAdmissao": "2024-01-15",
    "cargoId": "cargo-uuid",
    "departamentoId": "depto-uuid",
    "gestorId": "gestor-uuid"
  }'
```

#### Solicitar Férias

```bash
curl -X POST https://localhost:5001/api/ferias \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "dataInicio": "2024-07-01",
    "dataFim": "2024-07-15",
    "observacao": "Férias de meio de ano"
  }'
```

---

## Testes

### Backend

```bash
# Executar todos os testes
cd backend
dotnet test

# Executar com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Executar testes de integração (requer Docker)
dotnet test --filter Category=Integration
```

### Frontend

```bash
# Testes unitários
cd frontend/hrms-web
ng test

# Testes com cobertura
ng test --code-coverage

# Testes e2e
ng e2e
```

---

## Segurança

### Autenticação

- JWT com expiração de 1 hora
- Refresh tokens com rotação
- Senhas hasheadas com BCrypt (cost factor 12)

### Autorização

O sistema implementa RBAC (Role-Based Access Control) com 4 perfis:

| Role | Permissões |
|------|------------|
| Admin | Acesso total ao sistema |
| RH | Gestão de colaboradores, aprovação final de férias, calibração |
| Gestor | Visualização da equipe, aprovação de férias, avaliações |
| Colaborador | Dados próprios, solicitações, autoavaliação |

### Boas Práticas Implementadas

- HTTPS obrigatório em produção
- Rate limiting por IP e usuário
- Validação de input em todas as camadas
- Logs sem dados sensíveis
- Headers de segurança (HSTS, CSP, etc.)

---

## Monitoramento

### Health Checks

```
GET /health         # Status geral
GET /health/ready   # Dependências prontas
GET /health/live    # Aplicação viva
```

### Logs

Logs estruturados em JSON via Serilog:

```json
{
  "Timestamp": "2024-01-15T10:30:00.000Z",
  "Level": "Information",
  "MessageTemplate": "Solicitação de férias {SolicitacaoId} aprovada",
  "Properties": {
    "SolicitacaoId": "uuid",
    "AprovadorId": "uuid",
    "CorrelationId": "correlation-uuid"
  }
}
```

---

## Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`)
3. Commit suas mudanças (`git commit -m 'feat: adiciona nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request

### Padrões de Commit

Este projeto segue Conventional Commits (https://www.conventionalcommits.org/):

- `feat:` Nova funcionalidade
- `fix:` Correção de bug
- `docs:` Documentação
- `style:` Formatação
- `refactor:` Refatoração
- `test:` Testes
- `chore:` Manutenção

---

## Licença

Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

---

## Autor

**Seu Nome**

- GitHub: @seu-usuario (https://github.com/seu-usuario)
- LinkedIn: Seu Nome (https://linkedin.com/in/seu-perfil)
- Email: seu.email@exemplo.com

---

## Agradecimentos

- Documentação do .NET (https://docs.microsoft.com/dotnet/)
- Angular Docs (https://angular.dev/)
- Clean Architecture - Jason Taylor (https://github.com/jasontaylordev/CleanArchitecture)
- MediatR - Jimmy Bogard (https://github.com/jbogard/MediatR)