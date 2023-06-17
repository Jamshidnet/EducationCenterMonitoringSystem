using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.UseCases.Teachers.Models;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.Common.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Filters;


public record TeacherOverFiftyQuery
: IRequest<PaginatedList<TeacherDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class TeachersOverFiftyHandler : IRequestHandler<TeacherOverFiftyQuery, PaginatedList<TeacherDto>>
{

    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public TeachersOverFiftyHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TeacherDto>> Handle(TeacherOverFiftyQuery request, CancellationToken cancellationToken)
    {
        Teacher[] teachers = await _dbContext.Teachers.ToArrayAsync();

        var SortedTeachers = from teacher in teachers
                             where teacher.BirthDate.AddYears(55) <= DateTime.Now
                             select teacher;

        List<TeacherDto> dtos = _mapper.Map<TeacherDto[]>(SortedTeachers).ToList();

        PaginatedList<TeacherDto> paginatedList =
             PaginatedList<TeacherDto>.CreateAsync(
                dtos, request.PageNumber, request.PageSize);

        return paginatedList;
    }
}
