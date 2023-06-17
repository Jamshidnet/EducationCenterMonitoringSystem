using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.Common.Models;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.StudentsListBy20Age
{
    public  record  StudentsLIstBy20AgeQuery(int PageNumber=1, int PageSize = 10) : IRequest<PaginatedList<StudentDto>>;

    public class StudentsLIstBy20AgeQueryHandler : IRequestHandler<StudentsLIstBy20AgeQuery, PaginatedList<StudentDto>>
    {


        IApplicationDbContext _dbContext;
        IMapper _mapper;

        public StudentsLIstBy20AgeQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedList<StudentDto>> Handle(StudentsLIstBy20AgeQuery request, CancellationToken cancellationToken)
        {
            Student[] students = await _dbContext.Students.Include(x => x.Grades).ToArrayAsync();

            var SortedStudents = from student in students
                             where student.BirthDate.AddYears(20)>=DateTime.Now
                             select student;

            List < StudentDto > dtos = _mapper.Map<StudentDto[]>(SortedStudents).ToList();

            PaginatedList<StudentDto> paginatedList =
                 PaginatedList<StudentDto>.CreateAsync(
                    dtos, request.PageNumber, request.PageSize);

            return paginatedList;


        }
    }


}
