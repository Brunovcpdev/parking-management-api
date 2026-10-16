# Estacionamento

Sistema de gestão de estacionamento com registro de entrada e saída de veículos, cálculo automático de cobrança por tempo e persistência em banco de dados. Construído em .NET (net10.0) seguindo Clean Architecture.

Projeto pessoal desenvolvido para praticar arquitetura em camadas, EF Core, testes automatizados e boas práticas de API REST em C#.

## Arquitetura

O projeto é dividido em camadas, cada uma em seu próprio projeto, seguindo o fluxo de dependência `Api → Application → Domain` e `Infrastructure → Application/Domain`:

- **Estacionamento.Domain** — Entidades e regras de negócio puras (`Vaga`, `Veiculo`, `Ticket`, `TabelaPreco`, `Usuario`), enums, interfaces de repositório e exceções de domínio. Não depende de nenhuma outra camada.
- **Estacionamento.Application** — Casos de uso (`EntradaService`, `SaidaService`, `CalculadoraPrecoService`, `AutenticacaoService`), DTOs de request/response e as interfaces desses serviços.
- **Estacionamento.Infrastructure** — Acesso a dados via Entity Framework Core (SQL Server), implementação dos repositórios e migrations.
- **Estacionamento.Api** — Camada de apresentação: Controllers REST, autenticação JWT e configuração do Swagger.
- **Estacionamento.Tests** — Testes automatizados (xUnit + Moq).

## Status atual

✅ Fluxo completo de entrada e saída de veículos funcionando de ponta a ponta, com persistência real em SQL Server e testes automatizados cobrindo as principais regras de negócio.

### Funcionalidades implementadas

- Registro de entrada de veículo, com verificação de vaga disponível e bloqueio de entrada duplicada para a mesma placa
- Registro de saída de veículo, com cálculo automático do valor a pagar
- Consulta do resumo de vagas (total, livres, ocupadas)
- Cadastro automático de veículo na primeira vez que uma placa é vista pelo sistema
- Liberação e ocupação de vagas de forma consistente com o fluxo de entrada/saída
- Tratamento de erro com exceções específicas de domínio, mapeadas para os códigos HTTP corretos (404, 409, 400, 401, 403)
- Autenticação com JWT e autorização por perfis (`Operador` e `Admin`)
- Senhas armazenadas como hash BCrypt (nunca em texto puro)
- Criação automática do primeiro usuário Admin (seed idempotente) a partir de configuração segura

### Ainda não implementado

- Histórico de movimentações por placa/período
- Cadastro de tabela de preços via API (hoje é feito direto no banco)
- Troca de senha e refresh token
- Logging estruturado
- Testes unitários do `AutenticacaoService`

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

## Autenticação e autorização

A API usa **JWT Bearer**. Todos os endpoints, exceto o login, exigem um token válido.

| Endpoint | Sem token | Operador | Admin |
|---|---|---|---|
| `POST /api/Autenticacao/login` | ✅ | ✅ | ✅ |
| `POST /api/Autenticacao/registrar` | 401 | 403 | ✅ |
| `POST /api/Entrada`, `POST /api/Saida`, `GET /api/Vaga` | 401 | ✅ | ✅ |

Só um Admin pode criar novos usuários; não existe cadastro público.

### `POST /api/Autenticacao/login`

**Request**
```json
{
  "login": "seu-login",
  "senha": "sua-senha"
}
```

**Response — 200 OK**
```json
{
  "token": "eyJhbGciOi...",
  "login": "seu-login",
  "role": "Admin",
  "expiraEm": "2026-10-16T15:00:00Z"
}
```

**Possíveis erros**: `401 Unauthorized` (login ou senha inválidos — a mensagem é a mesma nos dois casos, para não revelar quais logins existem).

### `POST /api/Autenticacao/registrar` (somente Admin)

**Request**
```json
{
  "login": "operador1",
  "senha": "uma-senha-forte",
  "role": 1
}
```

`role` segue o enum: `1 = Operador`, `2 = Admin`.

