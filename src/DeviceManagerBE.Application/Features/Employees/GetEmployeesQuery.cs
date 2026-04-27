using DeviceManagerBE.Application.DTOs.Employee;
using MediatR;

namespace DeviceManagerBE.Application.Features.Employees;

public sealed class GetEmployeesQuery : IRequest<GetEmployeesResultDto>
{
}
