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
using Microsoft.AspNetCore.Authorization;

namespace CinemaMusicBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PostsController : Controller
    {
        // --- BAĞLANTILAR (Dependency Injection) ---
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PostsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

       

        // Şimdilik test için basit bir Index koyalım
        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.ToListAsync());
        }

        #region 2. POST YÖNETİMİ (CRUD - Admin Only)


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
                // İş bitince Admin listesine değil, normal sitenin detay sayfasına git
                return RedirectToAction("Details", "Posts", new { area = "", id = post.Id });
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
                    //  Yeni resim varsa yükle, yoksa eskisini koru
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
                // İş bitince Admin listesine değil, normal sitenin detay sayfasına git
                return RedirectToAction("Details", "Posts", new { area = "", id = post.Id });
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
            return RedirectToAction("Index", "Posts", new { area = "" });
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
                return RedirectToAction("Details", "Posts", new { area = "", id = item.PostId });
            }
            ViewBag.PostId = item.PostId;
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
            return RedirectToAction("Details", "Posts", new { area = "", id = mevcutMadde.PostId });
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
            int originalPostId = item.PostId; 

            _context.PostItems.Remove(item);
            await _context.SaveChangesAsync();

            
            return RedirectToAction("Details", "Posts", new { area = "", id = originalPostId });
        }

        #endregion

        #region 4. YARDIMCI METODLAR (Private Helpers)

        // CLEAN CODE: Resim Yükleme İşlemi Tek Bir Yerde!
        private async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            string extension = Path.GetExtension(imageFile.FileName);
            string newName = Guid.NewGuid().ToString() + extension;
            string path = Path.Combine(_environment.WebRootPath, "images", newName);

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