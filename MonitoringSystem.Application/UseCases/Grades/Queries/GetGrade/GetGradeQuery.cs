using AutoMapper;
using MediatR;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Grades.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Grades.Queries.GetGradeQuery;

public  record GetGradeQuery(Guid Id) : IRequest<GradeDto>;

public class GetGradeQueryHandler : IRequestHandler<GetGradeQuery, GradeDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetGradeQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task<GradeDto> Handle(GetGradeQuery request, CancellationToken cancellationToken)
    {
        Grade grade = FilterIfGradeExsists(request.Id);

          return  _mapper.Map<GradeDto>(grade);
    }

    private Grade FilterIfGradeExsists(Guid id)
    {
        Grade? grade = _dbContext.Grades.Find(id);

        if (grade is null)
        {
            throw new NotFoundException(" There is no grade with this Id. ");
        }

        return grade;
    }
}

