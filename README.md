<div align="center">

<!-- HERO BANNER -->
<img width="100%" src="https://capsule-render.vercel.app/api?type=waving&color=0:1a1a2e,50:16213e,100:0f3460&height=220&section=header&text=PakClassified&fontSize=72&fontColor=e94560&animation=fadeIn&fontAlignY=38&desc=Pakistan's%20Enterprise-Grade%20Classified%20Marketplace&descAlignY=58&descSize=18&descColor=a8b2d8"/>

<br/>

<!-- BADGES -->
[![.NET](https://img.shields.io/badge/.NET_9-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular_21-DD0031?style=for-the-badge&logo=angular&logoColor=white)](https://angular.io/)
[![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white)](https://www.typescriptlang.org/)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)](https://jwt.io/)
[![License: MIT](https://img.shields.io/badge/License-MIT-e94560?style=for-the-badge)](LICENSE.txt)

<br/>

> **🚀 A production-ready, full-stack classified ads platform engineered with clean architecture, enterprise-grade patterns, and modern web technologies — built for Pakistan's digital marketplace.**

<br/>

[**View Demo**](#) · [**Report Bug**](https://github.com/muhammad-abuzar-chughtai/FullStack-PakClassified.WebApp/issues) · [**Request Feature**](https://github.com/muhammad-abuzar-chughtai/FullStack-PakClassified.WebApp/issues) · [**Star the Repo ⭐**](https://github.com/muhammad-abuzar-chughtai/FullStack-PakClassified.WebApp)

</div>

---

## 📌 Table of Contents

- [✨ Overview](#-overview)
- [🏗️ Architecture](#️-architecture)
- [⚡ Tech Stack](#-tech-stack)
- [🎯 Key Features](#-key-features)
- [📂 Project Structure](#-project-structure)
- [🚀 Getting Started](#-getting-started)
- [🔐 Authentication Flow](#-authentication-flow)
- [🗄️ Data Modeling](#️-data-modeling)
- [🧱 Backend Patterns](#-backend-patterns)
- [🌐 API Endpoints](#-api-endpoints)
- [🖥️ Frontend Highlights](#️-frontend-highlights)
- [🗺️ Roadmap](#️-roadmap)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)

---

## ✨ Overview

**PakClassified** is a scalable, full-stack classified ads marketplace tailored for the Pakistani market. It brings together the power of a robust **.NET 9 Web API** backend and a blazing-fast **Angular 21** frontend — connected through clean, enterprise-grade architecture.

Whether you're listing a car in Lahore, renting an apartment in Karachi, or selling electronics in Islamabad — PakClassified is the platform that makes it seamless.

```
📦 Real-world classifieds logic
🔒 Secure JWT Authentication
⚙️  Clean Architecture Backend
🎨 Responsive Angular SPA Frontend
📊 Optimized Data Modeling
```

---

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    CLIENT (Angular 21)                   │
│   ┌────────────┐  ┌────────────┐  ┌──────────────────┐  │
│   │  Auth Guard│  │  Services  │  │   Components     │  │
│   └────────────┘  └────────────┘  └──────────────────┘  │
└──────────────────────────┬──────────────────────────────┘
                           │ HTTP / REST
┌──────────────────────────▼──────────────────────────────┐
│                  API LAYER (.NET 9)                      │
│  ┌─────────────┐  ┌──────────────┐  ┌────────────────┐  │
│  │  Controllers│  │  Middleware  │  │  JWT Auth      │  │
│  └─────────────┘  └──────────────┘  └────────────────┘  │
│  ┌─────────────────────────────────────────────────────┐ │
│  │              APPLICATION LAYER                      │ │
│  │  ┌──────────────┐    ┌──────────────────────────┐   │ │
│  │  │  Services    │    │  DTOs / Validators        │   │ │
│  │  └──────────────┘    └──────────────────────────┘   │ │
│  └─────────────────────────────────────────────────────┘ │
│  ┌─────────────────────────────────────────────────────┐ │
│  │              DOMAIN / INFRASTRUCTURE                │ │
│  │  ┌──────────────┐    ┌──────────────────────────┐   │ │
│  │  │  Repositories│    │  Entity Framework Core    │   │ │
│  │  └──────────────┘    └──────────────────────────┘   │ │
│  └─────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────┘
                           │
                    ┌──────▼──────┐
                    │  DATABASE   │
                    │  (SQL Server│
                    │  / SQLite)  │
                    └─────────────┘
```

---

## ⚡ Tech Stack

### Backend
| Technology | Version | Purpose |
|---|---|---|
| **.NET** | 9 | Core Framework |
| **ASP.NET Core Web API** | 9 | REST API |
| **Entity Framework Core** | Latest | ORM & Migrations |
| **JWT Bearer Auth** | — | Authentication & Authorization |
| **C#** | 13 | Primary Language |

### Frontend
| Technology | Version | Purpose |
|---|---|---|
| **Angular** | 21 | SPA Framework |
| **TypeScript** | 5.x | Type Safety |
| **Angular Router** | 21 | Client-side Routing |
| **RxJS** | — | Reactive Programming |
| **CSS / SCSS** | — | Styling |

---

## 🎯 Key Features

### 🔒 Security First
- **JWT Authentication** — Stateless, secure token-based auth with refresh token support
- **Role-Based Authorization** — Fine-grained access control (Admin, Seller, Buyer)
- **Password Hashing** — Industry-standard bcrypt hashing
- **CORS Configuration** — Properly configured for cross-origin requests

### 🏛️ Clean Architecture
- Strict **Separation of Concerns** across layers
- **Repository Pattern** — Abstracted data access for testability
- **Service Layer** — Business logic completely decoupled from controllers
- **DTO Mapping** — Clean request/response models prevent over-posting

### 📊 Optimized Data Modeling
- **Normalized Schema** — Efficient relational data design
- **EF Core Migrations** — Version-controlled database schema changes
- **Seeded Test Data** — Ready for development & demo environments
- **Indexed Queries** — Performance-optimized database lookups

### 🎨 Modern Frontend
- **Angular 21 Standalone Components** — Lightweight, modular architecture
- **Lazy Loading** — Route-based code splitting for performance
- **Reactive Forms** — Strongly typed, validated form handling
- **HTTP Interceptors** — Automatic JWT token injection on every request
- **Auth Guards** — Protected routes for authenticated users only

### 🗂️ Classified Marketplace Logic
- ✅ Create, Edit, Delete Listings
- ✅ Category-based browsing
- ✅ Search & Filter functionality
- ✅ User profile & listing management
- ✅ Image upload support
- ✅ Listing status management (Active / Sold / Expired)

---

## 📂 Project Structure

```
FullStack-PakClassified.WebApp/
│
├── 📁 PakClassified.WebApp/          # .NET 9 Backend
│   ├── Controllers/                  # API endpoint controllers
│   ├── Models/                       # Domain entities
│   ├── DTOs/                         # Data Transfer Objects
│   ├── Services/                     # Business logic layer
│   ├── Repositories/                 # Data access abstraction
│   ├── Data/                         # DbContext & migrations
│   ├── Middleware/                   # Custom pipeline middleware
│   ├── Helpers/                      # JWT, utilities
│   └── Program.cs                    # App entry & DI configuration
│
├── 📁 PakClassified.Client/          # Angular 21 Frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/           # UI components
│   │   │   ├── services/             # HTTP & state services
│   │   │   ├── guards/               # Route protection
│   │   │   ├── interceptors/         # HTTP interceptors
│   │   │   ├── models/               # TypeScript interfaces
│   │   │   └── app.routes.ts         # Lazy-loaded routing
│   │   └── environments/             # Dev / prod configs
│
├── .gitignore
├── .gitattributes
├── LICENSE.txt
└── README.md
```

---

## 🚀 Getting Started

### Prerequisites

Make sure you have the following installed:

```bash
✅ .NET SDK 9.0+         https://dotnet.microsoft.com/download
✅ Node.js 20+           https://nodejs.org/
✅ Angular CLI 21        npm install -g @angular/cli
✅ SQL Server / SQLite   (or any EF Core supported DB)
```

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/muhammad-abuzar-chughtai/FullStack-PakClassified.WebApp.git
cd FullStack-PakClassified.WebApp
```

### 2️⃣ Backend Setup

```bash
# Navigate to the API project
cd PakClassified.WebApp

# Restore packages
dotnet restore

# Apply database migrations
dotnet ef database update

# Run the API
dotnet run
```

> 🌐 API will be available at: `https://localhost:7000` (or as configured)

### 3️⃣ Frontend Setup

```bash
# Navigate to the Angular client
cd ../PakClassified.Client

# Install dependencies
npm install

# Start the development server
ng serve
```

> 🌐 App will be available at: `http://localhost:4200`

### ⚙️ Configuration

Update `appsettings.json` in the backend:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_DB_Connection_String_Here"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-min-32-chars",
    "Issuer": "PakClassified",
    "Audience": "PakClassifiedClient",
    "ExpiryInMinutes": 60
  }
}
```

---

## 🔐 Authentication Flow

```
  User Login Request
        │
        ▼
  ┌─────────────┐
  │  Controller │  Validates credentials
  └──────┬──────┘
         │
         ▼
  ┌─────────────┐
  │   Service   │  Checks DB, validates password hash
  └──────┬──────┘
         │
         ▼
  ┌─────────────┐
  │  JWT Helper │  Generates signed Bearer token
  └──────┬──────┘
         │
         ▼
  Token returned to Angular Client
        │
        ▼
  Stored in memory / localStorage
        │
        ▼
  HTTP Interceptor injects token
  on every subsequent request ✅
```

---

## 🗄️ Data Modeling

The data model is designed with scalability and normalization in mind:

```
Users ──────────────────────────────┐
  │ (1)                             │
  │ has many                        │
  ▼ (N)                             ▼
Listings ──────► Categories      UserProfiles
  │
  │ has many
  ▼
ListingImages

Listings ──► LocationData (City, Province)
Listings ──► ListingStatus (Active, Sold, Expired)
```

Key design decisions:
- **Soft deletes** on Listings — data is never permanently lost
- **Audit fields** (`CreatedAt`, `UpdatedAt`) on all entities
- **Indexed foreign keys** for optimized JOIN performance
- **Enum-backed status** fields for type safety

---

## 🧱 Backend Patterns

This project implements the following enterprise backend patterns:

| Pattern | Implementation |
|---|---|
| **Repository Pattern** | `IRepository<T>` generic interface with EF Core implementations |
| **Service Layer** | Business logic isolated in service classes, away from controllers |
| **DTO Pattern** | Input/Output DTOs prevent over-posting and control API contracts |
| **Dependency Injection** | Full constructor DI via .NET's built-in IoC container |
| **Middleware Pipeline** | Custom exception handling, request logging middleware |
| **Options Pattern** | Strongly-typed configuration binding via `IOptions<T>` |

---

## 🌐 API Endpoints

### Auth
```http
POST   /api/auth/register      # Register new user
POST   /api/auth/login         # Login & receive JWT
```

### Listings
```http
GET    /api/listings           # Get all listings (paginated)
GET    /api/listings/{id}      # Get listing by ID
POST   /api/listings           # Create listing [Auth Required]
PUT    /api/listings/{id}      # Update listing [Auth Required]
DELETE /api/listings/{id}      # Delete listing [Auth Required]
```

### Categories
```http
GET    /api/categories         # Get all categories
GET    /api/categories/{id}    # Get category with listings
```

### Users
```http
GET    /api/users/profile      # Get own profile [Auth Required]
PUT    /api/users/profile      # Update profile [Auth Required]
```

---

## 🖥️ Frontend Highlights

- 📱 **Responsive Design** — Mobile-first layout, works on all screen sizes
- ⚡ **Standalone Components** — Modern Angular 21 architecture with no NgModules
- 🔄 **Reactive State** — RxJS-powered services for real-time UI updates
- 🛡️ **Route Guards** — `canActivate` guards prevent unauthorized access
- 🔗 **Interceptors** — Auth token automatically attached to API calls
- 📋 **Reactive Forms** — Complex forms with real-time validation feedback
- 🌍 **Environment Configs** — Separate dev/prod API base URL management

---

## 🗺️ Roadmap

- [x] JWT Authentication System
- [x] Listing CRUD Operations
- [x] Category Management
- [x] Clean Architecture Implementation
- [x] Angular 21 SPA Frontend
- [ ] 🔜 Real-time Notifications (SignalR)
- [ ] 🔜 Image Upload to Cloud (Azure Blob / Cloudinary)
- [ ] 🔜 Advanced Search & Filters
- [ ] 🔜 Messaging System between Buyers & Sellers
- [ ] 🔜 Admin Dashboard
- [ ] 🔜 Docker Containerization
- [ ] 🔜 CI/CD Pipeline (GitHub Actions)
- [ ] 🔜 Payment Integration

---

## 🤝 Contributing

Contributions are what make the open-source community such an amazing place to learn and grow. Any contributions you make are **greatly appreciated**!

1. **Fork** the Project
2. **Create** your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. **Commit** your Changes (`git commit -m 'Add some AmazingFeature'`)
4. **Push** to the Branch (`git push origin feature/AmazingFeature`)
5. **Open** a Pull Request

Please make sure to follow the existing code style and include tests where applicable.

---

## 👨‍💻 Author

<div align="center">

**Muhammad Abuzar Chughtai**

[![GitHub](https://img.shields.io/badge/GitHub-muhammad--abuzar--chughtai-181717?style=for-the-badge&logo=github)](https://github.com/muhammad-abuzar-chughtai)

*Full Stack .NET & Angular Developer*

</div>

---

## 📄 License

Distributed under the **MIT License**. See [`LICENSE.txt`](LICENSE.txt) for more information.

---

<div align="center">

<img width="100%" src="https://capsule-render.vercel.app/api?type=waving&color=0:0f3460,50:16213e,100:1a1a2e&height=120&section=footer"/>

**⭐ If you found this project helpful, please give it a star! It means a lot. ⭐**

*Built with ❤️ in Pakistan 🇵🇰*

</div>
