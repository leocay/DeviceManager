namespace DeviceManagerFE.Features.Auth.Application.DTOs;

public sealed record LoginRequestDto(string Username, string Password, bool RememberMe);

