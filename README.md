# 🚚 Envio Rápido API

API REST desenvolvida para cálculo e gerenciamento de envios, utilizando:
- Autenticação JWT
- Consulta de CEP via ViaCEP
- Cálculo de frete via Melhor Envio
- Persistência em MySQL
- Publicação de mensagens no RabbitMQ

---

## 🧠 Tecnologias Utilizadas

| Tecnologia | Versão | Finalidade |
|-----------|--------|------------|
| .NET 8    | SDK    | Backend/API |
| MySQL     | 8+     | Banco de dados relacional |
| Entity Framework Core | 8+ | ORM para persistência |
| RabbitMQ  | 3.x    | Mensageria assíncrona |
| Swagger UI | - | Documentação interativa |

---

## 🔐 Autenticação
A API utiliza **JWT** para autenticação e autorização dos endpoints protegidos.

### Gerar Token:
POST /api/usuarios/login

Copiar código
Corpo:
json
{
  "email": "seuemail@email.com",
  "senha": "suasenha"
}

Usar o Token no Swagger
Clique no botão Authorize e insira:

Bearer + TOKEN GERADO pelo Login

📦 Endpoints Principais
👤 Usuários

Método	Rota	Descrição:

POST	/api/usuarios/cadastro	Cadastra um novo usuário
POST	/api/usuarios/login	Autentica e retorna token JWT
DELETE	/api/usuarios/{email}	Exclui usuário (💡 Requer JWT)

🚚 Envios
Método	Rota	Descrição:

POST	/api/envios	Realiza o cálculo, salva no banco e publica no RabbitMQ
GET	/api/envios/{id}	Busca um envio pelo ID
DELETE	/api/envios/{id}	Remove um envio do banco

🗄 Estrutura de Banco (Tabela envios)

Campo	                                      Tipo	                                                                          Descrição
Id	                                        int	                                                                            Identificador único
OrigemCep	                                  varchar	                                                                        CEP do remetente
DestinoCep	                                varchar	                                                                        CEP do destinatário
Peso	                                      decimal	                                                                        Peso da encomenda
Altura	                                    decimal	                                                                        Altura da embalagem
Largura	                                    decimal	                                                                        Largura da embalagem
Comprimento	                                decimal	                                                                        Comprimento da embalagem
ValorFrete	                                decimal	                                                                        Valor calculado da entrega

🐇 Mensageria (RabbitMQ)
A cada envio cadastrado, uma mensagem é publicada na fila:

fila_calculo_frete
Exemplo da mensagem:

{
  "Id": 5,
  "OrigemCep": "01001000",
  "DestinoCep": "30140071",
  "ValorFrete": 37.50,
  "Data": "2025-11-04 16:02:18"
}

🚀 Como Executar
1. Clonar o repositório

git clone https://github.com/SEU-USUARIO/SEU-REPOSITORIO.git

2. Configurar o appsettings.json

"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=enviorapido;user=root;password=SENHA"
},
"MelhorEnvio": {
  "Token": "SEU_TOKEN_AQUI"
}
3. Criar o banco de dados

dotnet ef database update

4. Rodar a aplicação

dotnet run

E acessar:

https://localhost:5145/swagger
