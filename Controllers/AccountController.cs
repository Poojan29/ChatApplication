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
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            else
            {
                var existing = _context.Users.Find(u => u.Username == registerViewModel.Username).FirstOrDefault();
                if (existing != null)
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View(registerViewModel);
                }
                var user = new User
                {
                    Username = registerViewModel.Username,
                    PasswordHash = registerViewModel.Password,
                    Email = registerViewModel.Email,
                    PhoneNumber = registerViewModel.PhoneNumber,
                    Address = registerViewModel.Address,
                    City = registerViewModel.City
                };

                user.PasswordHash = HashPassword(user.PasswordHash);
                await _context.Users.InsertOneAsync(user);
                return RedirectToAction("Login");
            }

        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var hashed = HashPassword(model.Password);
                var user = _context.Users.Find(u => u.Username == model.Username && u.PasswordHash == hashed).FirstOrDefault();

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid credentials.");
                    return View(model);
                }

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Chat");
            }
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
