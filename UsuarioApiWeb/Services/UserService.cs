using Microsoft.AspNetCore.Identity;
using UsuarioApiWeb.Models;

namespace UsuarioApiWeb.Services
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly TokenService _tokenService;

        public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        /// <summary>
        /// Realiza operação de cadastro
        /// </summary>
        /// <param name="model">Model para processamento de informações</param>
        /// <returns>Task async de cadastro</returns>
        public async Task<UserRegistrationResult> RegisterUserAsync(RegisterModel model)
        {
            // Verifica se o usuário já existe
            if (await GetUserByEmailAsync(model.Email) != null)
            {
                return new UserRegistrationResult
                {
                    Success = false,
                    Conflict = true,
                    Message = "Usuário já existe."
                };
            }

            // Cria um novo usuário
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            // Tenta registrar o usuário
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return new UserRegistrationResult
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            return new UserRegistrationResult
            {
                Success = true,
                Message = "Usuário cadastrado com sucesso!"
            };
        }

        /// <summary>
        /// Realiza operação de Login
        /// </summary>
        /// <param name="model">Model para processamento de informações</param>
        /// <returns>Task async do login</returns>
        public async Task<UserLoginResult> LoginUserAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new UserLoginResult
                {
                    Success = false,
                    NotFound = true,
                    Message = "Usuário não encontrado."
                };
            }

            // Verifica se o usuário está bloqueado
            if (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow)
            {
                return new UserLoginResult
                {
                    Success = false,
                    Message = $"Usuário bloqueado. Liberado em {GetFormattedTimeRemaining(user.LockoutEnd.Value)}."
                };
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                int remainingAttempts = 5 - user.AccessFailedCount;

                // Verifica se o usuário deve ser bloqueado
                if (user.AccessFailedCount == 0)
                {
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(15)); // Bloqueia por 15 minutos
                    return new UserLoginResult
                    {
                        Success = false,
                        Message = $"Usuário bloqueado. Liberado em {GetFormattedTimeRemaining(user.LockoutEnd.Value)}."
                    };
                }

                return new UserLoginResult
                {
                    Success = false,
                    Message = $"Senha incorreta. Tentativas restantes: {remainingAttempts}"
                };
            }

            // Gera o token JWT
            var token = _tokenService.GenerateToken(user);
            return new UserLoginResult
            {
                Success = true,
                Message = "Login bem-sucedido!",
                Token = token
            };
        }
        /// <summary>
        /// Classe de Resultado de cadastro
        /// </summary>
        public class UserRegistrationResult
        {
            public bool Success { get; set; }
            public bool Conflict { get; set; }
            public string Message { get; set; }
            public List<string> Errors { get; set; }
        }
        /// <summary>
        /// Classe de Resultado de login
        /// </summary>
        public class UserLoginResult
        {
            public bool Success { get; set; }
            public bool NotFound { get; set; }
            public string Message { get; set; }
            public string Token { get; set; }
        }

        /// <summary>
        /// Busca um usuario a partir de um e-mail
        /// </summary>
        /// <param name="email">email para busca</param>
        /// <returns>Task async de busca de e-mail</returns>
        public async Task<IdentityUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Formata tempo restante para nova tentativa de acesso
        /// </summary>
        /// <param name="lockoutEndTime">Date Time de final do lockout</param>
        /// <returns>String formatado</returns>
        public string GetFormattedTimeRemaining(DateTimeOffset lockoutEndTime)
        {
            var timeRemaining = lockoutEndTime - DateTimeOffset.UtcNow;
            return string.Format("{0:D2}:{1:D2}", (int)timeRemaining.TotalMinutes, timeRemaining.Seconds);
        }

        /// <summary>
        /// Metodo para Criacao Automatica de Registros para Fins de Teste
        /// </summary>
        /// <returns>Await task de criacao de usuarios padrão</returns>
        public async Task CreateDefaultUsersAsync()
        {
            // Criar a lista de usuários manualmente
            var users = new List<IdentityUser>
        {
            new IdentityUser { Email = "vinipomp@gmail.com", UserName = "vinipomp@gmail.com" },
            new IdentityUser { Email = "pompvini@hotmail.com", UserName = "pompvini@hotmail.com" },
            new IdentityUser { Email = "vinivini@yahoo.com", UserName = "vinivini@yahoo.com" },
            new IdentityUser { Email = "pomppomp@outlook.com", UserName = "pomppomp@outlook.com" },
        };

            // Iterar sobre a lista de usuários e registrá-los
            foreach (var user in users)
            {
                // Verifica se o usuário já existe
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser == null)
                {
                    var password = "Password123!"; // Senha padrão
                    var result = await _userManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                    {
                        // Lidar com erros, se necessário
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine(error.Description); // Para depuração
                        }
                    }
                }
            }
        }
    }
}
