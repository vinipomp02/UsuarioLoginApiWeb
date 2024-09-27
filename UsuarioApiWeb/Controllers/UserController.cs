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
        private readonly SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private TokenService _tokenService;

        public UserController(UserService userService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, TokenService tokenService)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }


        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        // Ação para a página de Cadastro
        [HttpGet("cadastro")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet("sucess")]
        public IActionResult Success()
        {
            // Verifica se o cookie do JWT existe

            if (!Request.Cookies.TryGetValue("JWT", out var token) || !_tokenService.IsTokenValid(token))
            {
                // Redireciona para a tela de login com uma mensagem de erro
                TempData["ErrorMessage"] = "Acesso Negado. Para acessar a tela faça o login";
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // Verifica se o modelo é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tenta registrar o usuário
            var result = await _userService.RegisterUserAsync(model);
            if (!result.Success)
            {
                return result.Conflict ? Conflict(result.Message) : BadRequest(result.Errors);
            }

            // Armazena a mensagem de sucesso no TempData
            TempData["SuccessMessage"] = result.Message;

            // Retorna a resposta com redirecionamento
            return Ok(new { message = result.Message, redirectUrl = Url.Action("Login", "User") });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Remove o cookie com o token
            Response.Cookies.Delete("JWT");

            TempData["SuccessMessage"] = "Logout Feito com Sucesso";

            // Redireciona para a página de login
            return Ok(new { message = "Logout Feito com Sucesso!", redirectUrl = Url.Action("Login", "User") });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tenta realizar o login
            var result = await _userService.LoginUserAsync(model);

            if (!result.Success)
            {
                return result.NotFound ? NotFound(result.Message) : BadRequest(result.Message);
            }

            // Adiciona o token no cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Habilitar se estiver usando HTTPS
                SameSite = SameSiteMode.Lax
            };
            Response.Cookies.Append("JWT", result.Token, cookieOptions);

            // Retorna o resultado de sucesso
            return Ok(new { message = result.Message, Token = result.Token });
        }
    }
}
