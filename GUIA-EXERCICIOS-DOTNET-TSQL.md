# Guia Completo de Exercicios - .NET & T-SQL
## Do Fundamento ao Nivel Pleno/Senior

> Cada modulo tem exercicios progressivos: **Basico → Intermediario → Avancado**.
> Faca todos em ordem. Nao pule etapas. A base solida e o que separa um senior de verdade.

---

# PARTE 1 - T-SQL (SQL Server)

---

## Modulo 1: Fundamentos de SQL - DDL e DML

### 1.1 Criacao de Banco e Tabelas

**Exercicio 1 - Modelagem basica**
Crie um banco de dados `Empresa` com as seguintes tabelas:

```
- Departamentos (Id, Nome, Orcamento, DataCriacao)
- Funcionarios (Id, Nome, Email, Salario, DataAdmissao, DepartamentoId)
- Projetos (Id, Nome, DataInicio, DataFim, Orcamento, Status)
- FuncionariosProjetos (FuncionarioId, ProjetoId, HorasAlocadas, Papel)
```

Requisitos:
- Use tipos de dados apropriados (VARCHAR vs NVARCHAR, DECIMAL vs MONEY, etc.)
- Defina PKs, FKs, constraints NOT NULL onde fizer sentido
- Adicione constraints CHECK (ex: Salario > 0, Email com formato valido)
- Adicione DEFAULT values onde apropriado
- Crie indices nas FKs

**Exercicio 2 - ALTER TABLE**
- Adicione uma coluna `Telefone` na tabela Funcionarios
- Altere o tamanho da coluna `Nome` de Funcionarios
- Adicione uma constraint UNIQUE no Email
- Crie um indice composto em (DepartamentoId, Nome) na tabela Funcionarios
- Remova uma coluna e depois adicione-a novamente

**Exercicio 3 - Esquemas e permissoes**
- Crie schemas separados: `rh`, `projetos`, `financeiro`
- Mova tabelas para os schemas corretos
- Crie logins e users com permissoes diferentes por schema

---

### 1.2 INSERT, UPDATE, DELETE

**Exercicio 4 - Inserindo dados**
- Insira pelo menos 5 departamentos
- Insira pelo menos 30 funcionarios distribuidos nos departamentos
- Insira pelo menos 10 projetos com status variados ('Ativo', 'Concluido', 'Cancelado', 'Pausado')
- Insira alocacoes de funcionarios em projetos
- Use INSERT com SELECT para duplicar dados entre tabelas
- Use INSERT com OUTPUT para capturar IDs gerados

**Exercicio 5 - Atualizacoes**
- Aumente em 10% o salario de todos os funcionarios do departamento "TI"
- Atualize o status de projetos cuja DataFim ja passou para 'Concluido'
- Use UPDATE com JOIN para atualizar dados baseado em outra tabela
- Use UPDATE com subquery
- Faca UPDATE com OUTPUT para auditar mudancas (valores antes/depois)

**Exercicio 6 - Delecao**
- Delete funcionarios que nao estao alocados em nenhum projeto
- Use DELETE com JOIN
- Demonstre a diferenca entre DELETE e TRUNCATE
- Use MERGE para fazer upsert (INSERT ou UPDATE conforme existencia)

---

### 1.3 SELECT - Do Basico ao Avancado

**Exercicio 7 - Consultas basicas**
- Liste todos os funcionarios ordenados por salario decrescente
- Encontre funcionarios cujo nome comeca com 'A' ou termina com 'a'
- Liste funcionarios admitidos nos ultimos 2 anos
- Use BETWEEN, IN, LIKE, IS NULL em consultas variadas
- Use TOP, OFFSET/FETCH para paginacao

**Exercicio 8 - Agregacoes**
- Calcule o salario medio, minimo, maximo e total por departamento
- Conte quantos funcionarios cada departamento tem
- Encontre departamentos com mais de 5 funcionarios (HAVING)
- Calcule a media de horas alocadas por projeto
- Use GROUP BY com ROLLUP e CUBE para subtotais

**Exercicio 9 - JOINs**
- Liste funcionarios com o nome do seu departamento (INNER JOIN)
- Liste TODOS os departamentos, mesmo os sem funcionarios (LEFT JOIN)
- Liste funcionarios que estao em TODOS os projetos ativos (combinacao de JOINs)
- Faca um CROSS JOIN para gerar combinacoes
- Use SELF JOIN para encontrar funcionarios do mesmo departamento
- Demonstre a diferenca entre LEFT JOIN e LEFT JOIN com WHERE na tabela direita

