using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using UsersMS.Domain.Entities;

namespace UsersMS.Infrastructure.DataBase;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB");
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("UsersMS_ReadDB");
    }

    public IMongoCollection<UserReadModel> Users => _database.GetCollection<UserReadModel>("Users");
}
