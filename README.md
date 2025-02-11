# ToDoList API

A **ToDoList API** é uma aplicação de gerenciamento de tarefas que permite aos usuários realizar operações de CRUD (Create, Read, Update, Delete) para tarefas e autenticação de usuários. A API foi construída utilizando o framework **.NET 9** com **SQL Server** para persistência de dados e **JWT** para autenticação.

## Funcionalidades

- **Autenticação JWT**: Protege os endpoints da API garantindo que apenas usuários autenticados possam acessar as funcionalidades.
- **Gestão de Tarefas**: Permite criar, atualizar, visualizar e excluir tarefas.
- **Criação de Usuários**: Permite registrar novos usuários e autenticar com credenciais válidas.

## Tecnologias Utilizadas

- **.NET 9** (ASP.NET Core)
- **SQL Server**
- **Entity Framework Core**
- **JWT (JSON Web Tokens)**
- **Swagger** para documentação da API
- **xUnit** para testes unitários

#### **Configuração do Banco de Dados**

No `appsettings.json`, configure a string de conexão para o SQL Server:

```json
"ConnectionStrings": {
   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SimpleBlogDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

Caso deseje utilizar uma instância do SQL Server diferente, altere o valor de `DefaultConnection`. Em seguida, crie o banco de dados e as tabelas com o comando:

```bash
dotnet ef database update
```

Se você estiver utilizando uma configuração personalizada de `User` (como no projeto), o `ApplicationDbContext` já estará configurado para usar o modelo de usuário extendido.

O projeto já inclui as migrações necessárias para o banco de dados de autenticação, então execute o seguinte para garantir que todas as migrações sejam aplicadas corretamente:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Estrutura do Projeto

A estrutura de diretórios do projeto é organizada da seguinte maneira:

```
Controllers/
  AuthController.cs        # Gerencia autenticação de usuários
  TasksController.cs       # Gerencia operações de tarefas
  UsersController.cs       # Gerencia usuários
Data/
  ApplicationDbContext.cs  # Contexto do banco de dados
Enums/
  TasksStatus.cs           # Enum para os status das tarefas
Helpers/
  PasswordHelper.cs        # Classe para hash de senhas
Migrations/
  20250209230413_InitialCreate.cs  # Scripts de migração do banco de dados
Models/
  LoginRequest.cs          # Model para login
  User.cs                  # Model para o usuário
  ToDoTask.cs              # Model para tarefa
Services/
  JwtService.cs            # Serviço para geração e validação de JWT
  PasswordHasher.cs        # Classe para hashing de senhas
  TaskService.cs           # Serviço para operações de tarefas
  UserService.cs           # Serviço para operações de usuários
Program.cs                 # Arquivo de configuração da aplicação
```
## Endpoints da API

A API possui os seguintes endpoints principais:

### Autenticação

- **POST /api/auth/login**: Realiza o login do usuário, retornando um token JWT.

### Usuários

- **POST api/users/register**: Cria um novo usuário.
- **POST api/users/login**: Realiza o login do usuário.

### Tarefas

- **GET /api/tasks**: Obtém todas as tarefas do usuário autenticado.
- **GET /api/tasks/{id}**: Obtém uma tarefa específica.
- **POST /api/tasks**: Cria uma nova tarefa.
- **PUT /api/tasks/{id}**: Atualiza uma tarefa existente.
- **DELETE /api/tasks/{id}**: Exclui uma tarefa.

## Testes

Os testes unitários foram implementados utilizando **xUnit**. Para rodar os testes, execute o comando:

```bash
   dotnet test
```

Os testes abrangem funcionalidades como autenticação, criação de usuários, criação de tarefas e operações CRUD sobre tarefas.


