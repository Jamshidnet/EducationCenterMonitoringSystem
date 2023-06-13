﻿using AutoMapper;
using MediatR;
using MonitoringSystem.Application.Common.Exceptions;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.UseCases.Teachers.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Teachers.Commands.CreateTeacher;

public  class CreateTeacherCommand  : IRequest<TeacherDto>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

}
public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, TeacherDto>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public CreateTeacherCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<TeacherDto> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
    {

        FilterIfTeacherExsists(request.FirstName, request.LastName);

        Teacher teacher = new Teacher()
        {
            FirstName = request.FirstName,

            LastName = request.LastName,

            BirthDate = request.BirthDate,

            PhoneNumber = request.PhoneNumber,

            Email = request.Email
        };

        await _dbContext.Teachers.AddAsync(teacher);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TeacherDto>(teacher);
    }

    private void FilterIfTeacherExsists(string? FirstName, string? LastName)
    {
        Teacher? teacher = _dbContext.Teachers.FirstOrDefault(x => x.FirstName == FirstName && x.LastName == LastName);

        if (teacher is not null)
        {
            throw new AlreadyExsistsException(" There is a  teacher with this full name. Teacher should be unique.  ");
        }
    }

}
