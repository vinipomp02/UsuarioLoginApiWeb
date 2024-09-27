using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UsuarioApiWeb.Services
{
    public class TokenService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public TokenService(UserManager<IdentityUser> userManager, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _tokenValidationParameters = tokenValidationParameters;
        }
        /// <summary>
        /// Gera o Token JWT
        /// </summary>
        /// <param name="user">Usuario que será associado ao token</param>
        /// <returns>Token JWT</returns>
        public string GenerateToken(IdentityUser user)
        {
            Claim[] claims = new Claim[]
            {
                new Claim("id",user.Id),
                new Claim(ClaimTypes.Email,user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SAJND8ehjknJSAD8sadn8JSDn82Fg441ggh67g"));
            var signCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(expires: DateTime.Now.AddMinutes(30),
                claims: claims,
                signingCredentials: signCredentials
                );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Verifica se o Token é Válida
        /// </summary>
        /// <param name="token">token JWT</param>
        /// <returns>Bool de validade do Token</returns>
        public bool IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Usa os parâmetros de validação injetados
                tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);
                return true; // O token é válido
            }
            catch (SecurityTokenExpiredException)
            {
                return false; // O token expirou
            }
            catch (Exception)
            {
                return false; // Qualquer outro erro de validação
            }
        }
    }
}
