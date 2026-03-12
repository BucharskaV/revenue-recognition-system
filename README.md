# Revenue Recognition System
A REST API Application solves the revenue recognition problem, which involves determining when a company can officially record received payments as revenue. More complex financial cases, such as long-term service contracts or subscriptions, require the revenue to be distributed over the specified period. The system helps manage these rules to ensure accurate financial reporting.
# Installation instructions
1. Clone repository
   ```bash
   git clone https://github.com/BucharskaV/revenue-recognition-system.git
   ```
2. Create appsettings.json in the API folder following to this template:
   ```json
   {
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
      "DefaultConnection": "Server= your server,port;Database=your db;User Id=username;Password=password;TrustServerCertificate=True"
    },
3. Navigate to the project root directory.
    ```
    cd RevenueRecognitionSystem
    ```
4. Create Docker Network
    ```
    docker network create revenuesystem-network
    ```
5. Start SQL Server Container
    ```
    docker run -e "ACCEPT_EULA=Y" `
    -e "SA_PASSWORD=your password" `
    --network revenuesystem-network `
    --name sqlserver `
    -p 1433:1433 `
    -d mcr.microsoft.com/mssql/server:2022-latest
    ```
6. Create database
    ```
    docker exec -it sqlserver /opt/mssql-tools18/bin/sqlcmd `
    -S your server `
    -U your username `
    -P your password `
    -C `
    -Q "CREATE DATABASE your database name"
    ```
7. Apply Entity Framework Migrations
   ```
     dotnet ef database update `
      --project RevenueRecognitionSystem.Infrastructure `
      --startup-project RevenueRecognitionSystem.API
   ```
8. Build the API Docker Image
   ```
     cd RevenueRecognitionSystem.API
     docker build -t revenuesystem-api .
   ```
9. Run the API Container
    ```
      docker run -d -p 5000:80 `
      --network revenuesystem-network `
      -e ASPNETCORE_ENVIRONMENT=Development `
      -e ConnectionStrings__DefaultConnection="you connection string" `
      --name revenuesystem-api `
      revenuesystem-api
    ```
10. Verify Containers
    ```
    docker ps
    ```
    Expected output:
    ```
      sqlserver
      revenuesystem-api
    ```
11. Access the API:
      ```
      http://localhost:5000/swagger
      ```
# Database schema
<img width="1362" height="827" alt="revenue-recognition-system schema" src="https://github.com/user-attachments/assets/d92cdab0-0c29-422b-9e9d-f5a3b9e96a11" />

# Used technoloqies & techniques:
- Programming language: C#
- A horizontal multi-layer architecture – API layer, Service layer, Infrastructure layer, Domain layer
- ASP.NET Core – Backend framework
- Entity Framework Core (Code-First approach) – ORM for data access and migrations
- Microsoft SQL Server – Database management system
- Docker support
- RESTful API Architecture
- Postman / Swagger – API testing
- HTTP Status codes
- JWT Auth – 'token-based' API access
- AutoMapper – mapping
- FluentValidation - validation
- ILogger – logging
- Custom Middleware – exceptions managment
- Async programming patterns
- External Exchange Rate Service
- IActionFilter – filtering
- NUnit & Moq – testing

# API endpoints
### Employee Authentication
| Method | Endpoint | Description | RequestBody | ResponseBody |
|--------|----------|-------------|-------------|-------------|
| POST | `/api/employees/register` | Register new user |RegisterRequest|-|
| POST | `/api/employees/login` | Log in and receive JWT token |LoginRequest|LoginResponse|
| POST | `/api/employees/refresh` | Refresh JWT token |RefreshTokenRequest|RefreshTokenResponse|
| POST | `/api/employees/logout` | Log out |-|-|

### Clients
| Method | Endpoint | Description | RequestBody | ResponseBody |
|--------|----------|-------------|-------------|-------------|
| GET | `/api/clients` | Get all clients|-|IEnumerable of GetClientResponse|
| GET | `/api/clients/{id}` | Get client by id |-|GetClientResponse|
| DELETE | `/api/clients/{id}` | Soft-Delete client |-|-|
| POST | `/api/clientst/individual` | Add new individual client |AddIndividualRequest|-|
| POST | `/api/clientst/company` | Add new company client |AddCompanyRequest|-|
| PUT | `/api/clientst/individual/{id}` | Update individual client |UpdateIndividualRequest|-|
| PUT | `/api/clientst/company/{id}` | Update company client |UpdateCompanyRequest|-|

### Contracts
| Method | Endpoint | Description | RequestBody | ResponseBody |
|--------|----------|-------------|-------------|-------------|
| GET | `/api/contracts` | Get all contracts|-|IEnumerable of GetAllContractsResponse|
| DELETE | `/api/contracts/{contractId}` | Delete contract |-|-|
| POST | `/api/contracts` | Add new contract |CreateUpfrontContractRequest|-|
| POST | `/api/contracts/{contractId}/payment` | Add new payment for the contract |-|-|

### Subscriptions
| Method | Endpoint | Description | RequestBody | ResponseBody |
|--------|----------|-------------|-------------|-------------|
| GET | `/api/subscriptions` | Get all subscriptions|-|IEnumerable of GetAllSubscriptionsResponse|
| POST | `/api/subscriptions` | Add new subscription |CreateSubscriptionRequest|-|
| POST | `/api/subscriptions/{subscriptionId}/payment` | Add new payment for the subscription |-|-|

### Revenue
| Method | Endpoint | Description | RequestBody | ResponseBody |
|--------|----------|-------------|-------------|-------------|
| GET | `/api/revenue` | Get the revenue for the specific client and software|RevenueRequest|RevenueResponse|

# License
MIT License

Copyright (c) [2026] [Vladyslava Bucharska]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
