using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RestAPINetMongoDB.Configuration;

namespace RestAPINetMongoDB.Services
{
    public class MongoDBService
    {
        public readonly MongoClient _mongoClient;

        public MongoDBService(IConfiguration configuration)
        {
            var connectionStrings = configuration.GetSection("ConnectionStrings").Get<ConnectionStringsSettings>();
            _mongoClient = new MongoClient(connectionStrings.MongoDB);
        }
    }
}
