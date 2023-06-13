using FluentValidation;
using MonitoringSystem.Application.UseCases.Students.Models;

namespace MonitoringSystem.Application.UseCases.Students.Queries.GetStudent;

public  class GetStudentQueryValidation : AbstractValidator<GetStudentsWithGrades>
{
    public GetStudentQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty().NotEqual((Guid)default).WithMessage(" invalid Guid Format. ");
    }
}
