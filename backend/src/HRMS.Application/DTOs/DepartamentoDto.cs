namespace HRMS.Application.DTOs;

public record DepartamentoDto(
    Guid Id,
    string Nome,
    string? Descricao,
    bool Ativo,
    int TotalColaboradores,
    DateTime CriadoEm,
    DateTime? AtualizadoEm
);
