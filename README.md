# PDI - Estudo de Mensageria com RabbitMQ e .NET

Este repositório contém o projeto desenvolvido como parte do meu Plano de Desenvolvimento Individual (PDI) focado em aprofundar conhecimentos em Mensageria e Arquitetura de Software.

## 1. Contexto e Motivação

O objetivo principal deste PDI foi solidificar meu entendimento sobre sistemas de mensageria desde sua concepção inicial. Ao ingressar na equipe atual, a infraestrutura de mensageria já estava implementada e em pleno funcionamento. Embora isso tenha sido ótimo para a produtividade do time, eu senti a necessidade de construir um sistema do zero para entender as fundações, as decisões de design e os desafios práticos da implementação.

Este projeto foi a oportunidade de simular esse cenário, aplicando os padrões de alta qualidade que utilizamos no dia a dia.

## 2. Arquitetura e Padrões Adotados

Para tornar o estudo o mais próximo possível de um cenário de produção real, o projeto foi estruturado seguindo os mesmos padrões de arquitetura utilizados pela nossa equipe:

* **Clean Architecture:** A solução é dividida em camadas de responsabilidade claras (Domain, Application, Infrastructure, Api), garantindo o desacoplamento e a testabilidade.
* **CQRS (Command Query Responsibility Segregation):** Embora de forma simplificada, o projeto separa as responsabilidades de escrita (o `Producer` na API) e leitura (o `Consumer` no Worker), permitindo que escalem de forma independente.
* **Producer (API):** Um projeto `ASP.NET Core API` atua como o Produtor, expondo um endpoint REST para receber dados e publicar mensagens.
* **Consumer (Worker Service):** Um `Worker Service` do .NET atua como o Consumidor, rodando em background para escutar, receber e processar as mensagens da fila.

## 3. Tecnologias Utilizadas

* .NET 8 (ou 9)
* ASP.NET Core (para a API/Producer)
* Worker Service (para o Consumer)
* RabbitMQ v7.x (como Message Broker)
* Docker (para rodar a instância do RabbitMQ)
* Swashbuckle (Swagger) (para documentação e teste da API)

## 4. Principais Aprendizados

Este estudo permitiu uma compreensão muito mais profunda da "base" da mensageria, indo além do uso de bibliotecas de abstração de alto nível. Os principais pontos que pude aperfeiçoar foram:

* **O "Porquê" do Desacoplamento:** Entender na prática como um Producer pode funcionar (e responder ao usuário) sem saber se o Consumer está online ou não.
* **O Ciclo de Vida da Mensagem:** Acompanhar o fluxo completo:
    1.  Publicação (Publish) pelo Producer.
    2.  Armazenamento na Fila (Queue) pelo Broker (RabbitMQ).
    3.  Consumo (Consume) pelo Worker.
    4.  Confirmação (Ack) de que a mensagem foi processada com sucesso.
* **API do Cliente RabbitMQ:** Lidar diretamente com a biblioteca `RabbitMQ.Client`, especialmente a nova **API `async`** (v7+), entendendo a necessidade de gerenciar Conexões e Canais (`CreateConnectionAsync`, `CreateChannelAsync`, `BasicPublishAsync`, etc.).
* **Resiliência e Escalabilidade:** Compreender como é simples adicionar mais instâncias do Worker (Consumer) para processar uma fila grande em paralelo, ou como as mensagens permanecem na fila se o Consumer falhar, garantindo que nenhum dado seja perdido.

## 5. Como Executar o Projeto

**Pré-requisitos:**
* .NET SDK 8 (ou 9)
* Docker Desktop

**Passos:**

1.  **Iniciar o RabbitMQ:**
    Execute o comando abaixo no seu terminal para iniciar um container do RabbitMQ com a interface de gerenciamento:
    ```bash
    docker run -d --name my-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
    ```

2.  **Configurar a Solução:**
    Abra a solução (`Mensageria.sln`) no Visual Studio. Clique com o botão direito na Solução > "Configurar projetos de inicialização..." e defina os projetos `Mensageria.Api` e `Mensageria.Consumer` para **Iniciar**.

3.  **Executar:**
    Pressione F5 ou rode a solução. Duas janelas (um navegador com o Swagger e um console do Consumer) serão abertas.

4.  **Testar:**
    * No console do Consumer, você verá a mensagem: `Consumer configurado. Aguardando mensagens...`
    * No Swagger (API), utilize o endpoint `POST /api/messages` para enviar uma mensagem.
    * Observe a mensagem sendo recebida e logada no console do Consumer.
