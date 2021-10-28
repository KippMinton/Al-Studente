using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AlStudente.Auth.Models;
using AlStudente.Repositories;
using AlStudente.Models;
using Microsoft.Extensions.Configuration;

namespace AlStudente.Auth
{
    public class AccountController : Controller
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;

        public AccountController(IFirebaseAuthService firebaseAuthService, IUserProfileRepository userProfileRepository, ITeacherRepository teacherRepository, IStudentRepository studentRepository)
        {
            _userProfileRepository = userProfileRepository;
            _firebaseAuthService = firebaseAuthService;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
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
            return View();
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
                UserTypeId = 2
            };
            _userProfileRepository.Add(newUserProfile);

            var newTeacher = new Teacher
            {
                UserId = newUserProfile.Id
            };
            _teacherRepository.Add(newTeacher);

            await LoginToApp(newUserProfile);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult RegisterStudent()
        {
            return View();
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
                UserTypeId = 3
            };
            _userProfileRepository.Add(newUserProfile);

            var newStudent = new Student
            {
                UserId = newUserProfile.Id,
                TeacherId = 1,
                DOB = studentRegistration.DOB,
                StartDate = studentRegistration.StartDate,
                PlayingSince = studentRegistration.PlayingSince,
                LevelId = studentRegistration.LevelId,
                LessonDayId = studentRegistration.LessonDayId,
                LessonTimeId = studentRegistration.LessonTimeId
            };
            _studentRepository.Add(newStudent);

            await LoginToApp(newUserProfile);

            return RedirectToAction("Index", "Home");
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
