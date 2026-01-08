# 🗂️ Task Management System – ASP.NET Core Web API

A production-ready Task Management System built with .NET 9, ASP.NET Core Web API, PostgreSQL, EF Core 9, JWT Authentication, and Docker.

The project demonstrates real-world backend architecture including authentication, authorization, database migrations & seeding, health checks, and containerized deployment.

---

## 🚀 Tech Stack
- .NET 9 / ASP.NET Core Web API
- Entity Framework Core 9
- PostgreSQL 16
- JWT Authentication & Authorization
- Docker & Docker Compose
- Swagger / OpenAPI
- Health Checks

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
- │ ├── README.md
- │ ├── ARCHITECTURE.md
- │ ├── AUTHENTICATION.md
- │ ├── PERMISSIONS.md
- │ ├── TASKS.md
- │  └── API_FLOW.md

---

## 🗄️ Database
Relational SQL database using EF Core Code-First with migrations and proper constraints.
- PostgreSQL
- EF Core migrations applied automatically on startup
- Initial data seeding (admin user)

---

## 📚 Project Documentation

Detailed documentation describing how the system works:

- [Architecture Overview](https://github.com/bm1nev/TaskManagementSystem/blob/master/docs/ARCHITECTURE.md)
- [Authentication Flow](https://github.com/bm1nev/TaskManagementSystem/blob/master/docs/AUTHENTICATION.md)
- [Authorization & Permissions](https://github.com/bm1nev/TaskManagementSystem/blob/master/docs/PERMISSIONS.md)
- [Task Management](https://github.com/bm1nev/TaskManagementSystem/blob/master/docs/TASKS.md)
- [API Request Flow](https://github.com/bm1nev/TaskManagementSystem/blob/master/docs/API_FLOW.md)


---

## 🐳 Run with Docker
#### Prerequisites
- Docker Desktop
- Docker Compose

```
git clone https://github.com/bm1nev/TaskManagementSystem.git


docker compose up -d --build
```
---

## Services
- Swagger UI: http://localhost:8080/swagger

---

## 🔑 Default Admin Account (Seeded)

```
Email: admin@tms.local
Password: admin123!
```
⚠️ Used only for development / demo purposes.

---

## ❤️ Health Checks

The API exposes health endpoints for monitoring and container orchestration:

```
GET /health  
GET /health/ready
```

---

## 📌 License
This project is intended for educational and portfolio purposes.