using ConsultaCepCache.Models;

namespace ConsultaCepCache.Repository
{
    public interface IMongoRepository
    {
        Task<CepCache> GetCachedCepAsync(string cep, string version);
        Task SaveCepAsync(CepCache cepCache);
    }
}
