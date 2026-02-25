using CinemaMusicBlog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaMusicBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PostsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        #region 1. ANA SAYFA VE DETAY (Public Actions)

        // GET: Posts
        public async Task<IActionResult> Index(string searchString, string category)
        {
            var posts = _context.Posts
        .Include(p => p.Category)
        .AsQueryable();

            // Kategori Filtresi
            if (!string.IsNullOrEmpty(category))
            {
                posts = posts.Where(x => x.Category!.Name == category);
            }

            // Arama Filtresi
            if (!string.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Title.Contains(searchString)
                                      || s.Content.Contains(searchString)
                                      || (s.Tags != null && s.Tags.Contains(searchString)));
                ViewData["Title"] = $"Arama: {searchString}";
            }

            return View(await posts.OrderByDescending(p => p.CreatedDate).ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null) return NotFound();

            var post = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.Items)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null) return NotFound();

            ViewBag.Onerilenler = _context.Posts
                .Include(p => p.Category)
                .Where(p => p.Id != id)
                .OrderByDescending(p => p.CreatedDate)
                .Take(3)
                .ToList();

            return View(post);
        }


        // Kategoriye göre filtreleme (Alternatif Rota)
        public async Task<IActionResult> ByCategory(int id)
        {
            var posts = await _context.Posts
                .Include(p => p.Category)
                .Where(p => p.CategoryId == id)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            var categoryName = posts.FirstOrDefault()?.Category?.Name ?? "Kategori";
            ViewData["Title"] = categoryName + " Yazıları";

            return View("Index", posts);
        }

        #endregion

    }
}