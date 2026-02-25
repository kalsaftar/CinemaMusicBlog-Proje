# 🎬 CinemaMusicBlog
### ASP.NET Core MVC Blog Application

---

## 📌 Project Description

CinemaMusicBlog is a dynamic blog application developed using the ASP.NET Core MVC architecture.

### The System Consists of Two Main Sections:

- **Public Area:**  
  Visitors can view content anonymously.

- **Admin Area:**  
  Authorized users can log in to create, edit, and delete content.

The project was developed for educational purposes in accordance with object-oriented principles.

---

## 🏗 Technologies Used

- ASP.NET Core MVC
- Entity Framework Core (Code First)
- SQL Server / LocalDB
- LINQ
- Bootstrap 5
- HTML5 & CSS3
- Razor View Engine

---

## 🧠 Architectural Structure

The project was developed in accordance with the MVC (Model–View–Controller) architecture.

- **Model:** Post, Category, and Admin entity classes
- **View:** Razor-based dynamic interface
- **Controller:** CRUD operations and workflow management
- **Area Structure:** Admin and Public interfaces are separated

---

## 🗄 Database

The project was developed using the Entity Framework Core Code First approach.

When the application is started, the database is created automatically.

To create it manually:

```powershell
Update-Database
```

---

## 🔐 Admin Login

An admin user is automatically created on first run:

- **Username:** `admin`
- **Password:** `1234`

> Note: This project is for educational purposes. The password validation mechanism is kept simple.

---

## 🎯 Project Features

- Blog content listing
- Content detail page
- Category filtering
- Admin panel
- CRUD operations
- Image upload
- Cookie-based authentication
- Database integration with Entity Framework Core

---

## 📎 Submission Note

This project was prepared as part of the MCSD graduation project.

- ✔ Includes a database.
- ✔ Developed in accordance with Object-Oriented principles.
- ✔ Includes HTML and CSS design coding.
- ✔ ASP.NET Core MVC architecture was used.