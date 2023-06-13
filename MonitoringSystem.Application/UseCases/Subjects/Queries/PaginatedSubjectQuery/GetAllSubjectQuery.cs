using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentPaymentSystem.Application.Common.Interfaces;
using StudentPaymentSystem.Application.Common.Models;
using StudentPaymentSystem.Application.UseCases.Students.Models;
using StudentPaymentSystem.Domein.Entities;

namespace StudentPaymentSystem.Application.UseCases.Students.Queries.PaginatedStudentQuery;

public record GetAllSubjectQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<StudentDto>>;

public class GetallStudentCommmandHandler : IRequestHandler<GetAllSubjectQuery, PaginatedList<StudentDto>>
{

    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetallStudentCommmandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedList<StudentDto>> Handle(GetAllSubjectQuery request, CancellationToken cancellationToken)
    {
        Student[] orders = await _dbContext.Students.Include(x => x.Courses).ToArrayAsync();

        List<StudentDto> dtos = _mapper.Map<StudentDto[]>(orders).ToList();

        PaginatedList<StudentDto> paginatedList =
             PaginatedList<StudentDto>.CreateAsync(
                dtos, request.PageNumber, request.PageSize);

        return paginatedList;
    }
}
