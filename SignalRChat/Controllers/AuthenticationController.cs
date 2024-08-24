using CoreLayer.Services.Users;
using CoreLayer.ViewModels.authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SignalRChat.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;
        public AuthenticationController(IUserService service)
        {
            _userService = service;
        }
        public IActionResult ShowsAuthentication()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ShowsAuthentication", model);
            }
            var result = await _userService.RegisterUser(model);
            if (!result)
            {
                ModelState.AddModelError(model.Username, "نام کاربری تکراری هست");
                return View("ShowsAuthentication", model);
            }
            return Redirect("/Authentication#login");
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View("ShowsAuthentication", model);

            var user = await _userService.LoginUser(model);
            if (user == null)
            {
                return View("ShowsAuthentication", model);
            }

            var claim = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username.ToString())
            };
            var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            var principle = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties() { IsPersistent = true };
            await HttpContext.SignInAsync(principle, properties);

            return Redirect("/");
        }
    }
}
