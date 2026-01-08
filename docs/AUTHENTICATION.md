# 🔐 Authentication & Authorization

The Task Management System uses **JWT (JSON Web Token) authentication** combined with **role-based authorization** to secure API access.

---

## 🧾 Authentication Flow

1. User submits credentials (email & password)
2. Credentials are validated against stored password hash
3. On success, a JWT token is generated
4. The token is returned to the client
5. Client includes the token in subsequent requests

Authorization: Bearer <JWT_TOKEN>


---

## 🔑 JWT Token Contents

Each token includes the following claims:
- User Id
- Email
- Role
- Issuer
- Audience
- Expiration time

The token is signed using a symmetric signing key.

---

## 🔒 Password Security

- Passwords are **never stored in plain text**
- Password hashing is performed via an abstraction defined in the Application layer (`IPasswordHasher`)
- The concrete implementation uses the ASP.NET Core password hashing mechanism
- Password verification is done using secure hash comparison

This approach ensures:
- No direct dependency on framework-specific APIs in the Application layer
- Easy replacement or extension of the hashing strategy
- Better testability and separation of concerns


---

## 👥 Authorization Model

The system uses **role-based authorization**:

### Roles
- **Admin**
- **User**

Roles are embedded as claims inside the JWT token.

---

## 🛂 Authorization Enforcement

- `[Authorize]` attributes protect endpoints
- Role checks are enforced using claims
- Business-level permission checks are performed inside Application services

This ensures that:
- Authorization is not only controller-based
- Business rules cannot be bypassed

---

## 🧠 Security Considerations

- Token expiration is enforced
- Invalid or expired tokens result in `401 Unauthorized`
- Insufficient permissions result in `403 Forbidden`
- Signing key, issuer, and audience are configurable per environment

---

## 🔄 Token Lifecycle

- Tokens are stateless
- No server-side session storage
- Token validity is verified on every request

---

## 📌 Summary

The authentication system is designed to be secure, extensible, and suitable for production environments, while remaining simple for client integration.
