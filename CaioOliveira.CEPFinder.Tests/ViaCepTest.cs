using System.Net.Http;
using System.Threading.Tasks;
using CaioOliveira.CepFinder.ViaCep;
using CaioOliveira.CepFinder.ViaCep.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CaioOliveira.CepFinder.Tests
{
    [TestClass]
    public class ViaCepTest
    {
        private readonly Provider _viaCepProvider;

        public ViaCepTest()
        {
            var loggerMock = new Mock<ILogger<Provider>>();
            var configurationMock = new Mock<IOptions<ServiceConfiguration>>();
            var httpClientFactory = new Mock<IHttpClientFactory>();

            httpClientFactory.Setup(x => x.CreateClient(Options.DefaultName)).Returns(new HttpClient());
            configurationMock.Setup(x => x.Value).Returns(new ServiceConfiguration
            {
                BaseUrl = "https://viacep.com.br/ws/"
            });

            _viaCepProvider = new Provider(
                loggerMock.Object,
                configurationMock.Object,
                httpClientFactory.Object);
        }

        [TestMethod]
        public async Task FindCep()
        {
            const string cepNormalized = "02033000";

            var result = await _viaCepProvider.Find(cepNormalized);

            Assert.IsNotNull(result, "O resultado da consulta de CEP não pode ser null");
            Assert.AreEqual("Avenida General Ataliba Leonel", result.Address, "Endereço retornado não é o esperado");
            Assert.AreEqual("São Paulo", result.City, "Endereço retornado não é o esperado");
            Assert.AreEqual("SP", result.State, "Endereço retornado não é o esperado");
            Assert.AreEqual("3550308", result.CityIGBECode, "Código IBGE divergente");
        }

        [TestMethod]
        public async Task FindUnnormalizedCep()
        {
            const string cep = "02033-000";

            var result = await _viaCepProvider.Find(cep);

            Assert.IsNotNull(result, "O resultado da consulta de CEP não pode ser null");
            Assert.AreEqual("Avenida General Ataliba Leonel", result.Address, "Endereço retornado não é o esperado");
            Assert.AreEqual("São Paulo", result.City, "Endereço retornado não é o esperado");
            Assert.AreEqual("SP", result.State, "Endereço retornado não é o esperado");
            Assert.AreEqual("3550308", result.CityIGBECode, "Código IBGE divergente");
        }

        [TestMethod]
        public async Task NotFoundCep()
        {
            const string cep = "02033-001";

            var result = await _viaCepProvider.Find(cep);

            Assert.IsNull(result, "Este cep não deveria retornar resultado porque não existe");
        }
    }
}