# CalendarApp

Personal Calendar Management System — a school project built with ASP.NET Core 8.0 MVC.

## Features

- **Multiple Calendars** — create and manage separate calendars (Work, Personal, School)
- **Event Management** — full CRUD with recurring events (daily, weekly, monthly, yearly)
- **Event Categories** — color-coded categories (Meeting, Lecture, Sport, etc.)
- **Calendar Sharing** — share calendars with other users (ReadOnly / ReadWrite)
- **Event Invitations** — invite users to events, accept/decline
- **In-App Notifications** — reminders and activity alerts
- **Admin Dashboard** — full management of all entities
- **Web API** — REST endpoints with JWT auth for future mobile app
- **Profile Image Upload** — user avatars

## Tech Stack

| Component | Technology |
|-----------|-----------|
| Backend | ASP.NET Core 8.0 MVC |
| ORM | Entity Framework Core (Code-First) |
| Database | SQLite |
| Auth | ASP.NET Core Identity + JWT |
| Frontend | Razor Views + Bootstrap 5 |
| Testing | xUnit + Moq |

## Architecture

Multi-layered architecture with strict dependency flow:

```
CalendarApp.Web (Presentation)
    └── CalendarApp.Application (Business Logic, Services, DTOs)
            └── CalendarApp.Domain (Entities, Enums, Interfaces)
    └── CalendarApp.Infrastructure (EF Core, Repositories, Migrations)
            └── CalendarApp.Domain
            └── CalendarApp.Application
```

| Layer | Responsibility |
|-------|---------------|
| **Domain** | Entities, enums, repository interfaces |
| **Application** | Services, DTOs, ViewModels, validation |
| **Infrastructure** | DbContext, repository implementations, migrations, seeding |
| **Web** | MVC & API controllers, views, DI configuration |

## User Roles

| Role | Access |
|------|--------|
| **Admin** | Full system management (Admin Area) |
| **Manager** | Manages calendars, events, user interactions |
| **User** | Own calendars, events, sharing, invitations |

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Run

```bash
git clone https://github.com/godblessamki/CalendarProject.git
cd CalendarProject
dotnet build CalendarApp.sln
dotnet run --project src/CalendarApp.Web
```

### Run Tests

```bash
dotnet test
```

## Project Structure

```
CalendarApp/
├── src/
│   ├── CalendarApp.Domain/           # Entities, Enums, Interfaces
│   ├── CalendarApp.Application/      # Services, DTOs, ViewModels, Validation
│   ├── CalendarApp.Infrastructure/   # EF Core, Repositories, Migrations
│   └── CalendarApp.Web/             # Controllers, Views, API, Admin Area
└── tests/
    └── CalendarApp.Tests/           # Unit tests
```

## Documentation

See [plan.md](plan.md) for the full architecture plan, entity definitions, ER diagram, and implementation milestones.
