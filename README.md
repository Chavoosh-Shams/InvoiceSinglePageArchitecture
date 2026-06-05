# 🧾 Invoice Management System

A clean architecture invoice management system with Domain-Driven Design (DDD), featuring N-to-N master-detail relationships handled via Stored Procedures, Code First approach with Fluent API, Repository pattern, and AJAX API calls through a JavaScript proxy.

---

## 🚀 Key Features

- ✅ **Clean Architecture** (Domain, ApplicationServices, Infrastructure, Presentation layers)
- ✅ **Domain-Driven Design** (Entities, Value Objects, Aggregates, Repositories)
- ✅ **N-to-N Master-Detail Relationship** (`Invoice` ↔ `Product` via `InvoiceProduct` junction table)
- ✅ **Code First Approach** with EF Core
- ✅ **Fluent API** for entity configurations (relationships, constraints, indexes)
- ✅ **Repository Pattern** for data access abstraction
- ✅ **Stored Procedures** for complex N-to-N transactions (INSERT, UPDATE, DELETE on master-detail) with sending JSON to Database run SP
- ✅ **Pure JavaScript AJAX** (XMLHttpRequest) for API calls - Single Page Architecture -no page refresh 
- ✅ **JavaScript Proxy Pattern** to intercept and manage all API requests (logging, error handling, auth)
- ✅ **ASP.NET Core** with Swagger documentation

---

## 🧱 Tech Stack

| Layer               | Technology                                 |
|---------------------|--------------------------------------------|
| Backend API         | ASP.NET Core 9                             |
| ORM                 | Entity Framework Core (Code First)         |
| Database Mapping    | Fluent API                                 |
| Data Access         | Repository Pattern + Stored Procedures     |
| Architecture        | Clean Architecture + DDD                   |
| Client-Side         | HTML5, CSS3, Vanilla JavaScript            |
| API Communication   | AJAX (XMLHttpRequest) + Custom Proxy       |
| Database            | SQL Server / PostgreSQL                    |

---

## 📁 Project Structure (Clean Architecture)
