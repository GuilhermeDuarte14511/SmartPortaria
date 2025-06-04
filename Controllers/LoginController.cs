using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SmartPortaria.Application.DTOs;
using SmartPortaria.Application.Interfaces;
using System.Security.Claims;

namespace SmartPortaria.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAdminService _adminService;

        public LoginController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Autenticar")]
        public async Task<IActionResult> Autenticar([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
                return BadRequest(new { sucesso = false, mensagem = "Email e senha são obrigatórios." });

            var admin = await _adminService.ObterPorEmailAsync(request.Email);
            if (admin == null || !await _adminService.VerificarLoginAsync(request.Email, request.Senha))
                return Unauthorized(new { sucesso = false, mensagem = "Credenciais inválidas." });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Name, admin.Nome),
                new Claim(ClaimTypes.Email, admin.Email)
            };

            var identidade = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identidade);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(new { sucesso = true, mensagem = "Login realizado com sucesso." });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { sucesso = true, mensagem = "Logout realizado com sucesso." });
        }

        [HttpGet("AcessoNegado")]
        public IActionResult AcessoNegado()
        {
            return Unauthorized(new { sucesso = false, mensagem = "Acesso negado." });
        }
    }
}
