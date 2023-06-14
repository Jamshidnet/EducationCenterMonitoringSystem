using FluentValidation;
using MonitoringSystem.Application.UseCases.Subjects.Models;

namespace StudentPaymentSystem.Application.UseCases.Students.Queries.GetStudent;

public  class GetSubjectQueryValidation : AbstractValidator<SubjectDto>
{
    public GetSubjectQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty().NotEqual((Guid)default).WithMessage(" invalid Guid Format. ");
    }
}
