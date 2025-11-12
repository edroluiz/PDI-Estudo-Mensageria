# Estágio 1: Build
# ----- MUDE A LINHA ABAIXO -----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia todos os arquivos .csproj e restaura as dependências
COPY Mensageria.sln .
COPY Mensageria.Consumer/Mensageria.Consumer.csproj Mensageria.Consumer/
COPY Mensageria.Application/Mensageria.Application.csproj Mensageria.Application/
COPY Mensageria.Domain/Mensageria.Domain.csproj Mensageria.Domain/
COPY Mensageria.Infrastructure/Mensageria.Infrastructure.csproj Mensageria.Infrastructure/
COPY Mensageria.Api/Mensageria.Api.csproj Mensageria.Api/
RUN dotnet restore Mensageria.sln

# Copia o resto do código fonte e faz o build
COPY . .
WORKDIR "/src/Mensageria.Consumer"
RUN dotnet publish -c Release -o /app/publish

# Estágio 2: Runtime
# ----- MUDE A LINHA ABAIXO -----
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Mensageria.Consumer.dll"]