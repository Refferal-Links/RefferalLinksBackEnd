# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory in the container
WORKDIR /app

# Copy the remaining files
COPY . ./
# Expose the port the app will run on
EXPOSE 5170

# Set the entry point for the application
ENTRYPOINT ["dotnet run --project ./RefferalLinks.API/RefferalLinks.API.csproj"]