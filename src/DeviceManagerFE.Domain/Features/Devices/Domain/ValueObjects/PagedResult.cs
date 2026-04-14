namespace DeviceManagerFE.Features.Devices.Domain.ValueObjects;

public sealed class PagedResult<TItem>
{
    public IReadOnlyList<TItem> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
}

