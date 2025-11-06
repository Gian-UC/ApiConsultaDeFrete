using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using EnvioRapidoApi.Controllers;
using EnvioRapidoApi.Repositories;
using EnvioRapidoApi.Services;
using EnvioRapidoApi.DTOs;
using EnvioRapidoApi.Models;
using System.Threading.Tasks;

public class EnviosControllerPostTests
{
    [Fact]
    public async Task Post_DeveRetornarAccepted_QuandoEnvioForCriado()
    {
        // Arrange
        var dto = new EnvioDTO
        {
            OrigemCep = "01001000",
            DestinoCep = "30140071",
            Peso = 2,
            Altura = 20,
            Largura = 14,
            Comprimento = 30
        };

        // Mock ViaCep
        var viaCepMock = new Mock<IViaCepService>();
        viaCepMock.Setup(v => v.ValidarCepAsync(It.IsAny<string>())).ReturnsAsync(true);

        // Mock MelhorEnvio
        var melhorEnvioMock = new Mock<IMelhorEnvioService>();
        melhorEnvioMock.Setup(m => m.CalcularFreteAsync(It.IsAny<Envio>()))
            .ReturnsAsync(23.72m);

        // Mock Repository
        var repoMock = new Mock<IEnvioRepository>();
        repoMock.Setup(r => r.CriarAsync(It.IsAny<Envio>()))
            .Returns(Task.CompletedTask);

        // Mock RabbitMQ
        var rabbitMock = new Mock<IRabbitMqService>();

        var controller = new EnviosController(
            viaCepMock.Object,
            melhorEnvioMock.Object,
            repoMock.Object,
            rabbitMock.Object
        );

        // Act
        var resultado = await controller.Post(dto);

        // Assert
        var accepted = Assert.IsType<AcceptedResult>(resultado);
        Assert.NotNull(accepted.Value);

        // Verifica se os serviços foram chamados
        viaCepMock.Verify(v => v.ValidarCepAsync(dto.OrigemCep), Times.Once);
        melhorEnvioMock.Verify(m => m.CalcularFreteAsync(It.IsAny<Envio>()), Times.Once);
        repoMock.Verify(r => r.CriarAsync(It.IsAny<Envio>()), Times.Once);
        rabbitMock.Verify(r => r.PublicarMensagem(It.IsAny<object>()), Times.Once);
    }
}
