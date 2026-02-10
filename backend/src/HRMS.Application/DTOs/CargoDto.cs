namespace HRMS.Application.DTOs;

public record CargoDto(
    Guid Id,
    string Nome,
    string? Descricao,
    int Nivel,
    bool Ativo,
    DateTime CriadoEm,
    DateTime? AtualizadoEm
);
