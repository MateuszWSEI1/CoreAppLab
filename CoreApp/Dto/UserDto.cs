using CoreApp.Enums;

namespace CoreApp.Dto;

public record UserDto
{
    public string Id { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public SystemUserStatus Status { get; init; }
    public string Department { get; init; } = string.Empty;
    public IList<string> Roles { get; init; } = new List<string>();
}