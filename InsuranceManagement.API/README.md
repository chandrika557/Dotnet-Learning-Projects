# Insurance Management API learning guide

This solution implements the insurance API roadmap using ASP.NET Core 8, Minimal APIs, EF Core, SQL Server, JWT bearer authentication, and layered architecture. The original MVC application remains a separate project.

## Projects and dependency direction

| Project | Responsibility |
| --- | --- |
| `InsuranceManagement.Domain` | Customer, policy, claim, user-account entities, enums, and role names. |
| `InsuranceManagement.Application` | DTOs, repository contracts, services, validation, AutoMapper profiles, and customer CQRS use cases. |
| `InsuranceManagement.Infrastructure` | EF Core `ApplicationDbContext`, SQL Server mappings, migrations, and repository implementations. |
| `InsuranceManagement.API` | HTTP routes, request binding, authentication/authorization, Swagger, exception handling, and composition root. |
| `InsuranceManagement.API.Tests` | Unit tests for application use cases and validation. |

The API project is the composition root and references the other projects. Application references Domain; Infrastructure references Application and Domain; Domain has no project dependencies. The intended request flow is:

```text
HTTP endpoint -> Application use case/service -> repository interface -> Infrastructure repository -> EF Core -> SQL Server
```

## Requirements and first run

- Install the .NET 8 SDK.
- Start a local SQL Server or SQL Server Express instance.
- Update `ConnectionStrings:DefaultConnection` in the API settings for your SQL Server instance. The API database is `InsuranceManagementApiDb`; it is separate from the MVC database.
- Set secrets and apply database migrations as described below.

From the repository root:

```powershell
dotnet restore InsuranceManagement.sln
dotnet build InsuranceManagement.sln
dotnet test InsuranceManagement.sln
dotnet run --project InsuranceManagement.API\InsuranceManagement.API.csproj
```

The development profile listens on `http://localhost:5209`. Open `http://localhost:5209/swagger` for interactive API documentation. Requests for all routes are also in `InsuranceManagement.API.http`.

## First APIs: in-memory GET and POST

Start with the public demo endpoints in Swagger under **Demo items**:

- `GET /api/demo/items` returns the current list.
- `GET /api/demo/items/{id}` returns one item or 404.
- `POST /api/demo/items` accepts `{ "name": "Sample item" }` and returns 201 with a `Location` URL for the new item.

These demo items live only in memory; restarting the API clears them. This keeps the first HTTP exercise simple. Customer, policy, and claim data use SQL Server separately.

## Secret configuration and initial administrator

JWT signing keys and bootstrap administrator passwords are deliberately absent from source-controlled JSON. Configure them with .NET user secrets for local development:

```powershell
dotnet user-secrets init --project InsuranceManagement.API\InsuranceManagement.API.csproj
dotnet user-secrets set "Jwt:SigningKey" "<a-random-secret-at-least-32-bytes>" --project InsuranceManagement.API\InsuranceManagement.API.csproj
dotnet user-secrets set "BootstrapAdministrator:Email" "admin@example.com" --project InsuranceManagement.API\InsuranceManagement.API.csproj
dotnet user-secrets set "BootstrapAdministrator:Password" "<a-strong-password-at-least-12-characters>" --project InsuranceManagement.API\InsuranceManagement.API.csproj
```

Use a cryptographically random signing key, keep it private, and do not paste it into source code, chat, or a checked-in configuration file. Production should use its managed secret store. The application refuses to start without a valid JWT signing key. The bootstrap account is created only when both administrator settings are present; public registration always assigns the `Customer` role. Bootstrap credentials should be provisioned through a secure deployment process and removed from that process after initial provisioning. If a configured bootstrap email already belongs to a non-administrator, startup fails instead of silently granting or ignoring privileges.

## Database migrations

Migrations live in the Infrastructure project. The first migration creates customers; the second adds policies, claims, foreign keys, and accounts. After configuring the connection string and JWT key:

```powershell
dotnet ef migrations list --project InsuranceManagement.Infrastructure\InsuranceManagement.Infrastructure.csproj --startup-project InsuranceManagement.API\InsuranceManagement.API.csproj
dotnet ef database update --project InsuranceManagement.Infrastructure\InsuranceManagement.Infrastructure.csproj --startup-project InsuranceManagement.API\InsuranceManagement.API.csproj
```

