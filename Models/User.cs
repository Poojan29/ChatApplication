using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatApp.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Username")]
        public string? Username { get; set; }

        [BsonElement("PasswordHash")]
        public string? PasswordHash { get; set; }
    }
}
