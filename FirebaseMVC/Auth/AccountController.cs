using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AlStudente.Auth.Models;
using AlStudente.Repositories;
using AlStudente.Models;
using AlStudente.Models.ViewModels;
using Microsoft.Extensions.Configuration;


namespace AlStudente.Auth
{
    public class AccountController : Controller
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly ILessonDayRepository _lessonDayRepository;
        private readonly ILessonTimeRepository _lessonTimeRepository;
        private readonly ILevelRepository _levelRepository;


        public AccountController(IFirebaseAuthService firebaseAuthService, IUserProfileRepository userProfileRepository, 
                                 ITeacherRepository teacherRepository, IStudentRepository studentRepository,
                                 IInstrumentRepository instrumentRepository,ILessonDayRepository lessonDayRepository,
                                 ILessonTimeRepository lessonTimeRepository, ILevelRepository levelRepository)
        {
            _userProfileRepository = userProfileRepository;
            _firebaseAuthService = firebaseAuthService;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _instrumentRepository = instrumentRepository;
            _lessonDayRepository = lessonDayRepository;
            _lessonTimeRepository = lessonTimeRepository;
            _levelRepository = levelRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return View(credentials);
            }

            var fbUser = await _firebaseAuthService.Login(credentials);
            if (fbUser == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(credentials);
            }

            var userProfile = _userProfileRepository.GetByFirebaseUserId(fbUser.FirebaseUserId);
            if (userProfile == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to Login.");
                return View(credentials);
            }

            await LoginToApp(userProfile);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            List<Instrument> instruments = _instrumentRepository.GetAll();

            var regForm = new Registration
            {
                Instruments = instruments
            };

