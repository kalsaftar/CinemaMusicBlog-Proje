# 🎬 Cinema & Music Blog Project

![NetCore](https://img.shields.io/badge/.NET%20Core-8.0-purple)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-blue)
![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-green)

Bu proje, **ASP.NET Core MVC** mimarisi kullanılarak geliştirilmiş, modern arayüze sahip dinamik bir içerik yönetim sistemidir (CMS). Kullanıcıların sinema ve müzik üzerine makaleler okuyabileceği, listeler oluşturabileceği ve medya içerikleriyle etkileşime girebileceği bir platformdur.

## 🚀 Öne Çıkan Özellikler

* **Clean Code Mimarisi:** Proje, katmanlı mimari ve SOLID prensiplerine uygun olarak, okunabilir ve sürdürülebilir kod yapısıyla geliştirilmiştir.
* **Dinamik İçerik Yönetimi (CRUD):** Gelişmiş admin paneli sayesinde yazı ekleme, düzenleme ve silme işlemleri.
* **Listicle Formatı:** Her yazıya sınırsız sayıda alt madde (Film/Şarkı) ekleyebilme özelliği.
* **Görsel Yönetimi:** `Guid` yapısı kullanılarak benzersiz isimlendirme ile güvenli resim yükleme sistemi.
* **Responsive Tasarım:** Bootstrap 5 ile her cihaza (Mobil/Tablet/PC) tam uyumlu modern arayüz.
* **Spotify Entegrasyonu:** Yazı detaylarında ilgili çalma listelerinin gömülü (embed) olarak sunulması.
* **Admin Güvenliği:** İçerik yönetimi sayfalarına yetkisiz erişimi engelleyen `[Authorize]` koruması.
* **Data Annotations:** Veri bütünlüğü için hem Client hem Server taraflı validasyon (doğrulama) kuralları.

## 🛠️ Kullanılan Teknolojiler

* **Backend:** C#, ASP.NET Core 8.0 MVC
* **ORM:** Entity Framework Core (Code First Yaklaşımı)
* **Veritabanı:** MSSQL (LocalDB)
* **Frontend:** HTML5, CSS3, Bootstrap 5, Razor View Engine
* **Tools:** Visual Studio 2022, Git

## 📷 Ekran Görüntüleri
![1](https://github.com/user-attachments/assets/10320f59-ed5c-4e75-88ec-51aa82c1151f)
![4](https://github.com/user-attachments/assets/0aba3854-ad89-45c3-9626-d85b0cbbae94)
![3](https://github.com/user-attachments/assets/73cbf192-5b46-4e61-bfb5-8b6291ad3120)
![2](https://github.com/user-attachments/assets/c5a4a085-0f82-410f-93da-c004cf17d8ea)



## ⚙️ Kurulum

Projeyi kendi bilgisayarınızda çalıştırmak için:

1.  Repoyu klonlayın:
    ```bash
    git clone [https://github.com/KULLANICIADIN/PROJEADIN.git](https://github.com/KULLANICIADIN/PROJEADIN.git)
    ```
2.  `appsettings.json` dosyasındaki Connection String'i kendi veritabanı sunucunuza göre düzenleyin.
3.  Package Manager Console üzerinden veritabanını oluşturun:
    ```powershell
    Update-Database
    ```
4.  Projeyi çalıştırın.

---
Developed by **[Sercan Demir]**
