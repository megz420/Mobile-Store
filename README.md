<div align="center">

# MobileStore

### A layered .NET solution for a mobile-store management API

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4?logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core)
[![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)

</div>

MobileStore is a **.NET 9** solution for managing an online mobile-store catalog and its related operations. Its ASP.NET Core Web API is organized into separate API, application, domain, and infrastructure projects. The solution also includes a standalone ASP.NET Core Razor Pages portfolio website.

## Features

- REST API controllers for users, products, brands, categories, product images, orders, order items, and reviews.
- User registration and login endpoints with JWT bearer authentication.
- Role-restricted user-management operations.
- Entity Framework Core with SQL Server and a committed initial database migration.
- Application services, DTO mappings, and request validation.
- Repository and dependency-injection patterns to separate API, application, domain, and persistence concerns.
- Swagger/OpenAPI documentation in the API's Development environment.
- A separate Razor Pages portfolio website in the same solution.

## Technology stack

| Area | Technologies |
| --- | --- |
| Runtime and language | .NET 9, C# |
| API | ASP.NET Core Web API, Swagger/Swashbuckle |
| Data access | Entity Framework Core 9, SQL Server |
| Authentication | JWT bearer tokens, BCrypt password hashing |
| Application layer | AutoMapper, FluentValidation |
| Portfolio site | ASP.NET Core Razor Pages |

## Solution structure

```text
MobileStore.sln
├── MobileStore.API/             HTTP API, controllers, JWT authentication
├── MobileStore.Application/     Services, DTOs, mappings, validation
├── MobileStore.Domain/          Domain entities
├── MobileStore.Infrastructure/  EF Core, SQL Server, migrations, repositories
└── Portfolio/                   Standalone Razor Pages website
```

## Getting started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- A running SQL Server instance and a database account with permission to create or update the application database
- .NET Entity Framework Core CLI (`dotnet-ef`) to apply migrations

### Clone and restore

```bash
git clone https://github.com/megz420/Mobile-Store.git
cd Mobile-Store
dotnet restore MobileStore.sln
```

### Configure local secrets

The API requires a SQL Server connection string and a JWT signing key. Keep these values out of source control. For local development, store them with .NET User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your SQL Server connection string>" --project .\MobileStore.API
dotnet user-secrets set "JwtSettings:SecretKey" "<a unique random signing key of at least 32 characters>" --project .\MobileStore.API
```

Replace each placeholder with your own value before running the commands. The signing key should be randomly generated and kept private. For deployment, provide the same settings through your hosting platform's secret or environment-variable configuration rather than committing them to `appsettings.json`.

### Create or update the database

Use the EF Core CLI major version that matches this solution's EF Core 9 packages. Install it if needed:

```bash
dotnet tool install --global dotnet-ef --version 9.0.0
```

If `dotnet-ef` is already installed with a different version, update it instead:

```bash
dotnet tool update --global dotnet-ef --version 9.0.0
```

Apply the existing migration to your configured SQL Server database:

```bash
dotnet ef database update --project ./MobileStore.Infrastructure/MobileStore.Infrastructure.csproj --startup-project ./MobileStore.API/MobileStore.API.csproj
```

### Run the API

```bash
dotnet run --project ./MobileStore.API/MobileStore.API.csproj
```

With the included HTTPS Development profile, the API listens at `https://localhost:7078` and `http://localhost:5207`. Open [https://localhost:7078/swagger](https://localhost:7078/swagger) for the Swagger UI. Swagger is enabled only in the Development environment.

### Run the portfolio website

In a separate terminal:

```bash
dotnet run --project ./Portfolio/Portfolio.csproj
```

The included HTTPS Development profile listens at `https://localhost:7241` and `http://localhost:5227`.

## API overview

Controllers use the `api/[controller]` route convention. The main resources are:

| Resource | Route |
| --- | --- |
| Users | `/api/User` |
| Products | `/api/Product` |
| Brands | `/api/Brand` |
| Categories | `/api/Category` |
| Product images | `/api/ProductImage` |
| Orders | `/api/Order` |
| Order items | `/api/OrderItem` |
| Reviews | `/api/Review` |

User authentication endpoints:

- `POST /api/User/register`
- `POST /api/User/login`

Use the Swagger UI to inspect the available operations and request/response schemas. Authenticated endpoints accept JWT bearer tokens through the `Authorization: Bearer <token>` header.

## Configuration reference

| Setting | Purpose |
| --- | --- |
| `ConnectionStrings:DefaultConnection` | SQL Server connection used by Entity Framework Core |
| `JwtSettings:SecretKey` | Private signing key used to issue and validate JWTs |
| `JwtSettings:Issuer` | Expected JWT issuer |
| `JwtSettings:Audience` | Expected JWT audience |
| `JwtSettings:ExpiryMinutes` | JWT lifetime in minutes |

Issuer, audience, and token lifetime have non-secret development defaults in `MobileStore.API/appsettings.json`. Override settings as needed for your environment. Do not commit real credentials or signing keys.

## Build

```bash
dotnet build MobileStore.sln
```

## License

No license has been specified for this repository. Unless a license is added, all rights remain reserved by the copyright holder.
