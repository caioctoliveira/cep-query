namespace CaioOliveira.CepFinder.Models
{
    public class AddressInformation
    {
        /// <summary>
        ///     Código postal do endereço
        /// </summary>
        public string CEP { get; set; }

        /// <summary>
        ///     Logradouro do endereço
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///     Complemento do endereço
        /// </summary>
        public string Complement { get; set; }

        /// <summary>
        ///     Bairro do endereço
        /// </summary>
        public string Neighborhood { get; set; }

        /// <summary>
        ///     Cidade do endereço
        /// </summary>
        public string City { get; set; }

        /// <summary>
        ///     Unidade federativa do estado
        /// </summary>
        public string State { get; set; }

        /// <summary>
        ///     Código IBGE da cidade
        /// </summary>
        public string CityIGBECode { get; set; }
    }
}