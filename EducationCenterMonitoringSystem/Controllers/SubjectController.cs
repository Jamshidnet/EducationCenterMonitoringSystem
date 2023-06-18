using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Application.UseCases.Students.Queries.PaginatedStudentQuery;
using MonitoringSystem.Application.UseCases.Subjects;
using MonitoringSystem.Application.UseCases.Subjects.Commands.CreateSubject;
using MonitoringSystem.Application.UseCases.Subjects.Commands.DeleteSubject;
using MonitoringSystem.Application.UseCases.Subjects.Commands.UpdateSubject;
using MonitoringSystem.Application.UseCases.Subjects.Models;
using MonitoringSystem.Application.UseCases.Teachers.Queries.GetAllTeachers;
using X.PagedList;

namespace EducationCenterMonitoringSystem.Controllers
{


    [Authorize]
    public class SubjectController : ApiBaseController
    {

        public SubjectController(IAppCache appCache, IConfiguration configuration)
        {
            _appCache = appCache;
            _configuration = configuration;
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetAll(int page = 1)
        {
            ViewData["Teachers"] = await Mediator.Send(new GetAllTeacherQuery());
            IPagedList<SubjectDto> query = (await Mediator
                .Send(new GetAllSubjectQuery()))
                .ToPagedList(page, 10);
            return View(query);
        }

        [HttpGet]
        public async ValueTask<IActionResult> Create()
        {

            var item = await Mediator.Send(new GetAllTeacherQuery());
            ViewData["Teachers"] = item;
            return View();
        }


        [HttpPost]
        public async ValueTask<IActionResult> Create([FromForm] CreateSubjectCommmand subject)
        {
            await Mediator.Send(subject);
            return RedirectToAction("GetAll");
        }



        public async ValueTask<IActionResult> Delete(Guid Id)
        {
            await Mediator.Send(new DeleteSubjectCommand(Id));
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public async ValueTask<IActionResult> Update(SubjectDto subject)
        {
            var item = await Mediator.Send(new GetAllTeacherQuery());
            ViewData["Teachers"] = item;
            return View(subject);
        }

        [HttpPost]
        public async ValueTask<IActionResult> Update([FromForm] UpdateSubjectCommand subject)
        {
            await Mediator.Send(subject);
            return RedirectToAction("GetAll");
        }
    }
}
