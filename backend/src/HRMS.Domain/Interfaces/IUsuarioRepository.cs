
using HRMS.Domain.Entities;

namespace HRMS.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorEmailComColaboradorAsync(string email, CancellationToken cancellationToken = default);
}
