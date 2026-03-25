using CoreApp.Validators.Shared;
using CoreApp.Dto;
using CoreApp.Repositories;
using FluentValidation;

namespace CoreApp.Validators;

public class UpdatePersonDtoValidator : AbstractValidator<UpdatePersonDto>
{
    private readonly ICompanyRepositoryAsync _companyRepository;

    public UpdatePersonDtoValidator(ICompanyRepositoryAsync companyRepository)
    {
        _companyRepository = companyRepository;

        RuleFor(x => x.FirstName)
            .MaximumLength(100)
            .Matches(@"^[\p{L}\s\-]+$")
            .When(x => x.FirstName != null);

        RuleFor(x => x.LastName)
            .MaximumLength(200)
            .Matches(@"^[\p{L}\s\-]+$")
            .When(x => x.LastName != null);

        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(200)
            .When(x => x.Email != null);

        RuleFor(x => x.Phone)
            .Matches(@"^\+?[0-9]{7,15}$")
            .When(x => x.Phone != null);

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Today.AddYears(-18))
            .GreaterThan(DateTime.Today.AddYears(-120))
            .When(x => x.BirthDate.HasValue);

        RuleFor(x => x.Gender)
            .IsInEnum()
            .When(x => x.Gender.HasValue);

        RuleFor(x => x.EmployerId)
            .MustAsync(EmployerExistsAsync)
            .When(x => x.EmployerId.HasValue);

        RuleFor(x => x.Address)
            .SetValidator(new AddressDtoValidator())
            .When(x => x.Address != null);
    }

    private async Task<bool> EmployerExistsAsync(Guid? employerId, CancellationToken ct)
    {
        var company = await _companyRepository.FindByIdAsync(employerId!.Value);
        return company != null;
    }
}