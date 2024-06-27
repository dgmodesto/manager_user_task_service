
#Imagem de execu��o
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80


#Imagem para build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

#Nome do projeto
ARG COMPOSE_PROJECT_NAME

#Define o diret�rio de trabalho
WORKDIR "/project"

#Copia tudo para pasta do workdir, faz o restore e copia os pacotes
COPY . .

# Instala o pacote do Entity Framework Core globalmente
RUN dotnet tool install --global dotnet-ef --version 7.0.0

# Adiciona o caminho do pacote global no PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet restore "ManagerUserTaskApi.sln"
COPY . .

# Adiciona os comandos EF migrations diretamente no Dockerfile
RUN dotnet ef migrations add initial-migrations-docker -s src/ManagerUserTaskApi.Api -p src/ManagerUserTaskApi.Infrastructure -o Database/Migrations/Domain -c ManagerUserTaskApiDbContext -v
RUN dotnet ef migrations add initial-migrations-docker -s src/ManagerUserTaskApi.Api -p src/ManagerUserTaskApi.Infrastructure -o Database/Migrations/EventStore -c EventStoreDbContext -v


#Define o diret�rio de trabalho sobre o projeto de API
WORKDIR "/project/src/ManagerUserTaskApi.Api"
RUN dotnet build "ManagerUserTaskApi.Api.csproj" -c Release -o /app

#Pela imagem de build, gera o deploy
FROM build AS publish
RUN dotnet publish "ManagerUserTaskApi.Api.csproj" -c Release -o /app

#Copia o deploy para imagem base
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80
ENTRYPOINT ["dotnet", "ManagerUserTaskApi.Api.dll"]
