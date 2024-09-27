# Projeto de Autenticação e Registro de Usuários com ASP.NET Core

Este projeto é uma aplicação ASP.NET Core que implementa um sistema de registro e login de usuários utilizando **ASP.NET Identity** e **JWT (JSON Web Tokens)** para autenticação. A aplicação segue princípios de arquitetura limpa, separando a lógica de negócio em serviços (`UserService` e `TokenService`), e é projetada com um design **mobile-first** na interface.

## Funcionalidades

- **Registro de Usuário**: Novo usuário pode se registrar utilizando e-mail e senha.
- **Login de Usuário**: Usuário pode efetuar login e receber um token JWT.
- **Bloqueio de Usuário**: Após cinco tentativas de login incorretas, o usuário é temporariamente bloqueado por 15 minutos.
- **JWT em Cookies**: O token JWT é armazenado em cookies para garantir autenticação nas próximas requisições.

## Tecnologias Utilizadas

- **ASP.NET Core 6 MVC**: Framework para construir aplicações web modernas.
- **ASP.NET Identity**: Sistema de autenticação e gerenciamento de usuários.
- **JWT**: Para autenticação segura via token.
- **InMemoryStore**: Armazenamento temporário de dados durante o desenvolvimento (sem uso de banco de dados persistente).
- **Razor Pages e Bootstrap**: Para criação de views e design responsivo.

## Estrutura do Projeto

- **Controllers/UserController.cs**: Controlador responsável pelas ações de login e registro de usuários.
- **Services/UserService.cs**: Lógica de negócio que lida com autenticação e manipulação de usuários.
- **Services/TokenService.cs**: Responsável pela geração e validação de tokens JWT.
- **Models/UserModel.cs**: Modelo de dados para usuários, herdando de `IdentityUser`.

## Funcionalidades Detalhadas

### Registro de Usuário

O método `Register` no `UserController` verifica se o e-mail já existe e, caso não exista, cria um novo usuário com as seguintes etapas:
1. Validação do modelo.
2. Verificação se o usuário já está registrado.
3. Registro do novo usuário.
4. Redirecionamento para a página de login.

### Login de Usuário

O método `Login` no `UserController` realiza as seguintes etapas:
1. Validação do modelo.
2. Verificação se o usuário está bloqueado.
3. Tentativa de login via `SignInManager`.
4. Geração de um token JWT em caso de sucesso e armazenamento no cookie.
5. Exibição de mensagem de erro e contagem de tentativas restantes em caso de falha.

### Layout Mobile-First

O projeto utiliza Bootstrap para garantir que as páginas sejam responsivas e otimizadas para dispositivos móveis. O formulário é estruturado para adaptar-se em telas pequenas e grandes.

### Segurança

- **JWT**: Os tokens são gerados e armazenados nos cookies com as opções `HttpOnly` e `Secure` (para HTTPS), garantindo segurança contra acessos indevidos via JavaScript.
- **Bloqueio de Conta**: Após um número determinado de tentativas de login incorretas, a conta do usuário é bloqueada temporariamente.

## Como Executar o Projeto

1. Clone o repositório via Prompt de comando:
   git clone https://github.com/vinipomp02/UsuarioLoginApiWeb.git no repositorio de preferencia
    para acessar o diretorio que deseja clonar deve usar o comando cd pasta/onde/deseja/clonar

1. 1. o Git deve ser instalado para que possa ser usado os comandos git
Link de Download do git: https://git-scm.com/downloads

2. Navegue até a pasta do projeto executavel: 
cd pasta/onde/deseja/clonar/UsuarioApiWeb/UsuarioApiWeb

3. Restaure os pacotes:
digite no CMD dotnet restore

4. Execute a aplicação:
digite no CMD dotnet run

5. Abra o localhost no seu navegador
https://localhost:7140 ou http://localhost:5140

6. (Opcional) Via Menu ou Botão de "Ainda não tem um cadastro?" acesse a tela de cadastro
Crie um novo usuário

7. Acesse a Tela de Login via
Utilize um dos usúarios pré existentes, carregados ao iniciar o projeto ou um úsuario recém-criado via tela de cadastro

8. Faça o Logout

9. Para encerrar a aplicação volte ao Cmd e Insira o comando Ctrl + C

### Melhoria Futuras Possíveis
- **Integração com banco de dados.**
- **Implementação de recuperação de senha via e-mail.**
- **Auditoria de acessos e gerenciamento avançado de usuários.**


### Considerações finais sobre o projeto
1. Decide seguir no modelo ASP.NET CORE MVC por ter mais conforto
2. Não fiz integração via banco de dados, por se tratar de uma aplicação simples e para facilitar no momento de executar o projeto em outras máquinas
3. Utilzei Bootstrap para manter o código mais clean e de facil manutenção
4. Criei uma feature a mais para cadastro de dados, para facilitar o uso e ter mais opções de testes caso seja necessário

