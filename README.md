# Estacionamento

Sistema de gestão de estacionamento com registro de entrada e saída de veículos, cálculo automático de cobrança por tempo e persistência em banco de dados. Construído em .NET (net10.0) seguindo Clean Architecture.

Projeto pessoal desenvolvido para praticar arquitetura em camadas, EF Core, testes automatizados e boas práticas de API REST em C#.

## Arquitetura

O projeto é dividido em camadas, cada uma em seu próprio projeto, seguindo o fluxo de dependência `Api → Application → Domain` e `Infrastructure → Application/Domain`:

- **Estacionamento.Domain** — Entidades e regras de negócio puras (`Vaga`, `Veiculo`, `Ticket`, `TabelaPreco`), enums, interfaces de repositório e exceções de domínio. Não depende de nenhuma outra camada.
- **Estacionamento.Application** — Casos de uso (`EntradaService`, `SaidaService`, `CalculadoraPrecoService`), DTOs de request/response e as interfaces desses serviços.
- **Estacionamento.Infrastructure** — Acesso a dados via Entity Framework Core (SQL Server), implementação dos repositórios e migrations.
- **Estacionamento.Api** — Camada de apresentação: Controllers REST e configuração do Swagger.
- **Estacionamento.Tests** — Testes automatizados (xUnit + Moq).

## Status atual

✅ Fluxo completo de entrada e saída de veículos funcionando de ponta a ponta, com persistência real em SQL Server e testes automatizados cobrindo as principais regras de negócio.

### Funcionalidades implementadas

- Registro de entrada de veículo, com verificação de vaga disponível e bloqueio de entrada duplicada para a mesma placa
- Registro de saída de veículo, com cálculo automático do valor a pagar
- Consulta do resumo de vagas (total, livres, ocupadas)
- Cadastro automático de veículo na primeira vez que uma placa é vista pelo sistema
- Liberação e ocupação de vagas de forma consistente com o fluxo de entrada/saída
- Tratamento de erro com exceções específicas de domínio, mapeadas para os códigos HTTP corretos (404, 409, 400)

### Ainda não implementado

- Histórico de movimentações por placa/período
- Cadastro de tabela de preços via API (hoje é feito direto no banco)
- Autenticação/autorização
- Logging estruturado

## Regras de negócio

- **Entrada de veículo**: só é permitida se existir vaga livre compatível com o tipo do veículo, e se não houver outra entrada ativa para a mesma placa.
- **Cálculo do valor a pagar**:
  1. Existe uma tolerância (em minutos) sem cobrança.
  2. Após a tolerância, cobra-se por fração de tempo configurável (ex: a cada 15 minutos), sempre arredondando frações parciais para cima.
  3. Existe um valor de diária como teto máximo — se o cálculo por fração ultrapassar a diária, cobra-se a diária.
- **Saída de veículo**: só ocorre se existir um ticket ativo para o Id informado; ao finalizar, a vaga volta a ficar livre.

## Endpoints

### `POST /api/Entrada`

Registra a entrada de um veículo.

**Request**
```json
{
  "placa": "ABC1234",
  "tipoVeiculo": 1,
  "modelo": "Civic",
  "cor": "Prata"
}
```

**Response — 201 Created**
```json
{
  "ticketId": 1,
  "placa": "ABC1234",
  "vagaCodigo": "A01",
  "dataHoraEntrada": "2026-08-04T23:47:03.5651166",
  "status": "Ativo"
}
```

**Possíveis erros**: `409 Conflict` (placa já possui entrada ativa, ou não há vaga disponível), `400 Bad Request` (dados inválidos, ex: placa vazia).

`tipoVeiculo` segue o enum: `1 = Carro`, `2 = Moto`, `3 = Caminhonete`.

### `POST /api/Saida`

Registra a saída de um veículo e calcula o valor a pagar.

**Request**
```json
{
  "ticketId": 1
}
```

**Response — 200 OK**
```json
{
  "ticketId": 1,
  "placa": "ABC1234",
  "dataHoraEntrada": "2026-08-04T23:47:03.5651166",
  "dataHoraSaida": "2026-08-05T01:12:06.9901433",
  "tempoPermanencia": "01:25:03.4250267",
  "valorCobrado": 15.00
}
```

**Possíveis erros**: `404 Not Found` (ticket, vaga, veículo ou tabela de preço não encontrados), `409 Conflict` (ticket já finalizado).

### `GET /api/Vaga`

Retorna o resumo de ocupação das vagas.

**Response — 200 OK**
```json
{
  "totalVagas": 1,
  "vagasLivres": 1,
  "vagasOcupadas": 0
}
```

## Testes

O projeto conta com 13 testes unitários (xUnit + Moq) cobrindo:

- **RN01** — bloqueio de entrada duplicada para a mesma placa (`EntradaServiceTests`)
- **RN02** — cálculo de valor: dentro da tolerância, por fração (com `[Theory]`/`[InlineData]` cobrindo múltiplos cenários) e com teto de diária (`CalculadoraPrecoServiceTests`)
- **RN03** — bloqueio de entrada quando não há vaga disponível (`EntradaServiceTests`)
- **Fluxo de saída** — todos os cenários de erro (ticket/vaga/veículo/tabela de preço não encontrados, ticket já finalizado) e o caminho de sucesso completo, incluindo verificação dos efeitos colaterais nas entidades (`SaidaServiceTests`)

Para rodar os testes:

```bash
dotnet test
```

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
```

Aplique as migrations para criar o banco de dados:

```bash
dotnet ef database update --project Estacionamento.Infrastructure --startup-project Estacionamento.Api
```

Rode a API:

```bash
dotnet run --project Estacionamento.Api
```

O Swagger estará disponível em `https://localhost:{porta}/swagger`.

> **Nota**: para testar o fluxo de saída, é necessário cadastrar ao menos uma linha na tabela `TabelasPreco` diretamente no banco, já que o cadastro via API ainda não foi implementado.

## Tecnologias

- C# / .NET 10
- ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- xUnit + Moq
- Swagger / Swashbuckle

## Padrões aplicados

- Clean Architecture (separação em camadas)
- Repository Pattern
- Dependency Injection
- Strategy Pattern (cálculo de preço isolado em `ICalculadoraPrecoService`)
- DTOs para isolar contratos de API das entidades de domínio
