using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ConsultaCepCache.Models;

public class CepCache
{
    public ObjectId Id { get; set; }
    public string Cep { get; set; }
    public string Version { get; set; }
    public string Response { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class CepResponse
{
    public string cep { get; set; }
    public string logradouro { get; set; }
    public string complemento { get; set; }
    public string unidade { get; set; }
    public string bairro { get; set; }
    public string localidade { get; set; }
    public string uf { get; set; }
    public string estado { get; set; }
    public string regiao { get; set; }
    public string ibge { get; set; }
    public string gia { get; set; }
    public string ddd { get; set; }
    public string siafi { get; set; }
}
