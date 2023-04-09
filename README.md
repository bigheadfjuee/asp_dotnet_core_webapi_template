# aps_dotnet_core_webapi_template
ASP.NET Core WebAPI Template

## Enity Framework Core

https://learn.microsoft.com/en-us/ef/core/get-started/overview/install

dotnet add package Microsoft.EntityFrameworkCore.SqlServer

dotnet tool install --global dotnet-ef

dotnet tool update --global dotnet-ef

dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet add package Microsoft.EntityFrameworkCore.Tools


## YourProjectDB - EF Code First

dotnet ef migrations add InitialCreate

dotnet ef migrations add init --context YourProjectContext

dotnet ef database update --context YourProjectContext