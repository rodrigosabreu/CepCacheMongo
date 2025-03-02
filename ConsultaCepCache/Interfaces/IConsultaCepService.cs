using ConsultaCepCache.Models;

namespace ConsultaCepCache.Interfaces
{
    public interface IConsultaCepService
    {
        Task<CepResponse> GetCepAsync(string cep, string version);
    }
}