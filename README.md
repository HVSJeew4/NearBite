# NearBite

A trusted, always-updated local food discovery platform.
Personal upskilling project — pro-code full-stack build.

## Project

- **PRD:** see `NearBite_PRD_v1.1.docx`
- **Plan:** see `NearBite_Sprint_Plan.docx`
- **Current sprint:** Sprint 1 — Walking Skeleton (backend only, in-memory data)

## Tech stack

- **Backend:** ASP.NET Core Web API (C#), .NET 9
- **Frontend:** Next.js + React (TypeScript) — coming in Sprint 7
- **Database:** PostgreSQL via EF Core — coming in Sprint 4
- **Deployment:** Azure App Service (API) + Vercel (frontend) — coming in Sprint 7

## Repository layout
nearbite/
├── backend/               # ASP.NET Core Web API
│   ├── NearBite.sln
│   └── src/NearBite.Api/
└── frontend/              # Next.js — coming in Sprint 7

## Running the backend

Prerequisites: .NET 9 SDK, Visual Studio 2022 (or VS Code + C# Dev Kit).

```bash
cd backend/src/NearBite.Api
dotnet run
```

Then open the Swagger UI at the HTTPS URL shown in the console
(usually `https://localhost:7289/swagger`).

## Current endpoints

- `GET /api/listings` — returns all sample listings
- `GET /api/listings/{id}` — returns one listing, or 404