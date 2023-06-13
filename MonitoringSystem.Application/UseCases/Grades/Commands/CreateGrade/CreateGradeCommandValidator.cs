using FluentValidation;

namespace MonitoringSystem.Application.UseCases.Grades.Commands.CreateGrade
{
    public class CreateGradeCommandValidator : AbstractValidator<CreateGradeCommand>

    {
        public CreateGradeCommandValidator()
        {
            RuleFor(x => x.SubjectId)
                .NotEqual(default(Guid))
                .WithMessage(" Guid format is inccorect. ");

            RuleFor(x => x.StudentId)
                .NotEqual(default(Guid)).
                WithMessage(" Guid format is inccorect. ");

            RuleFor(x => x.GradeNum).GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .WithMessage(" Grad should be between 0 and 100.  ");
        }
    }
}
