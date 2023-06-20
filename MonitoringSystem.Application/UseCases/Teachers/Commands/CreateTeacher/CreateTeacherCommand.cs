using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Teachers.Models;
using MonitoringSystem.Domein.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MonitoringSystem.Application.UseCases.Teachers.Commands.CreateTeacher;

public  class CreateTeacherCommand  : IRequest<TeacherDto>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    [RegularExpression(@"^\+998(33|9[0-9])\d{7}$", ErrorMessage = " Invalid PhoneNumber style. ")]
    public string PhoneNumber
    {
        get; set;
    }

    public IFormFile Img { get; set; }

    [EmailAddress]
    public string Email { get; set; }

}
public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, TeacherDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;
    IWebHostEnvironment webHostEnvironment;

    public CreateTeacherCommandHandler(
        IApplicationDbContext dbContext,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        this.webHostEnvironment = webHostEnvironment;
    }

    public async Task<TeacherDto> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
    {

        FilterIfTeacherExsists(request.FirstName, request.LastName);

        Teacher teacher = new ()
        {
            FirstName = request.FirstName,

            LastName = request.LastName,

            BirthDate = request.BirthDate,

            PhoneNumber = request.PhoneNumber,

            Email = request.Email,
            Img = SaveImage(request.Img)
        };

        await _dbContext.Teachers.AddAsync(teacher);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TeacherDto>(teacher);
    }

    private void FilterIfTeacherExsists(string? FirstName, string? LastName)
    {
        Teacher? teacher = _dbContext.Teachers
            .FirstOrDefault(x => x.FirstName == FirstName && x.LastName == LastName);

        if (teacher is not null)
        {
            throw new AlreadyExistsException(
                " There is a  teacher with this full name. Teacher should be unique.  ");
        }
    }
    private string SaveImage(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            // Image fayli mavjud emas yoki bo'sh
            return string.Empty;
        }

        string uploadsFolder = Path
            .Combine(webHostEnvironment.WebRootPath, "images");
        string uniqueFileName = Guid.NewGuid()
            .ToString() + "_" + imageFile.FileName;
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            imageFile.CopyTo(fileStream);
        }
        return uniqueFileName;
    }

}
