# DevList

A platform to curate a collection of links to developer resources.

The goal of this project is to demonstrate web applications built with recent ASP.Net Core technologies.

## Project Overview

This sample project prioritizes scalability and maintainability. It follows a modular structure and currently consists of two main projects:-

- `Core` — a .NET Standard Class Library that serves as the core library for the business logic and data access. It contains the data models, data access code, and any shared functionality that can be used by multiple ASP.NET Core web app projects such as:-

  - [ASP.NET Core Blazor][3]
  - [ASP.NET Core Razor Pages][4]
  - [ASP.NET Core MVC][5]
  - [ASP.NET Core Web API (Controller-based)][6]
  - [ASP.NET Core Web API (Minimal API)][7]

  > Note that an ASP .NET Core web app can contain various combinations of all of the above.

- `Web` — The startup project that is serving the web application. It is a [Blazor Server][9] application and is responsible for handling user interations and rendering the UI. It also serves api for frontend client applications using [Minimal API][7] endpoints. It uses [Razor components][8] with [Bootstrap][10] for UI and leverages pre-built UI components from [Blazor Bootstrap][11].

The project uses the mediator pattern, which is implemented using the [MediatR][12] package. The mediator acts as an itermediary between the `Web` and the `Core` library. It handles incoming requests sent from the UI or API in the `Web` project and routes them to the corresponding handlers in the `Core` library. This allows for loose coupling between the different components of the application and promotes a clean and maintainable codebase, allowing for changes in either layer without affecting the other.

## Get Started

### Prerequisites

Before you begin, ensure you have the following software installed:

- .NET SDK
- Git
- SQL Server
- Visual Studio

### Project Setup

Clone the repository and open the solution file in the root directory with Visual Studio.

The application's configuration can be found in `appsettings.json` file within the `Web` project.
The preferred way to overwrites the configuration values is by creating the environment version of `appsettings.json` file, `appsettings.{Environment}.json`.

> `appsettings.{Environment}.json` values override keys in `appsettings.json`.

In development, you may have `appsettings.Development.json` file which look like this:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=devlist;User=root;Password=mysuperstrongpassword"
  }
}
```

### Database Setup

> This project uses Entity Framework Core's Migrations to manage the database schemas. For more information, see the official documentation on
> [Migrations][1].

The application's Migrations files can be found in the `Core` project: `src/Core/Common/Data/Migrations`.

During development, a local database copy can easily be created or updated by applying the migrations using EF Core command via Package Manager Console. First, open Package Manager Console panel in Visual Studio (select _Tools > NuGet Package Manager > **Package Manager Console**_ from the top menu). Next, change the _Default Project_ option to the `Core` project to ensures that commands are executed to the correct project. Then, run the following command:

```powershell
Update-Database
```

Verify that there are no errors upon database creation.

For productions, the [recommended][2] way to apply migrations is by using EF Core command to generate SQL scripts and running them on the production database.

The following generates a SQL script from a blank database to the latest migration:

```powershell
Script-Migration
```

> The generated script should be reviewed for accuracy or in some cases, tuned to fit the production database before executing it; this is important since applying schema changes to production databases is a potentially dangerous operation that could involve data loss.

## **Technologies**

- ASP.NET Core 8
- ASP.NET Core Identity framework
- Entity Framework Core 8
- SQL Server
- Blazor Server
- MediatR
- AutoMapper [TODO]
- FluentValidator [TODO]

[1]: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=vs "Migrations"
[2]: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=vs#sql-scripts "Applying Migrations"
[3]: https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-8.0 "ASP.NET Core Blazor"
[4]: https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0 "ASP.NET Core Razor Pages"
[5]: https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-8.0 "ASP.NET Core MVC"
[6]: https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0 "ASP.NET Core Web API (Controller-based)"
[7]: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/overview?view=aspnetcore-8.0 "ASP.NET Core Web API (Minimal API)"
[8]: https://learn.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-8.0 "ASP.NET Razor Components"
[9]: https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-8.0#blazor-server "Blazor Server"
[10]: https://getbootstrap.com/docs "Bootstrap"
[11]: https://demos.blazorbootstrap.com/ "Blazor Bootstrap"
[12]: https://github.com/jbogard/MediatR "MediatR"
