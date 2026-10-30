# MobileStore

MobileStore is a .NET 9 solution containing an ASP.NET Core Web API, application and domain layers, infrastructure, and a portfolio project.

## Requirements

- .NET 9 SDK
- SQL Server

## Configure the API

The API reads its database connection string and JWT signing key from .NET user secrets during development. These values are intentionally not stored in the repository.

From the solution directory, set them with your own local values:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your SQL Server connection string>" --project .\MobileStore.API
dotnet user-secrets set "JwtSettings:SecretKey" "<a unique random key of at least 32 characters>" --project .\MobileStore.API
```

Run the API:

```powershell
dotnet run --project .\MobileStore.API
```

The API's Swagger UI is enabled in the Development environment.

## Projects

- `MobileStore.API` - ASP.NET Core API and authentication
- `MobileStore.Application` - application services and mappings
- `MobileStore.Domain` - domain models
- `MobileStore.Infrastructure` - persistence and repositories
- `Portfolio` - portfolio web application
