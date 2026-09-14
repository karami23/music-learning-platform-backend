# 🎵 Music Education Platform (Backend API)

A scalable, robust, and **API-first** backend for an online music learning platform (inspired by Coursera/Udemy). Designed to serve multiple clients seamlessly, including Web, Android, iOS apps, and an Admin Dashboard from a single unified core.

---

## 🛠️ Tech Stack & Technologies

* **Language:** C#
* **Framework:** ASP.NET Core Web API (.NET 10)
* **Architecture:** Clean Architecture
* **ORM:** Entity Framework Core (EF Core)
* **Database:** Microsoft SQL Server
* **Security & Auth:** JWT Authentication & Refresh Tokens
* **API Documentation:** Swagger / OpenAPI

---

## 🏗️ Architecture Overview (Clean Architecture)

The solution is structured following **Clean Architecture / Onion Architecture** principles:
```text
MusicEducation
│
├── MusicEducation.sln
│
└── src
├── MusicEducation.Domain          # Core entities, Enums, Value Objects, and business rules
├── MusicEducation.Application     # Use Cases, Commands, Queries, and service interfaces
├── MusicEducation.Infrastructure  # EF Core DbContext, Repositories, JWT Service, Password Hashing
└── MusicEducation.API             # Controllers, Middlewares, DTOs, and Swagger setup
🚀 Core Features & Modules
1. Identity & Access Management
User registration and authentication via Email and Phone Number
Secure password hashing
JWT Access Token generation and Refresh Token lifecycle
Role-Based Access Control (RBAC): Student, Teacher, Admin
2. Course & Content Management
Hierarchical structure: Course ➔ Chapter ➔ Lesson ➔ LessonMedia
Course status lifecycle: Draft, Published, Archived
Pricing models: Free vs Paid
Support for multiple media attachments: Video, Audio, and PDF files
3. Commerce & Checkout Workflow
End-to-end purchasing process: Course ➔ Cart ➔ Order ➔ Payment ➔ Course Access
Flexible discount and coupon code engine (DiscountCode)
Modular payment gateway integration readiness
4. Learning & Progress Tracking
Automatic enrollment upon successful checkout (CourseAccess)
Granular progress tracking per lesson (LessonProgress)
Resume learning from the last watched lesson
5. Community & Content
Reviews & Ratings: Student feedback and star ratings on courses
Blog & Free Content: Articles and category system for content marketing
Notifications: Infrastructure for in-app and system announcements
6. Security & Error Handling
Centralized exception handling middleware mapped to standard HTTP status codes (400, 401, 404, 409, 500)
Protected endpoints with role-based policies
⚙️ Getting Started
Prerequisites
.NET 10 SDK
SQL Server
Visual Studio 2022+ or VS Code
Installation & Setup
Clone the repository:
bash
   git clone https://github.com/karami23/music-learning-platform-backend.git
   cd music-learning-platform-backend
   
Configure Connection String:Update your database settings in appsettings.json:
json
   {
"ConnectionStrings": {
"DefaultConnection": "Server=.;Database=MusicEducationDb;Trusted_Connection=True;TrustServerCertificate=True;"
},
"Jwt": {
"SecretKey": "YOUR_SECURE_SECRET_KEY",
"Issuer": "MusicEducation",
"Audience": "MusicEducationClient"
}
   }
   
Apply Database Migrations:
bash
   dotnet ef database update --project src/MusicEducation.Infrastructure --startup-project src/MusicEducation.API
   
Run the Application:
bash
   dotnet run --project src/MusicEducation.API
   
Explore Swagger UI:Open your browser and navigate to:
text
   https://localhost:xxxx/swagger
   
🔮 Roadmap & Future Enhancements
Live virtual classrooms & webinars
Student assignment submission and teacher feedback system
Quizzes and automatic certificate issuance
Cloud object storage & CDN integration for video streaming
AI-driven personalized course recommendations
