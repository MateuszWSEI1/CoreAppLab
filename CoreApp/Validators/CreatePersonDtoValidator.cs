using CoreApp.Validators.Shared;
using CoreApp.Dto;
using CoreApp.Repositories;
using FluentValidation;

namespace CoreApp.Validators;

public class CreatePersonDtoValidator : AbstractValidator<CreatePersonDto>
{
    private readonly ICompanyRepositoryAsync _companyRepository;

    public CreatePersonDtoValidator(ICompanyRepositoryAsync companyRepository)
    {
        _companyRepository = companyRepository;

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Imię jest wymagane.")
            .MaximumLength(100).WithMessage("Imię max 100 znaków.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Niedozwolone znaki.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane.")
            .MaximumLength(200).WithMessage("Nazwisko max 200 znaków.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Niedozwolone znaki.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email wymagany.")
            .EmailAddress().WithMessage("Niepoprawny email.")
            .MaximumLength(200);
        
        RuleFor(x => x.Phone)
            .Matches(@"^\+?[0-9]{7,15}$")
            .WithMessage("Nieprawidłowy numer telefonu.")
            .When(x => x.Phone != null);

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Today.AddYears(-18))
            .WithMessage("Min. 18 lat.")
            .GreaterThan(DateTime.Today.AddYears(-120))
            .WithMessage("Nieprawidłowa data.")
            .When(x => x.BirthDate.HasValue);

        RuleFor(x => x.Gender)
            .IsInEnum();

        RuleFor(x => x.EmployerId)
            .MustAsync(EmployerExistsAsync)
            .WithMessage("Firma nie istnieje.")
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