**Exercicio 10 - Subqueries**
- Encontre funcionarios com salario acima da media do seu departamento
- Liste departamentos cujo orcamento e maior que a soma dos salarios
- Use EXISTS para encontrar departamentos que tem projetos ativos
- Use subquery correlacionada para rank de salarios
- Converta subqueries em JOINs e compare performance

---

## Modulo 2: T-SQL Intermediario

### 2.1 Window Functions

**Exercicio 11 - ROW_NUMBER, RANK, DENSE_RANK**
- Numere os funcionarios por departamento ordenados por salario
- Encontre o funcionario mais bem pago de cada departamento usando ROW_NUMBER
- Compare os resultados de RANK vs DENSE_RANK com salarios iguais
- Encontre os TOP 3 salarios de cada departamento

**Exercicio 12 - Funcoes de agregacao como Window Functions**
- Calcule o salario acumulado (running total) por departamento ordenado por data de admissao
- Calcule a media movel dos ultimos 3 funcionarios admitidos
- Compare o salario de cada funcionario com a media do departamento na mesma query
- Calcule a porcentagem que cada salario representa do total do departamento

**Exercicio 13 - LAG, LEAD, FIRST_VALUE, LAST_VALUE**
- Compare o salario de cada funcionario com o anterior e o proximo (por data de admissao)
- Calcule a diferenca de orcamento entre projetos consecutivos
- Encontre o primeiro e ultimo funcionario admitido em cada departamento
- Use NTILE para dividir funcionarios em quartis de salario

---

### 2.2 CTEs e Queries Recursivas

**Exercicio 14 - Common Table Expressions**
- Reescreva as subqueries do Exercicio 10 usando CTEs
- Use CTE para calcular metricas intermediarias e depois filtrar
- Use multiplas CTEs encadeadas para relatorios complexos

**Exercicio 15 - CTEs Recursivas**
Crie uma tabela `Funcionarios_Hierarquia` com coluna `GerenteId` (self-reference).
- Monte a arvore hierarquica completa da empresa
- Calcule o nivel de cada funcionario na hierarquia
- Encontre todos os subordinados (diretos e indiretos) de um gerente
- Calcule o custo total (soma de salarios) da equipe completa de cada gerente

---

### 2.3 PIVOT, UNPIVOT e Queries Dinamicas

**Exercicio 16 - PIVOT**
- Crie um relatorio com departamentos nas linhas e meses nas colunas mostrando total de admissoes
- Faca PIVOT de status de projetos por departamento
- Use PIVOT dinamico quando nao souber as colunas antecipadamente

**Exercicio 17 - UNPIVOT**
- Transforme colunas de metricas em linhas para facilitar analises
- Combine UNPIVOT com agregacoes

---

### 2.4 Manipulacao de Dados Complexa

**Exercicio 18 - Strings**
- Extraia o primeiro nome e sobrenome dos funcionarios
- Concatene enderecos de multiplas colunas usando CONCAT e STRING_AGG
- Use CHARINDEX, SUBSTRING, REPLACE, STUFF para manipular textos
- Valide formatos de email e telefone usando PATINDEX e LIKE
- Trabalhe com STRING_SPLIT para desnormalizar dados

**Exercicio 19 - Datas**
- Calcule a idade de cada funcionario
- Calcule o tempo de empresa em anos, meses e dias
- Encontre funcionarios que fazem aniversario este mes
- Calcule dias uteis entre duas datas (excluindo fins de semana)
- Use DATEADD, DATEDIFF, DATEPART, EOMONTH, FORMAT
- Gere uma tabela calendario (calendar table) com CTE recursiva

**Exercicio 20 - JSON e XML**
- Armazene dados flexiveis em colunas JSON
- Use JSON_VALUE, JSON_QUERY, OPENJSON para extrair dados
- Converta resultado de queries para JSON com FOR JSON PATH
- Faca o mesmo com XML usando FOR XML PATH
- Use CROSS APPLY com OPENJSON para desnormalizar JSON

---

## Modulo 3: T-SQL Avancado

### 3.1 Stored Procedures

