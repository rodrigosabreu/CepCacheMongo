namespace ConsultaCepCache.Settings;

public interface IMongoDBSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string Collection { get; set; }
    public int TtlFromSeconds { get; set; }
}