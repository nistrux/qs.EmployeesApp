# build context: project root

# Node.js
FROM node:20-alpine AS frontend-builder
WORKDIR /app/frontend

COPY ./frontend/package.json ./frontend/package-lock.json* ./

RUN npm install --frozen-lockfile --production=false

COPY ./frontend/ . ./

RUN npm run build

# backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-builder
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

COPY ./backend/ ./

WORKDIR /app/qs.EmployeesApp.Server/

RUN dotnet restore ./qs.EmployeesApp.Server.sln

RUN dotnet build "./qs.EmployeesApp.Web/qs.EmployeesApp.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# publish backend
FROM backend-builder AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "./qs.EmployeesApp.Web/qs.EmployeesApp.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

COPY --from=frontend-builder /app/frontend/dist /app/publish/wwwroot

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

USER app
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8888
#EXPOSE 8089

ENTRYPOINT ["dotnet", "qs.EmployeesApp.Web.dll"]
