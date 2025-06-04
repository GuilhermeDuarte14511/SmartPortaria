using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartPortaria.Application.DTOs;
using SmartPortaria.Application.Interfaces;
using SmartPortaria.Controllers;

namespace SmartPortaria.Tests;

public class LoginControllerTests
{
    [Fact]
    public async Task Autenticar_ReturnsBadRequest_WhenEmailOrSenhaMissing()
    {
        var serviceMock = new Mock<IAdminService>();
        var controller = new LoginController(serviceMock.Object);

        var result = await controller.Autenticar(new LoginRequest { Email = "", Senha = "" });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Autenticar_ReturnsUnauthorized_WhenLoginInvalid()
    {
        var serviceMock = new Mock<IAdminService>();
        serviceMock.Setup(s => s.ObterPorEmailAsync("admin@test"))
                    .ReturnsAsync(new AdminDto { Id = Guid.NewGuid(), Nome = "Admin", Email = "admin@test" });
        serviceMock.Setup(s => s.VerificarLoginAsync("admin@test", "wrong"))
                    .ReturnsAsync(false);
        var controller = new LoginController(serviceMock.Object);

        var result = await controller.Autenticar(new LoginRequest { Email = "admin@test", Senha = "wrong" });

        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}
