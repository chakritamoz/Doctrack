# Doctrack

**New Project DOTNET MVC**
```c#
dotnet new mvc -o {projectName}
```

**Install Tool on Machine**
```c#
dotnet tool uninstall --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef
```

**Install Package to Project**
```c#
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

**Code generatate Controller**
```c#
dotnet aspnet-codegenerator controller -name MoviesController -m Movie -dc MvcMovieContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries -sqlite
```

**Register Database**
```c#
builder.Services.AddDbContext<MvcMovieContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("{NameContext}")));
        
Registers the database context in the Program.cs file
```

**Connect Database**
```c#
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DoctrackContext": "Data Source=Doctrack.db"
  }
}

Adds a database connection string to the appsettings.json file.
```

**Create Context**
```c#
using Microsoft.EntityFrameworkCore;
using Doctrack.Models;

namespace Doctrack.Data
{
  public class DoctrackContext : DbContext
  {
    public DoctrackContext(DbContextOptions<DoctrackContext> options)
      : base(options)
      {
      }

    public DbSet<Job> Jobs {get; set;}
    public DbSet<Rank> Ranks {get; set;}
    public DbSet<DocumentType> DocumentTypes {get; set;}
    public DbSet<Document> Documents {get; set;}
    public DbSet<Employee> Employees {get; set;}
  }
}
```
