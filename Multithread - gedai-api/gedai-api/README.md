# GEDAI Solution

## Visão Geral

Este repositório contém a solução GEDAI, desenvolvida em .NET 8, utilizando a linguagem C#. O projeto segue uma arquitetura em camadas, separando responsabilidades entre domínio, aplicação, infraestrutura e APIs, visando escalabilidade, manutenção e testabilidade.

## Tecnologias Utilizadas

- **.NET 8**
- **C#**
- **ASP.NET Core** (para APIs)
- **Entity Framework Core** (para persistência de dados)
- **Visual Studio 2022** (ambiente de desenvolvimento)

## Arquitetura

A solução está organizada nos seguintes projetos:

- **GEDAI.Domain**: Contém as entidades de domínio e regras de negócio.
- **GEDAI.Application**: Implementa os casos de uso e orquestra as operações do domínio.
- **GEDAI.Infrastructure**: Responsável pela persistência de dados e integrações externas.
- **GEDAI.Api / GEDAI.Authorization.Api**: Exposição de endpoints REST para consumo externo.

A arquitetura em camadas permite desacoplamento entre regras de negócio, lógica de aplicação e infraestrutura, facilitando testes e evolução do sistema.

## Fluxo de Documentos e Utilização de Threads

O processamento de documentos é um dos pontos centrais do sistema. Para garantir performance e escalabilidade, foram implementados métodos que utilizam **threads** (paralelismo) e métodos tradicionais (sequenciais).

### Métodos Sem Threads

- **Execução sequencial**: Cada documento é processado um após o outro.
- **Vantagens**: Simplicidade, fácil depuração.
- **Desvantagens**: Baixo desempenho em cenários de grande volume de documentos.

### Métodos Com Threads

- **Execução paralela**: Utiliza recursos como `Task`, `Parallel.ForEach` ou `Thread` para processar múltiplos documentos simultaneamente.
- **Vantagens**: Aumento significativo de performance, melhor uso de recursos do servidor.
- **Desvantagens**: Maior complexidade, necessidade de tratar concorrência e sincronização.

#### Exemplo de Fluxo com Threads

1. Os documentos são carregados em memória.
2. Cada documento é atribuído a uma thread/tarefa para processamento paralelo.
3. O sistema aguarda a conclusão de todas as tarefas antes de prosseguir para a próxima etapa.

#### Exemplo de Fluxo Sem Threads

1. Os documentos são carregados em memória.
2. Cada documento é processado individualmente, de forma sequencial.
3. O sistema só avança após o término do processamento de cada documento.

## Como Executar

1. Clone o repositório.
2. Abra a solution no Visual Studio 2022.
3. Restaure os pacotes NuGet.
4. Defina o projeto de inicialização desejado (ex: `GEDAI.Api`).
5. Execute a aplicação (`F5`).

---

> Para dúvidas técnicas, consulte a documentação interna de cada projeto ou abra uma issue.
