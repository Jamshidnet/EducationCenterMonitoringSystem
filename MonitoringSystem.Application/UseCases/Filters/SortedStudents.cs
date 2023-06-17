using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.Common.Models;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Filters
{

    public record SortedStudentsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<StudentDto>>;

    public class SortedStudentsQueryHandler : IRequestHandler<SortedStudentsQuery, PaginatedList<StudentDto>>
    {


        IApplicationDbContext _dbContext;
        IMapper _mapper;

        public SortedStudentsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedList<StudentDto>> Handle(SortedStudentsQuery request, CancellationToken cancellationToken)
        {
            Student[] students = await _dbContext.Students.ToArrayAsync();

            var SortedStudents = from student in students
                                 where IsBetweenRage(student.BirthDate)
                                 select student;

            List<StudentDto> dtos = _mapper.Map<StudentDto[]>(SortedStudents).ToList();

            PaginatedList<StudentDto> paginatedList =
                 PaginatedList<StudentDto>.CreateAsync(
                    dtos, request.PageNumber, request.PageSize);

            return paginatedList;


        }

        private static bool IsBetweenRage(DateTime birthDate)
        {
           DateOnly actualDate=DateOnly
                .FromDateTime(birthDate);
            DateOnly InitialDate = new (2001,8, 12);
            DateOnly LastDate = new (2001,9, 18);

            return
                true && actualDate.DayOfYear> InitialDate.DayOfYear
               && actualDate.DayOfYear < LastDate.DayOfYear;
        }
    }

}
