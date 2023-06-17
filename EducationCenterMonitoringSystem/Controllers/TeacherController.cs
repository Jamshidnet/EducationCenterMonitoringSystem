using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MonitoringSystem.Application.UseCases.Teachers.Commands.CreateTeacher;
using MonitoringSystem.Application.UseCases.Teachers.Commands.DeleteTeacher;
using MonitoringSystem.Application.UseCases.Teachers.Commands.UpdateTeacher;
using MonitoringSystem.Application.UseCases.Teachers.Models;
using MonitoringSystem.Application.UseCases.Teachers.Queries.GetAllTeachers;
using MonitoringSystem.Application.UseCases.Teachers.Queries.GetTeacher;

namespace EducationCenterMonitoringSystem.Controllers
{
    public class TeacherController : ApiBaseController
    {
        public TeacherController(IAppCache appCache, IConfiguration configuration)
        {
            _appCache = appCache;
            _configuration = configuration;
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetAll(GetAllTeacherQuery query)
        {
                return View(await Mediator.Send(query));
        }


        [HttpGet]
        public async ValueTask<IActionResult> Details(GetTeacherQuery teacher)
        {
            return View(await Mediator.Send(teacher));
        }

       

        [HttpGet]
        public async ValueTask<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        public async ValueTask<IActionResult> Create([FromForm] CreateTeacherCommand teacher)
        {
            TeacherDto dto = await Mediator.Send(teacher);
            return RedirectToAction("GetAll");
        }



        public async ValueTask<IActionResult> Delete(Guid Id)
        {
            await Mediator.Send(new DeleteTeacherCommand(Id));
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public async ValueTask<IActionResult> Update(TeacherWithSubjectsDto teacher)
        {
            return View(teacher);
        }

        [HttpPost]
        public async ValueTask<IActionResult> Update([FromForm] UpdateTeacherCommand teacher)
        {
            await Mediator.Send(teacher);
            return RedirectToAction("GetAll");
        }


    }
}
