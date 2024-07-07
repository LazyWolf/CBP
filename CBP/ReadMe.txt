Scaffolding a .NET Core app with data migrations.

For the pupose of this guide, the application will be called "APP". Please replace "APP" or "App" with whatever the appropriate project name is.

Create new project called "APP". Choose the template for the desired development framework. (E.g., ASP.NET Core Web App)
Create new project called "APP.Data". Choose the Class Library template.
Create new project called "APP.Services". Choose the Class Library template.

Under "APP" project, right click dependencies, and choose "Add Project Reference".
 - Add APP.Data
 - Add APP.Services

Under "APP.Services" project, right click dependencies, and choose "Add Project Reference".
 - Add APP.Data

Right click "APP" project, and select "Manage NuGet Packages...".
 - Find and install Microsoft.EntityFrameworkCore.Design

Right click "APP.Data" project, and select "Manage NuGet Packages...".
 - Find and install Microsoft.EntityFrameworkCore
 - Find and install Microsoft.EntityFrameworkCore.SqlServer
 - Find and install Microsoft.EntityFrameworkCore.Tools

Run command prompt (WINDOWS + R, cmd)
 - run: sqllocaldb info
 - if a relevant server name exits for your project, skip this step. If not, continue
 - run: sqllocaldb create APP
 - ensure it was created successfully by once again running: sqllocaldb info

Open SSMS, and connect to server: (localdb)\APP

Under the (localdb)\APP server, right click the Databases folder, and create a new database. Give it a suitable name, preferable one that's likely to be unique. (I'm choosing AppData)


Under "APP.Data" project
 - create a new Models folder
 - add an entity model to use as a base for all other data models
   public class Entity
   {
     public long Id { get; set; }
     public string CreatedBy { get; set; }
     public DateTime CreatedUtc { get; set; }
     public string UpdatedBy { get; set; }
     public DateTime UpdatedUtc { get; set; }
   }
 - add each relevant data model, inheriting from Entity

///
// DataContext + Interface
///

Under "APP" project, find and edit appsettings.json
 - add section to top:
   "ConnectionStrings": {
     "CBP": "Server=(localdb)\\APP; Database=AppData; Trusted_Connection=true; Trust Server Certificate=true; MultipleActiveResultSets=true; Integrated Security=true;"
   },
   
Under "APP" project, find and edit Project.cs
 - after builder is created, add:
   builder.Services.AddDbContext<IDataContext, DataContext>(options => options.UseSqlServer(buildr.Configuration.GetConnectionString("APP")))
 - // Check IDataContext implemented

Open the PMC (Tools > Nuget Package Manager > Package Manager Console)
 - under Default Project, select APP.Data
 - run: Add-Migration Init