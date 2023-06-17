using Microsoft.AspNetCore.Mvc;
using MonitoringSystem.Application.UseCases.Filters;
using MonitoringSystem.Application.UseCases.StudentsListBy20Age;
using MonitoringSystem.Application.UseCases.Teachers.Queries.GetAllTeachers;

namespace EducationCenterMonitoringSystem.Controllers
{
    public class FiltersController : ApiBaseController
    {
        [HttpGet]
        public async Task<IActionResult> StudentsBy20Age(StudentsLIstBy20AgeQuery students) =>
            View(await Mediator.Send(students));

        [HttpGet]
        public async Task<IActionResult> SortedStudents(SortedStudentsQuery students) =>
            View(await Mediator.Send(students));

        [HttpGet]
        public async Task<IActionResult> TeachersOverFifty(TeacherOverFiftyQuery teachers) =>
            View(await Mediator.Send(teachers));
         
        [HttpGet]
        public async Task<IActionResult> BeelineUsers(BeelineUsersQuery people) =>
            View(await Mediator.Send(people));
        
        [HttpGet]
        public async Task<IActionResult> SearchedStudents(SearchedStudents students) =>
            View(await Mediator.Send(students));

        [HttpGet]
        public async Task<IActionResult> BestSubject()
        {
            ViewData["Teachers"] = await Mediator.Send(new GetAllTeacherQuery());
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BestSubjects(BestSubjectQuery students)
        {
            ViewData["Teachers"] = await Mediator.Send(new GetAllTeacherQuery());
            return View(await Mediator.Send(students));
        }

        [HttpGet]
        public async Task<IActionResult> FilteredTeachers(FilteredTeacherQuery teachers) =>
            View(await Mediator.Send(teachers));
    }
}
