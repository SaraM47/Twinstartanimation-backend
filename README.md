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

Example configuration structure:
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "Jwt": {
    "Key": ""
  },
  "Stripe": {
    "SecretKey": "",
    "WebhookSecret": ""
  }
}