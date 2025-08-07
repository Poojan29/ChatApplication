// using Microsoft.AspNetCore.Mvc;
// using ChatApp.Models;

// public class UserController : Controller
// {
//     private readonly MongoDbContext _messageService;

//     public UserController(MongoDbContext userService)
//     {
//         _messageService = userService;
//     }

//     public async Task<IActionResult> Index()
//     {
//         var users = await _messageService.get;
//         return View(users);
//     }
// }
