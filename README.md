# Task Manager API

A secure REST API built with **ASP.NET Core 9**, Entity Framework Core and PostgreSQL. Users can register, log in with JWT authentication, and manage their own tasks.

**Live demo (Swagger UI):** https://taskmanager-api-jcm7.onrender.com/swagger/index.html

> Hosted on Render's free tier, so the first request after idle time can take 30-50 seconds to wake up.

## Features

- JWT authentication (register / login) with BCrypt password hashing
- Full CRUD for tasks, scoped per user (users can only see their own tasks)
- Filter tasks by completion status
- Input validation with data annotations
- Automatic EF Core migrations on startup
- Swagger UI with Bearer token support
- Dockerized and deployed on Render, with Neon PostgreSQL

## Tech Stack

ASP.NET Core 9, Entity Framework Core, PostgreSQL (Npgsql), JWT Bearer, BCrypt, Swagger, Docker

## API Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | /api/auth/register | No | Create an account, returns a JWT |
| POST | /api/auth/login | No | Log in, returns a JWT |
| GET | /api/tasks | Yes | List your tasks (`?completed=true` to filter) |
| GET | /api/tasks/{id} | Yes | Get a single task |
| POST | /api/tasks | Yes | Create a task |
| PUT | /api/tasks/{id} | Yes | Update a task |
| DELETE | /api/tasks/{id} | Yes | Delete a task |

## Try it in the live demo

1. Open the Swagger link above.
2. Call `POST /api/auth/register` with an email and password, and copy the returned token.
3. Click **Authorize**, paste the token (without the word "Bearer"), and confirm.
4. Now try the `/api/tasks` endpoints.

## Run locally

**Requirements:** .NET 9 SDK and a PostgreSQL database (local, or a free one from [Neon](https://neon.tech)).

1. Clone the repo:
   ```bash
   git clone https://github.com/tanishqarya007/TaskManager.API.git
   cd TaskManager.API
   ```
2. Create `appsettings.Development.json` with your own connection string and JWT key:
   ```json
   {
     "ConnectionStrings": {
       "Default": "Host=localhost;Port=5432;Database=taskmanager;Username=postgres;Password=your_password"
     },
     "Jwt": {
       "Key": "your-long-random-secret-at-least-32-characters",
       "Issuer": "TaskManager.Api"
     }
   }
   ```
3. Run the app (tables are created automatically on startup):
   ```bash
   dotnet run
   ```
4. Open the `/swagger` page on the URL shown in the terminal.

## Run with Docker

```bash
docker build -t taskmanager-api .
docker run -p 8080:8080 \
  -e "ConnectionStrings__Default=<your-connection-string>" \
  -e "Jwt__Key=<your-secret>" \
  -e "Jwt__Issuer=TaskManager.Api" \
  taskmanager-api
```

## Project Structure

```
Controllers/   AuthController, TasksController
Data/          AppDbContext (EF Core)
Models/        User, TaskItem
Dtos/          Request / response models
Services/      TokenService (JWT creation)
Migrations/    EF Core migrations
```

## Author

Tanishq Arya - .NET Backend Developer