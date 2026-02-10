namespace HRMS.API.Controllers;

using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class DepartamentosController : ApiControllerBase
{
    private readonly IRepository<Departamento> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DepartamentosController(
        IRepository<Departamento> repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken)
    {
        var departamentos = await _repository.ObterTodosAsync(cancellationToken);
        return Ok(departamentos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var departamento = await _repository.ObterPorIdAsync(id, cancellationToken);

        if (departamento is null)
            return NotFound();

        return Ok(departamento);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(
        [FromBody] CriarDepartamentoRequest request,
        CancellationToken cancellationToken)
    {
        var departamento = Departamento.Criar(request.Nome, request.Descricao);

        await _repository.AdicionarAsync(departamento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = departamento.Id },
            departamento);
    }
}

public record CriarDepartamentoRequest(string Nome, string? Descricao);
