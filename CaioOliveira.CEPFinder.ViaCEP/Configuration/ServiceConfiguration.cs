namespace CaioOliveira.CepFinder.ViaCep.Configuration
{
    public class ServiceConfiguration
    {
        public string BaseUrl { get; set; }

        internal void Bind(ServiceConfiguration config)
        {
            BaseUrl = config.BaseUrl;
        }
    }
}