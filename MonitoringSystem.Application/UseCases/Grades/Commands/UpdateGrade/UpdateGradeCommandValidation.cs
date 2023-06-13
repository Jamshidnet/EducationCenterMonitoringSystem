using FluentValidation;
using MonitoringSystem.Application.UseCases.Grades.Commands.UpdateGrade;

namespace MonitoringSystem.Application.UseCases.Students.Commands.UpdateStudent
{
    public  class UpdateGradeCommandValidation : AbstractValidator<UpdateGradeCommand>
    {
        public UpdateGradeCommandValidation() 
        {

        }


    }
}
