using ConsultaCepCache.Models;
using Refit;

namespace ConsultaCepCache.Interfaces
{
    public interface ICorreiosApi
    {
        [Get("/ws/{cep}/json/")]
        Task<CepResponse> GetCepAsync(string cep);
    }
}
