using Microsoft.Extensions.Hosting;

namespace CinemaMusicBlog.Models
{
    public class Category
    {
        public int Id { get; set; }

        // Soru işareti (?) ekleyerek bu alanın boş kalabileceğini sisteme bildiriyoruz
        public string? Name { get; set; }

        // Eğer Post ile bağlantı kurduysan bu satır da şöyle olmalı
        public ICollection<Post>? Posts { get; set; }
    }
}