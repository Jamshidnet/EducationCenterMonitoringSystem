using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.Common.Models;
using MonitoringSystem.Application.UseCases.Subjects.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Subjects;

public record GetAllSubjectQuery : IRequest<List<SubjectDto>>;

public class GetallSubjectCommmandHandler : IRequestHandler<GetAllSubjectQuery, List<SubjectDto>>
{

    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetallSubjectCommmandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<SubjectDto>> Handle(GetAllSubjectQuery request, CancellationToken cancellationToken)
    {
        Subject[] orders = await _dbContext.Subjects.Include(x => x.Grades).ToArrayAsync();

        List<SubjectDto> dtos = _mapper.Map<SubjectDto[]>(orders).ToList();

        return dtos;
    }
}
