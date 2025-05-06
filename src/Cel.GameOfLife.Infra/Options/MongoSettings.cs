namespace Cel.GameOfLife.Infra.Options;

public class MongoSettings
{
    public string ConnectionString { get; set; } = string.Empty; 
    public string DataBaseName { get; set; } = string.Empty; 
}