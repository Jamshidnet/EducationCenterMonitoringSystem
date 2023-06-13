using FluentValidation;
using MonitoringSystem.Application.UseCases.Grades.Queries.GetGradeQuery;

namespace MonitoringSystem.Application.UseCases.Courses.Queries.GetCourseQuery;

public  class GetGradeQueryValidation :AbstractValidator<GetGradeQuery>
{
    public GetGradeQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty().NotEqual((Guid)default).WithMessage(" invalid Guid Format. ");
    }
}
