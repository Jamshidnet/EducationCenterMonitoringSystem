using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Grades.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Grades.Commands.UpdateGrade;

public class UpdateGradeCommand : IRequest<GradeDto>
{
    public Guid Id { get; set; }
    public Guid SubjectId { get; set; }

    public Guid StudentId { get; set; }

    public decimal GradeNum { get; set; }

}
public class UpdateGradeCommandHandler : IRequestHandler<UpdateGradeCommand, GradeDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public UpdateGradeCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<GradeDto> Handle(UpdateGradeCommand request, CancellationToken cancellationToken)
    {
        Grade srade = await FilterIfGradeExsists(request.Id);

        srade.StudentId = request.StudentId;
        srade.SubjectId = request.SubjectId;
        srade.GradeNum = request.GradeNum;
        _dbContext.Grades.Update(srade);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GradeDto>(srade);
    }

    private async Task<Grade> FilterIfGradeExsists(Guid id)
    {
        Grade? srade = await _dbContext.Grades
            .FirstOrDefaultAsync(x => x.Id == id);

        return srade
            ?? throw new NotFoundException(
                " there is no Grade with this id. ");
    }
}
