# 🧾 Invoice Management System

A clean architecture invoice management system with Domain-Driven Design (DDD), featuring N-to-N master-detail relationships handled via Stored Procedures, and AJAX API calls through a JavaScript proxy.

---

## 🚀 Key Features

- ✅ **Clean Architecture** (Domain, Application, Infrastructure, Presentation layers)
- ✅ **Domain-Driven Design** (Entities, Value Objects, Aggregates, Repositories)
- ✅ **N-to-N Master-Detail Relationship** (`Invoice` ↔ `Product` via `InvoiceProduct` junction table)
- ✅ **Stored Procedures** for all critical database operations (INSERT, UPDATE, DELETE on master-detail)
- ✅ **Pure JavaScript AJAX** (XMLHttpRequest) for API calls - no page refresh
- ✅ **JavaScript Proxy Pattern** to intercept and manage all API requests (logging, error handling, auth)
- ✅ **ASP.NET Core Web API** with Swagger documentation

---

## 🧱 Tech Stack

| Layer               | Technology                                 |
|---------------------|--------------------------------------------|
| Backend API         | ASP.NET Core 9                     |
| Database Access     | ADO.NET with Stored Procedures             |
| Architecture        | Clean Architecture + DDD                   |
| Client-Side         | HTML5, CSS3, Vanilla JavaScript            |
| API Communication   | AJAX (XMLHttpRequest) + Custom Proxy       |
| Database            | SQL Server / PostgreSQL                    |

---

## 📁 Project Structure (Clean Architecture)
