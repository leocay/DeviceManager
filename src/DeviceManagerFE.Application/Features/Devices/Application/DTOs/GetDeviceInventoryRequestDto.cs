namespace DeviceManagerFE.Features.Devices.Application.DTOs;

public sealed record GetDeviceInventoryRequestDto(
    string? SearchTerm,
    string? CategoryId,
    string? Status,
    int PageNumber,
    int PageSize);

