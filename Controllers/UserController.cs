using dotnetEtsyApp.Helpers.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace dotnetEtsyApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;
        public UserController(ILogger<UserController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string username, string password, string returnUrl)
        {
            var result = await _userService.Login(username, password);
            if (result.Success)
            {
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = result.Message;
            }
            return View();
        }
        [Route("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _userService.Logout();
            return RedirectToAction("Index", "Home");
        }
        [Route("GrantAccess")]
        public IActionResult GrantAccess()
        {
            return View();
        }
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(string username, string password, string email)
        {
            var result = _userService.Register(username, password, email, "");
            if (result.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = result.Message;
            }
            return View();
        }



    }
}