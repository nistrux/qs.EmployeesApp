FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["qs.EmployeesApp.Web/qs.EmployeesApp.Web.csproj", "qs.EmployeesApp.Web/"]
RUN dotnet restore "qs.EmployeesApp.Web/qs.EmployeesApp.Web.csproj"
COPY . .
WORKDIR "/src/qs.EmployeesApp.Web"
RUN dotnet build "./qs.EmployeesApp.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./qs.EmployeesApp.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "qs.EmployeesApp.Web.dll"]
