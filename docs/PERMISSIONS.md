# 🛂 Authorization & Permissions

The system uses a **two-level authorization model**:

1. **Platform roles** (JWT role claim): `Admin`, `User`
2. **Business permissions** (project-scoped): `Owner`, `Manager`, `Member`

This separation ensures that security is not limited to controller attributes and cannot be bypassed by calling services directly.

---

## 🔐 Level 1: Platform Roles (JWT)

### Roles
- **Admin** – elevated access (administration and management features)
- **User** – standard access

### Where roles are enforced
- At the API boundary using `[Authorize]` and role/claim checks (where applicable)
- Also inside application services for business-critical operations

### Expected behavior
- Missing/invalid token → `401 Unauthorized`
- Valid token but insufficient role → `403 Forbidden`

---

## 🧩 Level 2: Business Permissions (Project Scope)

Projects have scoped membership rules that control what a user can do inside a project.

### Project membership roles
- **Owner** – full control over the project (including membership management)
- **Manager** – can manage tasks and members (depending on policy)
- **Member** – can work on tasks, read project data

> Note: The exact capabilities are enforced by centralized permission checks, not by UI/client assumptions.

---

## 🏗️ Centralized Permission Checks

All project-scoped permissions are enforced through a dedicated service (e.g. `ProjectAccessService`).

### Why centralized checks matter
- Keeps controllers thin and consistent
- Prevents duplicated logic across endpoints
- Ensures business rules remain correct even if new endpoints are added
- Makes the system easier to audit and extend

---

## ✅ Access Rules (High-Level)

### Project access
A user can access a project if:
- They are the **Owner**, or
- They are a **Project Member** (Owner/Manager/Member), or
- They are **Admin** (depending on admin policy)

### Membership management
Typically restricted to:
- Project **Owner**
- Optionally **Admin**

### Task access
A user can access tasks if:
- They belong to the task's project and have at least `Member` access
- Admin policy may allow cross-project access

### Task assignment
Allowed when:
- The acting user has sufficient project role (Owner/Manager)
- The target user is a member of the same project (or is being added based on policy)

---

## 📌 Permission Matrix

This is a clear model that matches the common intent of your roles:

| Action | Owner | Manager | Member |
|---|---:|--------:|-------:|
| View project | ✅ |       ✅ |      ✅ |
| Update project details | ✅ |       ✅ |      ❌ |
| Delete project | ✅ |       ❌ |      ❌ |
| Add/remove members | ✅ |       ✅ |      ❌ |
| Create tasks | ✅ |       ✅ |      ✅ |
| Update tasks | ✅ |       ✅ |      ✅ |
| Change task status | ✅ |       ✅ |      ✅ |
| Assign tasks | ✅ |       ✅ |      ❌ |


---

## 🚫 Failure Modes & Responses

The API uses consistent HTTP responses:

- `401 Unauthorized` – no token / invalid token / expired token
- `403 Forbidden` – authenticated, but missing permission/role
- `404 Not Found` – resource doesn’t exist OR access is intentionally hidden
- `400 Bad Request` – invalid input / rule violation
- `409 Conflict` – uniqueness constraints or domain conflicts (where applicable)

---

## 🧪 Practical Examples

### Example: User tries to access a project they are not a member of
- Expected result: `403 Forbidden`

### Example: Member attempts to remove another member
- Expected result: `403 Forbidden`

### Example: Manager assigns a task to a user outside the project
- Expected result: `400` or `403` 

---

## 🔍 Where to look in code

- `TaskManagementSystem.Application.Services` – business workflows
- `TaskManagementSystem.Infrastructure.Repositories` – data access
- `ProjectAccessService` – centralized permission checks
- Controllers – only HTTP boundary and routing, no business authorization logic

---

## 📌 Summary

The permissions model is designed to be:
- **Secure** (cannot be bypassed)
- **Consistent** (centralized checks)
- **Maintainable** (easy to evolve as features expand)

Platform roles (Admin/User) define global capabilities, while project roles (Owner/Manager/Member) define project-scoped permissions.
