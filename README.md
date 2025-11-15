# MOSAICO+ Core API (.NET 8)

API RESTful desenvolvida em C# / .NET para o projeto **Global Solution – O Futuro do Trabalho (FIAP)**.

A **MOSAICO+ Core API** representa o núcleo backend de uma plataforma gamificada de trilhas de aprendizagem, missões e badges, onde usuários podem evoluir suas habilidades de forma contínua e rastreável.

---

## 🎯 Objetivo

Demonstrar uma solução tecnológica alinhada ao tema **“O Futuro do Trabalho”**, aplicando:

- Boas práticas REST (verbo correto + status code adequado);
- Versionamento da API por URL (`/api/v1/...`);
- Integração com banco de dados via **Entity Framework Core + SQL Server**;
- Documentação via **Swagger / OpenAPI**;
- Estrutura arquitetural clara para avaliação acadêmica.

---

## 🔗 Versionamento da API

A API utiliza **versionamento por URL**, na forma:

- Versão atual: `v1`
- Exemplo de rota:  
  - `GET /api/v1/users`
  - `GET /api/v1/tracks`

Essa estratégia permite evoluir a API no futuro (`/api/v2/...`) sem quebrar integrações existentes.  
Todas as rotas desta versão seguem o prefixo: `api/v1`.

---

## 🧱 Principais Recursos (v1)

### 👤 Users

Gerenciamento de usuários da plataforma.

- `GET /api/v1/users`  
- `GET /api/v1/users/{id}`  
- `POST /api/v1/users`  
- `PUT /api/v1/users/{id}`  
- `DELETE /api/v1/users/{id}`  

Uso de status codes:

- `200 OK` (consulta bem-sucedida)  
- `201 Created` (criação de usuário)  
- `204 NoContent` (atualização/remoção)  
- `400 BadRequest` (dados inválidos)  
- `404 NotFound` (usuário inexistente)

---

### 📚 Tracks

Trilhas de aprendizagem que representam jornadas de estudo.

- `GET /api/v1/tracks`  
- `GET /api/v1/tracks/{id}`  
- `POST /api/v1/tracks`  
- `PUT /api/v1/tracks/{id}`  
- `DELETE /api/v1/tracks/{id}`  

Cada trilha contém título, área, número total de aulas e horas estimadas.

---

### 📈 User Tracks (Progresso nas trilhas)

Consulta e atualização do progresso do usuário em uma trilha específica.

- `GET /api/v1/users/{userId}/tracks`  
  Lista o progresso do usuário em cada trilha.

- `POST /api/v1/users/{userId}/tracks/{trackId}/progress`  
  Atualiza o número de aulas concluídas e recalcula a porcentagem de progresso.

---

### 🎯 Missions & User Missions

Missões diárias/semanais que guiam o comportamento do usuário.

**Missões (CRUD):**

- `GET /api/v1/missions`  
- `GET /api/v1/missions/{id}`  
- `POST /api/v1/missions`  
- `PUT /api/v1/missions/{id}`  
- `DELETE /api/v1/missions/{id}`  

Cada missão possui título, descrição, tipo (`daily`/`weekly`) e XP de recompensa.

**Missões do usuário:**

- `GET /api/v1/users/{userId}/missions`  
  Lista as missões associadas ao usuário e se estão concluídas ou não.

- `POST /api/v1/users/{userId}/missions/{missionId}/complete`  
  Marca uma missão como concluída, registra data/hora e aplica **XP** no usuário.

---

### 🏅 Badges (conquistas do usuário)

Simulação da camada de recompensas/badges (futura integração com blockchain/metaverso).

- `GET /api/v1/users/{userId}/badges`  
  Lista todos os badges já conquistados pelo usuário.

- `POST /api/v1/users/{userId}/badges`  
  Cria um novo badge para o usuário (por exemplo, “Primeira trilha concluída”).

---

## 🗄️ Banco de Dados & Entity Framework Core

- Banco: **SQL Server (LocalDB ou Express)**  
- ORM: **Entity Framework Core**

### Entidades principais

- `User`  
- `Track`  
- `UserTrackProgress`  
- `Mission`  
- `UserMission`  
- `Badge`

### Migrations (EF Core)

Para criar/atualizar o banco (quando clonar o repositório):

```bash
dotnet ef database update
```

*(A migration `InitialCreate` já está incluída no projeto.)*

---

## 📚 Documentação da API (Swagger)

A documentação está disponível via **Swagger UI**.

Ao executar o projeto:

* Acesse:

```text
https://localhost:xxxx/swagger
```

(Porta conforme gerada na sua máquina.)

Pelo Swagger é possível:

* Inspecionar todos os endpoints;
* Enviar requisições HTTP de teste;
* Validar comportamento e status codes.

---

## 🧩 Arquitetura (Visão Geral)

A solução segue esta visão em camadas:

* **Cliente**: Swagger UI / Postman / (futuro app mobile – MOSAICO+).
* **API MOSAICO+**: Web API em .NET 8, com controllers REST, serviços de aplicação e EF Core.
* **Banco de Dados**: SQL Server, acessado via MosaicoContext.

Os diagramas arquiteturais em Mermaid estão descritos no arquivo `docs/arquitetura.md` e podem ser visualizados em qualquer editor compatível com Mermaid (ex.: VS Code + extensão ou mermaid.live).

---

## ▶️ Como executar localmente

1. Restaurar dependências:

```bash
dotnet restore
```

2. Atualizar o banco de dados:

```bash
dotnet ef database update
```

3. Executar a API:

```bash
dotnet run
```

4. Acessar o Swagger:

```text
https://localhost:xxxx/swagger
```

---

## 🎥 Vídeo de Demonstração

O vídeo (máximo 5 minutos) demonstra:

1. Contextualização rápida do tema **“O Futuro do Trabalho”** e da proposta MOSAICO+.
2. Arquitetura da API (diagrama).
3. Navegação pelo Swagger:

   * criação de usuário e trilha;
   * atualização de progresso em uma trilha;
   * criação e conclusão de missão;
   * concessão e listagem de badges.
4. Visualização dos dados no banco (SQL Server).

**Link do vídeo:** *[LINK DO YOUTUBE]*

---

## 👤 Autores

* Nikolas Rodrigues Moura dos Santos  – RM: 551566
* Thiago Jardim de Oliveira - RM: 551624
* Rodrigo Brasileiro - RM: 98952
