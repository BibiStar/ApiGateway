AQUI, FOI UTILIZADO TESTE UNITÁRIO, NÃO TDD

-- ORDER SERVICE --
Um microserviço de pedidos

Antes de rodar as operações RESTFUL no swagger, deve-se pegar autorização no API Gateway
https://localhost:7016/swagger/index.html

-- CLIENT SERVICE -- 
Um microserviço de cliente

Antes de rodar as operações RESTFUL no swagger, deve-se pegar autorização no API Gateway
https://localhost:7073/swagger/index.html

--- API GATEWAY ---
O API Gateway é um padrão de arquitetura utilizado para gerenciar e centralizar a comunicação entre os clientes (front-end ou outros sistemas) e os microserviços. Ele atua como uma "porta de entrada" para todos os pedidos feitos aos microserviços, 
realizando tarefas como roteamento de requisições, autenticação, autorização, balanceamento de carga, cache, e muito mais.
Um exemplo breve: Usa proxy reverso para rotear Client Service e Order Service
https://localhost:7080/swagger/index.html
Como podemos ver, cada microserviço está numa porta diferente, o que poderia ser também em um servidor diferente. O API GATEWAY usa o proxy reverso para direcionar para esses microserviços, mas mantem o site como se estivesse em: https://localhost:7080
A autenticação e autorização são feitas com JWT. Evite colocar dados sensíveis no JWT (tipo CPF, senha, etc.) — mesmo que ele seja assinado, ele é codificado, não criptografado.

-- Teste Unitário --

-- Teste de Carga --
Usa o K6 do grafana para realizar o teste. Os testes são executados através de um arquivo .bat, que chama um script chamado teste.js e teste_n_cenários.js , chamando o json usuario.json para popular o teste.



