# CandidateBoard
The project contain 2 part: API and UI
[Database](#Database)
[API](#API)
[UI](#UI)

Database
========
In this project, I use SQL Server. 
You need to install Sql Server to your local, please follow this guide https://support.academicsoftware.eu/hc/en-us/articles/360006784838-How-to-install-MS-SQL-Server-2014 to setup
I use the code first, so that we don 't need the sql script to create table

API
========
This guide will show you how to set up API
1. Run directly when your PC already have Visual Studio
    - First, you have to Restore Nuget Package (all of Nuget can be found in nuget.org page)
    - Then, please edit the ConnectionString in appsettings.json file to the server that you install the SQL Server (if you install it in your local PC, it should be localhost)
    - Please check the user name and password in ConnectionString. By default I use sa account and password is 123456
    - Then you can fress F5 to run the project
    - If the API show the Swagger UI, it meen your setup is for API success

UI
========
This guide will show you how to set up UI
    - First, open file src/UI/index.js, update serverUrl to API in your local
    - Then open file src/UI/index.html with Chrome


