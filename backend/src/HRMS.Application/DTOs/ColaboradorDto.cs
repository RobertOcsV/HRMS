namespace HRMS.Application.DTOs;

public record ColaboradorDto(
    Guid Id,
    string Nome,
    string CPF,
    string Email,
    DateTime DataNascimento,
    DateTime DataAdmissao,
    DateTime? DataDesligamento,
    bool Ativo,
    Guid CargoId,
    string CargoNome,
    Guid DepartamentoId,
    string DepartamentoNome,
    Guid? GestorId,
    string? GestorNome,
    DateTime CriadoEm,
    DateTime? AtualizadoEm
);

public record ColaboradorResumoDto(
    Guid Id,
    string Nome,
    string Email,
    string CargoNome,
    string DepartamentoNome,
    bool Ativo
);
