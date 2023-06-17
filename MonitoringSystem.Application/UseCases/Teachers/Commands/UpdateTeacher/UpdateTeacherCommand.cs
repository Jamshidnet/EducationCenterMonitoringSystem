using AutoMapper;
using MediatR;
using MonitoringSystem.Application.UseCases.Teachers.Models;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Domein.Entities;
using Microsoft.EntityFrameworkCore;

namespace MonitoringSystem.Application.UseCases.Teachers.Commands.UpdateTeacher;

public  class UpdateTeacherCommand : IRequest<TeacherWithSubjectsDto>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

}

public class UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand, TeacherWithSubjectsDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public UpdateTeacherCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<TeacherWithSubjectsDto> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
    {

       Teacher teacher=  FilterIfTeacherExsists(request.Id);


        teacher.FirstName = request.FirstName;
        teacher.PhoneNumber = request.PhoneNumber;
        teacher.LastName = request.LastName;
        teacher.BirthDate=request.BirthDate;
        teacher.Email = request.Email;

         _dbContext.Teachers.Update(teacher);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TeacherWithSubjectsDto>(teacher);
    }

    private Teacher FilterIfTeacherExsists(Guid Id)
    {
        Teacher? teacher = _dbContext.Teachers.
            Include(x=>x.Subjects)
            .FirstOrDefault(x => x.Id==Id);

        if (teacher is null)
        {
            throw new NotFoundException(
                " There is no   teacher with this Id . ");
        }

        return teacher;
    }
}
