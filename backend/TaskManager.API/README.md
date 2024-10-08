# Bem vindo ao projeto do backend do desafio técnico

## Tecnologias

 Esse projeto foi desenvolvido com as seguintes tecnologias:

 - [.NET Core](https://learn.microsoft.com/pt-br/dotnet/) 
 - [Entity Framework Core](https://learn.microsoft.com/pt-br/ef/core/)
 - [Fluent Validator](https://docs.fluentvalidation.net/en/latest/)
 - [Swagger](https://swagger.io/)
 - [Redis](https://redis.io/docs/latest/)
 - [xUnit](https://xunit.net/)
 - [Moq](https://github.com/devlooped/moq)
 - [AutoMapper](https://automapper.org/)


 ## Como rodar o projeto.
 Para conseguir executar esse projeto em sua máquina, é necessário seguir os seguintes passos

 1 - Criar uma instância do banco de dados, eu utilizei docker compose para subir um banco juntamente com o redis(existe cache nos use cases de projects), se não tiver Docker instalado em sua máquina, segue o link para instalação: https://docs.docker.com/engine/install/
 Após a instalação, entrar na pasta TaskManager.API (onde se encontra a solution) e rodar o seguinte comando

 ```bash
    docker compose up -d
```

 2 - Subir as tabelas do banco de dados rodando as migrations no .NET

 ```bash
    dotnet ef migrations add AddBancoDeDados --project TaskManager.Infrastructure --startup-project TaskManager.API
    dotnet ef database update --project TaskManager.Infrastructure --startup-project TaskManager.API
```

 3 - Após isso, na primeira execução do projeto, ele vai criar três usuários com a mesma senha (senha123), segue os logins dos usuários:
 - user1
 - user2
 - user3

 
 4 - Se desejar rodar os testes unitários do projeto, na pasta da solution digite no terminal:
 ```bash
    dotnet test
```


## Dúvidas
Se tiver alguma dúvida ou sugestão para esse projeto, basta entrar em contato comigo aqui no github, linked-in ou whatsapp.
