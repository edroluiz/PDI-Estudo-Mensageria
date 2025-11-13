# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Mensageria.sln .
COPY Mensageria.Api/Mensageria.Api.csproj Mensageria.Api/
COPY Mensageria.Application/Mensageria.Application.csproj Mensageria.Application/
COPY Mensageria.Domain/Mensageria.Domain.csproj Mensageria.Domain/
COPY Mensageria.Infrastructure/Mensageria.Infrastructure.csproj Mensageria.Infrastructure/
COPY Mensageria.Consumer/Mensageria.Consumer.csproj Mensageria.Consumer/

RUN dotnet restore Mensageria.sln

COPY . .
WORKDIR "/src/Mensageria.Api"
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Mensageria.Api.dll"]