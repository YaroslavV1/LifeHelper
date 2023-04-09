# LifeHelper

This project was created to help the user in everyday life by adding daily notes and subnotes, as well as controlling money spending by maintaining categories.

## What's inside

### Web Api  (Backend)
The Web Api project includes all the required endpoints to access the backend functionality, which can be utilized to build a custom frontend application.

## Installation

### What must be installed
- .NET 7
- Microsoft SQL Server
  - Created database

### How to install and run
1) Download/Clone the solution from repository
    - If download make sure to have the solution directory unarchived
2) Open solution directory in terminal
3) Run command:  
   `dotnet user-secrets set "ConnectionStrings:LifeHelperDatabase" "<your_connection_string>" -p <startup_project>`  
   where:
    - `<your_connection_string>` - Connection String to your Microsoft SQL Server database
    - `<startup_project>` - Naming of the startup project you want to run. Watch possible values below
4) Be sure your database is run
5) Run command:  
   `dotnet run --project <startup_project>`  
   where `<startup_project>` - Naming of the startup project you want to run. Watch possible values below

> `<startup_project>` possible values:
> - `LifeHelper.Api` - Web Api (backend)

## What technologies were used

- .NET 7 + ASP.NET Core
- Microsoft SQL Server
- Entity Framework Core
- Fluent Validator
- AutoMapper  
