# Data-Lab
Data-Lab is a simple full-stack web application with UI and a backend for displaying data from different clients using tables and charts for statistical analysis,
inspired by Solita's Dev Academy program 2022 pre-entry assignment https://github.com/solita/dev-academy-2022-exercise.
Data is received from clients in CSV files processed and added to the database.

#### Live Demo: https://data-lab.azurewebsites.net

## Architecture
#### Languages, Libraries, And Frameworks
- C#
- ASP MVC Core 5
- Entity Framework
- Microsoft SQL server
- Javascript, Html ,CSS
- Bootstrap
- AdminLTE 3 (open source Bootstrap admin dashboard template)
- Chart js ( open-source JavaScript library for data visualization)

## Tools
#### Development tools
- Visual studio 2019
- Github
- Azure

#### Data process tools
- Microsoft SQL server management studio

## Unit Testing
#### Frameworks
- xUnit.net
- Moq

## Application Maps
![Site Map1](https://user-images.githubusercontent.com/53993050/148553941-9d7acd36-746d-49f1-a7e9-71dd653565a7.png)
![Site Map 2](https://user-images.githubusercontent.com/53993050/148554579-021fc63e-58eb-4fd7-9e2d-638e0c559b09.png)

## Database relationship model (ER model)
![DataBase Model](https://user-images.githubusercontent.com/53993050/148555339-f3d1e860-8cbd-4df6-873a-514b6b15e6ed.png)

## Data flow diagrams

#### Application users
![DataFlowChart1](https://user-images.githubusercontent.com/53993050/148556436-71627fda-47b1-4256-9336-fa231e217f2b.png)

#### Admin and SuperAdmin users
![DataFlowChat2](https://user-images.githubusercontent.com/53993050/148556574-2d43e745-65dc-41f9-8eed-bdb26f514c3a.png)

## Prerequisites before running project 
 - Visual studio 2019
 - ASP.Net Core 5
 - SQL Server 2017

## How to run the project

 - Add this project to a location in your disk.
 - Open the solution file using the Visual Studio 2019.
 - Restore the NuGet packages by rebuilding the solution.
 - Change the connection string in the appsettings.json file that points to your SQL Server
 - Execute a migration to create database tables
 - Run the project. Database will be seeded during runtime.


