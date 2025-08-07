using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ChatApp.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly MongoDbContext _context;

        public ChatController(MongoDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var currentUser = User.Identity.Name;
            var users = _context.Users.Find(u => u.Username != currentUser).ToList();
            return View(users);
        }

        public IActionResult Chat(string username)
        {
            var currentUser = User.Identity.Name;
            var messages = _context.Messages
                .Find(m => (m.SenderUsername == currentUser && m.ReceiverUsername == username) ||
                           (m.SenderUsername == username && m.ReceiverUsername == currentUser))
                .SortBy(m => m.Time)
                .ToList();

            ViewBag.Receiver = username;
            return View(messages);
        }

        [HttpPost]
        public IActionResult SendMessage(string receiver, string message)
        {
            var msg = new Chat
            {
                SenderUsername = User.Identity.Name,
                ReceiverUsername = receiver,
                Message = message
            };

            _context.Messages.InsertOne(msg);
            return RedirectToAction("Chat", new { username = receiver });
        }
    }
}
