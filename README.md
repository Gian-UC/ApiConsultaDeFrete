# 🚚 Envio Rápido API

![Banner](https://capsule-render.vercel.app/api?type=waving&color=0:0d0d0d,100:1a73e8&height=260&section=header&text=Envio%20Rápido%20API&fontSize=48&fontAlignY=38&animation=fadeIn&fontColor=ffffff&desc=Frete%20%7C%20RabbitMQ%20%7C%20JWT%20%7C%20Gamer%20Style&descSize=16&descAlignY=55)

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet" />
  <img src="https://img.shields.io/badge/MySQL-005C84?style=for-the-badge&logo=mysql&logoColor=white" />
  <img src="https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white" />
  <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" />
  <img src="https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white" />
  <img src="https://img.shields.io/badge/Tests-xUnit-5C2D91?style=for-the-badge" />
</p>

<p align="center">
  <img src="https://img.shields.io/badge/STATUS-✅%20Concluído-blue?style=for-the-badge">
</p>


# 🚀 Envio Rápido API

API para cálculo e gerenciamento de envios, incluindo autenticação, integração com serviços externos, mensageria e testes unitários.  
Projeto desenvolvido com foco em **boas práticas**, **escalabilidade** e **arquitetura limpa**.

---

## 📦 Funcionalidades

| Funcionalidade | Descrição |
|---|---|
| Cadastro e Login de Usuário | Gera **JWT** para autenticação |
| Cadastro de Envios | Valida CEP, calcula frete e salva no banco |
| Integração com ViaCEP | Validação de endereço e formato do CEP |
| Integração com MelhorEnvio | Cálculo real de frete |
| Publicação no RabbitMQ | Notificação assíncrona para processamento |
| Consulta de Envio | Retorna valores formatados e status |
| Exclusão de Envios | Protegido com **JWT** |
| Testes Unitários | Cobertura mínima de 80% com **xUnit + Moq** |

---

## 🧱 Arquitetura do Projeto:
|-----------------------------|
APICONSULTAFRETE/
├─ Controllers/
├─ DTOs/
├─ Models/
├─ Services/
├─ Repositories/
├─ Data/
├─ Migrations/
├─ Program.cs
└─ EnvioRapidoApi.Tests/
---------------------------------------------------------------------------------
- **Controllers** → Camada responsável por receber e responder requisições HTTP  
- **Services** → Regras de negócio, integrações externas  
- **Repositories** → CRUD e persistência com Entity Framework  
- **Tests** → Testes unitários isolando comportamento

---

## 🔐 Autenticação:

O login retorna um **JWT**, utilizado para acessar rotas protegidas.

### Login:

POST /api/usuarios/login

### Enviar Token no Swagger
Clique em **Authorize** → cole:

Bearer SEU_TOKEN_AQUI

Configuração de Segredos (JWT & Melhor Envio):

Para isso, usamos o User Secrets durante o desenvolvimento.

1) Inicialize o User Secrets no projeto:
dotnet user-secrets init

2) Configure a chave do JWT:
dotnet user-secrets set "MelhorEnvio:ApiKey" "sua-chave-ultra-secreta-aqui"
dotnet user-secrets set "Jwt:Key" "sua-chave-ultra-secreta-aqui"

Recomenda-se usar uma chave com no mínimo 32 caracteres
Ex: gerada em base64 ou GUID longo.

3) Configure sua chave do Melhor Envio:
dotnet user-secrets set "MelhorEnvio:ApiKey" "sua_api_key_do_melhor_envio"
dotnet user-secrets set "Jwt:Key" "SUA_CHAVE_ULTRA_SECRETA_AQUI"


5) No appsettings.json, deixe assim:
"Jwt": {
  "Key": ""
},
"MelhorEnvio": {
  "ApiKey": ""
}

---

## 🚚 Cadastro de Envio

POST /api/envios

### Fluxo:
1. Valida CEP com **ViaCEP**
2. Calcula frete com **MelhorEnvio**
3. Salva envio no **MySQL**
4. Publica notificação no **RabbitMQ**
5. Responde com **202 Accepted**

---

## 📬 Consulta de Envio

GET /api/envios/{id}

Retorna:



```{  
  "Id": 1,
  "OrigemCep": "01001000",
  "DestinoCep": "30140071",
  "ValorFrete": 23.72,
  "Data": "2025-11-04 16:12"
}
```


🧪 Testes Unitários

Executar:

dotnet test

Cobertura:

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov

Tecnologias:

xUnit
Moq
Coverlet

🐇 RabbitMQ

Fila utilizada:

fila_calculo_frete

Mensagem publicada:

```{
  "Id": 1,
  "OrigemCep": "01001000",
  "DestinoCep": "30140071",
  "ValorFrete": 23.72,
  "Data": "2025-11-04 16:12"
}
```

### 🛠️ Tecnologias Utilizadas

<p align="center">
  <img src="https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/Entity%20Framework%20Core-6DB33F?style=for-the-badge&logo=ef&logoColor=white"/>
  <img src="https://img.shields.io/badge/MySQL-005C84?style=for-the-badge&logo=mysql&logoColor=white"/>
  <img src="https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white"/>
  <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black"/>
  <img src="https://img.shields.io/badge/xUnit-5C2D91?style=for-the-badge"/>
  <img src="https://img.shields.io/badge/Moq-000000?style=for-the-badge&logo=mocha&logoColor=white"/>
</p>



Desenvolvido por:

Giancarlo Salomone
