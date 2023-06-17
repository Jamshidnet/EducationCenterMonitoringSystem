using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.UseCases.Teachers.Models;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.Common.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Filters;


public record FilteredTeacherQuery
: IRequest<PaginatedList<TeacherDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetallTeacherQueryHandler : IRequestHandler<FilteredTeacherQuery, PaginatedList<TeacherDto>>
{

    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetallTeacherQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TeacherDto>> Handle(FilteredTeacherQuery request, CancellationToken cancellationToken)
    {
        var teachers = from mark in _dbContext.Grades
                       join subject in _dbContext.Subjects
                       on mark.SubjectId equals subject.Id
                       join teacher in _dbContext.Teachers
                       on subject.TeacherId equals teacher.Id
                       where mark.GradeNum > 97
                       select teacher;

       var ListTeachers = teachers.Distinct().ToList();

        List < TeacherDto > dtos = _mapper.Map<TeacherDto[]>(ListTeachers).ToList();

        PaginatedList<TeacherDto> paginatedList =
             PaginatedList<TeacherDto>.CreateAsync(
                dtos, request.PageNumber, request.PageSize);

        return paginatedList;
    }
}
