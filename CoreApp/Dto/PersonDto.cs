using CoreApp.Entities;
using CoreApp.Dto;
using CoreApp.Enums;

public record PersonDto : ContactBaseDto
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? Position { get; init; }
    public DateTime? BirthDate { get; init; }
    public Gender Gender { get; init; }
    public Guid? EmployerId { get; init; }
    
    public static PersonDto FromEntity(Person p)
    {
        return new PersonDto
        {
            Id = p.Id,
            Email = p.Mail,
            Phone = p.Phone,
            Status = p.Status,
            CreatedAt = p.CreatedAt,

            FirstName = p.FirstName,
            LastName = p.LastName,
            Position = p.Position,
            BirthDate = p.BirthDate,
            Gender = p.Gender,
            EmployerId = p.Employer?.Id,

            Address = p.Address == null ? null : new AddressDto(
                p.Address.Street,
                p.Address.City,
                p.Address.PostalCode,
                p.Address.Country,
                p.Address.Type
            ),

            Tags = p.Tags.Select(t => t.Name).ToList()
        };
    }
    
    public static Person ToEntity(CreatePersonDto dto)
    {
        return new Person
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Mail = dto.Email,
            Phone = dto.Phone,
            Position = dto.Position,
            BirthDate = dto.BirthDate,
            Gender = dto.Gender,
            CreatedAt = DateTime.UtcNow,
            Status = ContactStatus.Active,

            Address = dto.Address == null ? null : new Address
            {
                Street = dto.Address.Street,
                City = dto.Address.City,
                PostalCode = dto.Address.PostalCode,
                Country = dto.Address.Country,
                Type = dto.Address.Type
            }
        };
    }
}