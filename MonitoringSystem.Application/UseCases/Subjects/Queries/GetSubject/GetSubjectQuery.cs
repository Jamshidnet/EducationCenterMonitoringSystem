using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Grades.Models;
using MonitoringSystem.Application.UseCases.Subjects.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Subjects.Queries.GetSubject;

public  record GetSubjectQuery(Guid Id) : IRequest<SubjectDto>;

public class GetSubjectQueryHandler : IRequestHandler<GetSubjectQuery, SubjectDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetSubjectQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task<SubjectDto> Handle(GetSubjectQuery request, CancellationToken cancellationToken)
    {
        SubjectDto subject = FilterIfSubjectExsists(request.Id);

        return _mapper.Map<SubjectDto>(subject);
    }

    private SubjectDto FilterIfSubjectExsists(Guid id)
    {
        Subject? subject = _dbContext.Subjects
            .Include(x=>x.Grades)
            .FirstOrDefault(x => x.Id == id);

        GradeDto[] mappedSt = _mapper.Map<GradeDto[]>(subject.Grades);

        SubjectDto getAllSubjectDto = new()
        {
          SubjectName = subject.SubjectName,
          TeacherId=subject.TeacherId,
          Id= subject.Id
        };
        

        if (subject is null)
        {
            throw new NotFoundException(
                " There is on subject with this Id. ");
        }

        return getAllSubjectDto;
    }


}