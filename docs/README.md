# 🗂️ Task Management System – ASP.NET Core Web API

Backend-focused Task Management System (mini Jira / Trello) built with **ASP.NET Core** and **Entity Framework Core**, demonstrating clean architecture, secure API design, and real-world business logic.

---

## 🚀 Tech Stack
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server (SQLEXPRESS)
- JWT Authentication
- Swagger / OpenAPI

---

## 🎯 Features 
- JWT authentication (register / login)
- Role-based authorization (Admin / User)
- Projects with ownership
- Project members (Owner / Manager / Member)
- Tasks with lifecycle: Open → InProgress → Done
- Centralized permission checks
- Global exception handling with unified error responses

---

## 🧱 Architecture
Clean Architecture–inspired structure with clear separation of concerns.

- TaskManagementSystem/
- │
- ├── src/
- │ ├── TaskManagementSystem.Application
- │ ├── TaskManagementSystem.Domain
- │ ├── TaskManagementSystem.Infrastructure
- │ └── TaskManagementSystem.WebApi
- │
- ├── docs/
- ├── README.md
- ├── ARCHITECTURE.md
- ├── AUTHENTICATION.md
- ├── PERMISSIONS.md
- ├── TASKS.md
- └── API_FLOW.md

---

## 🗄️ Database
Relational SQL database using EF Core Code-First with migrations and proper constraints.

---

## 📚 Project Documentation

Detailed documentation describing how the system works:

- [Architecture Overview](docs/ARCHITECTURE.md)
- [Authentication Flow](docs/AUTHENTICATION.md)
- [Authorization & Permissions](docs/PERMISSIONS.md)
- [Task Management](docs/TASKS.md)
- [API Request Flow](docs/API_FLOW.md)

---

## 🛠️ Run Locally

```
git clone https://github.com/bm1nev/TaskManagementSystem.git

dotnet ef database update
dotnet run --project src/TaskManagementSystem.WebApi
```

---

## 📌 License
Educational and portfolio purposes only.
