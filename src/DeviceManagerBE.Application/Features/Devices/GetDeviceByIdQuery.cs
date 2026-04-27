using DeviceManagerBE.Application.DTOs.Device;
using MediatR;

namespace DeviceManagerBE.Application.Features.Devices;

public sealed record GetDeviceByIdQuery(int DeviceId) : IRequest<DeviceDetailDto?>;
