using AutoMapper;
using MediatR;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Subjects.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Subjects.Commands.CreateSubject;

public class CreateSubjectCommmand : IRequest<SubjectDto>
{
    public string SubjectName { get; set; }

    public Guid TeacherId { get; set; }

}
public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommmand, SubjectDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public CreateSubjectCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<SubjectDto> Handle(CreateSubjectCommmand request, CancellationToken cancellationToken)
    {

        FilterIfSubjectExsists(request.SubjectName);

        Subject subject = new ()
        {
        SubjectName=request.SubjectName,
        TeacherId=request.TeacherId
        };

        await  _dbContext.Subjects.AddAsync(subject);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SubjectDto>(subject);
    }

    private void FilterIfSubjectExsists(string? SubjectName)
    {
        Subject? subject = _dbContext.Subjects.FirstOrDefault(x => x.SubjectName==SubjectName);

        if (subject is not null)
        {
            throw new AlreadyExsistsException(" There is a  subject with this name. Subject should be unique.  ");
        }
    }
}
