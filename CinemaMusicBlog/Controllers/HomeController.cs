using CinemaMusicBlog.Data; // Veritabaný context'i için
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaMusicBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

         var sonYazilar = await _context.Posts
         .Include(p => p.Category)
         .OrderByDescending(p => p.CreatedDate)
         .ToListAsync();

            return View(sonYazilar);
        }
        public IActionResult About()
        {
            return View();
        }
    }
}