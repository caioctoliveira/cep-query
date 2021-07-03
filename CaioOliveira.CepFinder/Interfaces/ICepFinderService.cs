using System.Threading.Tasks;
using CaioOliveira.CepFinder.Models;

namespace CaioOliveira.CepFinder.Interfaces
{
    public interface ICepFinderService
    {
        Task<AddressInformation> Find(string cep);
    }
}