**Exercicio 21 - SPs basicas**
- Crie SP para CRUD completo de Funcionarios com validacoes
- Crie SP que receba parametros opcionais para filtros dinamicos
- Crie SP com parametros OUTPUT para retornar valores
- Use TRY...CATCH para tratamento de erros
- Use THROW e RAISERROR adequadamente
- Retorne codigos de erro com RETURN

**Exercicio 22 - SPs avancadas**
- Crie SP que gere relatorio mensal com totalizadores
- Crie SP com paginacao usando OFFSET/FETCH
- Crie SP que use tabela temporaria interna para processamento intermediario
- Crie SP com SQL dinamico usando sp_executesql com parametros tipados
- Crie SP que faca processamento em lote (batch) com controle de transacao

**Exercicio 23 - Transacoes**
- Implemente transferencia de funcionario entre departamentos como transacao
- Use SAVE TRANSACTION para savepoints
- Implemente padrao de retry para deadlocks
- Demonstre niveis de isolamento: READ UNCOMMITTED, READ COMMITTED, REPEATABLE READ, SERIALIZABLE, SNAPSHOT
- Simule e resolva um deadlock

---

### 3.2 Functions e Views

**Exercicio 24 - Funcoes**
- Crie Scalar Function para calcular imposto sobre salario com faixas
- Crie Table-Valued Function (inline) para retornar funcionarios por departamento com filtros
- Crie Multi-Statement TVF para calculo complexo
- Entenda quando usar Function vs SP (e por que inline TVF e preferivel)

**Exercicio 25 - Views**
- Crie View para consolidar dados de funcionarios + departamentos + projetos
- Crie Indexed View (View materializada) para relatorio pesado
- Use WITH SCHEMABINDING e entenda as restricoes
- Crie View com INSTEAD OF trigger para permitir INSERT/UPDATE

---

### 3.3 Triggers

**Exercicio 26 - Triggers DML**
- Crie trigger AFTER INSERT para log de auditoria
- Crie trigger AFTER UPDATE para historico de mudancas de salario
- Crie trigger INSTEAD OF DELETE para soft delete
- Crie trigger que valide regras de negocio complexas
- Entenda as tabelas INSERTED e DELETED

**Exercicio 27 - Triggers DDL e Logon**
- Crie trigger DDL para impedir DROP TABLE em producao
- Crie trigger DDL para auditar mudancas de schema
- Crie trigger de Logon para controle de acesso

---

### 3.4 Performance e Indices

**Exercicio 28 - Indices**
- Crie indice clustered vs nonclustered e compare performance
- Crie indice com INCLUDE para covering queries
- Crie indice filtrado para queries especificas
- Analise fragmentacao com sys.dm_db_index_physical_stats
- Faca rebuild e reorganize de indices

**Exercicio 29 - Planos de Execucao**
- Ative o plano de execucao e analise queries simples e complexas
- Identifique table scans, index scans, index seeks
- Identifique key lookups e elimine-os com covering index
- Identifique sort operators desnecessarios
- Use SET STATISTICS IO e TIME para medir performance
- Compare estimated vs actual execution plans

**Exercicio 30 - Otimizacao**
- Reescreva queries com cursor para set-based operations
- Otimize queries com parameter sniffing usando OPTION(RECOMPILE) ou OPTIMIZE FOR
- Use tabelas temporarias vs variaveis de tabela e entenda a diferenca
- Identifique e resolva implicit conversions
- Use Query Store para monitorar regressoes de performance

---

### 3.5 Recursos Avancados

**Exercicio 31 - Tabelas temporais (System-Versioned)**
- Crie tabela temporal para Funcionarios
- Consulte dados historicos com FOR SYSTEM_TIME
- Consulte estado da tabela em um ponto especifico no tempo
- Restaure dados excluidos acidentalmente usando a tabela de historico

**Exercicio 32 - Particionamento**
- Crie partition function e partition scheme
- Particione a tabela de Projetos por ano
- Faca switch de particoes para carga rapida
- Consulte com eliminacao de particoes

**Exercicio 33 - Seguranca**
- Implemente Row-Level Security para multi-tenancy
- Use Dynamic Data Masking para dados sensiveis
- Use Always Encrypted para colunas criticas
- Configure Transparent Data Encryption (TDE)

---

