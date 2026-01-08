# 🧱 Architecture Overview

The Task Management System is built using a **Clean Architecture–inspired approach**, focused on separation of concerns, testability, and long-term maintainability.

The solution is divided into four main layers, each with a clear responsibility and strict dependency direction.

---

## 🏗️ Solution Structure

The solution is organized using a physical `src/` folder for source code
and a `docs/` folder for technical documentation.

```text
TaskManagementSystem
│
├── src
│   ├── TaskManagementSystem.Domain
│   ├── TaskManagementSystem.Application
│   ├── TaskManagementSystem.Infrastructure
│   └── TaskManagementSystem.WebApi
│
├── docs
│   ├── ARCHITECTURE.md
│   ├── AUTHENTICATION.md
│   ├── PERMISSIONS.md
│   ├── TASKS.md
│   └── API_FLOW.md
│
├── docker-compose.yml
├── README.md
└── TaskManagementSystem.sln
```

---

## 🔹 Domain Layer

**Purpose:**  
- Contains the core business entities and domain logic.

**Responsibilities:**
- Domain entities (User, Project, Task, ProjectMember, etc.)
- Enums and value objects
- Business rules that are independent of frameworks or infrastructure

**Key characteristics:**
- No dependencies on other layers
- Pure C# classes
- Represents the problem domain

---

## 🔹 Application Layer

**Purpose:**  
- Defines application use cases and orchestrates domain logic.

**Responsibilities:**
- Application services (AuthService, ProjectService, TaskService, etc.)
- Interfaces for repositories and infrastructure services
- Business workflows and validation logic

**Key characteristics:**
- Depends only on the Domain layer
- Contains no EF Core, HTTP, or infrastructure-specific code
- Acts as the core of the application behavior

---

## 🔹 Infrastructure Layer

**Purpose:**  
- Implements technical details and external integrations.

**Responsibilities:**
- EF Core DbContext and entity configurations
- Repository implementations
- JWT token generation
- Password hashing
- Database migrations and seeding

**Key characteristics:**
- Depends on Application and Domain
- Contains framework-specific code (EF Core, Npgsql)
- Fully replaceable without affecting higher layers

---

## 🔹 Web API Layer

**Purpose:**  
- Exposes the application functionality via HTTP.

**Responsibilities:**
- Controllers
- Authentication & authorization middleware
- Swagger configuration
- Global exception handling
- Health check endpoints

**Key characteristics:**
- Thin layer (no business logic)
- Delegates all work to Application services
- Handles HTTP concerns only

---

## 🔄 Dependency Flow
WebApi

↓

Application

↓

Domain


Infrastructure is injected into the Application layer via **Dependency Injection**.

---

## ⚙️ Cross-Cutting Concerns

- **Authentication & Authorization** – JWT-based, handled at the Web API level
- **Exception Handling** – centralized global middleware
- **Validation & Permissions** – enforced inside Application services
- **Health Checks** – exposed for container orchestration and monitoring

---

## 🎯 Architectural Goals

- High cohesion, low coupling
- Clear ownership of responsibilities
- Easy testing and future extension
- Production-ready backend foundation

---

## 📌 Summary

This architecture enables the system to scale in complexity while remaining understandable, maintainable, and secure.
