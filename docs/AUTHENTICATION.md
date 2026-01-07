# Authentication Flow

The system uses JWT (JSON Web Tokens) for authentication.

## Flow
1. User registers with email and password
2. Password is securely hashed and stored
3. User logs in with credentials
4. Server generates JWT token
5. Token is sent in Authorization header for protected endpoints

## Security
- Password hashing
- Token expiration
- Authorization middleware
- Role-based access control
