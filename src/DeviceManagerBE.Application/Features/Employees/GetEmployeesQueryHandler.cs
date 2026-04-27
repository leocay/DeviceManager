using DeviceManagerBE.Application.DTOs.Employee;
using DeviceManagerBE.Application.Services.Employee;
using MediatR;

namespace DeviceManagerBE.Application.Features.Employees;

public sealed class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, GetEmployeesResultDto>
{
    private readonly IEmployeeReadRepository _employeeReadRepository;

    public GetEmployeesQueryHandler(IEmployeeReadRepository employeeReadRepository)
    {
        _employeeReadRepository = employeeReadRepository;
    }

    public async Task<GetEmployeesResultDto> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _employeeReadRepository.GetEmployeesAsync(cancellationToken);
        return new GetEmployeesResultDto
        {
            Employees = employees
        };
    }
}
