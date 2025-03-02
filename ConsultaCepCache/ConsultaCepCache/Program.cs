
using ConsultaCepCache.Interfaces;
using ConsultaCepCache.Models;
using ConsultaCepCache.Repository;
using ConsultaCepCache.Services;
using ConsultaCepCache.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Refit;

namespace ConsultaCepCache
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Carregar configura��es do MongoDB a partir do appsettings.json
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));

            // Registrar o reposit�rio MongoDB
            builder.Services.AddScoped<IMongoRepository, MongoRepository>();

            // Registrar o servi�o de consulta de CEP
            builder.Services.AddScoped<IConsultaCepService, ConsultaCepService>();

            // Configurar o cliente Refit para a API Correios
            builder.Services.AddRefitClient<ICorreiosApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://viacep.com.br"));

            // Adicionar os servi�os padr�o da API
            builder.Services.AddControllers();

            // Configurar Swagger para documenta��o da API
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configura��o do pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            // Mapeamento de controladores
            app.MapControllers();

            // Configurar o �ndice TTL ap�s a inicializa��o do banco de dados
            ConfigureMongoIndexes(app);

            app.Run();
        }

        private static void ConfigureMongoIndexes(WebApplication app)
        {
            // Acessar a configura��o do MongoDB
            var mongoSettings = app.Services.GetRequiredService<IOptions<MongoDBSettings>>().Value;

            var mongoClient = new MongoClient(mongoSettings.ConnectionString);
            var database = mongoClient.GetDatabase(mongoSettings.DatabaseName);
            var collection = database.GetCollection<CepCache>(mongoSettings.CollectionName);

            // Verificar e remover o �ndice existente com o nome "CreatedAtCep"
            var existingIndexes = collection.Indexes.List().ToList();
            var existingIndex = existingIndexes.FirstOrDefault(i => i["name"] == mongoSettings.IndexName);

            if (existingIndex != null)
            {
                collection.Indexes.DropOneAsync(mongoSettings.IndexName);
            }

            // Criando o �ndice TTL
            var indexKeysDefinition = Builders<CepCache>.IndexKeys.Ascending(x => x.CreatedAt);
            var indexOptions = new CreateIndexOptions { 
                ExpireAfter = TimeSpan.FromSeconds(mongoSettings.IndexTTLSeconds),
                Name = mongoSettings.IndexName
            };
            collection.Indexes.CreateOneAsync(new CreateIndexModel<CepCache>(indexKeysDefinition, indexOptions)).Wait();
        }
    }
}