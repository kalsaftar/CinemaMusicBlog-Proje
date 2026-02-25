# 🎬 CinemaMusicBlog
### ASP.NET Core MVC Blog Uygulaması

---

## 📌 Proje Tanımı

CinemaMusicBlog, ASP.NET Core MVC mimarisi kullanılarak geliştirilmiş dinamik bir blog uygulamasıdır.

### Sistem İki Ana Bölümden Oluşur:

- **Public Alan:**  
  Ziyaretçiler anonim olarak içerikleri görüntüleyebilir.

- **Admin Alanı:**  
  Yetkili kullanıcı giriş yaparak içerik oluşturabilir, düzenleyebilir ve silebilir.

Proje, eğitim kapsamında object-oriented prensiplere uygun şekilde geliştirilmiştir.

---

## 🏗 Kullanılan Teknolojiler

- ASP.NET Core MVC
- Entity Framework Core (Code First)
- SQL Server / LocalDB
- LINQ
- Bootstrap 5
- HTML5 & CSS3
- Razor View Engine

---

## 🧠 Mimari Yapı

Proje MVC (Model–View–Controller) mimarisine uygun olarak geliştirilmiştir.

- **Model:** Post, Category ve Admin entity sınıfları
- **View:** Razor tabanlı dinamik arayüz
- **Controller:** CRUD işlemleri ve iş akışı yönetimi
- **Area Yapısı:** Admin ve Public arayüzler ayrılmıştır

---

## 🗄 Veritabanı

Proje Entity Framework Core Code First yaklaşımı ile geliştirilmiştir.

Uygulama başlatıldığında veritabanı otomatik oluşturulur.

Manuel oluşturmak için:

```
Update-Database

```
## 🔐 Admin Girişi

İlk çalıştırmada otomatik admin oluşturulur:

- **Username:** `admin`
- **Password:** `1234`

> Not: Bu proje eğitim amaçlıdır. Şifre doğrulama mekanizması basit tutulmuştur.

---

## 🎯 Proje Özellikleri

- Blog içerik listeleme
- İçerik detay sayfası
- Kategori filtreleme
- Admin paneli
- CRUD işlemleri
- Resim yükleme
- Cookie tabanlı kimlik doğrulama
- Entity Framework Core ile veritabanı entegrasyonu

---

## 📎 Teslim Notu

Bu proje MCSD bitirme projesi kapsamında hazırlanmıştır.

- ✔ Veritabanı içermektedir.
- ✔ Object-Oriented prensiplere uygun geliştirilmiştir.
- ✔ HTML ve CSS tasarımsal kodlama içermektedir.
- ✔ ASP.NET Core MVC mimarisi kullanılmıştır.
