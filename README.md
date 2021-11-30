# Simulação de um E-Commerce com RabbitMQ

## Subir o ambiente

Para subir o RabbitMQ em um container, execute o comando `docker run -p 15672:15672 -p 5672:5672 rabbitmq:3-management`. É necessário que o docker esteja instalado na máquina.

Para configurar a fila, acesse `http://localhost:15672/` com o usuário "guest" e senha "guest". Na aba "Queues", clique no botão "Add queue" e adicione uma fila com o nome "pedidos".

## Execução

Para simular com um sistema monolito, execute o projeto "ECommerce".
Para a simulação do projeto com o RabbitMQ (microsserviços), execute o projeto "ECommerce.Produtor" junto com o projeto "ECommerce.Consumidor". O projeto produtor é uma API que contém uma rota "SimularBlackFriday". Chame esta rota para publicar mensagens na fila de pedidos. Caso queira simular o processamento com múltiplos consumidores, suba mais instâncias do projeto consumidor.
