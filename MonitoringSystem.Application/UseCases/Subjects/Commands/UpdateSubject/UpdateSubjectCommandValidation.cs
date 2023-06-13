using FluentValidation;

namespace MonitoringSystem.Application.UseCases.Students.Commands.UpdateStudent
{
    public  class UpdateSubjectCommandValidation : AbstractValidator<UpdateSubjectCommand>
    {
        public UpdateSubjectCommandValidation() 
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(" Name is required . ");
        }


    }
}
