namespace StudentAffairs.Server.Validations;

public class StudentValidation : AbstractValidator<StudentToCreateDto>
{
    public StudentValidation(IStringLocalizer<Resource> localizer)
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(localizer["Full Name Validation"])
            .Length(1, 100).WithMessage(localizer["Full Name Length Validation"]);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage(localizer["Date Of Birth Validation"])
            .LessThan(DateTime.Now).WithMessage(localizer["Date of Birth must be in the past"]);

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage(localizer["Gender Validation"])
            .Matches("^(Male|Female)$").WithMessage(localizer["Gender Validation correct Message"]);

        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage(localizer["Mobile Validation"])
            .MaximumLength(15).WithMessage(localizer["Mobile Length Validation"])
            .MinimumLength(5).WithMessage(localizer["Mobile Length Validation"]);

        RuleFor(x => x.GradeLevel)
            .InclusiveBetween(1, 12).WithMessage(localizer["Grade Level Validation"]);

        RuleFor(professor => professor.DepartmentId)
            .GreaterThan(0).WithMessage(localizer["Department Validation"]);
    }
}
