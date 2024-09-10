namespace StudentAffairs.Server.Validations;

public class AddProfessorValidator : AbstractValidator<AddProfessorDto>
{
    public AddProfessorValidator(IStringLocalizer<Resource> localizer)
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(localizer["Full Name Validation"])
            .Length(1, 100).WithMessage(localizer["Full Name Length Validation"]);

        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage(localizer["Mobile Validation"])
            .MaximumLength(15).WithMessage(localizer["Mobile Length Validation"])
            .MinimumLength(5).WithMessage(localizer["Mobile Length Validation"]);

        RuleFor(professor => professor.DepartmentId)
            .GreaterThan(0).WithMessage(localizer["Department Validation"]);
    }
}
