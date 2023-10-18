using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonPicture.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonPicture.Controllers
{


    public class AccountController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;

        public AccountController(UserManager<Person> userManager, SignInManager<Person> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Person { Email = model.Email, UserName = model.Email };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await _signInManager.SignInAsync(user, true);
                    return Ok();
                }


            }
            return NoContent();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }
       
        [HttpPost("Login")]
  
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return Ok();                  
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return BadRequest();

        }

        [HttpPost("Logout")]  
        public async Task<IActionResult> Logout()
        {            
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Person");
        }
    }
}
