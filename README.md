# Projeto de Autenticação e Registro de Usuários com ASP.NET Core

Este projeto é uma aplicação ASP.NET Core que implementa um sistema de registro e login de usuários utilizando **ASP.NET Identity** e **JWT (JSON Web Tokens)** para autenticação. A aplicação segue princípios de clean code, separando a lógica de negócio em serviços (`UserService` e `TokenService`), e é projetada com um design **mobile-first** na interface.

## Funcionalidades

- **Registro de Usuário**: Novo usuário pode se registrar utilizando e-mail e senha.
- **Login de Usuário**: Usuário pode efetuar login e receber um token JWT.
- **Bloqueio de Usuário**: Após cinco tentativas de login incorretas, o usuário é temporariamente bloqueado por 15 minutos.
- **JWT em Cookies**: O token JWT é armazenado em cookies para garantir autenticação nas próximas requisições.

## Tecnologias Utilizadas

- **ASP.NET Core 6 MVC**: Framework de desenvolvimento web.
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
   ```bash
   git clone https://github.com/vinipomp02/UsuarioLoginApiWeb.git no repositório de preferência
   ```
   Para acessar o diretório que deseja clonar, deve usar o comando:
   ```bash
   cd pasta/onde/deseja/clonar
   ```
   (O Git deve ser instalado para que possa ser usado os comandos git.  
   Link de Download do Git: [https://git-scm.com/downloads](https://git-scm.com/downloads))

2. Navegue até a pasta do projeto executável:
   ```bash
   cd pasta/onde/deseja/clonar/UsuarioApiWeb/UsuarioApiWeb
   ```

3. Restaure os pacotes:
   Digite no CMD:
   ```bash
   dotnet restore
   ```

4. Execute a aplicação:
   Digite no CMD:
   ```bash
   dotnet run
   ```

5. Abra o localhost no seu navegador:
   - [https://localhost:7140](https://localhost:7140) ou
   - [http://localhost:5140](http://localhost:5140)

6. (Opcional) Via Menu ou Botão de "Ainda não tem um cadastro?", acesse a tela de cadastro e crie um novo usuário.

7. Acesse a Tela de Login via:
   Utilize um dos usuários pré-existentes, carregados ao iniciar o projeto, ou um usuário recém-criado via tela de cadastro.

8. Faça o Logout.

9. Para encerrar a aplicação, volte ao CMD e insira o comando:
   ```bash
   Ctrl + C
   ```

### Melhoria Futuras Possíveis
- **Integração com banco de dados.**
- **Implementação de recuperação de senha via e-mail.**
- **Auditoria de acessos e gerenciamento avançado de usuários.**


### Considerações Finais sobre o Projeto

A principal premissa deste desafio foi o tempo disponível para desenvolvimento. Dentre as opções de modelos que considerei, optei por seguir com ASP.NET CORE MVC devido ao meu maior domínio nessa tecnologia e arqitetura.

Por se tratar de uma aplicação simples e visando garantir a fácil execução do projeto em qualquer computador, decidi não implementar uma integração com banco de dados nesta primeira versão.

Outra premissa importante durante meu desenvolvimento foi a manutenção de um código limpo. Para isso, utilizei o Bootstrap, que acredito que facilite a manutenção e o design da aplicação.

Identifiquei uma oportunidade de melhoria nas funcionalidades e incluí a criação de cadastro de novos usuários. Isso tornará a realização de diversos testes mais interessantes e dinâmicos.
