# DevList

A platform to curate a collection of links to developer resources.

The goal of this project is to demonstrate web applications built with recent ASP.Net Core technologies.

## Prerequisites

Before you begin, ensure you have the following software installed:

- .NET SDK
- MySQL
- Visual Studio

## Setup

Clone the repository and open the solution file in the root directory with Visual Studio.

The application consists of two separate project:

- `Core` - The core library project
- `Web` - The web application project

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
> [Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=vs).

The application's Migrations files can be found in the `Core` project: `src/Core/Providers/Data/Migrations`.

During development, a local database copy can easily be created or updated by applying the migrations using EF Core command via Package Manager Console. First, open Package Manager Console panel in Visual Studio (select _Tools > NuGet Package Manager > **Package Manager Console**_ from the top menu). Next, change the _Default Project_ option to the `Core` project to ensures that commands are executed to the correct project. Then, run the following command:

```powershell
Update-Database
```

Verify that there are no errors upon database creation.

For productions, the [recommended](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=vs#sql-scripts) way to apply migrations is by using EF Core command to generate SQL scripts and running them on the production database.

The following generates a SQL script from a blank database to the latest migration:

```powershell
Script-Migration
```

> The generated script should be reviewed for accuracy or in some cases, tuned to fit the production database before executing it; this is important since applying schema changes to production databases is a potentially dangerous operation that could involve data loss.

## **Technologies**

- ASP.NET Core 8
- Entity Framework Core 8
- ASP.NET Core Identity framework [TODO]
- Blazor Server
- AutoMapper [TODO]
- FluentValidator [TODO]
- MediatR
- SQL Server [TODO]
- MySQL
