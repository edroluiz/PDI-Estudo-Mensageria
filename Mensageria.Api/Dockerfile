# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todos os arquivos .csproj e restaura as dependências
COPY Mensageria.sln .
COPY Mensageria.Api/Mensageria.Api.csproj Mensageria.Api/
COPY Mensageria.Application/Mensageria.Application.csproj Mensageria.Application/
COPY Mensageria.Domain/Mensageria.Domain.csproj Mensageria.Domain/
COPY Mensageria.Infrastructure/Mensageria.Infrastructure.csproj Mensageria.Infrastructure/

# ----- LINHA QUE FALTAVA ABAIXO -----
COPY Mensageria.Consumer/Mensageria.Consumer.csproj Mensageria.Consumer/

RUN dotnet restore Mensageria.sln

# Copia o resto do código fonte e faz o build
COPY . .
WORKDIR "/src/Mensageria.Api"
RUN dotnet publish -c Release -o /app/publish

# Estágio 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expor a porta que a API vai usar
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Mensageria.Api.dll"]