using LazyCache;
using Microsoft.AspNetCore.Mvc;
using MonitoringSystem.Application.UseCases.Students.Commands.CreateStudent;
using MonitoringSystem.Application.UseCases.Students.Commands.DeleteStudent;
using MonitoringSystem.Application.UseCases.Students.Commands.UpdateStudent;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Application.UseCases.Students.Queries.PaginatedStudentQuery;
using MonitoringSystem.Application.UseCases.Students.Queries.GetStudent;
using MonitoringSystem.Application.UseCases.Subjects;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EducationCenterMonitoringSystem.Filters;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Hosting;

namespace EducationCenterMonitoringSystem.Controllers;

public class StudentController : ApiBaseController
{
    public StudentController(IAppCache appCache, IConfiguration configuration)
    {
        _appCache = appCache;
        _configuration = configuration;
    }

    [HttpGet]
    [EnableRateLimiting("Sliding")]
    public async ValueTask<IActionResult> GetAll(int page = 1)
    {
        IPagedList<StudentDto> query = (await Mediator
            .Send(new GetAllStudentQuery()))
            .ToPagedList(page, 8);
        return View(query);
    }

    [HttpGet]
    public async ValueTask<IActionResult> Details(GetStudentQuery student)
    {
        ViewData["Subjects"]= await Mediator.Send(new GetAllSubjectQuery());
        return View(await Mediator.Send(student));
    }
     


    [HttpGet]
    public async ValueTask<IActionResult> Create()
    {
        return View();
    }


    [HttpPost]
    [EnableRateLimiting("Token")]
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
    public async ValueTask<IActionResult> Update(StudentDto student) => View(student);

    [HttpPost]
    public async ValueTask<IActionResult> Update([FromForm] UpdateStudentCommand student)
    {
            await Mediator.Send(student);
        return RedirectToAction("GetAll");
    }
}
