using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Grades.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Grades.Queries.PaginatedQuerty;

public record GetallGradeQuery: IRequest<List<GradeDto>>;


public class GetallGradeCommmandHandler : IRequestHandler<GetallGradeQuery, List<GradeDto>>
{

    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetallGradeCommmandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<GradeDto>> Handle(GetallGradeQuery request, CancellationToken cancellationToken)
    {
        Grade[] orders = await _dbContext.Grades.ToArrayAsync();

        List<GradeDto> dtos = _mapper.Map<GradeDto[]>(orders).ToList();

        //PaginatedList<GradeDto> paginatedList = 
        //     PaginatedList<GradeDto>.CreateAsync(
        //        dtos, request.PageNumber, request.PageSize);
         
        return dtos;
    }
}
 
