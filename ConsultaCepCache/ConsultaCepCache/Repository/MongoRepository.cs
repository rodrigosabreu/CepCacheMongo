using ConsultaCepCache.Models;
using ConsultaCepCache.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ConsultaCepCache.Repository
{
    public class MongoRepository : IMongoRepository
    {
        private readonly IMongoCollection<CepCache> _collection;

        public MongoRepository(IOptions<MongoDBSettings> mongoSettings)
        {
            var settings = mongoSettings.Value;

            var mongoClient = new MongoClient(settings.ConnectionString);
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<CepCache>(settings.CollectionName);
        }

        public async Task<CepCache> GetCachedCepAsync(string cep, string version)
        {
            return await _collection.Find(x => x.Cep == cep && x.Version == version).FirstOrDefaultAsync();
        }

        public async Task SaveCepAsync(CepCache cepCache)
        {
            await _collection.InsertOneAsync(cepCache);
        }
    }

}
