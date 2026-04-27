using Asp.Versioning;
using DeviceManagerBE.Application.Features.Employees;
using DeviceManagerBE.Contracts.Employee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagerBE.Controllers;

[ApiController]
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/employees")]
[Authorize]
public sealed class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetEmployeesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetEmployeesResponse>> GetEmployees(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetEmployeesQuery(), cancellationToken);

        return Ok(new GetEmployeesResponse
        {
            Employees = result.Employees
                .Select(employee => new EmployeeOptionDto
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeCode = employee.EmployeeCode,
                    FullName = employee.FullName,
                    Department = employee.Department,
                    DisplayName = string.IsNullOrWhiteSpace(employee.Department)
                        ? $"{employee.EmployeeCode} - {employee.FullName}"
                        : $"{employee.EmployeeCode} - {employee.FullName} ({employee.Department})"
                })
                .ToList()
        });
    }
}
