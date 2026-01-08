# 🔁 API Request Flow

This document describes the full lifecycle of an HTTP request in the Task Management System,
from the moment it enters the API to the final database operation and response.

The goal is to provide a clear understanding of how authentication, authorization,
business logic, and persistence interact.

---

## 🧭 High-Level Request Lifecycle

- Client
- ↓
- ASP.NET Core Middleware Pipeline
- ↓
- Authentication (JWT)
- ↓
- Authorization
- ↓
- Controller
- ↓
- Application Service
- ↓
- Permission Checks
- ↓
- Repository (EF Core)
- ↓
- PostgreSQL Database
- ↓
- Response


---

## 🌐 Step 1: Incoming HTTP Request

The client sends an HTTP request to the API.

Example:
```
GET /api/projects
Authorization: Bearer <JWT_TOKEN>
```


At this stage:
- Routing is resolved
- Middleware pipeline is entered

---

## 🔐 Step 2: Authentication (JWT)

- JWT token is extracted from the `Authorization` header
- Token signature, issuer, audience, and expiration are validated
- If validation fails → `401 Unauthorized`
- On success, a `ClaimsPrincipal` is created

Claims typically include:
- User Id
- Email
- Role

---

## 🛂 Step 3: Authorization

Authorization is enforced at two levels:

### 1️⃣ Controller-level authorization
- `[Authorize]` attributes protect endpoints
- Role-based checks may apply

Failure results in:
- `401 Unauthorized` (no/invalid token)
- `403 Forbidden` (insufficient role)

### 2️⃣ Business-level authorization
- Enforced inside application services
- Uses centralized permission checks (e.g. `ProjectAccessService`)
- Cannot be bypassed even if controller access is granted

---

## 🎯 Step 4: Controller Layer

Controllers are intentionally thin.

Responsibilities:
- Model binding
- HTTP validation
- Delegating work to application services

Controllers:
- Do **not** contain business logic
- Do **not** access the database directly

Example:
```text
ProjectsController → ProjectService
```

---

## 🧠 Step 5: Application Services

Application services orchestrate the business workflow.

#### Responsibilities:
- Business rules
- Permission checks
- Use case coordination

#### Examples:
- AuthService
- ProjectService
- TaskService

This layer represents the core behavior of the system.

--- 

## 🛡️ Step 6: Permission Checks

Project-scoped permissions are validated using centralized services.

Examples:
- Verify project membership
- Verify role inside the project (Owner / Manager / Member)
- Verify cross-project access (Admin policy)

Failure results in:
- `403 Forbidden`

---

## 🗄️ Step 7: Data Access (Repositories)

Repositories encapsulate database access.

Responsibilities:
- Querying entities
- Persisting changes
- Enforcing consistency through EF Core constraints

Key points:
- No business logic inside repositories
- EF Core handles change tracking and relationships

---

## 🐘 Step 8: Database Interaction (PostgreSQL)

- EF Core generates SQL queries
- PostgreSQL executes them
- Constraints and indexes enforce data integrity
- Results are mapped back to domain entities

---

## 🔄 Step 9: Response Generation

- Application service returns a result
- Controller maps it to an HTTP response
- Global exception middleware handles errors

Standard responses:
- `200 OK` / `201 Created`
- `400 Bad Request`
- `401 Unauthorized`
- `403 Forbidden`
- `404 Not Found`
- `409 Conflict`

---

## ⚠️ Global Exception Handling

All unhandled exceptions are captured by a centralized middleware.

Benefits:
- Consistent error format
- No leaking of internal details
- Clear error semantics for clients

---


## 📌 Summary

Each API request follows a clear, layered flow:
- Security is enforced early and repeatedly
- Controllers remain thin
- Business rules live in the Application layer
- Permissions are centralized and consistent
- Data access is isolated and controlled

This design ensures correctness, security, and maintainability as the system evolves.