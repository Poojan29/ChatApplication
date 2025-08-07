// using MongoDB.Driver;
// using ChatApp.Models;

// public class MessageService
// {
//     private readonly IMongoCollection<Chat> _users;
//     private readonly IMongoDatabase _database;

//     public MessageService(IConfiguration config)
//     {
//         var settings = config.GetSection("MongoDbSettings").Get<MongoDbSettings>();
//         var client = new MongoClient(settings.ConnectionString);
//         _database = client.GetDatabase(settings.DatabaseName);
//         _users = _database.GetCollection<Chat>("chats");
//     }
    
//     public IMongoCollection<User> Users => _database.GetCollection<User>("Users");

//     public async Task<List<Chat>> GetAllAsync()
//     {
//         try
//         {
//             return await _users.Find(user => true).ToListAsync();
//         }
//         catch (Exception ex)
//         {
//             // Handle exceptions (e.g., log them)
//             Console.WriteLine("An error occurred while retrieving users.", ex);
//             throw new Exception("An error occurred while retrieving users.", ex);
//         }
//     }
        

//     public async Task AddUserAsync(Chat user) =>
//         await _users.InsertOneAsync(user);
// }
