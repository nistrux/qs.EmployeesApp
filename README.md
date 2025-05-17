# qs.EmployeesApp

This project is a test task application demonstrating a full-stack application built with ASP.NET Core (.NET 8) for the backend and Svelte with Vite for the frontend, orchestrated using Podman Compose (or Docker Compose).

## 🧰 Technology Stack

**Backend:**

* **Framework:** ASP.NET Core (.NET 8)
* **Architecture:** CQRS (Command Query Responsibility Segregation) using MediatR
* **Validation:** FluentValidation
* **ORM:** Entity Framework Core 9
* **Database Provider:** Npgsql.EntityFrameworkCore.PostgreSQL for PostgreSQL
* **Data Seeding (Development):** Bogus
* **API Documentation:** Swagger/OpenAPI

**Frontend:**

* **Framework:** Svelte
* **Build Tool:** Vite
* **Language:** TypeScript
* **UI Components:** Bootstrap
* **Icons:** Font Awesome

**Containerization & Orchestration:**

* Podman Compose (compatible with Docker Compose)
* Docker Multi-stage Builds

**Database:**

* PostgreSQL 17

## ✅ Prerequisites

To build and run this project using Podman Compose, you need the following installed:

* Git
* [Podman](https://podman.io/get-started) or [Docker](https://www.docker.com/)
* [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) (Needed for building the .NET app in Docker, and for running `dotnet ef` if you run backend manually or generate migrations).
* [Node.js and npm](https://nodejs.org/) (Needed for building the frontend in Docker, and for running frontend manually).

## 🚀 Running with Podman Compose (Recommended)

This is the easiest way to get the entire application stack (Backend + Frontend + Database) up and running with a single command.

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/nistrux/qs.EmployeesApp.git
    cd qs.EmployeesApp
    ```
    *(Make sure your `docker-compose.yml` and the `backend` and `frontend` folders are in the root of the cloned directory)*

2.  **Configure Database Connection String:**
    The database connection string for the backend and database credentials for the PostgreSQL container are configured using environment variables directly in the `docker-compose.yml` file.
    * Open the `docker-compose.yml` file in the root directory.
    * Locate the `db` service and configure the `POSTGRES_DB`, `POSTGRES_USER`, and `POSTGRES_PASSWORD` environment variables. **Replace the placeholder values with your desired credentials.**
    * Locate the `backend` service and configure the `ConnectionStrings__EmpConnection` environment variable. This connection string should point to the `db` service using the service name (`db`) and the internal PostgreSQL port (`5432`). **Ensure the database name, username, and password in this connection string exactly match** the values you set for the `db` service.

    ```yaml
    services:
      db:
        # ...
        environment:
          POSTGRES_DB: your_db_name # <-- Configure
          POSTGRES_USER: your_db_user # <-- Configure
          POSTGRES_PASSWORD: your_db_password # <-- Configure
        # ...
      backend:
        # ...
        environment:
          # ...
          # Ensure this matches the db service credentials and points to 'db' service name
          ConnectionStrings__EmpConnection: "Host=db;Port=5432;Database=your_db_name;Username=your_db_user;Password:your_db_password;" # <-- Configure
          # ...
    ```

3.  **Build and Run:**
    Execute the following command from the root directory of the project (where `docker-compose.yml` is located):

    ```bash
    podman-compose up -d --build
    # Or if you use Docker Compose:
    # docker-compose up -d --build
    ```
    This command will:
    * Build the frontend application within a temporary Node.js Docker stage.
    * Build the backend application within a .NET SDK Docker stage, copying the built frontend into the backend's publish directory.
    * Create and start the PostgreSQL container, using a named volume (`emp_db_data`) for persistent storage.
    * Wait for the PostgreSQL container to pass its health checks (ensuring the DB is ready).
    * Create and start the backend application container.
    * Upon backend startup, automatically apply any pending Entity Framework Core migrations to the database.
    * Map the backend's exposed port (default 8888 internally) to port 8888 on your host machine.
    * Map the database's internal port 5432 to port 5433 on your host machine (allowing external DB clients like [pgAdmin](https://www.pgadmin.org/) or [DBeaver](https://dbeaver.io/) to connect via `localhost:5433`).

4.  **Access the Application:**
    Once the containers are running, the application should be available in your web browser at:
    ```
    http://localhost:8888/
    ```
    The Swagger UI for testing the backend API is usually available at:
    ```
    http://localhost:8888/swagger/
    ```

5.  **Stop the Application:**
    To stop the running containers, execute the following command in the same directory:
    ```bash
    podman-compose down
    # Or if you use Docker Compose:
    # docker-compose down
    ```
    This will stop and remove the containers, but the `emp_db_data` volume (containing your database data) will be preserved. To remove the volume as well (for a clean start), use `podman-compose down --volumes --remove-orphans`.

## 🖥️ Running the Backend Only (Manual/Development)

This is useful if you want to run the backend application directly on your host machine for debugging or development purposes, connecting to a locally running PostgreSQL server or another accessible DB instance.

1.  **Ensure a PostgreSQL server is running and accessible** from your host machine. Note its host, port, database name, username, and password.
2.  **Navigate to the Backend Web project directory:**
    ```bash
    cd ./backend/qs.EmployeesApp.Server/qs.EmployeesApp.Web/
    ```
3.  **Configure the Database Connection String using User Secrets:**
    To avoid storing your database credentials directly in `appsettings*.json` files in your source code, use the User Secrets feature. This stores secrets in a file outside your project folder, specific to your user account.
    * Initialize User Secrets for the project (if not already done):
        ```bash
        dotnet user-secrets init
        ```
    * Set the `EmpConnection` secret with your local database details:
        ```bash
        dotnet user-secrets set "ConnectionStrings:EmpConnection" "Host=YOUR_DB_HOST;Port=YOUR_DB_PORT;Database=YOUR_DB_NAME;Username=YOUR_DB_USER;Password=YOUR_DB_PASSWORD;"
        ```
        Replace `YOUR_DB_HOST`, `YOUR_DB_PORT`, etc., with the actual connection details for your local PostgreSQL server.
4.  **Run the Backend Application:**
    ```bash
    dotnet run
    ```
    The application will start and listen on the ports configured in its `Properties/launchSettings.json` or `appsettings.json`.

## 🌐 Running the Frontend Only (Manual/Development)

This is useful for frontend development with hot-reloading, connecting to a separately running backend API.

1.  **Ensure the Backend application is running** (via `dotnet run` manually). The frontend will expect the API to be available at `/api/*`.
2.  **Navigate to the Frontend project directory:**
    ```bash
    cd ./frontend/
    ```
3.  **Install dependencies:**
    ```bash
    npm install # or yarn install or pnpm install
    ```
4.  **Run the development server:**
    ```bash
    npm run dev # or yarn dev or pnpm dev
    ```
    The Vite development server will start, typically at `http://localhost:5173/` (check the console output). It is configured with a proxy (`/api`) in `vite.config.ts` to forward API calls to the backend address specified there (`http://localhost:5194` in your original config). Ensure this proxy URL matches where your backend is running.

## 📊 SQL Queries for Test Task

This project includes a collection of example SQL queries relevant to the test task requirements or demonstrating database interactions with the PostgreSQL database used by the application.

You can find these queries in the file located at:

[sql_scripts/queries.sql](https://github.com/nistrux/qs.EmployeesApp/blob/main/sql_scripts/queries.sql) (direct link)
