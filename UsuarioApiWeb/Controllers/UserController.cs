using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsuarioApiWeb.Models;
using UsuarioApiWeb.Services;

namespace UsuarioApiWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private TokenService _tokenService;

        public UserController(UserService userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("cadastro")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet("sucess")]
        public IActionResult Success()
        {
            if (!Request.Cookies.TryGetValue("JWT", out var token) || !_tokenService.IsTokenValid(token))
            {
                TempData["ErrorMessage"] = "Acesso Negado. Para acessar a tela faça o login";
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterUserAsync(model);
            if (!result.Success)
            {
                return result.Conflict ? Conflict(result.Message) : BadRequest(result.Errors);
            }

            TempData["SuccessMessage"] = result.Message;

            return Ok(new { message = result.Message, redirectUrl = Url.Action("Login", "User") });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JWT");

            TempData["SuccessMessage"] = "Logout Feito com Sucesso";

            return Ok(new { message = "Logout Feito com Sucesso!", redirectUrl = Url.Action("Login", "User") });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.LoginUserAsync(model);

            if (!result.Success)
            {
                return result.NotFound ? NotFound(result.Message) : BadRequest(result.Message);
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            };
            Response.Cookies.Append("JWT", result.Token, cookieOptions);

            return Ok(new { message = result.Message, Token = result.Token });
        }
    }
}
