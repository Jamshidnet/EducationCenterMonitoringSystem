using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

    public string Img { get; set; }

    public IFormFile ImageFile { get; set; }

    [EmailAddress]
    public string Email { get; set; }

}
public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, StudentDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;
    IWebHostEnvironment webHostEnvironment;

    public UpdateStudentCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        this.webHostEnvironment = webHostEnvironment;
    }

    public async Task<StudentDto> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        Student student = await FilterIfStudentExsists(request.Id);

        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.Email = request.Email;
        student.PhoneNumber = request.PhoneNumber;
        student.BirthDate = request.BirthDate;
        student.Img = request.ImageFile == null ? request.Img : SaveImage(student, request.ImageFile);
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

    private string SaveImage(Student student, IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            // Image fayli mavjud emas yoki bo'sh
            return string.Empty;
        }
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");

        if (student.Img is not null)
        {
            string filePathh = Path.Combine(uploadsFolder, student.Img);
            FileInfo fileInfo = new(filePathh);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }

        string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            imageFile.CopyTo(fileStream);
        }

        return uniqueFileName;
    }
}
