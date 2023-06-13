using AutoMapper;
using MediatR;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Students.Commands.DeleteStudent;

public record DeleteSubjectCommand(Guid Id) : IRequest<StudentDto>;

public class DeleteStudentCommandHandler : IRequestHandler<DeleteSubjectCommand, StudentDto>
{
    private IApplicationDbContext _dbContext;
    private IMapper _mapper;

    public DeleteStudentCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<StudentDto> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        Student student = FilterIfStudentExsists(request.Id);

        _dbContext.Students.Remove(student);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<StudentDto>(student);
    }

    private Student FilterIfStudentExsists(Guid id)
    {
        Student? student = _dbContext.Students.FirstOrDefault(c => c.Id == id);

        if (student is null)
        {
            throw new NotFoundException(" There is no student with id. ");
        }

        return student;
    }
}


