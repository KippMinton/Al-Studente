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
        private readonly ITeacherNoteRepository _teacherNoteRepository;

        public HomeController(IUserProfileRepository userProfileRepository, ITeacherRepository teacherRepository,
                              IStudentRepository studentRepository, IInstrumentRepository instrumentRepository,
                              ILessonDayRepository lessonDayRepository, ILessonTimeRepository lessonTimeRepository,
                              ILevelRepository levelRepository, ITeacherNoteRepository teacherNoteRepository)
        {
            _userProfileRepository = userProfileRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _instrumentRepository = instrumentRepository;
            _lessonDayRepository = lessonDayRepository;
            _lessonTimeRepository = lessonTimeRepository;
            _levelRepository = levelRepository;
            _teacherNoteRepository = teacherNoteRepository;
        }

        public IActionResult Index()
        {
            var userProfileId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userProfile = _userProfileRepository.GetById(userProfileId);
            var teacher = _teacherRepository.GetByUserId(userProfileId);
            var teacherInst = _instrumentRepository.GetById(userProfile.InstrumentId);

            List<StudentUserViewModel> students = _studentRepository.GetAllByTeacher(teacher.Id);
            
            TeacherUserViewModel vm = new TeacherUserViewModel
            {
                UserProfile = userProfile,
                Teacher = teacher,
                Students = students,
                Instrument = teacherInst
            };

            return View(vm);
        }

        public IActionResult StudentDetails(int id)
        {
            var studentUser = _userProfileRepository.GetById(id);
            var student = _studentRepository.GetByUserId(id);
            var teacherUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var teacherUser = _userProfileRepository.GetById(teacherUserId);
            var teacher = _teacherRepository.GetByUserId(teacherUserId);
            var lessonDay = _lessonDayRepository.GetById(student.LessonDayId);
            var lessonTime = _lessonTimeRepository.GetById(student.LessonTimeId);
            var instrument = _instrumentRepository.GetById(studentUser.InstrumentId);
            var level = _levelRepository.GetById(student.LevelId);
            var teacherNotes = _teacherNoteRepository.GetAllByStudentId(student.Id);

            StudentUserViewModel vm = new StudentUserViewModel
            {
                UserProfile = studentUser,
                Student = student,
                Teacher = teacher,
                TeacherUser = teacherUser,
                LessonDay = lessonDay,
                LessonTime = lessonTime,
                Instrument = instrument,
                Level = level,
                Notes = teacherNotes
            };

            return View(vm);
        }

        public IActionResult DeleteFromRoster(int id)
        {
            var student = _studentRepository.GetByUserId(id);
            _studentRepository.DeleteFromRoster(student);
            return RedirectToAction("Index");
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