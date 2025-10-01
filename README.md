# NoviExchange

NoviExchange is a currency exchange and wallet management application built with **C# .NET 8** and **SQL Server**, leveraging **Entity Framework Core** for data access, the **Options Pattern** to read configuration from `appsettings.json`, and **Quartz.NET** for scheduling periodic tasks. The project also includes **xUnit** tests to ensure the reliability of its currency conversion and wallet balance adjustment strategies.

---

## Table of Contents

- [Features](#features)
- [Project Structure](#project-structure)
- [Requirements](#requirements)
- [Setup](#setup)
- [Running the Application](#running-the-application)
- [Testing](#testing)

---

## Features

- Manage multiple wallets with adjustable balances using different strategies  
- Perform currency conversions with real-time rates  
- Data access and migrations handled via **Entity Framework Core**  
- Configuration loaded through the **Options Pattern** from `appsettings.json`  
- Periodic tasks, such as updating currency rates, scheduled using **Quartz.NET**  
- Comprehensive **xUnit** tests for services and business logic  
- Factory pattern implementation for flexible strategy management  
- **Swagger** integration for interactive API documentation and testing

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
## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)

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

## Running the Application
run the following command:
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
