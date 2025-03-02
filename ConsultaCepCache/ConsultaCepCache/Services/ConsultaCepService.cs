using ConsultaCepCache.Interfaces;
using ConsultaCepCache.Models;
using ConsultaCepCache.Repository;
using System.Text.Json;

namespace ConsultaCepCache.Services;

public class ConsultaCepService : IConsultaCepService
{
    private readonly ICorreiosApi _correiosApi;
    private readonly IMongoRepository _mongoRepository;

    public ConsultaCepService(ICorreiosApi correiosApi, IMongoRepository mongoRepository)
    {
        _correiosApi = correiosApi;
        _mongoRepository = mongoRepository;
    }

    public async Task<CepResponse> GetCepAsync(string cep, string version)
    {
        var cachedCep = await _mongoRepository.GetCachedCepAsync(cep, version);

        if (cachedCep != null)
        {
            return JsonSerializer.Deserialize<CepResponse>(cachedCep.Response);
        }

        var response = await _correiosApi.GetCepAsync(cep);
        await _mongoRepository.SaveCepAsync(new CepCache { Cep = cep, Version = version, Response = JsonSerializer.Serialize(response) });

        return response;
    }
}