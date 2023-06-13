using FluentValidation;
using MonitoringSystem.Application.UseCases.Teachers.Models;

namespace MonitoringSystem.Application.UseCases.Teachers.Commands.CreateTeacher
{
    public class CreateTeacherCommandValidation : AbstractValidator<TeacherDto>
    {
        public CreateTeacherCommandValidation()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(30).WithMessage(" the length of first name should be less or equal than 30. ");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(30).WithMessage(" the length of last name should be less or equal than 30. ");
        }
    }
}
