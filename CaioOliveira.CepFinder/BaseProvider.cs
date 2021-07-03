using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CaioOliveira.CepFinder.Models;
using Microsoft.Extensions.Logging;

namespace CaioOliveira.CepFinder
{
    public abstract class BaseProvider
    {
        public BaseProvider(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }

        protected abstract Task<HttpRequestMessage> CreateRequestMessage(string cep);
        protected abstract Task<HttpResponseMessage> CallService(HttpRequestMessage request);
        protected abstract Task<AddressInformation> ProcessResponseMessage(HttpResponseMessage response);

        public async Task<AddressInformation> Process(string cep)
        {
            AddressInformation result = null;
            Logger.LogDebug($"Iniciando processo de consulta do CEP {cep}");

            try
            {
                Logger.LogDebug($"Normalizando CEP {cep}");

                var rgx = new Regex(@"\d");
                var normalizedCep = string.Join("", rgx.Matches(cep)
                    .ToList()
                    .Select(m => m.Value));

                Logger.LogDebug($"CEP normalizado {normalizedCep}");

                Logger.LogDebug("Criando mensagem de request para consulta de CEP");
                var rq = await CreateRequestMessage(normalizedCep);

                Logger.LogDebug("Chamando serviço de consulta de CEP");
                var rs = await CallService(rq);

                Logger.LogDebug("Processando resposta do serviço de consulta de CEP");
                result = await ProcessResponseMessage(rs);
            }
            catch (Exception ex)
            {
                Logger.LogError("Erro ao consultar o CEP", ex);
            }

            Logger.LogDebug("Fim do processo de consulta de CEP");
            return result;
        }
    }
}