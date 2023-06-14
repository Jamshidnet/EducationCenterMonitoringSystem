using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Subjects.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Subjects.Commands.UpdateSubject;

public class UpdateSubjectCommand : IRequest<SubjectDto>
{
    public Guid Id { get; set; }
    public string SubjectName { get; set; }

    public Guid TeacherId { get; set; }

}
public class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand, SubjectDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public UpdateSubjectCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<SubjectDto> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
    {
        Subject subject = await FilterIfSubjectExsists(request.Id);

        subject.SubjectName = request.SubjectName;
        subject.TeacherId = request.TeacherId;
        _dbContext.Subjects.Update(subject);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SubjectDto>(subject);
    }

    private async Task<Subject> FilterIfSubjectExsists(Guid id)
    {
        Subject? subject = await _dbContext.Subjects
            .FirstOrDefaultAsync(x => x.Id == id);

        return subject
            ?? throw new NotFoundException(
                " there is no subject with this id. ");
    }
}
