using AutoMapper;
using MonitoringSystem.Application.UseCases.Grades.Models;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Application.UseCases.Subjects.Models;
using MonitoringSystem.Application.UseCases.Teachers.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.Common.Mapping;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<Student, StudentDto>();
    CreateMap<Student, GetStudentsWithGrades>();
    CreateMap<Subject, SubjectDto>();
    CreateMap<Subject, BestSubjectDto>();
    CreateMap<Teacher, TeacherDto>();
    CreateMap<Teacher, TeacherWithSubjectsDto>();
    CreateMap<Grade, GradeDto>();
        CreateMap<Teacher, PersonDto>()
        .ForMember(dest => dest.status, opt => opt.MapFrom(_ => Status.teacher));
        CreateMap<Student, PersonDto>()
        .ForMember(dest => dest.status, opt => opt.MapFrom(_ => Status.student));

    }

}
   
