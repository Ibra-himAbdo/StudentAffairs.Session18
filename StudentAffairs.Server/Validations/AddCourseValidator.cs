namespace StudentAffairs.Server.Validations;

public class AddCourseValidator : AbstractValidator<AddCourseDto>
{
    public AddCourseValidator(IStringLocalizer<Resource> localizer)
    {
        RuleFor(course => course.Name)
        .NotEmpty().WithMessage(localizer["Name Validation"])
        .MaximumLength(100).WithMessage(localizer["Name Length Validation"]);

        RuleFor(course => course.Code)
            .NotEmpty().WithMessage(localizer["Course Code Validation"])
            .MaximumLength(10).WithMessage(localizer["Course Code Length Validation"]);

        RuleFor(course => course.Credits)
            .InclusiveBetween(1, 10).WithMessage(localizer["Course Credits Validation"]);

        RuleFor(professor => professor.DepartmentId)
            .GreaterThan(0).WithMessage(localizer["Department Validation"]);
    }
}
