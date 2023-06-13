using FluentValidation;

namespace MonitoringSystem.Application.UseCases.Students.Commands.CreateStudent;

public class CreateSubjectCommandValidation : AbstractValidator<CreateSubjectCommand>
{
    public CreateSubjectCommandValidation()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage(" Name is required . ");
    }
}
