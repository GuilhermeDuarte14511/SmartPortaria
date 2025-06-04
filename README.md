# SmartPortaria

SmartPortaria is an ASP.NET Core MVC application designed to manage access control. It provides administrator registration and login, user management with facial recognition support and session-based authentication.

## Prerequisites

- .NET SDK 10.0 or later
- A SQL Server instance

## Configuration

1. Clone this repository and restore dependencies.
2. Set the database connection string in `appsettings.json` under `ConnectionStrings:DefaultConnection` or provide it via the environment variable `ConnectionStrings__DefaultConnection`.
3. Optionally set `ASPNETCORE_ENVIRONMENT` to `Development` or `Production`.

## Running the Application

Execute the following commands in the project directory:

```bash
# build the solution
 dotnet build SmartPortaria.sln

# run the web application
 dotnet run --project SmartPortaria.csproj
```

The application will start and be accessible at `https://localhost:5001` by default (port may vary).

## Major Features

- Administrator registration and login
- Cookie based authentication and session management
- User registration with facial data
- Facial recognition endpoint for identifying registered users

## Environment Variables

- `ConnectionStrings__DefaultConnection` – overrides the database connection string
- `ASPNETCORE_ENVIRONMENT` – sets the runtime environment (`Development`, `Staging`, `Production`)

