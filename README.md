# COMMANDS

## migrating database

### using dotnet-cli

> dotnet ef migrations add "Initial create"  
> dotnet ef database update

### using package manager console

> add-migration "Initial create"
> update-database

## Database scheme

- Library Assets
- Patrons
- Branches

**Library Assets**

```
    Title
    Author
    Year Published
    Cost
    ...



    Branch location
    checkout status
    ...
```

**Branches**

```
    Branch Name
    Telephone
    Address
    ...

    Total Costs of Assets,
    Total number of patrons
    ...
```

**Patron**

```
    First Name
    Last Name
    Address
    Phone number
    Date Of Birth
    ...


    Library Card ID
    Home Library  Branch

```

**Checkout**

```
    - associate an asset with a library card
    - define the time an asset was checked out
    - define the time an asset is due
```

**Library Card**

```
    - one to one relation ship with patron (represents a sort of layer of abstraction between the assets and patron who checkout and patron him or herself)
    - hold value for overdue fees
```

**Holds**

```
    - this functionality allow patrons to put holds on items while they are being checked out by other patrons
    when the patron who have already checkedout, returns the asset or item then it will automatically get checkout with the patron with earliest hold
```

**Checkout history**

```
    keep audit of every checkout that occurs in the system
```

**Assets status**

```
    table of static data that contain the all the different status that the library assets could have (like checked out, checked in, lost)
```

## MODEL classes

```
    - Book.cs
    - BranchHours.cs
    - Checkouts.cs
    - CheckoutHistory.cs
    - Holds.cs
    - LibraryAsset.cs
    - LibraryBranch.cs
    - LibraryCard.cs
    - Patron.cs
    - Status.cs
    - Video.cs
```


```
Table-Per-Hierarchy (TPH) strategy
    - One table for all entity types in a inheritance hierarcy

    - creates a single table for the Parent class and 
       inherited class will be represented using a column called discriminator on the table indicating parent class 

```



## for identity scafolding in mvc project

> dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
> dotnet add package Microsoft.AspNetCore.Identity.UI
> dotnet add package Microsoft.EntityFrameworkCore.SqlServer
> dotnet add package Microsoft.EntityFrameworkCore.Tools
> dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design

> dotnet aspnet-codegenerator identity -dc AuthSystem.Data.AuthDbContext --files "Account.Register;Account.Login;Account.Logout"
> dotnet aspnet-codegenerator identity -dc AuthSystem.Data.AuthDbContext -u "ApplicationUser" --files "Account.Register;Account.Login;Account.Logout"
