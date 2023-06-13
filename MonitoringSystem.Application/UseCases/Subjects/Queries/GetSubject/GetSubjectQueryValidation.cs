using FluentValidation;
using StudentPaymentSystem.Application.UseCases.Students.Models;

namespace StudentPaymentSystem.Application.UseCases.Students.Queries.GetStudent;

public  class GetSubjectQueryValidation : AbstractValidator<GetStudentsWithGrades>
{
    public GetSubjectQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty().NotEqual((Guid)default).WithMessage(" invalid Guid Format. ");
    }
}