To record future entity changes:

```powershell
dotnet ef migrations add <DescriptiveMigrationName> --project InsuranceManagement.Infrastructure\InsuranceManagement.Infrastructure.csproj --startup-project InsuranceManagement.API\InsuranceManagement.API.csproj --output-dir Data\Migrations
dotnet ef database update --project InsuranceManagement.Infrastructure\InsuranceManagement.Infrastructure.csproj --startup-project InsuranceManagement.API\InsuranceManagement.API.csproj
```

Always inspect a generated migration before applying it. Do not point the API migration at the MVC application's database.

## Phase-by-phase learning notes

### 1. Basic API and CRUD

`Program.cs` creates the host, registers application and infrastructure services, configures authentication and Swagger, then maps customer, policy, claim, and authentication routes. Resource-specific Minimal API extension classes keep HTTP routes grouped. Creation returns `201 Created` with a `Location` header; missing resources return 404; deletes return 204. DTOs keep the JSON API contract separate from database entities.

Interview focus: explain HTTP verbs, status codes, routing, dependency injection, Swagger/OpenAPI, and why entities should not be returned directly.

Common mistake: returning 200 from create, or returning a tracked EF entity with navigation properties and internal fields.

### 2. Repository pattern

Repository interfaces are in `InsuranceManagement.Application/Repositories`; implementations are in `InsuranceManagement.Infrastructure/Repositories`. Endpoints and services depend on contracts, not on `DbContext`. Reads use `AsNoTracking`; async database calls receive cancellation tokens.

Interview focus: repositories isolate persistence and enable substitution in tests. EF Core already implements repository/unit-of-work-like abstractions, so add a custom repository when it expresses application operations or creates a useful test boundary—not just to wrap each EF method mechanically.

Common mistake: allowing endpoint code to inject `ApplicationDbContext` after a repository has been introduced.

### 3. Service layer

Customer read and write use cases are separated into `ICustomerQueryService` and `ICustomerCommandService`; policy, claim, and account workflows have their own services. Services validate business conditions such as required customer/policy existence, preventing deletion when dependants exist, and allowing review only for pending claims.

Interview focus: HTTP concerns belong to endpoints, business rules belong to application use cases, and database persistence belongs behind repository contracts.

Common mistake: moving every line into a service without a meaningful use case, or putting domain rules in endpoint handlers.

### 4. DTOs and validation

API request and response contracts are sealed records in `InsuranceManagement.Application/DTOs`. FluentValidation rules are in `InsuranceManagement.Application/Validators`; the API's `ValidationFilter<TRequest>` runs the right validator and returns validation Problem Details. AutoMapper profiles centralize explicit entity/DTO mappings and cap mapping depth.

Interview focus: DTOs protect API compatibility and prevent over-posting; FluentValidation separates validation rules from HTTP; mapping libraries reduce repetitive mapping code but explicit mapping can be simpler for small systems.

Common mistake: assuming .NET 8 Minimal APIs automatically apply data-annotation validation. This project uses a registered endpoint filter instead.

### 5. Exception handling

`ApiExceptionHandler` maps expected application validation, not-found, and conflict exceptions into RFC Problem Details responses. Unexpected exceptions are not converted into success-shaped defaults; the exception middleware handles them and logs them.

Interview focus: centralized exception translation keeps route handlers clear and produces a consistent error contract.

Common mistake: exposing SQL exception messages, stack traces, or arbitrary exception details to API clients.

### 6. Logging

Serilog is configured from application configuration and writes structured events to the console. Request logging records HTTP requests, and the standard ASP.NET Core logging interfaces remain available to services.

Interview focus: structured logs preserve searchable properties such as status code and request path rather than embedding all data in a formatted string.

Common mistake: logging passwords, bearer tokens, signing keys, or sensitive personal/claims information. Configure protected sinks and retention before production deployment.

### 7. Authentication and authorization

Registration hashes passwords with ASP.NET Core's `PasswordHasher<TUser>` and always assigns `Customer`. Login returns a signed, short-lived JWT; invalid credentials have a generic 401 response. The signing key, issuer, audience, and expiry are checked when tokens are validated. Customer/policy/claim operations require a `ClaimsAdjuster` or `Administrator`; assigning roles requires `Administrator`.

