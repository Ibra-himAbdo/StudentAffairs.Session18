namespace StudentAffairs.Server.Validations;

public class AddDepartmentValidator : AbstractValidator<AddDepartmentDto>
{
    public AddDepartmentValidator(IStringLocalizer<Resource> localizer)
    {
        RuleFor(department => department.Name)
            .NotEmpty().WithMessage(localizer["Name Validation"])
            .MaximumLength(100).WithMessage(localizer["Name Length Validation"]);
    }
}
