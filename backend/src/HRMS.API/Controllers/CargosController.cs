namespace HRMS.API.Controllers;

using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class CargosController : ApiControllerBase
{
    private readonly IRepository<Cargo> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CargosController(
        IRepository<Cargo> repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken)
    {
        var cargos = await _repository.ObterTodosAsync(cancellationToken);
        return Ok(cargos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var cargo = await _repository.ObterPorIdAsync(id, cancellationToken);

        if (cargo is null)
            return NotFound();

        return Ok(cargo);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(
        [FromBody] CriarCargoRequest request,
        CancellationToken cancellationToken)
    {
        var cargo = Cargo.Criar(request.Nome, request.Nivel, request.Descricao);

        await _repository.AdicionarAsync(cargo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = cargo.Id },
            cargo);
    }
}

public record CriarCargoRequest(string Nome, int Nivel, string? Descricao);
