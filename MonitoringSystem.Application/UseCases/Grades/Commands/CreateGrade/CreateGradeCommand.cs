using AutoMapper;
using MediatR;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Grades.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Grades.Commands.CreateGrade;

public class CreateGradeCommand : IRequest<GradeDto>
{
    public Guid SubjectId { get; set; }

    public Guid StudentId { get; set; }

    public decimal GradeNum { get; set; }

}
public class CreateGradeCommandHandler : IRequestHandler<CreateGradeCommand, GradeDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public CreateGradeCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<GradeDto> Handle(CreateGradeCommand request, CancellationToken cancellationToken)
    {

        FilterIfGradeExsists(request.SubjectId, request.StudentId);

        Grade student = new()
        {
        SubjectId = request.SubjectId,
        StudentId=request.StudentId,
        GradeNum=request.GradeNum
        };

        await _dbContext.Grades.AddAsync(student);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GradeDto>(student);
    }

    private void FilterIfGradeExsists(Guid subjectId, Guid StudentId)
    {
        Grade? student = _dbContext.Grades.FirstOrDefault(
            x => x.StudentId == StudentId && x.SubjectId == subjectId);

        if (student is not null)
        {
            throw new AlreadyExistsException(
                " This student has alreadt taken grate for that subject.  ");
        }
    }
}
