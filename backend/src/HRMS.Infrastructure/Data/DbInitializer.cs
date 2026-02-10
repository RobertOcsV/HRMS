namespace HRMS.Infrastructure.Data;

using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HrmsDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<HrmsDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            await SeedDepartamentosAsync(context);
            await SeedCargosAsync(context);
            await SeedUsuarioAdminAsync(context, passwordHasher);

            logger.LogInformation("Database inicializado com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao inicializar o database.");
            throw;
        }
    }

    private static async Task SeedDepartamentosAsync(HrmsDbContext context)
    {
        if (await context.Departamentos.AnyAsync())
            return;

        var departamentos = new[]
        {
            Departamento.Criar("Tecnologia da Informacao", "Departamento de TI"),
            Departamento.Criar("Recursos Humanos", "Departamento de RH"),
            Departamento.Criar("Financeiro", "Departamento Financeiro")
        };

        await context.Departamentos.AddRangeAsync(departamentos);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCargosAsync(HrmsDbContext context)
    {
        if (await context.Cargos.AnyAsync())
            return;

        var cargos = new[]
        {
            Cargo.Criar("Estagiario", 1, "Profissional em formacao"),
            Cargo.Criar("Desenvolvedor Junior", 2, "Desenvolvedor iniciante"),
            Cargo.Criar("Desenvolvedor Pleno", 4, "Desenvolvedor com experiencia"),
            Cargo.Criar("Desenvolvedor Senior", 6, "Desenvolvedor experiente"),
            Cargo.Criar("Tech Lead", 7, "Lider tecnico"),
            Cargo.Criar("Analista de RH", 4, "Analista de Recursos Humanos"),
            Cargo.Criar("Gerente", 8, "Gerente de departamento"),
            Cargo.Criar("Diretor", 10, "Diretor de area")
        };

        await context.Cargos.AddRangeAsync(cargos);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUsuarioAdminAsync(HrmsDbContext context, IPasswordHasher passwordHasher)
    {
        if (await context.Usuarios.AnyAsync(u => u.Email == "admin@hrms.com"))
            return;

        var senhaHash = passwordHasher.Hash("Admin@123");
        var admin = Usuario.Criar("admin@hrms.com", senhaHash, Role.Admin);

        await context.Usuarios.AddAsync(admin);
        await context.SaveChangesAsync();
    }
}
