
# SQLBulkInsert

## Descrição
Este projeto `SQLBulkInsert` demonstra uma aplicação que realiza inserções em massa em um banco de dados SQL Server a partir de um arquivo de texto, utilizando Docker para facilitar a configuração e execução do ambiente de banco de dados.

## Pré-requisitos
- Docker
- .NET 8 SDK
- SQL Server Management Studio (opcional, para visualização de dados)

## Configuração do Ambiente

### Configuração do Docker

1. **Iniciar o container do SQL Server:**
   Utilize o Docker Compose para iniciar um container do SQL Server com as configurações necessárias. No diretório do projeto, execute:

   ```bash
   docker-compose up -d
   ```

   Isso irá configurar e iniciar o SQL Server no Docker, expondo a porta `1433` para conexões locais.

### Estrutura do Banco de Dados

Após iniciar o SQL Server, a aplicação configurará automaticamente as tabelas necessárias no banco de dados `SQLBulkInsert`.

As tabelas incluem:
- `Users`: Armazena informações dos usuários.
- `InvalidLogs`: Registra logs de dados inválidos encontrados durante a importação.

### Execução do Projeto

1. **Executar a Aplicação:**
   No Visual Studio ou usando a CLI do .NET, execute o comando a seguir para iniciar a aplicação:

   ```bash
   dotnet run
   ```

   Isso irá processar o arquivo `User.txt`, realizar inserções em massa na tabela `Users` e registrar quaisquer dados inválidos na tabela `InvalidLogs`.

## Funcionalidades

- **Leitura de Dados**: Lê dados de um arquivo TXT.
- **Validação de Dados**: Verifica a validade das datas de nascimento e completa a flag `IsComplete`.
- **Logging de Erros**: Registra erros de validação em uma tabela separada.
- **Inserção em Massa**: Utiliza `SqlBulkCopy` para efetuar inserções em massa no banco de dados.

## Comandos Úteis

- **Encerrar o Container do Docker**:
  Se precisar encerrar o container do Docker, utilize:

  ```bash
  docker-compose down
  ```

- **Reconstruir e Iniciar o Container**:
  Para reconstruir e iniciar o container após realizar mudanças, use:

  ```bash
  docker-compose up --build
  ```

## 📫 Como me encontrar
[![YouTube](https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://www.youtube.com/channel/UCjy19AugQHIhyE0Nv558jcQ)
[![Linkedin Badge](https://img.shields.io/badge/-Guilherme_Figueiras_Maurila-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/guilherme-maurila)](https://www.linkedin.com/in/guilherme-maurila)
[![Gmail Badge](https://img.shields.io/badge/-gfmaurila@gmail.com-c14438?style=flat-square&logo=Gmail&logoColor=white&link=mailto:gfmaurila@gmail.com)](mailto:gfmaurila@gmail.com)
