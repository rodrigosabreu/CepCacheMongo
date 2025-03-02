namespace ConsultaCepCache.Settings;

public class MongoDBSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
    public int IndexTTLSeconds { get; set; }
    public string IndexName { get; set; }
}