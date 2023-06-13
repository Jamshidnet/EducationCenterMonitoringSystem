using AutoMapper;
using MediatR;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Grades.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Grades.Commands.DeleteGrade;

public record DeleteGradeCommand(Guid Id) : IRequest<GradeDto>;

public class DeleteGradeCommandHandler : IRequestHandler<DeleteGradeCommand, GradeDto>
{
    private IApplicationDbContext _dbContext;
    private IMapper _mapper;

    public DeleteGradeCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<GradeDto> Handle(DeleteGradeCommand request, CancellationToken cancellationToken)
    {
        Grade grade = FilterIfGradeExsists(request.Id);

        _dbContext.Grades.Remove(grade);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GradeDto>(grade);
    }

    private Grade FilterIfGradeExsists(Guid id)
    {
        Grade? grade = _dbContext.Grades.FirstOrDefault(c => c.Id == id);

        if (grade is null)
        {
            throw new NotFoundException(" There is no grade with id. ");
        }

        return grade;
    }
}


