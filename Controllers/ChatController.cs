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

        public IActionResult Chat(string receiver)
        {
            ViewBag.Receiver = receiver;
            var currentUser = User.Identity.Name;
            var messages = _context.Messages
                .Find(m => (m.SenderUsername == currentUser && m.ReceiverUsername == receiver) ||
                           (m.SenderUsername == receiver && m.ReceiverUsername == currentUser))
                .SortBy(m => m.Time)
                .ToList();

            return View(messages);
        }

    }
}
