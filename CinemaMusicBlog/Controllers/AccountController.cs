using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CinemaMusicBlog.Data; 
using CinemaMusicBlog.Models; 
using Microsoft.EntityFrameworkCore;

namespace CinemaMusicBlog.Controllers
{
    public class AccountController : Controller
    {
        // Veritabanı bağlantısını buraya çağırıyoruz (Dependency Injection)
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            
            // Veritabanına gidip: "Bu kullanıcı adı ve şifreye sahip biri var mı?" diye soruyoruz.
            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);

            if (admin != null) // Eğer veritabanında böyle biri varsa
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Username)
                };

                var userIdentity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync("CookieAuth", principal);

                return RedirectToAction("Index", "Posts");
            }

            // Yoksa hata ver
            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
    }
}