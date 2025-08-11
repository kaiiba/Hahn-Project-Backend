# SETUP

Please move out from the solution folder:
- Project.API
- Project.Application
- Project.Application.Tests
- Project.Domain.Tests
- Project.Infrastructure

From the solution, add existing project and select each folder then rebuild the solution. The references already added.

Script of migration from SQL Server to C# Entities
Scaffold-DbContext Name="DefaultConnection" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Persistence -Context AppDbContext -Project Project.Infrastructure -StartupProject Project.API -Force

# Product Management API

A .NET 9 Web API application for managing products using Clean Architecture principles with Entity Framework Core and SQL Server.

## 📚 Architecture Principles

This application demonstrates:

- **Clean Architecture** with dependency inversion
- **Domain-Driven Design** with rich domain entities
- **Repository Pattern** for data access abstraction
- **SOLID Principles** throughout the codebase
- **Comprehensive Validation** at domain and API levels
- **Unit Testing** with high coverage
- **API Documentation** with Swagger/OpenAPI
- **Containerization** with Docker

## 🔧 Technology Stack

- **.NET 9** 					- Latest .NET framework
- **C# 13.0** 					- Latest C# language features
- **ASP.NET Core Web API** 			- RESTful API framework
- **Entity Framework Core 9.0.8** 		- ORM for database operations
- **SQL Server** 				- Database engine
- **Swagger/OpenAPI** 				- API documentation
- **MSTest** 					- Unit testing framework
- **Moq** 					- Mocking framework for tests
- **Docker** 					- Containerization
## 🛢 Database 

CREATE DATABASE ProductDb;
GO

USE ProductDb;
GO

CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

## 🚀 Features

### Product Management
- **Create Product** 				- Add new products with validation
- **Get All Products** 				- Retrieve all products
- **Get Product by ID** 			- Retrieve specific product
- **Decrease Stock** 				- Reduce product inventory with business rules


### API Endpoints
- GET    /api/products           			# Get all products 
- GET    /api/products/{id}      			# Get product by ID 
- POST   /api/products           			# Create new product 
- PATCH  /api/products/{id}/decrease-stock  	# Decrease product stock

### Domain Business Rules
- Product names are required and cannot exceed 100 characters
- Prices must be positive and have maximum 2 decimal places
- Stock cannot be negative
- Stock decrease operations validate available quantity
- Comprehensive input validation prevents invalid data types

## 🐳 Docker Setup

### Prerequisites
- Docker Desktop installed and running
- SQL Server container running on port 1433

**Key Points:**
- Uses `host.docker.internal` to connect from container to host
- Port `1433` is exposed for SQL Server communication
- Database: `ProductDb`
- User: `dockeruser` / Password: `123456`

## 🧪 Testing

### Run Unit Tests

### API Testing with Swagger
1. Navigate to the Swagger UI (root URL when running)
2. Use the interactive interface to test endpoints
3. Example payloads:

**Create Product:**
{ "name": "Sample Product", "price": 29.99, "stock": 100 }

**Decrease Stock:**
{ "quantity": 5 }

## 🚨 Troubleshooting

### Common Issues

1. **SQL Server Connection Failed**
   - Ensure SQL Server container is running: `docker ps`
   - Check if port 1433 is accessible: `telnet localhost 1433`
   - Verify firewall settings allow port 1433

2. **Database User Issues**
   - Recreate the `dockeruser` with proper permissions
   - Ensure the `ProductDb` database exists

3. **Container Communication**
   - Use `host.docker.internal` instead of `localhost` in connection strings
   - Verify both containers are on the same network (if using docker-compose)

4. **Port Conflicts**
   - Check if ports 8080, 8081, or 1433 are already in use
   - Use `docker port <container-name>` to check mapped ports

