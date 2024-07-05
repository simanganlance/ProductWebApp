
# ProductWebApp

This project contains unit tests for the repository, service, and controller layers of an ASP.NET Core web application.

## Table of Contents

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Setup](#setup)
- [Solution Structure](#soulution-structure)
- [Technologies Used](#technologies-used)


## Overview

This is a simple web application using C# and .NET Core that performs CRUD operations on a list of products. 

## Prerequisites

Before running the tests, ensure you have the following installed:
- .NET 8 SDK or later
- Visual Studio 2022 or Visual Studio Code
- Microsoft SQL Database

## Setup

1. **Clone the Repository**: `git clone https://github.com/simanganlance/ProductWebApp.git`
2. **Restore Packages**: Open the solution in Visual Studio and restore the NuGet packages.
3. **Configure AppSettings**: If necessary, update `appsettings.json` with appropriate connection strings or API URLs.
We have two configuration file as follows:

  ./ProductApi/appsettings.json
  ```json
  {
   "ConnectionStrings": {
    "DefaultConnection": "{Change Database Connection String}"
    }
  }
  ```
  ./ProductWebView/appsettings.json
  ```json
  {
   "ApiSettings": {
    "BaseUrl": "{Change to API URL}"
    }
  }
  ```
4. **Generate Database**: Using Code First Migration, run the command below in Package Manager Console found in Visual Studio `Tools > Nuget Package Manager > Package Manager Console`:
  ```shell
  Add-Migration InitialCreate
  Update-Database
  ```
6. **Run the API**: To run it locally user must select `Multiple startup project` as shown below found in the solution properties.
   ![image](https://github.com/simanganlance/ProductWebApp/assets/150671121/a4a004eb-4221-422e-9f16-740e9a55d1f2)
7. Once it is up and running it will automatically display both Web application and Swagger API documentation


## Solution Structure
The solution compose of 3 projects as follows:
1. `ProductApi` is the .NET Core Web API project which serves as the Backend
2. `ProductWebView` is the ASP.NET Core MVC project which serves as the Frontend.
3. `ProductTest` is the test project for both Backend and Frontend.


## Technologies Used

- **ASP.NET Core 8**: Web framework
- **Entity Framework Core**: Object-relational mapping (ORM) framework
- **xUnit**: Testing frameworks
- **Moq**: Mocking framework (if used)
- **HttpClient**: For integration testing API endpoints
