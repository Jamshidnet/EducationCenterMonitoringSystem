using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Students.Queries.PaginatedStudentQuery;

public record GetAllStudentQuery : IRequest<List<StudentDto>>;

public class GetallStudentCommmandHandler : IRequestHandler<GetAllStudentQuery, List<StudentDto>>
{

    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetallStudentCommmandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<StudentDto>> Handle(GetAllStudentQuery request, CancellationToken cancellationToken)
    {
        Student[] orders = await _dbContext.Students.Include(x => x.Grades).ToArrayAsync();

        List<StudentDto> dtos = _mapper.Map<StudentDto[]>(orders).ToList();

        return dtos;
    }
}
