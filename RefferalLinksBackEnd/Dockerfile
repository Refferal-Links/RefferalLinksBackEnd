FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
# Set the working directory in the container
WORKDIR /app

# Copy the project files and restore as distinct layers
 COPY *.sln ./
 COPY RefferalLinks.API/RefferalLinks.API.csproj ./RefferalLinks.API/
 COPY RefferalLinks.Common/RefferalLinks.Common.csproj ./RefferalLinks.Common/
 COPY Maynghien.Common/Maynghien.Common.csproj ./Maynghien.Common/
 COPY RefferalLinks.DAL/RefferalLinks.DAL.csproj ./RefferalLinks.DAL/
 COPY RefferalLinks.Models/RefferalLinks.Models.csproj ./RefferalLinks.Models/
 COPY RefferalLinks.Service/RefferalLinks.Service.csproj ./RefferalLinks.Service/
 COPY RefferalLinks.TransferCronjob/RefferalLinks.TransferCronjob.csproj ./RefferalLinks.TransferCronjob/


# Restore NuGet packages for all projects
RUN dotnet restore

# Copy the remaining files
COPY . ./

RUN dotnet publish -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port the app will run on
EXPOSE 5000

# Set the entry point for the application
ENTRYPOINT ["dotnet", "RefferalLinks.API.dll"]