**Resposta**: `201 Created`. **Possíveis erros**: `409 Conflict` (login já existe), `400 Bad Request` (dados inválidos), `401` (sem token ou token expirado), `403 Forbidden` (usuário autenticado não é Admin).

### Detalhes de segurança

- Senhas com **BCrypt** (salt embutido no hash).
- Token assinado com HMAC-SHA256, com validação de `Issuer`, `Audience`, assinatura e expiração (60 minutos por padrão).
- Logins são normalizados (`trim` + minúsculas) antes de salvar e consultar.
- O payload do JWT é apenas codificado, não criptografado: nunca coloque dados sensíveis nele.

### Usando o token no Swagger

1. Faça `POST /api/Autenticacao/login` e copie o valor de `token`.
2. Clique em **Authorize** (cadeado, no topo da página).
3. Cole **apenas o token**, sem a palavra `Bearer`, e confirme.

## Testes

O projeto conta com 13 testes unitários (xUnit + Moq) cobrindo as regras de negócio abaixo. O `AutenticacaoService` ainda **não** tem testes:

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

### appsettings.json (sem segredos)

Em `Estacionamento.Api/appsettings.json` ficam apenas valores **não sensíveis**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "sua-connection-string-aqui"
  },
  "Jwt": {
    "Issuer": "EstacionamentoApi",
    "Audience": "EstacionamentoApiUsuarios",
    "ExpiraMinutos": 60
  }
}
```

### Segredos (obrigatório)

A chave de assinatura do JWT e as credenciais do primeiro Admin **não estão no repositório** e precisam ser configuradas por quem clonar o projeto. Em desenvolvimento, use o user-secrets:

```bash
dotnet user-secrets set "Jwt:Key" "<chave-aleatoria-com-no-minimo-32-caracteres>" --project Estacionamento.Api
dotnet user-secrets set "AdminInicial:Login" "<login-do-admin>" --project Estacionamento.Api
dotnet user-secrets set "AdminInicial:Senha" "<senha-do-admin>" --project Estacionamento.Api
```

- Sem `Jwt:Key`, a API não consegue gerar nem validar tokens.
- Sem `AdminInicial:Login`/`AdminInicial:Senha`, o seed é ignorado (com um warning no log) e não haverá nenhum usuário para fazer login. Não existe senha padrão, de propósito.
- No startup, a API cria o Admin se ele ainda não existir. Se já existir, nada acontece (idempotente).
- O user-secrets **não é criptografado** e serve só para desenvolvimento. Em produção, use variáveis de ambiente (`Jwt__Key`, `AdminInicial__Login`, ...) ou um cofre de segredos.
- Ordem de precedência: `appsettings.json` < user-secrets < variáveis de ambiente.

> **Nunca** commite chaves, senhas ou tokens. O repositório é público.

## Como rodar

```bash
dotnet restore
dotnet build
```

Aplique as migrations para criar o banco de dados:

```bash
dotnet ef database update --project Estacionamento.Infrastructure --startup-project Estacionamento.Api
```

Configure os segredos (seção acima) e rode a API:

```bash
dotnet run --project Estacionamento.Api
```

O Swagger estará disponível em `https://localhost:{porta}/swagger`.

> **Nota 1**: para testar o fluxo de saída, é necessário cadastrar ao menos uma linha na tabela `TabelasPreco` diretamente no banco, já que o cadastro via API ainda não foi implementado. Sem ela, `POST /api/Saida` retorna `404`.
>
> **Nota 2**: para testar pelo Swagger, faça login com o Admin do seed e use o botão **Authorize**.

## Tecnologias

- C# / .NET 10
- ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- xUnit + Moq
- JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- BCrypt.Net-Next (hash de senhas)
- Swagger / Swashbuckle

## Padrões aplicados

- Clean Architecture (separação em camadas)
- Repository Pattern
- Dependency Injection
- Strategy Pattern (cálculo de preço isolado em `ICalculadoraPrecoService`)
- DTOs para isolar contratos de API das entidades de domínio
- Autorização baseada em roles (`[Authorize(Roles = "Admin")]`)
- Seed idempotente de dados iniciais