            return View(regForm);
        }

        [HttpPost]
        public async Task<IActionResult> Register(Registration registration)
        {
            if (!ModelState.IsValid)
            {
                return View(registration);
            }

            var fbUser = await _firebaseAuthService.Register(registration);

            if (fbUser == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to register, do you already have an account?");
                return View(registration);
            }

            var newUserProfile = new UserProfile
            {
                Email = fbUser.Email,
                FirebaseUserId = fbUser.FirebaseUserId,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                DisplayName = registration.DisplayName,
                UserTypeId = 2,
                InstrumentId = registration.InstrumentId,
                Bio = registration.Bio
            };
            _userProfileRepository.Add(newUserProfile);

            var newTeacher = new Teacher
            {
                UserId = newUserProfile.Id,
                LessonRate = registration.Rate,
                AcceptingStudents = registration.AcceptingStudents
            };
            _teacherRepository.Add(newTeacher);

            await LoginToApp(newUserProfile);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult EditTeacher()
        {
            var user = _userProfileRepository.GetById(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var teacher = _teacherRepository.GetByUserId(user.Id);
            List<Instrument> instruments = _instrumentRepository.GetAll();

            TeacherEditViewModel vm = new TeacherEditViewModel
            {
                UserProfile = user,
                Teacher = teacher,
                Instruments = instruments
            };

            if (user == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTeacher(TeacherEditViewModel teacherEditVM)
        { 
            var userProfile = new UserProfile
            {
                Id = teacherEditVM.UserProfile.Id,
                FirstName = teacherEditVM.UserProfile.FirstName,
                LastName = teacherEditVM.UserProfile.LastName,
                DisplayName = teacherEditVM.UserProfile.DisplayName,
                InstrumentId = teacherEditVM.UserProfile.InstrumentId,
                ImageLocation = teacherEditVM.UserProfile.ImageLocation,
                Bio = teacherEditVM.UserProfile.Bio
            };

            var teacher = new Teacher
            {
                UserId = teacherEditVM.UserProfile.Id,
                AcceptingStudents = teacherEditVM.Teacher.AcceptingStudents,
                LessonRate = teacherEditVM.Teacher.LessonRate
            };
          
            _userProfileRepository.Update(userProfile);
            _teacherRepository.Update(teacher);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult RegisterStudent()
        {
            List<Instrument> instruments = _instrumentRepository.GetAll();
            List<Level> levels = _levelRepository.GetAll();
            List<LessonDay> days = _lessonDayRepository.GetAll();
            List<LessonTime> times = _lessonTimeRepository.GetAll();

            var studentRegForm = new StudentRegistration
            {
                Instruments = instruments,
                Levels = levels,
                Days = days,
                Times = times
            };

            return View(studentRegForm);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterStudent(StudentRegistration studentRegistration)
        {
            if (!ModelState.IsValid)
            {
                return View(studentRegistration);
            }

            var fbUser = await _firebaseAuthService.RegisterStudent(studentRegistration);

            if (fbUser == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to register, do you already have an account?");
                return View(studentRegistration);
            }

            var newUserProfile = new UserProfile
            {
                Email = fbUser.Email,
                FirebaseUserId = fbUser.FirebaseUserId,
                FirstName = studentRegistration.FirstName,
                LastName = studentRegistration.LastName,
                DisplayName = studentRegistration.DisplayName,
                InstrumentId = studentRegistration.InstrumentId,
                UserTypeId = 3
            };
            _userProfileRepository.Add(newUserProfile);

            var teacherUserProfileId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var teacher = _teacherRepository.GetByUserId(teacherUserProfileId);

            var newStudent = new Student
            {
                UserId = newUserProfile.Id,
                TeacherId = teacher.Id,
                DOB = studentRegistration.DOB,
                StartDate = studentRegistration.StartDate,
                PlayingSince = studentRegistration.PlayingSince,
                LevelId = studentRegistration.LevelId,
                LessonDayId = studentRegistration.LessonDayId,
                LessonTimeId = studentRegistration.LessonTimeId
            };
            _studentRepository.Add(newStudent);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult EditStudent(int id)
        {
            var studentUser = _userProfileRepository.GetById(id);
            var student = _studentRepository.GetByUserId(id);
            var teacherUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var teacherUser = _userProfileRepository.GetById(teacherUserId);
            var teacher = _teacherRepository.GetByUserId(teacherUserId);
            List<LessonDay> lessonDays = _lessonDayRepository.GetAll();
            List<LessonTime> lessonTimes = _lessonTimeRepository.GetAll();
            List<Instrument> instruments = _instrumentRepository.GetAll();
            List<Level> levels = _levelRepository.GetAll();

            StudentEditViewModel vm = new StudentEditViewModel
            {
                UserProfile = studentUser,
                Student = student,
                Teacher = teacher,
                TeacherUser = teacherUser,
                LessonDays = lessonDays,
                LessonTimes = lessonTimes,
                Instruments = instruments,
                Levels = levels
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStudent(int id, StudentEditViewModel studentEditVM)
        {
            var userProfile = new UserProfile
            {
                Id = studentEditVM.UserProfile.Id,
                FirstName = studentEditVM.UserProfile.FirstName,
                LastName = studentEditVM.UserProfile.LastName,
                DisplayName = studentEditVM.UserProfile.DisplayName,
                InstrumentId = studentEditVM.UserProfile.InstrumentId,
                ImageLocation = studentEditVM.UserProfile.ImageLocation,
                Bio = studentEditVM.UserProfile.Bio
            };

            var student = new Student
            {
                UserId = studentEditVM.UserProfile.Id,
                TeacherId = studentEditVM.Student.TeacherId,
                DOB = studentEditVM.Student.DOB,
                StartDate = studentEditVM.Student.StartDate,
                PlayingSince = studentEditVM.Student.PlayingSince,
                LevelId = studentEditVM.Student.LevelId,
                LessonDayId = studentEditVM.Student.LessonDayId,
                LessonTimeId = studentEditVM.Student.LessonTimeId
            };

            _userProfileRepository.Update(userProfile);
            _studentRepository.Update(student);

            return RedirectToAction("StudentDetails", "Home", new { id = id });
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task LoginToApp(UserProfile userProfile)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }
    }
}
