using Microsoft.AspNetCore.Mvc;
using SmartPortaria.Application.DTOs;
using SmartPortaria.Application.Interfaces;
using System.Security.Claims;

namespace SmartPortaria.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("CadastrarViaModal")]
        public async Task<IActionResult> CadastrarViaModal([FromBody] UsuarioCadastroModalRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome) || string.IsNullOrWhiteSpace(request.Documento) || request.VetorFacial is null)
            {
                return BadRequest(new { sucesso = false, mensagem = "Dados obrigatórios ausentes." });
            }

            var adminIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(adminIdStr, out Guid adminId))
            {
                return Unauthorized(new { sucesso = false, mensagem = "Usuário não autenticado." });
            }

            request.CadastradoPorId = adminId;

            var sucesso = await _usuarioService.CadastrarUsuarioAsync(request);
            if (sucesso)
                return Ok(new { sucesso = true });

            return BadRequest(new { sucesso = false, mensagem = "Erro ao salvar usuário." });
        
        }

        [HttpPost("ReconhecerFace")]
        public async Task<IActionResult> ReconhecerFace([FromBody] ReconhecimentoFacialRequest request)
        {
            if (request?.VetorFacial == null || !request.VetorFacial.Any())
                return BadRequest(new { sucesso = false, mensagem = "Vetor facial inválido." });

            var usuario = await _usuarioService.ReconhecerUsuarioAsync(request.VetorFacial);
            if (usuario != null)
            {
                return Ok(new
                {
                    sucesso = true,
                    usuario = new
                    {
                        usuario.Nome,
                        usuario.Documento,
                        usuario.Email,
                        tipo = (int)usuario.Tipo,
                        usuario.EnderecoResidencial,
                        usuario.Observacao,
                        fotoBase64 = usuario.FotoBase64
                    }
                });
            }

            return Ok(new { sucesso = false });
        }


    }
}
