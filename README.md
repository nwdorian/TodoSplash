# TodoSplash

This full stack application is deisgned to demonstrate .NET minimal APIs and Javascript fetch API.

The frontend is a Typescript SPA bundled with Vite. Backend is a .NET WebAPI that provides endpoints to manage the database records.

The application allows users to manage todo tasks.

## Table of contents

- [TodoSplash](#todosplash)
  - [Table of contents](#table-of-contents)
  - [Running the application](#running-the-application)
  - [Features](#features)
    - [Vertical Slice Architecture](#vertical-slice-architecture)
    - [Single Page Application](#single-page-application)
    - [Standardized ProblemDetails responses](#standardized-problemdetails-responses)
    - [Global exception handler](#global-exception-handler)
    - [Database initialization](#database-initialization)
    - [Database seeding](#database-seeding)
    - [Startup process](#startup-process)
  - [Contributing](#contributing)
  - [License](#license)
  - [Contact](#contact)

## Running the application

1. Clone the repository
   - using **HTTPS**
     - `https://github.com/nwdorian/TodoSplash.git`
   - using **SSH**
     - `git@github.com:nwdorian/TodoSplash.git`
   - using **GitHub CLI**
     - `gh repo clone nwdorian/TodoSplash`

2. Configure `appsettings.json`
   - replace the connection string (optional)
     - details in [Database initialization](#database-initialization) section

3. Run the application
   - navigate to the project root directory `cd ./TodoSplash`
   - run command: `npm start`
     - details in [Startup process](#startup-process) section

## Features

### Vertical Slice Architecture

- The backend is developed with .NET 10 using "minimal APIs" and Vertical Slice Architecture (VSA).
- VSA removes abstractions and layers required by Clean Architecture (CA) architecture.
- Merging CA layers and keeping related components close together shits the focus to the use case and speeds up development process.

### Single Page Application

- Website for consuming the API is developed with Typescript and Vite as bundler.
- All user interaction is done on the same page, improving the user experience.
- Typescript is configured to `strict` mode, disallowing usage of `any` type.

### Standardized ProblemDetails responses

- API responses return a problem details response defined by [RFC 9457](https://www.rfc-editor.org/rfc/rfc9457) standard.
- `Result` and `Error` types along with `FluentValidation` enable mapping problems to a structured problem details response.
- This approach enables API consumers to easily handle error messages by providing a consistent response structure.

### Global exception handler

- Instead of wrapping critical paths into try-catch blocks, a global exception handler is used.
- Implemented with `IExceptionHandler` introduced in .NET 8
- Uses `IProblemDetailsService` to return exception information in a problem details response, aligned with the response structure of application errors.

### Database initialization

- Connection string can be changed in `appsettings.json`
  - `ConnectionStrings` - `TodoSplashDb`
- Default connection string is provided with `TodoSplashDb` database name and Windows Authentication.
- SQL Server local database is created on startup by applying EF Core migrations.
- Database is created if it doesn't already exist.

### Database seeding

- Database is seeded with 4 todo tasks when not running in `Production` environment.
- Seeding will be skipped is records already exist in the database.

### Startup process

- `npm start` script was created for a single command startup.
- The script executes the following actions
  - `npm install` - install npm dependencies
  - `tsc` - Typescript files compilation to Javascript
  - `vite build` - minifies the code and outputs the build artifacts to `wwwroot` folder
  - `dotnet run` - restores, builds and runs the .NET WebAPI
- After the startup process the application can be accessed on `http://localhost:5280`

## Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.

## Contact

For any questions or feedback, please open an issue.
