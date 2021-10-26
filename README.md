# Eventures C# App + RESTful API + Desktop Client + Android Client

Sample apps for demonstrating how to implement **continuous integration** and build a CI/CD pipeline with **GitHub Actions**.
  - Target platform: .NET 5
  - CI system: GitHub Actions
  - Seeded database with one user and three events
  - Default user credentials: **guest** / **guest**

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

Windows Forms client app for the Eventures RESTful API.
* Technologies: C#, .NET 5, Windows Forms, RestSharp

## Android Client

Android mobile app client for the Eventures RESTful API.
* Technologies: Java, Android SDK, Retrofit HTTP client
* Platform: Android

## Screenshots

### Eventures Web App

<kbd>![image](https://user-images.githubusercontent.com/69080997/135711976-d201d880-33c3-48ca-8105-f5686865242b.png)</kbd>
<kbd>![image](https://user-images.githubusercontent.com/69080997/135712009-b53b097a-6965-4633-b773-a3beccf54c69.png)</kbd>
<kbd>![image](https://user-images.githubusercontent.com/69080997/135712052-e0f6e38c-d3c9-4aa3-8bf1-df0677f7859b.png)</kbd>
<kbd>![image](https://user-images.githubusercontent.com/69080997/137502117-020b2db5-d01a-4712-ae4b-e9743d88dbfc.png)</kbd>

### Eventures RESTful API

<kbd>![image](https://user-images.githubusercontent.com/69080997/136526348-4a3c00d9-b4b0-40f8-81f9-9904785c0172.png)</kbd>
<kbd>![image](https://user-images.githubusercontent.com/69080997/136526560-721e6f6a-b3d4-4f1e-9646-2e2052c4912b.png)</kbd>
<kbd>![image](https://user-images.githubusercontent.com/69080997/136526724-01b3a68f-2909-4c4b-8799-97e6f19b6d87.png)</kbd>

### Desktop Client

<kbd>![Screenshot_39](https://user-images.githubusercontent.com/69080997/133249837-e5e3b55f-8668-47bd-90ba-800987d88af1.png)</kbd>
<kbd>![Screenshot_41](https://user-images.githubusercontent.com/69080997/133249861-5609ebcc-1d98-4a3c-84b4-75dc165d0167.png)</kbd>
<kbd>![Screenshot_40](https://user-images.githubusercontent.com/69080997/133249854-238fd94f-7c3a-4405-ab65-d67db4525d64.png)</kbd>
<kbd>![image](https://user-images.githubusercontent.com/69080997/135712288-2b281f60-74f0-4269-b8f2-db0aa02bd777.png)</kbd>
<kbd>![Screenshot_43](https://user-images.githubusercontent.com/69080997/133249884-505f52c5-a8ba-4764-a012-ec64f56baf05.png)</kbd>

### Android Client

![Screenshot_25](https://user-images.githubusercontent.com/69080997/133249110-02744bb1-8936-4854-9f3a-512034d79edd.png)
![Screenshot_26](https://user-images.githubusercontent.com/69080997/133249129-c7a27786-5331-498a-ae98-af4763603578.png)
![Screenshot_27](https://user-images.githubusercontent.com/69080997/133249144-b14f7967-3ffe-4434-acbc-7fa00581eb39.png)
![image](https://user-images.githubusercontent.com/69080997/135712372-ea0c6099-7f59-41ab-ae3d-75bf97733b7e.png)
![Screenshot_31](https://user-images.githubusercontent.com/69080997/133249220-c1ec76ae-10aa-4790-986f-ab4dcc4368b4.png)





