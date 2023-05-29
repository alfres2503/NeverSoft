# Neverland Condominium Application

The Neverland Condominium Application is a web-based application built using the ASP.NET MVC framework. It is designed to facilitate various tasks and operations related to the management of a fictional condominium called Neverland. This README.md file provides an overview of the application and its features, along with instructions for setting up and running the application.

## Features

The Neverland Condominium Application offers the following features:

- Resident Management: Allows the management of resident information, including registration, updates, and deletion. Administrators can view and manage resident details, such as contact information, unit allocation, and account status.
- Facility Booking: Residents can book various facilities within the condominium, such as the swimming pool, gym, and events hall. The application provides an interface for checking availability, making reservations, and managing bookings.
- News: Administrators can post announcements and notifications to keep residents informed about important updates, events, and maintenance schedules. Residents can view and access these announcements through their accounts.
- Incidences Requests: Residents can submit maintenance requests for issues or repairs within their units or common areas. Administrators can track and manage these requests, and update their status.
- Payments and Invoices: Residents can view their monthly invoices, make payments online, and track their payment history. The application integrates with popular payment gateways to provide a secure and convenient payment process.
- Security and Access Control: The application includes features to ensure the security and privacy of residents' information. User authentication and authorization mechanisms are implemented to restrict access to appropriate personnel and protect sensitive data.

## Technologies

The Neverland Condominium Application is developed using the following technologies and frameworks:

- ASP.NET MVC: Provides the architectural pattern for building the web application, separating concerns into models, views, and controllers.

- C#: The primary programming language used for server-side development and implementing business logic.

- CSHTML/CSS/Bootstrap 5: Used for creating the application's user interfaces and styling.

- JavaScript: Used for client-side scripting and enhancing user interactions.

- Entity Framework: Enables the mapping of database objects to C# objects, facilitating database operations and data access.

- SQL Server: The relational database management system used for storing application data.

## Contributing

Contributions to the Neverland Condominium Application are welcome! If you find any bugs, have suggestions for new features, or would like to contribute in any other way, please submit an issue or create a pull request on the GitHub repository.

- Ensure you have the following prerequisites installed:

  - Visual Studio 2022 (or Visual Studio Code) with ASP.NET MVC support.
  - SQL Server 2019 (or SQL Server Express) for database management.

- Clone the application repository from GitHub using the following command:

```bash
git clone https://github.com/alfres2503/condominium-mvc-app
```

- Open the solution file (.sln) in Visual Studio (or Visual Studio Code).

- Configure the database connection string in the Web/Web.config and Infrastructure/App.config file to connect to your SQL Server instance.

- Build the solution to restore packages and compile the application.

- Restore Database from using files in DB folder

- Start the application by running it in Visual Studio or using the command:

```bash
dotnet run
```
