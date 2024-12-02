# Task Management System  

A robust Task Management System built with **.NET Core APIs** for backend development. This project leverages **Entity Framework Core** for database operations, **LINQ** for querying, and adheres to best practices using design patterns like **CRUD Operations**, **Repository**, **Unit of Work**, and **n-Tier Layers** architecture.

## Features  

### 1. **User Authentication**  
- Secure user registration and login.  
- Authentication via **JWT Tokens** for session management.  

### 2. **Task Management**  
- Create, update, delete, and view tasks.  
- Ensure seamless task management operations.  

### 3. **Categories**  
- Tasks can be organized into categories.  
- CRUD operations for managing categories.  

### 4. **Search and Filter**  
- Search tasks by keywords.  
- Filter tasks based on criteria such as status, category, or due dates.  

### 5. **Permissions**  
- Role-based access control for secure API usage.  
- Different endpoints accessible based on user roles.  

---

## Technologies Used  

- **.NET Core**: For building high-performance APIs.  
- **Entity Framework Core**: Object-Relational Mapping (ORM) for database interaction.  
- **LINQ**: For efficient and readable querying of data.  
- **SQL Server**: Reliable and scalable database for storing application data.  

---

## Design Patterns  
The project follows established design principles for maintainable and scalable code:  
1. **CRUD Operations**: Simplified implementation of create, read, update, delete functionality.  
2. **Repository Pattern**: Centralized data access logic for better abstraction and testability.  
3. **Unit of Work Pattern**: Ensures atomicity in database operations.  
4. **n-Tier Architecture**: Clear separation of concerns across layers (API, Service, Data Access).  

---

## How to Run  

1. **Clone the Repository**  
   ```bash  
   git clone <repository-url>  
   cd <project-directory>  
   ```  

2. **Setup Database**  
   - Update connection string in `appsettings.json`.  
   - Run migrations using Entity Framework Core to create the database.  
     ```bash  
     dotnet ef database update  
     ```  

3. **Run the Application**  
   - Use the .NET CLI or an IDE like Visual Studio to start the project.  
     ```bash  
     dotnet run  
     ```  

4. **API Documentation**  
   - Access the Swagger UI at `http://localhost:<port>/swagger` for testing and documentation.  

---

## Future Enhancements  

- Add task reminders or notifications.  
- Integrate third-party tools for enhanced search functionality.  
- Build a frontend for seamless user interaction.  

---

This project demonstrates a clean and modular approach to building a scalable task management system with .NET Core. Contributions and suggestions are welcome!
