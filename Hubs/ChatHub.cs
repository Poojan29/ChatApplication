using Microsoft.AspNetCore.SignalR;
using System;
using ChatApp.Models;

public class ChatHub : Hub
{
    private readonly MongoDbContext _context;

    public ChatHub(MongoDbContext context)
    {
        _context = context;
    }

    public async Task SendMessage(string sender, string receiver, string message, string time)
    {
        // Save message to MongoDB
        var encryptedMessage = EncryptionHelper.Encrypt(message);
        var chat = new Chat
        {
            SenderUsername = sender,
            ReceiverUsername = receiver,
            Message = encryptedMessage,
            Time = DateTime.Now
        };

        _context.Messages.InsertOne(chat);

        // Broadcast message to all connected clients
        await Clients.All.SendAsync("ReceiveMessage", sender, receiver, message, time);
    }
}
