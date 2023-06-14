using FluentValidation;

namespace MonitoringSystem.Application.UseCases.Students.Commands.UpdateStudent
{
    public  class UpdateStudentCommandValidation : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentCommandValidation() 
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(" Name is required . ");
        }


    }
}
