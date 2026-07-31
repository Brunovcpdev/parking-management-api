# Estacionamento

Sistema de gestão de estacionamento com registro de entrada e saída de veículos, cobrança por tempo e persistência em banco de dados. Construído em .NET (net10.0) seguindo Clean Architecture.

## Arquitetura

O projeto é dividido em camadas, cada uma em seu próprio projeto:

- **Estacionamento.Domain** — Entidades e regras de negócio puras (`Vaga`, `Veiculo`, `Ticket`, `TabelaPreco`).
- **Estacionamento.Application** — Casos de uso e serviços de aplicação (ex: `EntradaService`).
- **Estacionamento.Infrastructure** — Acesso a dados via Entity Framework Core (SQL Server) e repositórios.
- **Estacionamento.Api** — Camada de apresentação (Web API, Swagger).
- **Estacionamento.Tests** — Testes automatizados.

## Status atual

🚧 Projeto em desenvolvimento inicial. A camada de domínio, aplicação e infraestrutura já possuem entidades, serviços e repositórios implementados. A camada de API ainda não expõe controllers/endpoints HTTP.

## Requisitos

- .NET SDK 10.0
- SQL Server (local ou remoto)

## Configuração

Antes de rodar o projeto, configure a connection string em `Estacionamento.Api/appsettings.json` (ou preferencialmente via `appsettings.Development.json` / variáveis de ambiente / user-secrets, que não vão para o repositório):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "sua-connection-string-aqui"
  }
}
```

## Como rodar

```bash
dotnet restore
dotnet build
dotnet run --project Estacionamento.Api
```

## Testes

```bash
dotnet test
```
