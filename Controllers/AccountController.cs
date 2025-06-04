using Microsoft.AspNetCore.Mvc;
using SmartPortaria.Application.DTOs;
using SmartPortaria.Application.Interfaces;
using SmartPortaria.Domain.Entities;

namespace SmartPortaria.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly IAdminService _adminService;

        public AccountController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View("RegisterAccount");
        }

        [HttpPost("CriarUsuario")]
        public async Task<IActionResult> CriarUsuarioAdmin([FromBody] CriarAdminRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Senha) ||
                string.IsNullOrWhiteSpace(request.Nome))
            {
                return BadRequest(new { sucesso = false, mensagem = "Todos os campos são obrigatórios." });
            }

            var adminExistente = await _adminService.ObterPorEmailAsync(request.Email);
            if (adminExistente != null)
            {
                return Conflict(new { sucesso = false, mensagem = "Já existe um administrador com este email." });
            }

            var novoAdmin = new Admin
            {
                Nome = request.Nome,
                Email = request.Email
            };

            await _adminService.CriarAsync(novoAdmin, request.Senha);

            return Ok(new { sucesso = true, mensagem = "Administrador criado com sucesso." });
        }
    }
}
