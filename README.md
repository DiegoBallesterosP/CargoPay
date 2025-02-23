# Setting Up Migrations with Entity Framework

## Steps to Set Up Migration with ORM to the Database

1. **Adjust the connection string** in the `appsettings.json` file of the project to match your database parameters.
2. **Open the Visual Studio terminal** (Package Manager Console).
3. **Run the command** `Add-Migration Initial` to generate a migration file with the model changes.
4. **Apply the migration** by running `Update-Database`, which will update the database with the structure defined in the code.
