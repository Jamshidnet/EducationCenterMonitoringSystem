using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.Common.Models;
using MonitoringSystem.Application.UseCases.Subjects.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Subjects;

public record GetAllSubjectQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<SubjectDto>>;

public class GetallSubjectCommmandHandler : IRequestHandler<GetAllSubjectQuery, PaginatedList<SubjectDto>>
{

    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetallSubjectCommmandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedList<SubjectDto>> Handle(GetAllSubjectQuery request, CancellationToken cancellationToken)
    {
        Subject[] orders = await _dbContext.Subjects.Include(x => x.Grades).ToArrayAsync();

        List<SubjectDto> dtos = _mapper.Map<SubjectDto[]>(orders).ToList();

        PaginatedList<SubjectDto> paginatedList =
             PaginatedList<SubjectDto>.CreateAsync(
                dtos, request.PageNumber, request.PageSize);

        return paginatedList;
    }
}
