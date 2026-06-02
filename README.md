# Twinstar Animation Backend API

## Description

Twinstar Animation Backend API is the backend component of the Twinstar Animation platform, a digital content and entertainment project focused on publishing comics, manga, animations and related media content.

The goal of the platform is to provide creators with a centralized environment where content creation, distribution, audience engagement and monetization can be managed within a single system. Instead of relying on multiple separate services for publishing, e-commerce, payments and content management, the platform aims to bring these functions together through one integrated solution.

This backend system is responsible for handling business logic, authentication, authorization, content management, e-commerce operations, order processing and payment verification. It exposes a REST API that communicates with the React frontend and serves as the foundation for managing users, series, chapters, media content, products and premium access functionality.

The project was developed as part of a higher education exams thesis in Web Development and demonstrates how a modern fullstack architecture can integrate digital publishing, e-commerce and external services into a unified platform.
The backend provides functionality for:

- User registration and login
- Role-based access control
- Creator and Customer roles
- Series and chapter management
- Product and order management
- Stripe payment integration
- Premium content access after purchase
- API communication with the React frontend

---

## Link to frontend project repository

Frontend repository : [Link](https://github.com/SaraM47/twinstaranimation-frontend).

---

## Technologies

The backend is built with:

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- ASP.NET Identity
- JWT Authentication
- HttpOnly Cookies
- Stripe API
- Swagger UI

---

## Project structure

### Folder overview

| Folder                 | Purpose                                                      |
| ---------------------- | ------------------------------------------------------------ |
| `Controllers/`         | Handles HTTP requests and API endpoints                      |
| `Data/`                | Contains the database context and database configuration     |
| `DTOs/`                | Defines data transfer objects used between client and server |
| `Models/`              | Contains entity models used by Entity Framework Core         |
| `Services/`            | Contains business logic                                      |
| `Services/Interfaces/` | Defines service contracts                                    |
| `Configurations/`      | Contains configuration classes, for example Stripe settings  |
| `Migrations/`          | Contains Entity Framework Core database migrations           |


## Main features
### Authentication and authorization

The backend uses ASP.NET Identity together with JWT Authentication and HttpOnly Cookies.

Main authentication features:

* Register new users
* Login users
* Store JWT token in HttpOnly Cookie
* Fetch current authenticated user
* Role-based authorization
* Protected API endpoints

The system uses two main roles:

* Creator
* Customer

A Creator can manage content, products and media through the creator dashboard.
A Customer can log in, purchase products and access premium content after payment.

## Database

The project uses SQL Server as a relational database and Entity Framework Core as ORM.

The database is built using the Code First approach, where C# model classes are used as the foundation for the database schema.

Main entities include:

* Users
* Series
* Chapters
* Pages
* Videos
* Products
* Orders
* OrderItems
* Ratings
* ExternalLinks

## Configuration

Configuration is handled through:

appsettings.json
appsettings.Development.json

## Installation
1. Clone the repository
```bash
git clone TwinstarAnimation-backend.API
```

2. Navigate to the backend project
```bash
cd TwinstarAnimation-backend.API
```

3. Restore dependencies
```bash
dotnet restore
```

4. Apply database migrations
```bash
dotnet ef database update
```

5. Start the API
```bash
dotnet run
```

## Entity Framework Core Commands
Add a new migration
```bash
dotnet ef migrations add MigrationName
```

Update the database
```bash
dotnet ef database update
```

Remove the latest migration
```bash
dotnet ef migrations remove
```

## API Documentation

Swagger UI is used to test and document the API endpoints.

After starting the backend, open Swagger in the browser:

```bash
https://localhost:<port>/swagger
```
or

```bash
http://localhost:<port>/swagger
```
depending on the local development port.

## Core API Areas
### Authentication

Example endpoints:
```bash
POST /api/Auth/register
POST /api/Auth/login
GET  /api/Auth/me
POST /api/Auth/logout
```
### Series

Example endpoints:

```bash
GET    /api/Series
GET    /api/Series/{id}
POST   /api/Series
PUT    /api/Series/{id}
DELETE /api/Series/{id}
```
### Chapters

Example endpoints:

```bash
GET    /api/Chapters/series/{seriesId}
POST   /api/Chapters
PUT    /api/Chapters/{id}
DELETE /api/Chapters/{id}
```
### Products

Example endpoints:
```bash
GET    /api/Products
GET    /api/Products/{id}
POST   /api/Products
PUT    /api/Products/{id}
DELETE /api/Products/{id}
```

### Orders and Checkout
Example endpoints:

```bash
POST /api/Orders/checkout
GET  /api/Orders/my-orders
```
### Stripe Webhook

Example endpoint:
```bash
POST /api/Webhook
```

## Payment flow

The payment flow is handled through Stripe.

Basic flow:

1. The customer adds products to the cart in the frontend.
2. The frontend sends the cart data to the backend.
3. The backend creates an order with status Pending.
4. The backend creates a Stripe PaymentIntent.
5. Stripe returns a clientSecret.
6. The frontend uses the clientSecret to complete payment.
7. Stripe sends a webhook event to the backend.
8. The backend verifies the webhook.
9. The order status is updated from Pending to Paid.
10. Premium content access can be granted based on the paid order.

## External integrations

The backend and platform are prepared for or connected with:

* Stripe for payments
* YouTube for video content
* Patreon for creator support
* BuyMeACoffee for creator support

Stripe is handled through backend integration. YouTube, Patreon and BuyMeACoffee are used as external platform integrations.

## Development status

This backend is part of an academic project and should be considered a working prototype.

Implemented core functionality includes:

- Authentication
- Role-based access
- Series management
- Product management
- Order handling
- Stripe checkout
- Premium content access logic

Not fully production-ready parts include:

- Deployment
- Advanced performance testing
- Automated test coverage
- Full production security review
- Complete public content material