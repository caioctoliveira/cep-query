using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CaioOliveira.CepFinder.Interfaces;
using CaioOliveira.CepFinder.Models;
using CaioOliveira.CepFinder.ViaCep.Configuration;
using CaioOliveira.CepFinder.ViaCep.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CaioOliveira.CepFinder.ViaCep
{
    public class Provider : BaseProvider, ICepFinderService
    {
        private readonly ServiceConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly JsonSerializerSettings _jsonSettings;

        public Provider(
            ILogger<Provider> logger,
            IOptions<ServiceConfiguration> configruation,
            IHttpClientFactory httpClientFactory) : base(logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configruation.Value;
            _jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            if (_configuration == null)
                throw new ArgumentException("Configuração inválida");
        }

        public Task<AddressInformation> Find(string cep)
        {
            return Process(cep);
        }

        protected override async Task<HttpResponseMessage> CallService(HttpRequestMessage request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration.BaseUrl);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await client.SendAsync(request);
        }

        protected override async Task<HttpRequestMessage> CreateRequestMessage(string cep)
        {
            return await Task.Run(() => new HttpRequestMessage(HttpMethod.Get, $"{cep}/json"));
        }

        protected override async Task<AddressInformation> ProcessResponseMessage(HttpResponseMessage response)
        {
            if (response == null)
            {
                Logger.LogError("A resposta da chamada do serviço de consulta de CEP não pode ser nula");
                return null;
            }

            CEPFindResult result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CEPFindResult>(json, _jsonSettings);
                    break;
                default:
                    Logger.LogError("O serviço de consulta de CEP não respondeu conforme esperado");
                    return null;
            }

            if (result?.Cep == null)
            {
                Logger.LogError("Cep não encontrado ou serviço indisponível");
                return null;
            }

            return new AddressInformation
            {
                CEP = result.Cep,
                Address = result.Logradouro,
                Complement = result.Complemento,
                Neighborhood = result.Bairro,
                City = result.Localidade,
                State = result.UF,
                CityIGBECode = result.IBGE
            };
        }
    }
}