namespace CoreApp.Dto;

public record UserDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
}