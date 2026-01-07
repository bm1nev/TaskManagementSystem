# Request Lifecycle

1. HTTP request hits the controller
2. Authentication middleware validates JWT token
3. Authorization checks permissions
4. Application service executes business logic
5. Domain rules are validated
6. Data is persisted via EF Core
7. Response is returned
8. Exceptions are handled by global exception middleware
