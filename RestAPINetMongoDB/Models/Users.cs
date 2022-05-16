using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestAPINetMongoDB.Models
{
    [BsonIgnoreExtraElements]
    public class Users
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string role { get; set; }
        public bool google { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string img { get; set; }
    }
}
