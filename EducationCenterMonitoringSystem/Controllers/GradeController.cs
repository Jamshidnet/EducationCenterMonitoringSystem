using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MonitoringSystem.Application.UseCases.Grades.Commands.CreateGrade;
using MonitoringSystem.Application.UseCases.Grades.Commands.DeleteGrade;
using MonitoringSystem.Application.UseCases.Grades.Commands.UpdateGrade;
using MonitoringSystem.Application.UseCases.Grades.Models;
using MonitoringSystem.Application.UseCases.Grades.Queries.PaginatedQuerty;
using MonitoringSystem.Application.UseCases.Subjects;
using MonitoringSystem.Application.UseCases.Students.Queries.PaginatedStudentQuery;

namespace EducationCenterMonitoringSystem.Controllers
{
    public class GradeController : ApiBaseController
    {

        public GradeController(IAppCache appCache, IConfiguration configuration)
        {
            _appCache = appCache;
            _configuration = configuration;
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetAll(GetallGradeQuery query)
        {
            ViewData["Subjects"] = (await Mediator
                .Send(new GetAllSubjectQuery())).Items;
            ViewData["Students"] = (await Mediator
                .Send(new GetAllStudentQuery())).Items;
                return View(await Mediator.Send(query));

        }

        [HttpGet]
        public async ValueTask<IActionResult> Create()
        {
            ViewData["Subjects"] = (await Mediator
                .Send(new GetAllSubjectQuery())).Items;
            ViewData["Students"] = (await Mediator
                .Send(new GetAllStudentQuery())).Items;
            return View();
        }


        [HttpPost]
        public async ValueTask<IActionResult> Create([FromForm] CreateGradeCommand grade)
        {
            await Mediator.Send(grade);
            return RedirectToAction("GetAll");
        }



        public async ValueTask<IActionResult> Delete(Guid Id)
        {
            await Mediator.Send(new DeleteGradeCommand(Id));
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public async ValueTask<IActionResult> Update(GradeDto grade)
        {

            ViewData["Subjects"] = (await Mediator.
                Send(new GetAllSubjectQuery())).Items;
            ViewData["Students"] = (await Mediator
                .Send(new GetAllStudentQuery())).Items;
            return View(grade);
        }

        [HttpPost]
        public async ValueTask<IActionResult> Update([FromForm] UpdateGradeCommand grade)
        {
            await Mediator.Send(grade);
            return RedirectToAction("GetAll");
        }
    }
}

