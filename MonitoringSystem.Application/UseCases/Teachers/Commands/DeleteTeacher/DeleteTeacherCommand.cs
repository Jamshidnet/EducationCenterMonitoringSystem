using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Teachers.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Teachers.Commands.DeleteTeacher;


public record DeleteTeacherCommand(Guid Id) : IRequest<TeacherDto>;

public class DeleteTeacherCommandHandler : IRequestHandler<DeleteTeacherCommand, TeacherDto>
{
    private IApplicationDbContext _dbContext;
    private IMapper _mapper;
    IWebHostEnvironment _webHostEnvironment;

    public DeleteTeacherCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<TeacherDto> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
    {
        Teacher teacher = FilterIfTeacherExsists(request.Id);

        _dbContext.Teachers.Remove(teacher);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TeacherDto>(teacher);
    }

    private Teacher FilterIfTeacherExsists(Guid id)
    {
        Teacher? teacher = _dbContext.Teachers.FirstOrDefault(c => c.Id == id);
        if (teacher.Img is not null)
        {
            string uplodFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            string filePath = Path.Combine(uplodFolder, teacher.Img);
            FileInfo fileInfo = new(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }

        if (teacher is null)
        {
            throw new NotFoundException(
                " There is no teacher with id. ");
        }

        return teacher;
    }
}

