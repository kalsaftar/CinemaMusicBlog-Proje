using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaMusicBlog.Models
{
    // Listelerdeki her bir maddeyi (Örn: Filmler, Şarkılar) temsil eden model sınıfı.
    public class PostItem
    {
        // Tablonun Birincil Anahtarı (Primary Key)
        public int Id { get; set; }

        // --- BAŞLIK ALANI ---
        // Kullanıcı arayüzünde görünecek etiket ve validasyon kuralları.
        // StringLength: Veritabanı optimizasyonu için karakter sınırı koyar.
        [Display(Name = "Madde Başlığı")]
        [Required(ErrorMessage = "Madde başlığı alanı zorunludur.")]
        [StringLength(150, ErrorMessage = "Başlık en fazla 150 karakter olabilir.")]
        public string Baslik { get; set; }

        // --- İÇERİK ALANI ---
        // Maddenin detaylı açıklaması veya kullanıcı yorumunu tutar.
        [Display(Name = "Yorum / İçerik")]
        [Required(ErrorMessage = "İçerik alanı boş bırakılamaz.")]
        public string Icerik { get; set; }

        // --- GÖRSEL ALANI ---
        // Resmin sunucudaki dosya yolunu (Path) tutar. 
        // Dosya yükleme işlemi Controller tarafında yönetildiği için burada sadece string yolu saklanır.
        [Display(Name = "Görsel")]
        public string? ResimYolu { get; set; }

        // --- İLİŞKİSEL VERİ (Foreign Key) ---
        // Bu maddenin hangi ana yazıya (Post) ait olduğunu belirtir.
        // Entity Framework bu ilişkiyi kullanarak tabloları birbirine bağlar.
        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public virtual Post? Post { get; set; }
    }
}