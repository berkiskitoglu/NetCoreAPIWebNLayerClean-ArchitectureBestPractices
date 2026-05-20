# 🚀 .NET Core 8 Web API — NLayer & Clean Architecture

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12-239120?style=flat-square&logo=csharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![EF Core](https://img.shields.io/badge/EF%20Core-8.0-purple?style=flat-square)](https://learn.microsoft.com/en-us/ef/core/)
[![License](https://img.shields.io/badge/License-MIT-yellow?style=flat-square)](LICENSE)

Fatih Çakıroğlu'nun Udemy'deki **"Net 8 API/WEB | NLayer/Clean Architecture | Best Practice"** eğitimi kapsamında geliştirilen örnek proje. Hem **N-Layer** hem de **Clean Architecture** prensipleri uygulanmış, production-ready bir .NET 8 Web API şablonu.

---

## 📐 Mimari Yapı

Bu repo iki bağımsız solution içermektedir: `NLayer` ve `CleanArchitecture`.

---

### 1️⃣ N-Layer Architecture

```
┌─────────────────────────────────────────────────────┐
│                    App.Api                          │
│         Controllers · Program.cs · appsettings      │
└──────────────────────┬──────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────┐
│                  App.Services                       │
│  Categories · Products · ExceptionHandler           │
│  Filters · Mapping · Extensions                     │
│  FluentValidationFilter · ServiceResult             │
└──────────────────────┬──────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────┐
│                App.Repositories                     │
│  Categories · Products · Migrations                 │
│  AppDbContext · BaseEntity · GenericRepository      │
│  IGenericRepository · IUnitOfWork · UnitOfWork      │
│  IAuditEntity · Interceptors · Extensions           │
└─────────────────────────────────────────────────────┘
```

---

### 2️⃣ Clean Architecture

```
Src/
├── API/
│   └── CleanApp.API          ← Controllers, ExceptionHandler,
│                                Filters, Extensions
│
├── Core/
│   ├── App.Application       ← Contracts, Features (CQRS),
│   │                            Extensions, ServiceResult
│   └── App.Domain            ← Entities, Events, Exceptions,
│                                Consts, Options
│
└── Infrastructure/
    ├── App.Bus               ← Message Bus
    ├── App.Caching           ← Cache Abstraction
    └── App.Persistence       ← AppDbContext, GenericRepository,
                                 CategoryConfiguration,
                                 Interceptors, Migrations
```

> **Temel kural:** Bağımlılıklar yalnızca **içe** doğru akar.  
> `API → Application → Domain` · `Persistence → Application → Domain`  
> Domain hiçbir katmana bağımlı değildir.

---

## 📁 Gerçek Proje Yapısı

### NLayer

```
NLayer/
├── App.Api/
│   ├── Controllers/
│   ├── Properties/
│   ├── appsettings.json
│   └── Program.cs
│
├── App.Repositories/
│   ├── Categories/
│   ├── Extensions/
│   ├── Interceptors/
│   ├── Migrations/
│   ├── Products/
│   ├── AppDbContext.cs
│   ├── BaseEntity.cs
│   ├── ConnectionStringOption.cs
│   ├── GenericRepository.cs
│   ├── IAuditEntity.cs
│   ├── IGenericRepository.cs
│   ├── IUnitOfWork.cs
│   ├── RepositoryAssembly.cs
│   └── UnitOfWork.cs
│
└── App.Services/
    ├── Categories/
    ├── ExceptionHandler/
    ├── Extensions/
    ├── Filters/
    ├── Mapping/
    ├── Products/
    ├── FluentValidationFilter.cs
    ├── ServiceAssembly.cs
    └── ServiceResult.cs
```

### CleanArchitecture

```
CleanArchitecture/
└── Src/
    ├── API/
    │   └── CleanApp.API/
    │       ├── Controllers/
    │       ├── ExceptionHandler/
    │       ├── Extensions/
    │       ├── Filters/
    │       └── Program.cs
    │
    ├── Core/
    │   ├── App.Application/
    │   │   ├── Contracts/
    │   │   ├── Extensions/
    │   │   ├── Features/
    │   │   ├── ApplicationAssembly.cs
    │   │   └── ServiceResult.cs
    │   │
    │   └── App.Domain/
    │       ├── Consts/
    │       ├── Entities/
    │       ├── Events/
    │       ├── Exceptions/
    │       └── Options/
    │
    └── Infrastructure/
        ├── App.Bus/
        ├── App.Caching/
        └── App.Persistence/
            ├── Categories/
            ├── Extensions/
            ├── Interceptors/
            ├── Migrations/
            ├── Products/
            ├── AppDbContext.cs
            ├── CategoryConfiguration.cs
            ├── GenericRepository.cs
            └── PersistenceAssembly.cs
```

---

## 🛠️ Kullanılan Teknolojiler

| Teknoloji | Kullanım Amacı |
|-----------|----------------|
| ASP.NET Core 8 | Web API Framework |
| Entity Framework Core 8 | ORM — Code First, Migrations |
| AutoMapper | DTO ↔ Entity Mapping (Mapping/) |
| FluentValidation | Request doğrulama (FluentValidationFilter) |
| JWT Bearer | Kimlik doğrulama |
| Swagger / OpenAPI | API dokümantasyonu |
| CQRS (MediatR) | Clean Architecture — Features katmanı |
| SQL Server | Veritabanı |

---

## ✅ Uygulanan Best Practices

- **Generic Repository Pattern** — `IGenericRepository<T>` / `GenericRepository<T>`
- **Unit of Work Pattern** — `IUnitOfWork` / `UnitOfWork` ile tek SaveChanges noktası
- **BaseEntity** — ortak alan yönetimi (`Id`, `CreatedDate` vb.)
- **IAuditEntity** — otomatik audit alanları (Interceptor ile doldurulur)
- **SaveChanges Interceptor** — EF Core interceptor ile audit otomasyonu
- **Custom Exception Handling** — `ExceptionHandler` middleware her iki projede
- **ServiceResult / ServiceResult\<T\>** — tutarlı API yanıt yapısı
- **FluentValidationFilter** — action filter ile otomatik model doğrulama
- **Assembly Marker** — `RepositoryAssembly`, `ServiceAssembly`, `PersistenceAssembly` ile temiz DI kaydı
- **Service Extensions** — `IServiceCollection` extension metodlarıyla modüler DI
- **CQRS + Features** — Clean Architecture'da her use-case kendi klasöründe
- **Async/Await** — tüm I/O operasyonları asenkron
- **Options Pattern** — `ConnectionStringOption` ile tip güvenli konfigürasyon

---

## ⚡ Kurulum ve Çalıştırma

### Gereksinimler

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- SQL Server
- Visual Studio 2022 / Rider / VS Code

### NLayer

```bash
cd NLayer

# appsettings.json içinde connection string'i güncelle
# "SqlCon": "Server=.;Database=NLayerDb;Trusted_Connection=True;"

dotnet ef database update --project App.Repositories --startup-project App.Api

dotnet run --project App.Api
```

### CleanArchitecture

```bash
cd CleanArchitecture

# appsettings.json içinde connection string'i güncelle

dotnet ef database update --project Src/Infrastructure/App.Persistence --startup-project Src/API/CleanApp.API

dotnet run --project Src/API/CleanApp.API
```

Swagger UI: `https://localhost:7xxx/swagger`

---

## 📚 Eğitim Kaynağı

Bu proje [Fatih Çakıroğlu](https://www.linkedin.com/in/fcakiroglu16/) hocamın Udemy'deki **"Net 8 API/WEB | NLayer/Clean Architecture | Best Practice"** adlı 24 saatlik eğitimi kapsamında geliştirilmiştir.

---
