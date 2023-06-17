using LazyCache;
using Microsoft.AspNetCore.Mvc;
using MonitoringSystem.Application.UseCases.Students.Commands.CreateStudent;
using MonitoringSystem.Application.UseCases.Students.Commands.DeleteStudent;
using MonitoringSystem.Application.UseCases.Students.Commands.UpdateStudent;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Application.UseCases.Students.Queries.PaginatedStudentQuery;
using MonitoringSystem.Application.UseCases.Students.Queries.GetStudent;
using MonitoringSystem.Application.UseCases.Teachers.Queries.GetAllTeachers;
using MonitoringSystem.Application.UseCases.Subjects;

namespace EducationCenterMonitoringSystem.Controllers;

public class StudentController : ApiBaseController
{
    public StudentController(IAppCache appCache, IConfiguration configuration)
    {
        _appCache = appCache;
        _configuration = configuration;
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAll(GetAllStudentQuery query)
    {
            return View(await Mediator.Send(query));
    }


    [HttpGet]
    public async ValueTask<IActionResult> Details(GetStudentQuery student)
    {
        ViewData["Subjects"]= (await Mediator.Send(new GetAllSubjectQuery())).Items;
        return View(await Mediator.Send(student));
    }
     


    [HttpGet]
    public async ValueTask<IActionResult> Create()
    {
        return View();
    }


    [HttpPost]
    public async ValueTask<IActionResult> Create([FromForm] CreateStudentCommand student)
    {
        await Mediator.Send(student);
        return RedirectToAction("GetAll");
    }



    public async ValueTask<IActionResult> Delete(Guid Id)
    {
        await Mediator.Send(new DeleteStudentCommand(Id));
        return RedirectToAction("GetAll");
    }

    [HttpGet]
    public async ValueTask<IActionResult> Update(StudentDto student)=>
          View(student);

    [HttpPost]
    public async ValueTask<IActionResult> Update([FromForm] UpdateStudentCommand student)
    {
        await Mediator.Send(student);
        return RedirectToAction("GetAll");
    }
}
