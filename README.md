# Minigram Auth Service

This service handles user authentication for the Minigram application.

## Getting Started

### Prerequisites

* Docker and Docker Compose
* .NET 10 SDK (if running without Docker)

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

* `POST /api/v1/auth/login` - User login
* `POST /api/v1/auth/register` - User registration
* `POST /api/v1/auth/refresh` - Refresh access token
* `POST /api/v1/auth/logout` - User logout

## Database

The service uses PostgreSQL for storing user credentials and refresh sessions.

## Configuration

Environment variables can be set in the `docker/.env` file or passed directly to the Docker containers.

---

## Authentication Flow

### Login / Register

1. User authenticates via `/login` or `/register`

2. Service returns:

   * Access Token (JWT)
   * Refresh Token (GUID)

3. Refresh session is stored in the database with:

   * User ID
   * IP address
   * UserAgent
   * Expiration time (7 days)

---

### Refresh Token Flow

1. Client sends refresh token to `/refresh`
2. Service:

   * validates token in database
   * checks expiration
3. Generates:

   * new access token
   * new refresh token
4. Updates existing session (refresh token rotation)

---

### Logout

1. Client sends request with access token
2. Service extracts user ID from JWT (`sub` claim)
3. Deletes all refresh sessions for the user

---

## JWT Token Details

Access tokens are generated with the following properties:

* Algorithm: HMAC SHA256
* Claims:

  * `sub` — user ID
  * `email` — user email
* Includes:

  * `iss` (issuer)
  * `aud` (audience)
  * `exp` (expiration time)

Token lifetime is configured via environment variables.

---

## Refresh Token Behavior

* Refresh tokens are stored in the database as GUIDs
* Each refresh token represents a session
* Tokens expire after 7 days
* Refresh token rotation is implemented:

  * old token is replaced with a new one after refresh
  * prevents reuse of old tokens

---

## Security Notes

* Passwords are hashed using ASP.NET Identity PasswordHasher
* JWT tokens are signed using HMAC SHA256
* Access tokens are stateless
* Refresh tokens are stateful and stored in the database
* Sessions include IP address and UserAgent

---

## Troubleshooting

* **Service does not start**

  * Check Docker containers: `docker-compose ps`

* **Database connection failed**

  * Verify PostgreSQL is running
  * Check connection string

* **JWT validation fails**

  * Ensure correct Secret, Issuer, and Audience

* **Refresh token invalid**

  * Token may be expired or already rotated