# PARTE 2 - .NET (C#)

---

## Modulo 4: Fundamentos C#

### 4.1 Tipos e Estruturas Basicas

**Exercicio 34 - Tipos de dados**
Crie um projeto console `FundamentosCSharp` e implemente:
- Demonstre a diferenca entre value types e reference types
- Use todos os tipos numericos (int, long, decimal, double, float) e entenda quando usar cada um
- Trabalhe com strings: interpolacao, verbatim, raw strings (.NET 7+)
- Use nullable types (int?, string?) e null-coalescing operators (??, ??=, ?., ?[])
- Demonstre boxing/unboxing e o impacto na performance
- Trabalhe com Span<T> e Memory<T> para manipulacao eficiente de dados

**Exercicio 35 - Colecoes**
- Implemente exemplos com List<T>, Dictionary<TKey,TValue>, HashSet<T>, Queue<T>, Stack<T>
- Use SortedList, SortedDictionary, SortedSet e entenda a diferenca
- Implemente IEnumerable<T> e IEnumerator<T> manualmente em uma colecao custom
- Use ConcurrentDictionary, ConcurrentQueue para cenarios thread-safe
- Compare performance de List vs LinkedList vs Array para diferentes operacoes
- Trabalhe com ReadOnlyCollection, ImmutableList, FrozenDictionary (.NET 8+)

**Exercicio 36 - Structs, Enums, Records**
- Crie struct `Dinheiro` com moeda e valor, implemente operadores (+, -, ==)
- Crie enum `StatusPedido` com atributos e metodos de extensao
- Crie record `Endereco` e demonstre imutabilidade e with-expressions
- Compare class vs struct vs record para diferentes cenarios
- Use record struct e readonly struct

---

### 4.2 OOP Profunda

**Exercicio 37 - Heranca e Polimorfismo**
Crie um sistema de calculo de impostos:
- Classe abstrata `Imposto` com metodo abstrato `Calcular(decimal valorBase)`
- Classes concretas: `ICMS`, `ISS`, `IPI`, cada uma com regras proprias
- Use `sealed` para impedir heranca em classes finais
- Demonstre polimorfismo chamando `Calcular` em uma lista de `Imposto`
- Use `virtual` e `override` vs `new` e entenda a diferenca

