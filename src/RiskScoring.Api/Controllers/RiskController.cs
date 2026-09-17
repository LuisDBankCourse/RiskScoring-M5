using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RiskScoring.Application.Commands;

namespace RiskScoring.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RiskController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<EvaluateRiskCommand> _validator;
    private readonly ILogger<RiskController> _logger;

    public RiskController(IMediator mediator, IValidator<EvaluateRiskCommand> validator, ILogger<RiskController> logger)
    {
        _mediator = mediator;
        _validator = validator;
        _logger = logger;
    }

    [HttpPost("evaluate")]
    [ProducesResponseType(typeof(EvaluateRiskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCustomer([FromBody] EvaluateRiskCommand command, CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(command, ct);

        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g
                    .Select(x => x.ErrorMessage)
                    .ToArray());

            return ValidationProblem(new ValidationProblemDetails(errors)
            {
                Title = "Validación fallida",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var result = await _mediator.Send(command, ct);

        if (!result.IsSuccess)
        {
            return Problem(
                title: "Risk evaluation failed",
                detail: result.Error,
                statusCode: StatusCodes.Status409Conflict

            );
        }

        return Ok(result.Value);

    }
}