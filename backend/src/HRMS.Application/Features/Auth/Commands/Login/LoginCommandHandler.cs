namespace HRMS.Application.Features.Auth.Commands.Login;

using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IRepository<Usuario> _usuarioRepository;
    private readonly IRepository<RefreshToken> _refreshTokenRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        IRepository<Usuario> usuarioRepository,
        IRepository<RefreshToken> refreshTokenRepository,
        IJwtService jwtService,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _usuarioRepository = usuarioRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository
            .Query()
            .Include(u => u.Colaborador)
            .FirstOrDefaultAsync(u => u.Email == request.Email.ToLowerInvariant(), cancellationToken);

        if (usuario is null)
            throw new UnauthorizedAccessException("Credenciais invalidas.");

        if (!usuario.Ativo)
            throw new UnauthorizedAccessException("Usuario inativo.");

        if (!_passwordHasher.Verify(request.Senha, usuario.SenhaHash))
            throw new UnauthorizedAccessException("Credenciais invalidas.");

        usuario.RegistrarLogin();

        var accessToken = _jwtService.GerarAccessToken(usuario);
        var refreshToken = RefreshToken.Criar(usuario.Id);

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponse(
            AccessToken: accessToken,
            RefreshToken: refreshToken.Token,
            ExpiresIn: 3600,
            Usuario: new UsuarioResponse(
                Id: usuario.Id,
                Nome: usuario.Colaborador?.Nome ?? "Administrador",
                Email: usuario.Email,
                Role: usuario.Role.ToString()
            )
        );
    }
}
