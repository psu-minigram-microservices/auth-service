# Minigram Auth Service

This service handles user authentication for the Minigram application.

## Getting Started

### Prerequisites
- Docker and Docker Compose
- .NET 10 SDK (if running without Docker)

### Running with Docker

1. Copy the example environment file:
   ```bash
   cp docker/.env.example docker/.env
   ```

2. Edit the environment variables in `docker/.env` as needed.

3. Start the services:
   ```bash
   cd docker
   docker-compose up -d
   ```

### Running without Docker

1. Navigate to the backend directory:
   ```bash
   cd backend/Minigram
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the service:
   ```bash
   dotnet run --project Minigram.Auth
   ```

## API Endpoints

- `POST /api/v1/auth/login` - User login
- `POST /api/v1/auth/register` - User registration
- `POST /api/v1/auth/refresh` - Refresh access token
- `POST /api/v1/auth/logout` - User logout

## Database

The service uses PostgreSQL for storing user credentials and refresh sessions.

## Configuration

Environment variables can be set in the `docker/.env` file or passed directly to the Docker containers.