Interview focus: authentication proves identity; authorization checks permissions. Password hashing is intentionally one-way; JWT claims carry identity and role but are not encrypted.

Common mistake: accepting roles in public registration, storing plaintext passwords, logging tokens, or using a default signing key. The key is required at startup and is never stored in this repository.

The learning sample does not implement refresh tokens, account lockout, password reset, email verification, revocation, or a complete production identity lifecycle. Use ASP.NET Core Identity or a trusted identity provider for a public production system.

### 8. Relationships

One customer can own many policies; one policy can have many claims. `CustomerId` and `PolicyId` are foreign keys. `DeleteBehavior.Restrict` prevents deleting a customer with policies or a policy with claims; application services return a conflict rather than cascading away insurance records.

Interview focus: explain primary/foreign keys, navigation properties, one-to-many cardinality, and why deletion behavior should reflect business rules.

Common mistake: using cascade delete for regulated business records without understanding its effect.

### 9. Pagination, filtering, sorting, and search

`GET /api/customers` accepts `search`, `pageNumber`, `pageSize`, `sortBy` (`name` or `email`), and `sortDescending`. The repository applies the filter, counts matches, orders deterministically, and applies `Skip`/`Take` in SQL. Page size is capped at 100.

Interview focus: paging bounds database and response work; deterministic ordering avoids records shifting unpredictably between pages.

Common mistake: loading the entire table into memory and filtering it after the query.

### 10. AutoMapper, tests, Clean Architecture, and CQRS

AutoMapper profiles live in Application and restrict mapping depth. Unit tests exercise application business rules with fake repository contracts; run them with `dotnet test InsuranceManagement.sln`. Clean Architecture dependencies are expressed through the project references listed above. CQRS is demonstrated for customer operations: commands change data; queries return data without changing it.

CQRS here is intentionally lightweight—there is no message bus, mediator framework, event store, or separate read database. Those are separate architectural choices, not requirements for a basic CQRS example.

Interview focus: unit tests verify application behavior without SQL Server; Clean Architecture keeps domain/application policy independent from the database and HTTP framework; CQRS separates reads from writes where it improves clarity or scaling.

Common mistake: adding architecture/frameworks without a clear benefit, or making the domain project depend on API, EF Core, or web-specific packages.

## Main API routes

| Method and route | Access | Purpose |
| --- | --- | --- |
| `POST /api/auth/register` | Anonymous | Register a basic customer account. |
| `POST /api/auth/login` | Anonymous | Exchange credentials for a JWT. |
| `GET /api/auth/me` | Any authenticated account | Get the current account's safe profile. |
| `PUT /api/auth/users/{id}/role` | Administrator | Assign an allowed application role. |
| `GET /api/customers` | Claims adjuster or administrator | Search, sort, and page customers. |
| `GET /api/customers/{id}` | Claims adjuster or administrator | Retrieve a customer. |
| `POST /api/customers` | Claims adjuster or administrator | Create a customer. |
| `PUT /api/customers/{id}` | Claims adjuster or administrator | Replace customer details. |
| `DELETE /api/customers/{id}` | Claims adjuster or administrator | Delete a customer without policies. |
| `GET /api/policies` and `GET /api/policies/{id}` | Claims adjuster or administrator | Read policies. |
| `POST /api/policies`, `PUT /api/policies/{id}`, `DELETE /api/policies/{id}` | Claims adjuster or administrator | Manage policies; policies with claims cannot be deleted. |
| `GET /api/claims` and `GET /api/claims/{id}` | Claims adjuster or administrator | Read claims. |
| `POST /api/claims` | Claims adjuster or administrator | Submit a pending claim. |
| `PUT /api/claims/{id}/status` | Claims adjuster or administrator | Approve or reject a pending claim. |

## Validation checklist

- `dotnet build InsuranceManagement.sln` completes with no errors or warnings.
- `dotnet test InsuranceManagement.sln` passes.
- Swagger is available in Development at `/swagger`.
- Unauthenticated protected routes return 401; a basic customer token cannot call adjuster routes (403).
- Create endpoints return 201 and a `Location` header.
- DTO validation, not-found, and conflict responses use the expected 400, 404, and 409 status codes.
- API request and response date/time fields use `DateTimeOffset`.
- The API database remains distinct from the MVC database.
