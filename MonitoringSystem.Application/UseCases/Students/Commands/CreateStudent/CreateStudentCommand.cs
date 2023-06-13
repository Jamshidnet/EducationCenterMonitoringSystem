using AutoMapper;
using MediatR;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Students.Commands.CreateStudent;

public class CreateSubjectCommand : IRequest<StudentDto>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

}
public class CreateStudentCommandHandler : IRequestHandler<CreateSubjectCommand, StudentDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public CreateStudentCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<StudentDto> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
    {

        FilterIfStudentExsists(request.PhoneNumber);

        Student student = new ()
        {
          FirstName=request.FirstName,
          LastName=request.LastName,
          Email=request.Email,
          PhoneNumber=request.PhoneNumber,
          BirthDate=request.BirthDate
        };

        await  _dbContext.Students.AddAsync(student);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<StudentDto>(student);
    }

    private void FilterIfStudentExsists(string? PhoneNumber)
    {
        Student? student = _dbContext.Students.FirstOrDefault(x => x.PhoneNumber == PhoneNumber);

        if (student is not null)
        {
            throw new AlreadyExsistsException(" There is a  student with this phonenumber. Student should be unique.  ");
        }
    }
}
