# Pokémon Tournament Assessment

This project contains both the backend API and the Angular frontend for a Pokémon tournament application.

## Project structure

- `Backend/` – ASP.NET Core Web API
- `Frontend/` – Angular application

## Tech stack

- Backend: ASP.NET Core 8, C#, Swagger
- Frontend: Angular 18, TypeScript, Bootstrap 5
- Communication: HTTP API requests between frontend and backend

## Backend setup

1. Open a terminal in the `Backend` folder.
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Run the API:
   ```bash
   dotnet run --project PokemonTournament/PokemonTournament.WebAPI.csproj
   ```
4. Swagger UI should open in the browser, or you can use:
   - HTTP: `http://localhost:5109`
   - HTTPS: `https://localhost:7068`

The backend config allows requests from the Angular app running on local development ports.

## Frontend setup

1. Open a terminal in the `Frontend` folder.
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the Angular app:
   ```bash
   ng serve
   ```
4. Open the application in the browser at:
   ```text
   http://localhost:4200/
   ```

## Running the app together

- Start the backend first.
- Then start the frontend.
- The frontend calls the backend through the configured API URL in the Angular environment file.

## Notes

- The backend exposes a tournament statistics endpoint for the frontend.
- The frontend allows sorting and pagination controls for the Pokémon tournament results.
- Swagger is enabled in development for testing the API.
