# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy solution + restore
COPY ["TaskManagementSystem.sln", "./"]
COPY ["src/TaskManagementSystem.WebApi/TaskManagementSystem.WebApi.csproj", "src/TaskManagementSystem.WebApi/"]
COPY ["src/TaskManagementSystem.Application/TaskManagementSystem.Application.csproj", "src/TaskManagementSystem.Application/"]
COPY ["src/TaskManagementSystem.Infrastructure/TaskManagementSystem.Infrastructure.csproj", "src/TaskManagementSystem.Infrastructure/"]
COPY ["src/TaskManagementSystem.Domain/TaskManagementSystem.Domain.csproj", "src/TaskManagementSystem.Domain/"]

RUN dotnet restore "src/TaskManagementSystem.WebApi/TaskManagementSystem.WebApi.csproj"

# copy everything and publish
COPY . .
RUN dotnet publish "src/TaskManagementSystem.WebApi/TaskManagementSystem.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskManagementSystem.WebApi.dll"]
