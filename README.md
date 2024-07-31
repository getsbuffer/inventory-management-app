# Inventory Management App

This is a .NET MAUI-based inventory management application designed to manage a shop's inventory and shopping cart. The application consists of a frontend built using .NET MAUI and a backend web API using ASP.NET Core. The backend API includes database persistence using MySQL.

## Project Structure

The project is organized into several key components:

- **IM.MAUI**: The frontend project built using .NET MAUI, which provides the user interface for managing inventory and shopping cart functionalities.
- **IM.API**: The backend project using ASP.NET Core to provide a RESTful API for managing inventory data. This project includes the implementation of database persistence using MySQL.
- **IM.Library**: A library project that stores the Models, Services, DTOs, Helpers, and Utility classes used by both the frontend and backend projects.

### IM.MAUI

This project contains the views and view models for the frontend, utilizing the MVVM pattern:

- **Views**: UI pages for displaying and interacting with the inventory and cart.
- **ViewModels**: Contains the business logic and state management for the corresponding views. Key ViewModels include:
  - `MainViewModel`
  - `ShopViewModel`
  - `InventoryManagementViewModel`
  - `CartViewModel`
  - `SubscriptionViewModel`

### IM.API

This project contains the API controllers and data access logic:

- **Controllers**: API endpoints to manage inventory items (`InventoryController`).
- **Database**: Contains the `AppDbContext` class for Entity Framework Core, handling database connections and schema definitions. This project utilizes MySQL for data persistence.
- **Persistence**: The application uses Entity Framework Core for ORM and database migrations.

### IM.Library

This project contains shared code used by both the frontend and backend projects:

- **DTO**: Data Transfer Objects used for transferring data between layers.
  - `Query.cs`
  - `ShopItemDTO.cs`
- **Helpers**: Helper classes for common functionalities.
  - `MappingHelper.cs`
- **Models**: Data models representing the structure of the database entities.
  - `ShopItem.cs`
  - `ShoppingCart.cs`
  - `ShoppingCartItem.cs`
  - `Subscription.cs`
- **Services**: Services used for business logic and data management.
  - `ShopItemService.cs`
  - `ShoppingCartProxy.cs`
  - `SubscriptionService.cs`
- **Utilities**: Utility classes for common tasks.
  - `WebRequestHandler.cs`

## Requirements

- .NET 8.0 or later
- MySQL Server
- Entity Framework Core
- .NET MAUI (for frontend development)

## Installation

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/inventory-management-app.git
   cd inventory-management-app ```
   
2. **Set Up MySQL Database**
  - Create a new MySQL Database called IMDB.
  - Create a user with appropriate privileges, like so:
  ```sql
   CREATE USER 'your_username_here'@'localhost' IDENTIFIED BY 'your_password_here';
   GRANT ALL PRIVILEGES ON IMDB.* TO 'your_username_here'@'localhost';
   FLUSH PRIVILEGES;
  ```
    
3. **Set Environment Variable**
  - On Windows:
    ```
    setx DB_CONNECTION "Server=localhost;Database=IMDB;User=your_username_here;Password=your_password_here;"
    ```
  - On Linux/Mac:
    ```
    export DB_CONNECTION="Server=localhost;Database=IMDB;User=getsbuffer;Password=password;"
    ```
    
4. **Install .NET Entity Framework Core Tools**
   ```
   dotnet tool install --global dotnet-ef
   ```
   
6. **Apply Database Migrations**
  - Navigate to the IM.API project directory and apply the migrations to create the database schema
     ```
     cd IM.API
     dotnet ef database update
     ```
     
7. **Run the Application**
  - Start the backend API
   ```
   cd IM.API
   dotnet ef database update
   ```
   - Start the frontend application:
   ```
   cd IM.MAUI
   dotnet run
   ```
    
## API Endpoints

- **GET /api/inventory**: Retrieves all inventory items.
- **GET /api/inventory/{id}**: Retrieves a specific inventory item by ID.
- **POST /api/inventory**: Adds a new inventory item.
- **PUT /api/inventory/{id}**: Updates an existing inventory item.
- **DELETE /api/inventory/{id}**: Deletes an inventory item.

## Persistence

The application uses MySQL for data persistence. Entity Framework Core is utilized for managing the database schema and performing CRUD operations. Migrations are used to keep the database schema in sync with the application models.

## Notes

- Ensure that the MySQL server is running and accessible from the machine where the application is deployed.

## Contributing

Feel free to open issues and submit pull requests. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License.
