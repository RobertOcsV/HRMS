using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;

namespace HRMS.Infrastructure.Data.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    public Task<Usuario?> ObterPorEmailComColaboradorAsync(string email, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
