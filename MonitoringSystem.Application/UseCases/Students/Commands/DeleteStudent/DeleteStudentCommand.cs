using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Students.Commands.DeleteStudent;

public record DeleteStudentCommand(Guid Id) : IRequest<StudentDto>;

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, StudentDto>
{
    private IApplicationDbContext _dbContext;
    private IMapper _mapper;
    IWebHostEnvironment _webHostEnvironment;
    public DeleteStudentCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<StudentDto> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        Student student = FilterIfStudentExsists(request.Id);

        _dbContext.Students.Remove(student);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<StudentDto>(student);
    }

    private Student FilterIfStudentExsists(Guid id)
    {
        Student? student = _dbContext.Students.FirstOrDefault(c => c.Id == id);
        if (student.Img is not null)
        {
            string uplodFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            string filePath = Path.Combine(uplodFolder, student.Img);
            FileInfo fileInfo = new (filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }
    
        if (student is null)
        {
            throw new NotFoundException(" There is no student with id. ");
        }

        return student;
    }
}


