# Task Manager API

REST API built with ASP.NET Core 8, Entity Framework Core and PostgreSQL.

**Live demo:** https://<your-app>.onrender.com/swagger (free tier, first load may take ~40s)

## Features
- JWT authentication (register / login) with BCrypt password hashing
- Full CRUD for tasks, scoped per user
- Filtering by completion status
- Automatic EF Core migrations on startup
- Swagger UI with Bearer auth support
- Dockerized, deployed on Render with Neon PostgreSQL

## Endpoints
- POST /api/auth/register
- POST /api/auth/login
- GET /api/tasks?completed=true
- GET /api/tasks/{id}
- POST /api/tasks
- PUT /api/tasks/{id}
- DELETE /api/tasks/{id}

## Run locally
1. Start PostgreSQL (or `docker run -e POSTGRES_PASSWORD=postgres -p 5432:5432 postgres`)
2. `dotnet run`
3. Open http://localhost:5000/swagger

## Tech
ASP.NET Core 8, EF Core 8, PostgreSQL, JWT, BCrypt, Docker, Swagger