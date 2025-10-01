# NoviExchange

NoviExchange is a currency exchange and wallet management application built with **C# .NET 8** and **SQL Server**, following **Domain-Driven Design (DDD)** principles. It uses **Entity Framework Core** with **Repository** and **Unit of Work patterns**, **Cache Decorator** for optimized data access, and **Quartz.NET** for scheduled tasks. Configuration is handled via the **Options Pattern**, while **Dependency Injection**, **Factory**, and **Decorator patterns** ensure flexibility and maintainability. The project also includes **xUnit** tests to ensure the reliability of its currency conversion and wallet balance adjustment strategies.

---

## Table of Contents

- [Features](#features)
- [Project Structure](#project-structure)
- [Approach & Architecture](#approach--architecture)
- [Requirements](#requirements)
- [Setup](#setup)
- [Running the Application](#running-the-application)
- [Testing](#testing)
- [Limitations & Future Enhancements](#limitations--future-enhancements)

---

## Features

- **Currency Exchange**: Convert between multiple currencies with up-to-date rates.
- **Wallet Management**: Maintain multiple wallets with automatic balance adjustments.
- **Domain-Driven Design (DDD)**: Clear separation of domain logic and application layers.
- **Entity Framework Core**: Efficient data access with **Repository** and **Unit of Work patterns**.
- **Caching**: Optimized data retrieval using the **Cache Decorator pattern**.
- **Scheduled Tasks**: Automatic updates of currency rates via **Quartz.NET**.
- **Configuration Management**: Use of the **Options Pattern** to load settings from `appsettings.json`.
- **Dependency Injection**: Flexible and maintainable service management.
- **Design Patterns**: Implementation of **Factory** and **Decorator patterns** for extensibility.
- **Testing**: **xUnit** tests for validating domain logic and application behavior.
- **Swagger**: integration for interactive API documentation and testing

---

## Project Structure
```csharp
├── NoviExchange
│   ├── NoviExchange.Api //ASP.NET Web API project (controllers)
│   ├── NoviExchange.Application //Application layer (services, interfaces)
│   ├── NoviExchange.Domain //Domain models & logic
│   ├── NoviExchange.Infrastructure //Repositories, DbContext, EF Migrations
│   ├── NoviExchange.EcbClient //handles third party calls to the ECB provider
│   └── NoviExchange.Tests //Unit tests
└── README.md
```

## Approach & Architecture

NoviExchange is designed following **Domain-Driven Design (DDD)** principles to ensure a clear separation between the **domain**, **application**, and **infrastructure** layers. This approach helps keep the core business logic isolated and maintainable, while infrastructure concerns like data access, caching, and scheduling are handled separately.

### Architectural Patterns

- **Repository Pattern**: Provides an abstraction over data access using **Entity Framework Core**, allowing the domain layer to interact with data without knowing the underlying database details.
- **Cache Decorator Pattern**: Optimizes performance by caching frequently accessed data (e.g., currency rates) using **Redis**, without polluting repository logic.
- **Factory Pattern**: Encapsulates the creation of complex objects, ensuring that dependencies and configurations are applied consistently.
- **Decorator Pattern**: Extends functionality of services or repositories without modifying their core behavior.
- **Dependency Injection (DI)**: Promotes loose coupling by allowing services and repositories to be injected where needed, improving testability and maintainability.

### Approaches

- **External Dependencies**: The `EcbClient` is in its own project, making it replaceable and allowing for future reuse or swapping of the data source without impacting the rest of the system.
- **Retry Logic**: Exponential backoff is used when calling external services to improve resilience.
- **Scheduled Tasks**: Quartz.NET handles periodic tasks such as fetching updated currency rates.
- **Code First Migrations**: Migrations were used in order to keep track of database changes and provide the ability to easily apply the database architecture for new databases.
- **Currency Conversion Logic**: All conversions use **Euro as the base currency**. To convert an amount from `FromCurrency` to `ToCurrency`, the following formula is applied: `AmountInToCurrency = AmountInFromCurrency * (RateOfToCurrencyToEuro / RateOfFromCurrencyToEuro)`

This architecture ensures that NoviExchange remains **maintainable, scalable, and robust**, while clearly separating business logic from technical infrastructure.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Redis](https://redis.io/download)

## Setup
1. Clone the repository:

```bash
git clone https://github.com/yourusername/NoviExchange.git
cd NoviExchange
```

2. Configure the connection string in NoviExchange.Api/appsettings.json:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=NoviExchangeDb;Trusted_Connection=True;"
}
```

3. Apply database migrations:

```bash
cd NoviExchange.Infrastructure
dotnet ef database update --startup-project ../NoviExchange.Api
```

4. Configure and install Redis
Download and install Redis and update appsettings.json value:
```json
"ConnectionStrings": {
  "Redis": "localhost:6379"
}
```

## Running the Application
run the following command: (add /swagger to the port opened to view endpoints in swagger)
```C#
cd NoviExchange.Api
dotnet run
```

## Testing
run the following command:
```C#
cd NoviExchange.Tests
dotnet test
```
## Limitations & Future Enhancements

- Transaction history is not persisted. With more time, a transaction table could provide an audit trail to verify all wallet operations.
- Automapper was downgraded due to licensing constraints, limiting some mapping features.
- Current caching and concurrency handling is basic; distributed multi-instance scenarios may require additional setup.
- Add database indexes for faster queries on frequently accessed columns like FromCurrency, ToCurrency, and Date.
