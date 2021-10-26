using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlStudente.Models;
using Microsoft.AspNetCore.Authorization;
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

        public HomeController(IUserProfileRepository userProfileRepository, ITeacherRepository teacherRepository)
        {
            _userProfileRepository = userProfileRepository;
            _teacherRepository = teacherRepository;
        }

        public IActionResult Index()
        {
            var userProfileId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userProfile = _userProfileRepository.GetById(userProfileId);
            var teacher = _teacherRepository.GetByUserId(userProfileId);

            TeacherUserViewModel vm = new TeacherUserViewModel
            {
                UserProfile = userProfile,
                Teacher = teacher
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
