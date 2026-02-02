using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Validasyonlar için 
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace CinemaMusicBlog.Models
{
    public class Post
    {
        public int Id { get; set; }

        // VALIDATION 1: Başlık Ayarları
        [Display(Name = "Başlık")] // Ekranda "Title" yerine "Başlık" yazacak.
        [Required(ErrorMessage = "Lütfen bir başlık giriniz.")] // Boş bırakılırsa bu hatayı verecek.
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Başlık 3 ile 200 karakter arasında olmalı.")]
        public string Title { get; set; }

        // VALIDATION 2: İçerik Ayarları
        [Display(Name = "İçerik")]
        [Required(ErrorMessage = "İçerik alanı boş bırakılamaz.")]
        public string Content { get; set; }

        [Display(Name = "Etiketler (Virgülle ayırın)")]
        public string? Tags { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Veritabanında dosya adını tutar
        [Display(Name = "Kapak Görseli")]
        public string? ImageUrl { get; set; }

        // Bu alan veritabanına kaydedilmez, sadece dosya yüklemek içindir
        [NotMapped]
        [Display(Name = "Yeni Resim Yükle")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<PostItem> Items { get; set; } = new List<PostItem>();
    }
}