using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatApp.Models
{
    public class Chat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("to")]
        public string? ReceiverUsername { get; set; }

        [BsonElement("from")]
        public string? SenderUsername { get; set; }

        [BsonElement("message")]
        public string? Message { get; set; }

        [BsonElement("time")]
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