**Exercicio 38 - Interfaces e composicao**
Crie um sistema de notificacoes:
- Interface `INotificador` com metodo `Enviar(Mensagem msg)`
- Implementacoes: `EmailNotificador`, `SmsNotificador`, `PushNotificador`
- Interface `ITemplateEngine` para formatar mensagens
- Use composicao ao inves de heranca: `ServicoNotificacao` recebe `INotificador[]`
- Implemente `IDisposable` corretamente com padrao Dispose
- Use interface com default implementation (C# 8+)

**Exercicio 39 - Generics**
- Crie `Resultado<T>` que encapsula sucesso/erro (Result Pattern)
- Crie `Repositorio<T>` generico com constraints `where T : class, IEntidade`
- Crie metodo generico com multiplas constraints
- Entenda covariancia (out) e contravariancia (in) em interfaces genericas
- Implemente um `Pipeline<TInput, TOutput>` generico

---

### 4.3 Recursos Modernos do C#

**Exercicio 40 - Pattern Matching**
- Use switch expressions com patterns para calcular descontos por tipo de cliente
- Use property patterns para validar objetos complexos
- Use list patterns (C# 11) para processar arrays
- Use relational e logical patterns (and, or, not)
- Aplique pattern matching com tipos (is, as, switch)

**Exercicio 41 - LINQ Profundo**
Dado uma lista de objetos complexos (Pedidos com Items, Clientes, etc.):
- Use Select, Where, OrderBy, GroupBy, Join
- Use SelectMany para achatar colecoes aninhadas
- Use Aggregate para reducoes customizadas
- Use Zip, Chunk, DistinctBy
- Escreva a mesma query em method syntax e query syntax
- Implemente um IQueryable provider basico para entender como EF Core funciona por baixo
- Compare performance de LINQ vs loops tradicionais com BenchmarkDotNet

**Exercicio 42 - Delegates, Events e Expressoes Lambda**
- Crie sistema de eventos para processamento de pedidos
- Use Action<T>, Func<T,TResult>, Predicate<T>
- Implemente o padrao Observer usando events
- Crie extension methods uteis para colecoes
- Use closures e entenda capturing de variaveis
- Trabalhe com Expression<Func<T, bool>> e entenda a diferenca para Func

---

### 4.4 Async/Await e Concorrencia

**Exercicio 43 - Programacao assincrona basica**
- Converta operacoes sincronas para async (leitura de arquivo, chamada HTTP)
- Use Task.WhenAll para paralelizar chamadas independentes
- Use Task.WhenAny para timeout pattern
- Implemente cancellation com CancellationToken
- Demonstre os problemas de async void
- Entenda ConfigureAwait(false) e quando usar

**Exercicio 44 - Concorrencia**
- Use SemaphoreSlim para limitar concorrencia
- Implemente producer/consumer com Channel<T>
- Use Parallel.ForEachAsync para processamento paralelo
- Demonstre race conditions e como resolver com lock, SemaphoreSlim
- Use Interlocked para operacoes atomicas
- Implemente circuit breaker pattern com Polly

**Exercicio 45 - IAsyncEnumerable**
- Crie um gerador assincrono de dados
- Consuma IAsyncEnumerable com await foreach
- Implemente paginacao assincrona de API
- Use System.Threading.Channels para streaming de dados

---

## Modulo 5: ASP.NET Core - Web APIs

### 5.1 APIs REST

**Exercicio 46 - API basica**
Crie uma Web API `EmpresaApi` com:
- Controllers para CRUD de Departamentos e Funcionarios
- Use DTOs separados para Request e Response
- Implemente validacao com Data Annotations e FluentValidation
- Configure Swagger/OpenAPI
- Use ActionResult<T> e retorne status codes corretos (200, 201, 204, 400, 404, 409, 422)
- Implemente versionamento de API

**Exercicio 47 - Minimal APIs**
Recrie a API do exercicio anterior usando Minimal APIs:
- Use MapGet, MapPost, MapPut, MapDelete
- Use TypedResults para retornos tipados
- Organize com Route Groups
- Use Filters para validacao e logging
- Compare a abordagem com Controllers

**Exercicio 48 - Middleware e Pipeline**
- Crie middleware de logging de requisicoes (metodo, path, tempo de resposta)
- Crie middleware de tratamento global de excecoes
- Crie middleware de correlacao (correlation ID)
- Entenda a ordem do pipeline e como ela afeta o comportamento
- Use IMiddleware para middleware com DI
- Implemente rate limiting com middleware

---

### 5.2 Dependency Injection

**Exercicio 49 - DI Fundamentos**
- Registre servicos como Transient, Scoped, Singleton e demonstre a diferenca
- Use IOptions<T>, IOptionsSnapshot<T>, IOptionsMonitor<T> para configuracao
- Implemente factory pattern com DI
- Use Keyed Services (.NET 8+) para multiplas implementacoes
- Registre decorators manualmente no container
- Valide a configuracao na inicializacao com ValidateOnStart

**Exercicio 50 - DI Avancado**
- Implemente o padrao Strategy com DI (selecionar implementacao em runtime)
- Use IServiceScopeFactory para criar escopos manuais em Singletons
- Implemente health checks customizados
- Crie hosted services (IHostedService) para background processing
- Use IHttpClientFactory com named e typed clients

---

### 5.3 Autenticacao e Autorizacao

**Exercicio 51 - JWT Authentication**
- Configure autenticacao JWT Bearer
- Crie endpoint de login que gera token com claims
- Use refresh tokens com rotacao
- Implemente [Authorize] com policies e roles
- Crie policy-based authorization customizada
- Implemente claims transformation

**Exercicio 52 - Identity**
- Configure ASP.NET Core Identity com Entity Framework
- Implemente registro, login, confirmacao de email
- Implemente recuperacao de senha
- Configure 2FA (Two-Factor Authentication)
- Customize o IdentityUser com campos adicionais

---

## Modulo 6: Entity Framework Core

### 6.1 Fundamentos EF Core

**Exercicio 53 - Code First**
- Crie DbContext para o banco Empresa
- Configure entidades com Fluent API (evite Data Annotations para config de banco)
- Configure relacionamentos 1:N, N:N, 1:1
- Use value objects com Owned Types
- Configure conversoes de tipo com ValueConverter
- Use TPH, TPT, TPC para hierarquia de heranca e entenda trade-offs

**Exercicio 54 - Migrations**
- Gere migration inicial
- Adicione novas propriedades e gere migration incremental
- Faca seed de dados com HasData
- Crie migration customizada com SQL puro
- Faca rollback de migration
- Entenda quando usar migracoes vs scripts SQL manuais

**Exercicio 55 - Queries**
- Faca queries com Include/ThenInclude (eager loading)
- Use explicit loading com Entry().Collection().Load()
- Configure lazy loading e entenda os problemas (N+1)
- Use Select para projecoes (carregue apenas o necessario)
- Use AsNoTracking para queries read-only
- Filtre includes com filtros globais (HasQueryFilter)
- Use Split Queries para evitar explosao cartesiana
- Use raw SQL com FromSqlInterpolated quando necessario

**Exercicio 56 - Operacoes de escrita**
- Implemente CRUD completo com SaveChanges
- Use ExecuteUpdate e ExecuteDelete (.NET 7+) para bulk operations
- Implemente otimistic concurrency com RowVersion/ConcurrencyToken
- Use transacoes explicitas com BeginTransaction
- Implemente Unit of Work pattern sobre o DbContext
- Use interceptors para auditoria automatica (CreatedAt, UpdatedAt)

---

### 6.2 EF Core Avancado

**Exercicio 57 - Performance**
- Use o EF Core logging para ver queries geradas
- Identifique e resolva N+1 queries
- Use compiled queries para queries frequentes
- Configure connection resiliency para SQL Server
- Use DbContext pooling
- Compare performance de tracking vs no-tracking
- Faca bulk insert eficiente (EFCore.BulkExtensions ou manual com SqlBulkCopy)

**Exercicio 58 - Patterns com EF Core**
- Implemente Repository Pattern (e discuta se faz sentido sobre EF Core)
- Implemente Specification Pattern para queries reutilizaveis
- Implemente soft delete global com query filters
- Use Domain Events com EF Core (dispatch antes/depois de SaveChanges)
- Implemente multi-tenancy com query filters

---

## Modulo 7: Arquitetura e Patterns

### 7.1 Clean Architecture

**Exercicio 59 - Estrutura do projeto**
Crie uma solution com a seguinte estrutura:
```
Empresa.Domain        -> Entidades, Value Objects, Interfaces de Repository, Domain Events
Empresa.Application   -> Use Cases/Services, DTOs, Interfaces, Validators
Empresa.Infrastructure -> EF Core, Repositories, External Services
Empresa.API           -> Controllers, Middleware, DI Configuration
Empresa.Tests         -> Unit Tests, Integration Tests
```
- Implemente as regras de dependencia (Domain nao referencia nada externo)
- Use MediatR para CQRS (Commands e Queries separados)
- Implemente Notification pattern para retorno de erros de dominio

**Exercicio 60 - Domain-Driven Design basico**
- Crie Entities com logica de negocio encapsulada (nao anemic models)
- Crie Value Objects (CPF, Email, Dinheiro) com validacao no construtor
- Implemente Aggregates com Aggregate Root
- Crie Domain Events e Handlers
- Implemente Domain Services para logica que nao pertence a uma entidade
- Use Specifications para regras de negocio reutilizaveis

---

### 7.2 Design Patterns na Pratica

**Exercicio 61 - Patterns Comportamentais**
Implemente no contexto do sistema Empresa:
- **Strategy**: Diferentes algoritmos de calculo de bonus por cargo
- **Chain of Responsibility**: Pipeline de validacao de pedidos
- **Observer/Mediator**: Comunicacao entre modulos desacoplados
- **State**: Maquina de estados para workflow de aprovacao

**Exercicio 62 - Patterns Criacionais e Estruturais**
- **Factory Method**: Criacao de diferentes tipos de relatorio
- **Builder**: Construcao de queries de relatorio complexas
- **Decorator**: Adicionar cache, logging, retry em servicos
- **Adapter**: Integrar com APIs externas com contratos diferentes
- **Proxy**: Lazy loading e controle de acesso

---

## Modulo 8: Testes

### 8.1 Testes Unitarios

**Exercicio 63 - xUnit fundamentals**
- Configure projeto de testes com xUnit
- Use [Fact] e [Theory] com [InlineData], [MemberData], [ClassData]
- Use FluentAssertions para asserts legiveis
- Organize testes com padrao AAA (Arrange, Act, Assert)
- Teste excecoes esperadas
- Use ITestOutputHelper para logging em testes

**Exercicio 64 - Mocking**
- Use Moq ou NSubstitute para mockar dependencias
- Mocke IRepository para testar Services isoladamente
- Verifique chamadas de metodo com Verify
- Use Setup com Returns e Callback
- Mocke HttpClient com MockHttpMessageHandler
- Entenda quando usar mock vs stub vs fake

**Exercicio 65 - Testes de Domain**
- Teste Value Objects (igualdade, validacao, imutabilidade)
- Teste Entities (regras de negocio, estado, Domain Events)
- Teste Domain Services
- Alcance 90%+ de cobertura no Domain layer

---

### 8.2 Testes de Integracao

**Exercicio 66 - Testes de API**
- Use WebApplicationFactory<Program> para testes de integracao
- Configure banco de dados in-memory ou Testcontainers com SQL Server
- Teste endpoints completos (request -> response)
- Teste autenticacao e autorizacao
- Use Respawn para limpar banco entre testes
- Teste middleware customizado

**Exercicio 67 - Testes de repositorio**
- Teste queries EF Core contra banco real (Testcontainers)
- Teste migracao (sobe banco do zero e verifica schema)
- Teste concorrencia otimista
- Teste transacoes e rollback

---

## Modulo 9: Integracao .NET + T-SQL

### 9.1 Dapper

**Exercicio 68 - Dapper basico**
- Configure Dapper no projeto
- Faca queries simples com Query<T> e QueryAsync<T>
- Use parametros para prevenir SQL injection
- Mapeie resultados para DTOs
- Use multi-mapping para JOINs (splitOn)
- Compare performance Dapper vs EF Core para as mesmas queries

**Exercicio 69 - Dapper avancado**
- Execute Stored Procedures com Dapper
- Use QueryMultiple para multiplos result sets
- Use DynamicParameters para parametros dinamicos
- Implemente paginacao com Dapper
- Use Dapper para bulk operations com SqlBulkCopy
- Combine Dapper (queries) e EF Core (escrita) no mesmo projeto

---

### 9.2 CQRS com SQL Server

**Exercicio 70 - Separacao de leitura e escrita**
- Use EF Core para Commands (escrita) com todo o tracking
- Use Dapper para Queries (leitura) com queries SQL otimizadas
- Crie Views no SQL Server otimizadas para leitura
- Use DTOs especificos para cada query
- Implemente com MediatR (IRequest<T> para Commands, IRequest<T> para Queries)

---

## Modulo 10: Topicos Avancados

### 10.1 Caching

**Exercicio 71 - Estrategias de cache**
- Implemente IMemoryCache para cache local
- Configure Redis com IDistributedCache
- Implemente cache-aside pattern
- Implemente cache invalidation por tags
- Use Output Caching (.NET 7+) para cache de respostas HTTP
- Implemente Hybrid Cache (.NET 9+)

### 10.2 Mensageria

**Exercicio 72 - Background processing**
- Use BackgroundService para processar filas
- Implemente outbox pattern para consistencia
- Use MassTransit ou Wolverine com RabbitMQ
- Implemente saga pattern para transacoes distribuidas
- Implemente retry e dead letter queue

### 10.3 Observabilidade

**Exercicio 73 - Logging e Monitoramento**
- Configure Serilog com sinks (Console, File, Seq)
- Use structured logging com templates
- Implemente distributed tracing com OpenTelemetry
- Crie metricas customizadas com System.Diagnostics.Metrics
- Configure health checks detalhados
- Use correlation IDs end-to-end

### 10.4 Docker e Deploy

**Exercicio 74 - Containerizacao**
- Crie Dockerfile multi-stage para a API
- Crie docker-compose com API + SQL Server + Redis
- Configure environment variables e secrets
- Implemente CI/CD basico com GitHub Actions
- Faca migration automatica no startup (com cuidado para producao)

---

# PARTE 3 - PROJETOS INTEGRADORES

---

## Projeto 1: Sistema de Gestao de Funcionarios (Basico)

**Objetivo**: Consolidar fundamentos de CRUD, SQL, e API

Requisitos:
- API REST completa para gerenciar Departamentos, Funcionarios, Projetos
- Banco SQL Server com Stored Procedures para relatorios
- Validacao completa de entrada
- Paginacao, ordenacao e filtros
- Testes unitarios e de integracao
- Documentacao com Swagger

Entregaveis:
1. Script SQL com DDL, seeds, SPs e views
2. Projeto .NET com Clean Architecture
3. Collection do Postman/Bruno com todos os endpoints
4. Projeto de testes com 80%+ cobertura

---

## Projeto 2: Sistema de Pedidos E-Commerce (Intermediario)

**Objetivo**: Praticar modelagem complexa, performance e patterns

Requisitos:
- Entidades: Clientes, Produtos, Categorias, Pedidos, ItensPedido, Pagamentos, Estoque
- Carrinho de compras com controle de concorrencia
- Workflow de pedido (Rascunho -> Confirmado -> Pago -> Enviado -> Entregue -> Cancelado)
- Relatorios SQL complexos (vendas por periodo, produtos mais vendidos, clientes VIP)
- Cache em Redis para catalogo de produtos
- Background job para processar pagamentos
- Event sourcing basico para historico de pedidos

Entregaveis:
1. Modelagem completa do banco com indices otimizados
2. API com CQRS (EF Core + Dapper)
3. Testes de integracao com Testcontainers
4. Docker Compose funcional
5. Documentacao de decisoes arquiteturais (ADRs)

---

## Projeto 3: Sistema Multi-Tenant SaaS (Avancado)

**Objetivo**: Dominar arquitetura profissional de nivel senior

Requisitos:
- Multi-tenancy com banco compartilhado (Row-Level Security no SQL Server)
- Autenticacao com JWT + Refresh Token + 2FA
- Autorizacao granular com policies e claims
- API versionada com contratos estabilizados
- Mensageria para operacoes assincronas
- Auditoria completa com tabelas temporais
- Observabilidade com OpenTelemetry
- Rate limiting por tenant
- Health checks detalhados
- CI/CD com GitHub Actions

Entregaveis:
1. Solution completa com Clean Architecture + DDD
2. Banco SQL Server com particionamento, RLS, TDE
3. Cobertura de testes > 85%
4. Pipeline CI/CD funcional
5. Runbook de operacoes
6. Documentacao completa (ADRs, diagramas C4)

---

# APENDICE - Checklist de Competencias

## T-SQL
- [ ] DDL completo (CREATE, ALTER, DROP, constraints, indices)
- [ ] DML fluente (INSERT, UPDATE, DELETE, MERGE)
- [ ] SELECT avancado (JOINs, subqueries, CTEs, Window Functions)
- [ ] Stored Procedures com error handling
- [ ] Functions (Scalar, Inline TVF, Multi-Statement TVF)
- [ ] Triggers (DML, DDL)
- [ ] Views e Indexed Views
- [ ] Transacoes e niveis de isolamento
- [ ] Indices e otimizacao de queries
- [ ] Execution Plans
- [ ] Tabelas temporais
- [ ] Particionamento
- [ ] Seguranca (RLS, DDM, TDE)
- [ ] JSON e XML
- [ ] Dynamic SQL seguro

## .NET / C#
- [ ] Tipos, colecoes, generics profundo
- [ ] OOP e SOLID
- [ ] Async/Await e concorrencia
- [ ] LINQ avancado
- [ ] Pattern Matching
- [ ] ASP.NET Core (Controllers + Minimal APIs)
- [ ] Middleware pipeline
- [ ] Dependency Injection avancada
- [ ] Autenticacao JWT + Identity
- [ ] EF Core (queries, migrations, performance)
- [ ] Dapper
- [ ] Testes (xUnit, Moq, integracao)
- [ ] Clean Architecture / DDD
- [ ] Design Patterns na pratica
- [ ] Caching (Memory, Redis)
- [ ] Mensageria
- [ ] Observabilidade
- [ ] Docker e CI/CD

---

> **Dica final**: Nao tente fazer tudo de uma vez. Faca um modulo por semana.
> Escreva codigo REAL. Nao apenas leia. A pratica e o que fixa o conhecimento.
> Quando travar em algum exercicio, pesquise, tente resolver sozinho antes de pedir ajuda.
> Revise codigo antigo depois de aprender conceitos novos - voce vai ver o quanto evoluiu.
