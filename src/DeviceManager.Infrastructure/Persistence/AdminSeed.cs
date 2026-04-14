namespace DeviceManager.Infrastructure.Persistence;

internal static class AdminSeed
{
    public const int DefaultAdminId = 1;
    public const string DefaultUsername = "admin";
    public const string DefaultPasswordHash = "ad89b64d66caa8e30e5d5ce4a9763f4ecc205814c412175f3e2c50027471426d";
    public const string DefaultFullName = "System Administrator";
    public static readonly DateTime DefaultCreatedAt = new(2026, 4, 14, 0, 0, 0, DateTimeKind.Unspecified);
}