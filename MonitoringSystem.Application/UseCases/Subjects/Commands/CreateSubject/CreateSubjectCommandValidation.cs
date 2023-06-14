using FluentValidation;
using MonitoringSystem.Application.UseCases.Subjects.Commands.CreateSubject;

namespace MonitoringSystem.Application.UseCases.Students.Commands.CreateStudent;

public class CreateSubjectCommandValidation : AbstractValidator<CreateSubjectCommand>
{
    public CreateSubjectCommandValidation()
    {
        RuleFor(x => x.SubjectName).NotEmpty().WithMessage(" Name is required . ");
    }
}
