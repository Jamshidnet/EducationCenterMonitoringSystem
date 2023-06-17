using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.Common.Models;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Filters
{

    public record SearchedStudents(string Pattern, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<StudentDto>>;

    public class SearchedStudentsHandler : IRequestHandler<SearchedStudents, PaginatedList<StudentDto>>
    {


        IApplicationDbContext _dbContext;
        IMapper _mapper;

        public SearchedStudentsHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedList<StudentDto>> Handle(SearchedStudents request, CancellationToken cancellationToken)
        {
            Student[] students = await _dbContext.Students.ToArrayAsync(cancellationToken: cancellationToken);
            var SortedStudents = students
           .Where(s => s.FirstName.Contains(request.Pattern, StringComparison.OrdinalIgnoreCase)
           || s.LastName.Contains(request.Pattern, StringComparison.OrdinalIgnoreCase))
           .ToList();

            List<StudentDto> dtos = _mapper.Map<StudentDto[]>(SortedStudents).ToList();

            PaginatedList<StudentDto> paginatedList =
                 PaginatedList<StudentDto>.CreateAsync(
                    dtos, request.PageNumber, request.PageSize);

            return paginatedList;


        }

       
    }

}
