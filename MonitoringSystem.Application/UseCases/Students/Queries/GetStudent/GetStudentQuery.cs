using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Grades.Models;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Students.Queries.GetStudent;

public  record GetStudentQuery(Guid Id) : IRequest<GetStudentsWithGrades>;

public class GetStudentQueryHandler : IRequestHandler<GetStudentQuery, GetStudentsWithGrades>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetStudentQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task<GetStudentsWithGrades> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        GetStudentsWithGrades student = FilterIfStudentExsists(request.Id);

        return _mapper.Map<GetStudentsWithGrades>(student);
    }

    private GetStudentsWithGrades FilterIfStudentExsists(Guid id)
    {
        Student? student = _dbContext.Students
            .Include(x=>x.Grades)
            .FirstOrDefault(x => x.Id == id);


        GradeDto[] mappedSt = _mapper.Map<GradeDto[]>(student?.Grades);
        GetStudentsWithGrades getAllStudentDto = new()
        {
            FirstName = student?.FirstName,
            LastName = student?.LastName,
            Id = student.Id,
            Email = student.Email,
            PhoneNumber = student.PhoneNumber,
            BirthDate = student.BirthDate,
            StudentRageNumber = student.StudentRageNumber,
            Grades = mappedSt
        };
        

        if (student is null)
        {
            throw new NotFoundException(
                " There is on student with this Id. ");
        }

        return getAllStudentDto;
    }


}