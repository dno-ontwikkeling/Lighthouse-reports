1. cd ....\LightHouseReports.Data\LightHouseReports.Data.csproj

2. dotnet ef migrations add MigrationName -s "C:\Users\olivi\Documents\GitHub\Lighthouse reports\LightHouseReports\LightHouseReports.Web\LightHouseReports.Web.csproj" 

3. dotnet ef database update -s "..\LightHouseReports.Web\LightHouseReports.Web.csproj"
