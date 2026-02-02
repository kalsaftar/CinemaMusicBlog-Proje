using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaMusicBlog.Data;
using CinemaMusicBlog.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization; // Güvenlik için

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
            var posts = _context.Posts.Include(p => p.Category).AsQueryable();

            // Kategori Filtresi
            if (!string.IsNullOrEmpty(category))
            {
                posts = posts.Where(x => x.Category.Name == category);
                ViewData["Title"] = category;
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
                .Include(p => p.Items) // Maddeleri dahil et
                .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null) return NotFound();

            // Yan Menü: Önerilenler (Şu anki hariç son 3 yazı)
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

        #region 2. POST YÖNETİMİ (CRUD - Admin Only)

        // Bu bölüme sadece giriş yapmış kullanıcılar erişebilir
        [Authorize]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                // Clean Code: Resim Yükleme Metodunu Çağırıyoruz
                if (post.ImageFile != null)
                {
                    post.ImageUrl = await UploadImageAsync(post.ImageFile);
                }

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Post post, IFormFile? image)
        {
            if (id != post.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Clean Code: Yeni resim varsa yükle, yoksa eskisini koru
                    if (image != null)
                    {
                        post.ImageUrl = await UploadImageAsync(image);
                    }

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var post = await _context.Posts.Include(p => p.Category).FirstOrDefaultAsync(m => m.Id == id);
            if (post == null) return NotFound();
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null) _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region 3. LİSTE MADDELERİ (Post Items)

        [Authorize]
        public IActionResult AddItem(int id)
        {
            ViewBag.PostId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddItem(PostItem item, IFormFile ResimDosyasi)
        {
            item.Id = 0;
            ModelState.Remove("Post"); // Validation hatasını önle

            if (ModelState.IsValid)
            {
                // Clean Code: Resim Yükleme
                if (ResimDosyasi != null)
                {
                    item.ResimYolu = await UploadImageAsync(ResimDosyasi);
                }

                _context.PostItems.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = item.PostId });
            }
            return View(item);
        }

        [Authorize]
        public async Task<IActionResult> EditItem(int id)
        {
            var item = await _context.PostItems.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditItem(PostItem item, IFormFile ResimDosyasi)
        {
            var mevcutMadde = await _context.PostItems.FindAsync(item.Id);
            if (mevcutMadde == null) return NotFound();

            mevcutMadde.Baslik = item.Baslik;
            mevcutMadde.Icerik = item.Icerik;

            // Clean Code: Resim Yükleme
            if (ResimDosyasi != null)
            {
                mevcutMadde.ResimYolu = await UploadImageAsync(ResimDosyasi);
            }

            _context.Update(mevcutMadde);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = mevcutMadde.PostId });
        }

        [Authorize]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.PostItems.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("DeleteItem")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteItemConfirmed(int id)
        {
            var item = await _context.PostItems.FindAsync(id);
            if (item != null)
            {
                _context.PostItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", new { id = item.PostId });
        }

        #endregion

        #region 4. YARDIMCI METODLAR (Private Helpers)

        // CLEAN CODE: Resim Yükleme İşlemi Tek Bir Yerde!
        private async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            string extension = Path.GetExtension(imageFile.FileName);
            string newName = Guid.NewGuid().ToString() + extension;
            string path = Path.Combine(_hostEnvironment.WebRootPath, "images", newName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return newName;
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        #endregion
    }
}