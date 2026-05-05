# Fiap Banco API

API desenvolvida para simular operações bancárias, incluindo gerenciamento de clientes, agências e contratações. O projeto segue boas práticas de arquitetura em camadas, testes automatizados e separação de responsabilidades.

---

## Objetivo

O objetivo deste projeto é fornecer uma base sólida para operações bancárias simples, permitindo:

- Cadastro e gerenciamento de clientes
- Controle de agências
- Processamento de contratações
- Testes automatizados das regras de negócio

---

## Estrutura do Projeto

| Projeto | Descrição |
|---|---|
| `Fiap.Banco.API` | Projeto principal contendo os endpoints da API, controllers e configurações |
| `Fiap.Banco.API.Tests` | Projeto de testes automatizados, garantindo a integridade das regras de negócio |

---

## Principais Componentes

**Controllers** — Responsáveis por expor os endpoints da API:
- `AgenciasController`
- `ClientesController`
- `ContratacoesController`

**DTOs (Data Transfer Objects)** — Utilizados para entrada e saída de dados da API.

**Services** — Contêm as regras de negócio da aplicação.

**TestHelpers** — Utilitários para facilitar a criação de testes.

---

## Tecnologias Utilizadas

- .NET (ASP.NET Core)
- C#
- Entity Framework (ou similar, dependendo da implementação)
- xUnit (ou framework de testes equivalente)

---

## Como Executar o Projeto

### Pre-requisitos

- .NET SDK instalado
- IDE recomendada: Visual Studio ou VS Code

### Passos

**1. Clonar o repositório:**
```bash
git clone 
```

**2. Acessar a pasta do projeto:**
```bash
cd Fiap.Banco
```

**3. Restaurar dependências:**
```bash
dotnet restore
```

**4. Executar a aplicação:**
```bash
dotnet run --project Fiap.Banco.API
```

**5. Acessar a API via Swagger:**
```
https://localhost:<porta>/swagger
```

---

## Testes

Para executar os testes automatizados:

```bash
dotnet test
```

Os testes cobrem principalmente:

- Regras de negócio de clientes
- Processos de contratação
- Comportamentos esperados dos serviços

---

## Endpoints Principais

**Clientes**
- Cadastro de cliente
- Consulta de cliente
- Atualização de dados

**Agências**
- Cadastro de agência
- Listagem de agências

**Contratações**
- Criação de contratação
- Consulta de contratos

---

## Diagrama de Arquitetura

<!-- Exporte o diagrama do draw.io como PNG ou SVG, adicione na pasta /assets e substitua a linha abaixo -->

![Diagrama de Arquitetura](./assets/diagrama.png)

---

## Integrantes

| RM 556892 | Guilherme Lunghini Teixeiram |
| RM 557538 | David Alexandre Cordeiro |
| RM 99856  | Marchel Augusto Ribeiro |

---

## Boas Práticas Aplicadas

- Separação em camadas (Controller, Service, DTO)
- Testes automatizados
- Organização modular do código
- Reutilização de componentes
- Clareza na definição de responsabilidades

---

## Possiveis Melhorias

- [ ] Implementação de autenticação e autorização
- [ ] Integração com banco de dados real
- [ ] Logs estruturados
- [ ] Monitoramento e métricas
- [ ] Containerização com Docker
