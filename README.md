# JobTrack

JobTrack is a full-stack application for tracking job and LIA internship applications.

The project was built to practice and demonstrate full-stack development with ASP.NET Core, Entity Framework Core, Vue.js and automated backend testing.

## Features

- Create job applications
- View all applications
- Edit existing applications
- Delete applications
- Track application status
- Search by company or position
- Filter applications by status
- Persistent data storage with SQLite
- Responsive Vue frontend
- Automated backend tests

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
- SQLite
- Swagger / OpenAPI

### Frontend

- Vue 3
- JavaScript
- Vite
- HTML
- CSS

### Testing

- xUnit
- Entity Framework Core InMemory

### Development

- Git
- GitHub
- Visual Studio 2022

## Architecture

The project is divided into separate frontend, backend and test projects.

```text
JobTrack
│
├── JobTrack.Api
│   ├── Controllers
│   ├── Data
│   ├── Models
│   └── Migrations
│
├── JobTrack.Api.Tests
│   └── ApplicationsControllerTests.cs
│
└── frontend
    └── src
        ├── components
        │   └── ApplicationCard.vue
        └── App.vue
```

The Vue frontend communicates with the ASP.NET Core API through HTTP requests.

The API uses Entity Framework Core to store application data in a local SQLite database.

## API Endpoints

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/Applications` | Get all applications |
| GET | `/api/Applications/{id}` | Get an application by ID |
| POST | `/api/Applications` | Create a new application |
| PUT | `/api/Applications/{id}` | Update an application |
| DELETE | `/api/Applications/{id}` | Delete an application |

## Running the Project

### 1. Clone the repository

```bash
git clone https://github.com/Isakbayat/JobTrack.git
cd JobTrack
```

### 2. Create the database

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

Swagger is available at:

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

## Testing

The backend contains automated tests for the application controller.

Current tests cover:

- Creating an application
- Retrieving applications
- Updating an application
- Deleting an application

Run the tests from the repository root:

```bash
dotnet test
```

The tests use an in-memory database so the local SQLite database is not modified.

## What I Practiced

This project gave me practical experience with:

- Building REST APIs with ASP.NET Core
- Implementing CRUD operations
- Working with Entity Framework Core
- Database migrations and SQLite
- Connecting a Vue frontend to a backend API
- Component-based frontend development
- Search and filtering
- Async programming in C#
- Automated testing with xUnit
- Git feature branches and pull requests

## Future Improvements

Possible future improvements include:

- Authentication and user accounts
- Server-side validation
- Additional application statistics
- More automated tests
- Deployment to a cloud platform

## Author

**Isak Bayat**

System Development student focused on .NET, backend development and IT security.