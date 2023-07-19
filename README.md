# COMMANDS

## migrating database

### using dotnet-cli

> dotnet ef migrations add "Initial create"  
> dotnet ef database update

### using package manager console

> add-migration "Initial create" 
> update-database


## for identity scafolding in mvc project

> dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
> dotnet add package Microsoft.AspNetCore.Identity.UI
> dotnet add package Microsoft.EntityFrameworkCore.SqlServer
> dotnet add package Microsoft.EntityFrameworkCore.Tools
> dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design

> dotnet aspnet-codegenerator identity -dc AuthSystem.Data.AuthDbContext --files "Account.Register;Account.Login;Account.Logout"
> dotnet aspnet-codegenerator identity -dc AuthSystem.Data.AuthDbContext -u "ApplicationUser" --files "Account.Register;Account.Login;Account.Logout"
