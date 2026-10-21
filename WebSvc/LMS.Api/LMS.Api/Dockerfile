FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebSvc/LMS.Api/LMS.Api/LMS.Api.csproj", "WebSvc/LMS.Api/LMS.Api/"]
RUN dotnet restore "WebSvc/LMS.Api/LMS.Api/LMS.Api.csproj"
COPY . .
WORKDIR "/src/WebSvc/LMS.Api/LMS.Api"
RUN dotnet publish "LMS.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LMS.Api.dll"]
