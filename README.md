## Architecture

The solution follows N-tier architecture with the following layers:

- **Data Layer** (`Vacation.Data`): Entity Framework models, repositories, and unit of work
- **Business Layer** (`Vacation.Business`): Business logic, DTOs, and services
- **API Layer** (`Vacation.API`): RESTful API endpoints
- **Web Layer** (`Vacation.Web`): MVC frontend application
  
## Database Setup

1. **Create the Database**: Run the SQL script in `Tables Creation.sql` in SQL Server Management Studio 

2. **Connection String**: Update the connection string in both:

   - `Vacation.API/appsettings.json`
   - `Vacation.Web/appsettings.json`

   Default connection string (for LocalDB):

   ```json
   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=VacationDB;Trusted_Connection=true;MultipleActiveResultSets=true"
   ```

## Running the Application

### Option 1: Run Both API and Web Applications
