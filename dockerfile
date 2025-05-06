# Use the official .NET SDK image to build the app
# FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0 AS build

WORKDIR /src

# Copy the solution and all project files
COPY src/Cel.GameOfLife.API/ Cel.GameOfLife.API/
COPY src/Cel.GameOfLife.Application/ Cel.GameOfLife.Application/
COPY src/Cel.GameOfLife.Domain/ Cel.GameOfLife.Domain/
COPY src/Cel.GameOfLife.Infra/ Cel.GameOfLife.Infra/
COPY src/Cel.GameOfLife.ApplicationUnitTest/ Cel.GameOfLife.ApplicationUnitTest/
COPY src/Cel.GameOfLife.sln ./

# Restore dependencies for the entire solution
RUN dotnet restore

# Copy the rest of the source code
COPY . ./

# Build the solution
WORKDIR /src/Cel.GameOfLife.API
RUN dotnet build -c Release -o /app/build
# Publish the API project
RUN dotnet publish -c Release -o /app/publish

# Use the official runtime image for the final image
FROM mcr.microsoft.com/dotnet/nightly/aspnet:9.0 AS final
WORKDIR /app

# Copy the published API project from the build stage
COPY --from=build /app/publish .

# Expose the port that the API will run on
EXPOSE 80

# Start the API when the container runs
ENTRYPOINT ["dotnet", "Cel.GameOfLife.API.dll"]