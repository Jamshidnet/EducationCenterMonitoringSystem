using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.UseCases.Teachers.Models;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Domein.Entities;
using Microsoft.AspNetCore.Hosting;

namespace MonitoringSystem.Application.UseCases.Teachers.Queries.GetTeacher;


public record GetTeacherQuery(Guid Id) : IRequest<TeacherWithSubjectsDto>;

public class GetTeacherQueryHandler : IRequestHandler<GetTeacherQuery, TeacherWithSubjectsDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;
    IWebHostEnvironment webHostEnvironment;
    public GetTeacherQueryHandler(IApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        this.webHostEnvironment = webHostEnvironment;
    }


    public async Task<TeacherWithSubjectsDto> Handle(GetTeacherQuery request, CancellationToken cancellationToken)
    {
        Teacher teacher = FilterIfTeacherExsists(request.Id);

        return _mapper.Map<TeacherWithSubjectsDto>(teacher);
    }

    private Teacher FilterIfTeacherExsists(Guid id)
    {
        Teacher? teacher = _dbContext.Teachers.Include(x=>x.Subjects)
            .FirstOrDefault(x => x.Id == id);
        teacher.Img = Path.GetRelativePath(webHostEnvironment
            .WebRootPath, "/images/"+ teacher?.Img);
        if (teacher is null)
        {
            throw new NotFoundException(
                " There is no teacher with this Id. ");
        }

        return teacher;
    }
}


