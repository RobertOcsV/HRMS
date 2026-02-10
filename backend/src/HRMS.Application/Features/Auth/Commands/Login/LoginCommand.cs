namespace HRMS.Application.Features.Auth.Commands.Login;

using MediatR;

public record LoginCommand(string Email, string Senha) : IRequest<LoginResponse>;

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn,
    UsuarioResponse Usuario
);

public record UsuarioResponse(
    Guid Id,
    string Nome,
    string Email,
    string Role
);
