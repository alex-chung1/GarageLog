# GarageLog

A vehicle maintenance tracker. Log service history for your cars, trucks, and SUVs — oil changes, tire rotations, brake jobs, whatever you actually did and when — instead of losing it all in a glovebox folder or a spreadsheet you forget to update.

Built as a full-stack portfolio project to get hands-on with modern .NET and a proper React Router SSR setup.

## Tech Stack

**Backend**

- .NET 10 / ASP.NET Core Web API
- EF Core + PostgreSQL
- ASP.NET Core Identity + JWT (HttpOnly cookies)
- Clean Architecture (Core / Application / Infrastructure / API)

**Frontend**

- React Router v8 (framework mode, SSR)
- TypeScript
- Tailwind CSS

## Features

**Working now**

- Register / login / logout (JWT in HttpOnly cookies)
- Add, edit, and delete vehicles
- Log service records — date, mileage, cost, shop or DIY, notes
- Attach multiple service types to a record, including custom/freeform entries
- Full service history view per vehicle

**Not built yet**

- Automated tests (backend)
- Docker / containerized local dev
- Deployment / hosting
- Receipt or photo attachments on service records
