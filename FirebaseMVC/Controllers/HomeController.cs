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

            //check for double bookings by building a list of each student's
            //LessonDayId and LessonTimeId as strings
            //if duplicates exist a lesson time is double booked
            //teacher is redirected to index view with an alert
            var bookings = new List<string>();

            foreach(StudentUserViewModel svm in students)
            {
                string bookingString = svm.Student.LessonDayId.ToString() + svm.Student.LessonTimeId.ToString();
                if(bookings.Contains(bookingString))
                {
                    return RedirectToAction("IndexWithDoubleBooking");
                }
                bookings.Add(bookingString);
            }


            return View(vm);
        }

        public IActionResult IndexWithDoubleBooking()
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

            StudentUserViewModel vm = _studentRepository.GetStudentVMByUserId(id);
            var teacherUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var teacherUser = _userProfileRepository.GetById(teacherUserId);
            var teacher = _teacherRepository.GetByUserId(teacherUserId);
            var teacherNotes = _teacherNoteRepository.GetAllByStudentId(vm.Student.Id);

            vm.Teacher = teacher;
            vm.TeacherUser = teacherUser;
            vm.Notes = teacherNotes;

            return View(vm);
        }

        public IActionResult DeleteFromRoster(int id)
        {
            var student = _studentRepository.GetByUserId(id);
            var studentUser = _userProfileRepository.GetById(student.UserId);
            var teacherUserProfileId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var teacher = _teacherRepository.GetByUserId(teacherUserProfileId);

            var vm = new StudentUserViewModel
            {
                Student = student,
                UserProfile = studentUser,
                Teacher = teacher
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFromRoster(Student student)
        {
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