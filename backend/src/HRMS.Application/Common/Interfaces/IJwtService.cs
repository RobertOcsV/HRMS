namespace HRMS.Application.Common.Interfaces;

using HRMS.Domain.Entities;

public interface IJwtService
{
    string GerarAccessToken(Usuario usuario);
    string? ValidarToken(string token);
}
