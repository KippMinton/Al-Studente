using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlStudente.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using AlStudente.Repositories;
using AlStudente.Models.ViewModels;

namespace AlStudente.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly ILessonDayRepository _lessonDayRepository;
        private readonly ILessonTimeRepository _lessonTimeRepository;
        private readonly ILevelRepository _levelRepository;

        public HomeController(IUserProfileRepository userProfileRepository, ITeacherRepository teacherRepository,
                              IStudentRepository studentRepository, IInstrumentRepository instrumentRepository,
                              ILessonDayRepository lessonDayRepository, ILessonTimeRepository lessonTimeRepository,
                              ILevelRepository levelRepository)
        {
            _userProfileRepository = userProfileRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _instrumentRepository = instrumentRepository;
            _lessonDayRepository = lessonDayRepository;
            _lessonTimeRepository = lessonTimeRepository;
            _levelRepository = levelRepository;
        }

        public IActionResult Index()
        {
            var userProfileId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userProfile = _userProfileRepository.GetById(userProfileId);
            var teacher = _teacherRepository.GetByUserId(userProfileId);
            List<StudentUserViewModel> students = _studentRepository.GetAllByTeacher(teacher.Id);
            
            TeacherUserViewModel vm = new TeacherUserViewModel
            {
                UserProfile = userProfile,
                Teacher = teacher,
                Students = students
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
