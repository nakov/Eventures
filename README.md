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

<kbd>![image](https://user-images.githubusercontent.com/69080997/133249350-f4fe7260-d0b6-46bf-8b86-fd47ff1ba491.png)</kbd>
<kbd>![image](https://user-images.githubusercontent.com/69080997/133249452-06ca31b9-18eb-4a5b-9c33-2ef868cd25ad.png)</kbd>
<kbd>![image](https://user-images.githubusercontent.com/69080997/133249530-83f99741-d09e-46e6-84b1-cc6ae45efd66.png)</kbd>
<kbd>![image](https://user-images.githubusercontent.com/69080997/133249601-e6b459da-fd6e-4258-a44e-2a4e140967b7.png)</kbd>

### Eventures RESTful API

<kbd>![Screenshot_36](https://user-images.githubusercontent.com/69080997/133249797-b876c83d-1c31-4e4b-93a3-55276f248bf5.png)</kbd>
<kbd>![Screenshot_37](https://user-images.githubusercontent.com/69080997/133249820-d67fbf73-235d-4887-82c9-0cdc0844015b.png)</kbd>
<kbd>![Screenshot_38](https://user-images.githubusercontent.com/69080997/133249825-6287e293-0e75-40f9-ae8d-9bbabedd1e73.png)</kbd>

### Desktop Client

<kbd>![Screenshot_39](https://user-images.githubusercontent.com/69080997/133249837-e5e3b55f-8668-47bd-90ba-800987d88af1.png)</kbd>
<kbd>![Screenshot_40](https://user-images.githubusercontent.com/69080997/133249854-238fd94f-7c3a-4405-ab65-d67db4525d64.png)</kbd>
<kbd>![Screenshot_41](https://user-images.githubusercontent.com/69080997/133249861-5609ebcc-1d98-4a3c-84b4-75dc165d0167.png)</kbd>
<kbd>![Screenshot_42](https://user-images.githubusercontent.com/69080997/133249867-a8020dd3-7e3a-49ef-83be-00dcefdf9718.png)</kbd>
<kbd>![Screenshot_43](https://user-images.githubusercontent.com/69080997/133249884-505f52c5-a8ba-4764-a012-ec64f56baf05.png)</kbd>

### Android Client

![Screenshot_25](https://user-images.githubusercontent.com/69080997/133249110-02744bb1-8936-4854-9f3a-512034d79edd.png)
![Screenshot_26](https://user-images.githubusercontent.com/69080997/133249129-c7a27786-5331-498a-ae98-af4763603578.png)
![Screenshot_27](https://user-images.githubusercontent.com/69080997/133249144-b14f7967-3ffe-4434-acbc-7fa00581eb39.png)
![Screenshot_31](https://user-images.githubusercontent.com/69080997/133249220-c1ec76ae-10aa-4790-986f-ab4dcc4368b4.png)
![Screenshot_30](https://user-images.githubusercontent.com/69080997/133249201-93dc9b63-a728-4e67-8495-5d206c986218.png)




