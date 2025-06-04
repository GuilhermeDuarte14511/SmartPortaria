using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartPortaria.Application.DTOs;
using SmartPortaria.Application.Interfaces;
using SmartPortaria.Controllers;
using System.Security.Claims;
using Xunit;
namespace SmartPortaria.Tests;

public class UsuarioControllerTests
{
    [Fact]
    public async Task CadastrarViaModal_ReturnsBadRequest_WhenRequiredDataMissing()
    {
        var serviceMock = new Mock<IUsuarioService>();
        var controller = new UsuarioController(serviceMock.Object);

        var result = await controller.CadastrarViaModal(new UsuarioCadastroModalRequest());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CadastrarViaModal_ReturnsUnauthorized_WhenUserNotAuthenticated()
    {
        var serviceMock = new Mock<IUsuarioService>();
        var controller = new UsuarioController(serviceMock.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var req = new UsuarioCadastroModalRequest { Nome = "Teste", Documento = "123", VetorFacial = System.Text.Json.JsonSerializer.Serialize(new float[] { 1 }) };
        var result = await controller.CadastrarViaModal(req);

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task CadastrarViaModal_ReturnsOk_WhenSuccess()
    {
        var serviceMock = new Mock<IUsuarioService>();
        serviceMock.Setup(s => s.CadastrarUsuarioAsync(It.IsAny<UsuarioCadastroModalRequest>())).ReturnsAsync(true);
        var controller = new UsuarioController(serviceMock.Object);
        var userId = Guid.NewGuid().ToString();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) }))
            }
        };

        var req = new UsuarioCadastroModalRequest
        {
            Nome = "Teste",
            Documento = "123",
            VetorFacial = System.Text.Json.JsonSerializer.Serialize(new float[] { 1 })
        };
        var result = await controller.CadastrarViaModal(req);

        Assert.IsType<OkObjectResult>(result);
    }
}
