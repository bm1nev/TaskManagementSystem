# Authorization & Permissions

The system enforces fine-grained authorization rules.

## System Roles
- Admin
- User

## Project Roles
- Owner
- Manager
- Member

## Permission Rules
- Only project members can access project data
- Owners and Managers can manage tasks and members
- Members have limited access
- Unauthorized access is blocked with proper HTTP status codes

## Validation
Permissions are validated in the application layer, not in controllers.
