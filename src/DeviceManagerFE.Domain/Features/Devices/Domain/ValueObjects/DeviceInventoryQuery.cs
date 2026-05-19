namespace DeviceManagerFE.Features.Devices.Domain.ValueObjects;

public sealed record DeviceInventoryQuery(
    string? SearchTerm,
    string? CategoryId,
    string? Status,
    int PageNumber,
    int PageSize)
{
    public static DeviceInventoryQuery Default => new(
        SearchTerm: null,
        CategoryId: string.Empty,
        Status: string.Empty,
        PageNumber: 1,
        PageSize: 8);
}

