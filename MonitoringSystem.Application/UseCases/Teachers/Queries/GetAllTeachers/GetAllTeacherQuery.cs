using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.UseCases.Teachers.Models;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.Common.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Teachers.Queries.GetAllTeachers;


public record GetAllTeacherQuery
: IRequest<List<TeacherDto>>;


public class GetallTeacherQueryHandler : IRequestHandler<GetAllTeacherQuery, List<TeacherDto>>
{

    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetallTeacherQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<TeacherDto>> Handle(GetAllTeacherQuery request, CancellationToken cancellationToken)
    {
        Teacher[] orders = await _dbContext.Teachers.ToArrayAsync();

        List<TeacherDto> dtos = _mapper.Map<TeacherDto[]>(orders).ToList();

        return dtos;
    }
}
