🚀 Funcionalidades
✅ Busca automática de dados ausentes
✅ Integração com múltiplas fontes (APIs externas, bancos de dados, etc.)
✅ Retorno do status HTTP correto da origem
✅ Tratamento de exceções e logging
✅ Segurança baseada em API Keys e Escopos

🛠 Configuração
📌 Pré-requisitos
.NET 7+
Docker (Opcional)
Postman ou cURL para testes
📌 Instalação
Clone o repositório e acesse a pasta do projeto:

sh
Copiar
Editar
git clone https://github.com/seu-usuario/api-fallback.git
cd api-fallback
Restaurar pacotes NuGet:

sh
Copiar
Editar
dotnet restore
Compilar o projeto:

sh
Copiar
Editar
dotnet build
Rodar a aplicação:

sh
Copiar
Editar
dotnet run
🔑 Autenticação e Segurança
A API utiliza API Keys para validar os clientes e escopos para definir permissões de acesso.

📌 Configuração das Chaves no appsettings.json
json
Copiar
Editar
{
  "ApiClients": {
    "cliente1": {
      "ApiKey": "TOKEN_CLIENTE1_ABC123",
      "AllowedEndpoints": [
        "/api/fallback/get-data",
        "/api/fallback/list-data"
      ]
    },
    "cliente2": {
      "ApiKey": "TOKEN_CLIENTE2_XYZ789",
      "AllowedEndpoints": [
        "/api/fallback/get-data"
      ]
    }
  }
}
📌 Como enviar o token na requisição
sh
Copiar
Editar
curl -X GET "https://fallback-api.com/api/fallback/get-data/123" \
     -H "Authorization: Bearer TOKEN_CLIENTE1_ABC123"
📡 Endpoints
📌 Buscar dados ausentes
http
Copiar
Editar
POST /api/fallback
📌 Exemplo de Corpo da Requisição:

json
Copiar
Editar
{
    "missingDataId": "1a2b3c4d-5e6f-7g8h-9i0j-k1l2m3n4o5p6",
    "sourceEnum": 1,
    "targetType": "OccurrenceContract"
}
📌 Possíveis Respostas

Status Code	Descrição
200 OK	Dados retornados com sucesso
400 Bad Request	Requisição inválida ou tipo de contrato não encontrado
404 Not Found	Dados não encontrados na API externa
500 Internal Server Error	Erro interno ao processar
⚙️ Arquitetura
A API é baseada em Clean Architecture e utiliza:

MediatR para orquestração
IHttpClientFactory + Polly para resiliência de chamadas HTTP
Reflection para chamadas genéricas de repositórios
Autenticação baseada em API Keys e Escopos
Logging centralizado para debug e monitoramento
📌 Fluxo de Processamento
1️⃣ O FallbackController recebe a requisição e delega a lógica para o FallbackService.
2️⃣ O FallbackService seleciona o handler correto com base no SourceEnum.
3️⃣ O handler correspondente faz uma chamada HTTP ao serviço externo.
4️⃣ A API de Fallback retorna os dados ou o status correto da origem.

🛠 Tecnologias Utilizadas
ASP.NET Core 7
Polly (resiliência de chamadas HTTP)
MediatR (arquitetura baseada em eventos)
IHttpClientFactory (gerenciamento eficiente de requisições HTTP)
Docker (para deploy e escalabilidade)
Swagger (documentação interativa da API)
📜 Licença
Este projeto está sob a licença MIT. Sinta-se à vontade para contribuir e modificar conforme necessário.

👨‍💻 Desenvolvido por Anderson Pinheiro 🚀
