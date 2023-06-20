using FluentValidation;

namespace MonitoringSystem.Application.UseCases.Students.Commands.CreateStudent;

public class CreateStudentCommandValidation : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidation()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage(" Name is required . ");
        RuleFor(x => x.BirthDate).LessThanOrEqualTo(DateTime.Now).WithMessage(" Ivalid attemt. since this date is from future. ");
    }
}
