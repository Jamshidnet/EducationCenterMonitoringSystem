using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.UseCases.Teachers.Models;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.Common.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Teachers.Queries.GetAllTeachers;


public record GetallTeacherQuery
: IRequest<PaginatedList<TeacherDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetallTeacherQueryHandler : IRequestHandler<GetallTeacherQuery, PaginatedList<TeacherDto>>
{

    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetallTeacherQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TeacherDto>> Handle(GetallTeacherQuery request, CancellationToken cancellationToken)
    {
        Teacher[] orders = await _dbContext.Teachers.ToArrayAsync();

        List<TeacherDto> dtos = _mapper.Map<TeacherDto[]>(orders).ToList();

        PaginatedList<TeacherDto> paginatedList =
             PaginatedList<TeacherDto>.CreateAsync(
                dtos, request.PageNumber, request.PageSize);

        return paginatedList;
    }
}
