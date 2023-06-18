using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Domein.Entities;
using System.ComponentModel.DataAnnotations;

namespace MonitoringSystem.Application.UseCases.Students.Commands.UpdateStudent;

public class UpdateStudentCommand : IRequest<StudentDto>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    [RegularExpression(@"^\+998(33|9[0-9])\d{7}$", ErrorMessage = " Invalid PhoneNumber style. ")]
    public string PhoneNumber { get; set; }

    [EmailAddress]
    public string Email { get; set; }

}
public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, StudentDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public UpdateStudentCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<StudentDto> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        Student student = await FilterIfStudentExsists(request.Id);

        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.Email = request.Email;
        student.PhoneNumber = request.PhoneNumber;
        student.BirthDate = request.BirthDate;
        _dbContext.Students.Update(student);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<StudentDto>(student);
    }

    private async Task<Student> FilterIfStudentExsists(Guid id)
    {
        Student? student = await _dbContext.Students
            .FirstOrDefaultAsync(x => x.Id == id);

        return student
            ?? throw new NotFoundException(
                " there is no student with this id. ");
    }
}
