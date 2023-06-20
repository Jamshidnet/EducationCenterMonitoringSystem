using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MonitoringSystem.Application.Common;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Domein.Entities;
using System.ComponentModel.DataAnnotations;

namespace MonitoringSystem.Application.UseCases.Students.Commands.CreateStudent;

public class CreateStudentCommand : IRequest<StudentDto>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    [RegularExpression(@"^\+998(33|9[0-9])\d{7}$", ErrorMessage =" Invalid PhoneNumber style. ")]
    public string PhoneNumber { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }

    public IFormFile Img { get; set; }

}
public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, StudentDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;
    IWebHostEnvironment webHostEnvironment;

    public CreateStudentCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        this.webHostEnvironment = webHostEnvironment;
    }

    public async Task<StudentDto> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {

        FilterIfStudentExsists(request.PhoneNumber);

        Student student = new ()
        {
          FirstName=request.FirstName,
          LastName=request.LastName,
          Email=request.Email,
          PhoneNumber=request.PhoneNumber,
          BirthDate=request.BirthDate,
          Img=SaveImage(request.Img)
        };

        await  _dbContext.Students.AddAsync(student);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<StudentDto>(student);
    }

    public  void FilterIfStudentExsists( string? PhoneNumber)
    {
        Student? student = _dbContext.Students.FirstOrDefault(x => x.PhoneNumber == PhoneNumber);

        if (student is not null)
        {
            throw new AlreadyExistsException(" There is a  student with this phonenumber. Student should be unique.  ");
        }
    }
    private string SaveImage(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            // Image fayli mavjud emas yoki bo'sh
            return string.Empty;
        }

        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            imageFile.CopyTo(fileStream);
        }

        return uniqueFileName;
    }
}
