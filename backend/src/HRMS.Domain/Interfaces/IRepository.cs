namespace HRMS.Domain.Interfaces;

using System.Linq.Expressions;
using HRMS.Domain.Common;

public interface IRepository<T> where T : Entity
{
    IQueryable<T> Query();
    Task<T?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<T?> ObterAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T> AdicionarAsync(T entity, CancellationToken cancellationToken = default);
    Task AtualizarAsync(T entity, CancellationToken cancellationToken = default);
    Task RemoverAsync(T entity, CancellationToken cancellationToken = default);
}
