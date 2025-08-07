using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;
using System.Text;

namespace ChatApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly MongoDbContext _context;

        public AccountController(MongoDbContext context)
        {
            _context = context;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            var existing = _context.Users.Find(u => u.Username == user.Username).FirstOrDefault();
            if (existing != null)
            {
                ModelState.AddModelError("", "Username already exists.");
                return View();
            }

            user.PasswordHash = HashPassword(user.PasswordHash);
            await _context.Users.InsertOneAsync(user);
            return RedirectToAction("Login");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var hashed = HashPassword(password);
            var user = _context.Users.Find(u => u.Username == username && u.PasswordHash == hashed).FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Chat");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
