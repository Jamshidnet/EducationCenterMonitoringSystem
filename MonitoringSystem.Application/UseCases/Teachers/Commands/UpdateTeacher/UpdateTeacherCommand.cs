using AutoMapper;
using MediatR;
using MonitoringSystem.Application.UseCases.Teachers.Models;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Domein.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace MonitoringSystem.Application.UseCases.Teachers.Commands.UpdateTeacher;

public  class UpdateTeacherCommand : IRequest<TeacherWithSubjectsDto>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    [RegularExpression(@"^\+998(33|9[0-9])\d{7}$", ErrorMessage = " Invalid PhoneNumber style. ")]
    public string PhoneNumber { get; set; }

    public string  Img { get; set; }

    public IFormFile ImageFile { get; set; }

    [EmailAddress]
    public string Email { get; set; }

}

public class UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand, TeacherWithSubjectsDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;
    IWebHostEnvironment webHostEnvironment;

    public UpdateTeacherCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment = null)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        this.webHostEnvironment = webHostEnvironment;
    }

    public async Task<TeacherWithSubjectsDto> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
    {

       Teacher teacher=  FilterIfTeacherExsists(request.Id);


        teacher.FirstName = request.FirstName;
        teacher.PhoneNumber = request.PhoneNumber;
        teacher.LastName = request.LastName;
        teacher.BirthDate=request.BirthDate;
        teacher.Email = request.Email;
        teacher.Img = request.ImageFile == null ? request.Img : SaveImage(teacher, request.ImageFile);
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
    private string SaveImage(Teacher teacher, IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            // Image fayli mavjud emas yoki bo'sh
            return string.Empty;
        }
        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");


        if (teacher.Img is not null)
        {
            string filePathh = Path.Combine(uploadsFolder, teacher.Img);
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
