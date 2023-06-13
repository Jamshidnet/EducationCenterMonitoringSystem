using FluentValidation;

namespace MonitoringSystem.Application.UseCases.Teachers.Commands.UpdateTeacher
{
    public  class UpdateTeacherCommandValidation : AbstractValidator<UpdateTeacherCommand>
    {
        public UpdateTeacherCommandValidation()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(30).WithMessage(" the length of first name should be less or equal than 30. ");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(30).WithMessage(" the length of last name should be less or equal than 30. ");
            RuleFor(x => x.Id).NotEqual((Guid)default).WithMessage(" the Guid format doesn't match . ");
        }
    }
}
