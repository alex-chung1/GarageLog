# GarageLog

GarageLog is a full-stack vehicle maintenance tracker designed to help users manage their vehicles and keep a detailed history of maintenance and service records.

## Overview

GarageLog provides a centralized place to track vehicle ownership and maintenance history. Users can add vehicles, record mileage, document maintenance and repairs, track service costs, and distinguish between DIY work and professional services.

The project was built as a full-stack application to explore modern application architecture, authentication, database design, API development, and containerized development workflows.

## Tech Stack

### Frontend

- React
- TypeScript
- Vite
- React Router v8
- Tailwind CSS

### Backend

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- ASP.NET Core Identity
- JWT Authentication
- Swagger

### Database & Infrastructure

- PostgreSQL 16
- Docker
- Docker Compose

## Architecture

GarageLog's backend follows a layered Clean Architecture approach, separating the domain, application logic, infrastructure, and API layers.

```text
GarageLog
├── GarageLog.API
├── GarageLog.Application
├── GarageLog.Core
└── GarageLog.Infrastructure
```

### API

Handles HTTP requests, controllers, middleware, authentication, and API configuration.

### Application

Contains application logic, DTOs, interfaces, and abstractions used by the API and infrastructure layers.

### Core

Contains the domain entities and core business rules of the application.

### Infrastructure

Handles data persistence, Entity Framework Core, PostgreSQL, repositories, and other infrastructure concerns.

## Features

### Authentication

- User registration
- User login and logout
- ASP.NET Core Identity
- JWT-based authentication
- HttpOnly authentication cookies

### Vehicle Management

- Add vehicles
- Edit vehicle information
- View vehicle details
- Track vehicle mileage
- Store VIN and vehicle information

### Maintenance Tracking

- Create maintenance and service records
- Record service date and mileage
- Track DIY and repair-shop services
- Record service provider information
- Track service costs
- Add maintenance notes
- Categorize services by service type
- View maintenance history for individual vehicles

## Getting Started

### 1. Prerequisites

Make sure the following are installed before running GarageLog:

- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [Node.js](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### 2. Clone the Repository

Clone the repository and navigate to the project directory:

```bash
git clone https://github.com/alex-chung1/GarageLog.git
cd GarageLog
```

### 3. Configure Environment Variables

GarageLog includes `.env.example` files with the required development configuration.

Create the root environment file used by Docker Compose:

```bash
cp .env.example .env
```

Create the frontend environment file:

```bash
cp client/.env.example client/.env
```

The root `.env` is used by the Docker Compose services, while `client/.env` contains configuration for the React frontend.

> Do not commit `.env` files or other files containing secrets to the repository.

### 4. Start Docker Compose

From the project root, build and start the Docker Compose services:

```bash
docker compose up --build
```

This starts:

- **GarageLog API** — ASP.NET Core API on `http://localhost:5000`
- **PostgreSQL** — PostgreSQL database on `localhost:5432`

When running in the Development environment, the API automatically:

- Applies pending Entity Framework Core migrations
- Seeds the application's initial service types

No manual database migration or setup commands are required.

Leave this terminal running while using GarageLog.

### 5. Start the Frontend

Open a new terminal and navigate to the frontend directory:

```bash
cd client
npm install
npm run dev
```

The frontend will be available at:

```text
http://localhost:3000
```

### 6. Open GarageLog

Once the API and frontend are running, open:

```text
http://localhost:3000
```

You can now register an account and begin adding vehicles and maintenance records.
