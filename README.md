# SmartPortaria

SmartPortaria é uma aplicação ASP.NET Core MVC voltada para o controle de acesso em condomínios e empresas. O sistema permite cadastrar administradores, gerenciar usuários com suporte a reconhecimento facial e controlar sessões de login por meio de cookies.

## Pré-requisitos

- .NET SDK 10.0 ou superior
- Instância do SQL Server

## Configuração

1. Clone este repositório e restaure as dependências.
2. Defina a string de conexão do banco em `appsettings.json` no caminho `ConnectionStrings:DefaultConnection` ou forneça-a via variável de ambiente `ConnectionStrings__DefaultConnection`.
3. Opcionalmente ajuste `ASPNETCORE_ENVIRONMENT` para `Development` ou `Production`.

## Executando a aplicação

No diretório do projeto execute:

```bash
# compilação da solução
 dotnet build SmartPortaria.sln

# execução da aplicação web
 dotnet run --project SmartPortaria.csproj
```

Após o início o site fica acessível em `https://localhost:5001` (a porta pode variar).

## Executando os testes unitários

Para rodar os testes utilize:

```bash
dotnet test SmartPortaria.sln
```

## Funcionalidades principais

- Cadastro e autenticação de administradores
- Gerenciamento de usuários com armazenamento de dados faciais
- Reconhecimento facial para identificação rápida
- Autenticação baseada em cookie e controle de sessão

## Arquitetura

O projeto segue a arquitetura em camadas, separando responsabilidades em:

- **Controllers**: expõem os endpoints MVC.
- **Serviços de aplicação**: regras de negócio e orquestração.
- **Repositórios**: acesso ao banco de dados via Entity Framework.
- **Camada de domínio**: entidades e contratos que definem o modelo de dados (não incluída integralmente neste repositório).

Essa estrutura facilita a manutenção e a evolução do código.

## Variáveis de ambiente

- `ConnectionStrings__DefaultConnection` – sobrescreve a string de conexão do banco
- `ASPNETCORE_ENVIRONMENT` – define o ambiente de execução (`Development`, `Staging`, `Production`)
