using FluentValidation;
using MonitoringSystem.Application.UseCases.Subjects.Commands.UpdateSubject;

namespace MonitoringSystem.Application.UseCases.Students.Commands.UpdateStudent
{
    public  class UpdateSubjectCommandValidation : AbstractValidator<UpdateSubjectCommand>
    {
        public UpdateSubjectCommandValidation() 
        {
            RuleFor(x => x.SubjectName).NotEmpty().WithMessage(" Name is required . ");
        }


    }
}
