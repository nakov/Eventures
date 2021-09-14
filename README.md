# Eventures C# App + RESTful API + Desktop Client + Android Client

Sample apps for demonstrating how to implement **continuous integration** and build a CI/CD pipeline with **GitHub Actions**.
  - Target platform: .NET 5
  - CI system: GitHub Actions


## Eventures Web App

The ASP.NET Core app "Eventures" is an app for creating events.
* Technologies: C#, ASP.NET Core, Entity Framework Core, ASP.NET Core Identity, NUnit
* The app supports the following operations:
 - Home page (view events count + menu): `/`
 - View events: `/Events/All`
 - Create a new task (name + place + start date + end date + total tickets + price per ticket): `/Events/Create`
 - Edit event: `/Events/Edit/:id`
 - Delete event: `/Events/Delete/:id`


## Eventures RESTful API

The following endpoints are supported:
 - `GET /api` - list all API endpoints 
 - `GET /api/events` - list all events
 - `GET /api/events/count` - returns events count
 - `GET /api/events/:id` - returns an event by given `id` 
 - `POST /api/events/create` - create a new event (post a JSON object in the request body, e.g. `{ "name": "Open Fest", place": "Borisova Garden", start": "2022-08-14T10:00:00.000Z", "end": "2022-08-15T18:00:00.000Z", "totalTickets": 500, "pricePerTicket": 15 }`)
 - `PUT /api/events/:id` - edit event by `id` (send a JSON object in the request body, holding all fields, e.g. `{ "name": "Open Fest", place": "Borisova Garden", start": "2022-08-14T10:00:00.000Z", "end": "2022-08-15T18:00:00.000Z", "totalTickets": 500, "pricePerTicket": 15 }`)
 - `PATCH /api/events/:id` - partially edit event by `id` (send a JSON object in the request body, holding the fields to modify, e.g. `{ place": "South Park Sofia", "pricePerTicket": 12 }`)
 - `DELETE /api/events/:id` - delete event by `id`
 - `GET /api/users` - list all users
 - `POST /api/users/login` - logs in an existing user (send a JSON object in the request body, holding all fields, e.g. `{"username": "username", "password": "pass123"}`)
 - `POST /api/users/register` - registers a new user (send a JSON object in the request body, holding all fields, e.g. `{"username": "username", "email": "user@example.com", "password": "pass123", "confirmPassword": "pass123", "firstName": "Test", "lastName": "User"}`)

## Desktop Client

Windows Forms Client for the Eventures RESTful API.
* Technologies: C#, .NET 5, Windows Forms, RestSharp

## Android Client

Android mobile app client for the Eventures RESTful API.
* Technologies: Java, Android SDK, Retrofit HTTP client
* Platform: Android


## Screenshots

### Eventures Web App

### Eventures RESTful API

### Desktop Client

### Android Client



