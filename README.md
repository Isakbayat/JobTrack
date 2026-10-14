# JobTrack

JobTrack is a full-stack web application for tracking job and LIA internship applications.

The project was built to practice and demonstrate full-stack development with ASP.NET Core, Entity Framework Core, Vue.js, automated testing, CI and cloud deployment.

## Live Demo

**Frontend:**  
https://job-track-ruby-ten.vercel.app

**Backend API:**  
https://jobtrack-4e5i.onrender.com/api/Applications

## Features

- Create job applications
- View all applications
- Edit existing applications
- Delete applications
- Track application status
- Search by company or position
- Filter applications by status
- Backend request validation
- Persistent database storage
- Responsive Vue frontend
- Automated backend tests
- Continuous Integration with GitHub Actions
- Cloud deployment

## Application Statuses

Applications can currently be tracked with the following statuses:

- Applied
- Interview
- Offer
- Rejected

## Tech Stack

### Backend

- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- Npgsql
- Swagger / OpenAPI

### Frontend

- Vue 3
- JavaScript
- Vite
- HTML
- CSS

### Database

- SQLite for local development
- PostgreSQL for production
- Neon PostgreSQL

### Testing

- xUnit
- Entity Framework Core InMemory

### DevOps / Deployment

- Git
- GitHub
- GitHub Actions
- Docker
- Vercel
- Render
- Neon

## Architecture

The project is divided into separate frontend, backend and test projects.

```text
JobTrack
│
├── JobTrack.Api
│   ├── Controllers
│   ├── Data
│   ├── Dtos
│   ├── Models
│   └── Migrations
│
├── JobTrack.Api.Tests
│   └── ApplicationsControllerTests.cs
│
├── frontend
│   └── src
│       ├── components
│       │   └── ApplicationCard.vue
│       └── App.vue
│
├── Dockerfile
└── .dockerignore
```

### Production Architecture

```text
User
  │
  ▼
Vercel
Vue Frontend
  │
  │ HTTPS / REST API
  ▼
Render
ASP.NET Core API
  │
  │ Entity Framework Core
  ▼
Neon
PostgreSQL Database
```

For local development, the backend uses SQLite.

In production, the backend uses PostgreSQL hosted by Neon.

The Vue frontend communicates with the ASP.NET Core API through HTTP requests. CORS is configured to allow the deployed Vercel frontend to communicate with the Render API.

## API Endpoints

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/Applications` | Get all applications |
| GET | `/api/Applications/{id}` | Get an application by ID |
| POST | `/api/Applications` | Create a new application |
| PUT | `/api/Applications/{id}` | Update an application |
| DELETE | `/api/Applications/{id}` | Delete an application |

## Validation

Create and update requests are validated by the backend.

The API validates:

- Company
- Position
- Applied date
- Application status

Valid statuses are:

```text
Applied
Interview
Offer
Rejected
```

Invalid requests return an HTTP `400 Bad Request`.

## Running the Project Locally

### 1. Clone the repository

```bash
git clone https://github.com/Isakbayat/JobTrack.git
cd JobTrack
```

### 2. Create the local database

Make sure the Entity Framework Core CLI is installed.

```bash
dotnet ef database update --project JobTrack.Api
```

### 3. Start the backend

```bash
dotnet run --project JobTrack.Api
```

The API runs locally on:

```text
http://localhost:5186
```

Swagger is available in development at:

```text
http://localhost:5186/swagger
```

### 4. Start the frontend

Open another terminal:

```bash
cd frontend
npm install
npm run dev
```

The frontend runs locally on:

```text
http://localhost:5173
```

The Vite development server proxies API requests to the local ASP.NET Core backend.

## Testing

The backend contains automated tests using xUnit.

Current tests cover:

- Creating an application
- Retrieving applications
- Updating an application
- Deleting an application
- Validation when company is missing
- Validation when an invalid status is provided

Run the tests from the repository root:

```bash
dotnet test
```

The tests use an Entity Framework Core in-memory database, so the local SQLite database is not modified.

## Continuous Integration

GitHub Actions automatically runs CI for changes to the project.

The pipeline verifies:

```text
Backend
├── Restore dependencies
├── Build solution
└── Run automated tests

Frontend
├── Install dependencies
└── Build Vue application
```

This helps ensure that the backend tests pass and the frontend builds successfully before changes are merged.

## Deployment

### Frontend

The Vue frontend is deployed to Vercel.

The production frontend uses the `VITE_API_URL` environment variable to connect to the deployed backend.

### Backend

The ASP.NET Core API is deployed to Render using Docker.

The Docker image uses a multi-stage .NET 8 build.

Production configuration is provided through environment variables rather than committed secrets.

### Database

The production database is PostgreSQL hosted by Neon.

The application uses:

- SQLite during local development
- PostgreSQL in production

Database credentials are stored securely as environment variables and are not committed to GitHub.

## What I Practiced

This project gave me practical experience with:

- Building REST APIs with ASP.NET Core
- Implementing CRUD operations
- Working with Entity Framework Core
- Working with SQLite and PostgreSQL
- Backend request validation
- Connecting a Vue frontend to a backend API
- Component-based frontend development
- Search and filtering
- Async programming in C#
- Automated testing with xUnit
- Git feature branches and pull requests
- Continuous Integration with GitHub Actions
- Dockerizing an ASP.NET Core application
- Environment variable configuration
- CORS configuration
- Deploying a full-stack application
- Debugging production issues using deployment logs

## Future Improvements

Possible future improvements include:

- Authentication and user accounts
- Application statistics and dashboard charts
- Pagination and sorting
- Additional automated tests
- Improved error handling
- End-to-end tests

## Author

**Isak Bayat**

System Development student focused on .NET, backend development and IT security.