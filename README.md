<!-- BANNER -->
<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=00eaff&height=230&section=header&text=Envio%20Rápido%20API&fontSize=45&fontColor=0d0d0d&animation=twinkling&fontAlignY=40"/>
</p>

<p align="center">
  <img src="https://readme-typing-svg.herokuapp.com?color=00eaff&center=true&vCenter=true&width=600&lines=🚚+API+de+Cálculo+de+Frete+com+Melhor+Envio;🐳+Docker+%2B+RabbitMQ+%2B+MySQL;💙+Arquitetura+Profissional+FullStack;🔥+Angular+Hacker+UI+Theme"/>
</p>

---

## **Visão Geral do Projeto**

Sistema completo para cálculo de frete utilizando a API **Melhor Envio**, com:

✨ Back-end em **.NET**  
🐳 Orquestração com **Docker Compose**  
📦 Banco **MySQL**  
📡 Mensageria **RabbitMQ**  
🔐 Autenticação **JWT**  
🎮 Front-end **Angular** com tema **Hacker Noturno Neon**

---

## 🛠️ **Tecnologias Utilizadas**

<p align="center">
  <img src="https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white"/>
  <img src="https://img.shields.io/badge/MySQL-005C84?style=for-the-badge&logo=mysql&logoColor=white"/>
  <img src="https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white"/>
  <img src="https://img.shields.io/badge/Docker-0db7ed?style=for-the-badge&logo=docker&logoColor=white"/>
  <img src="https://img.shields.io/badge/Docker%20Compose-384d54?style=for-the-badge&logo=docker&logoColor=white"/>
  <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black"/>
  <img src="https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens"/>
</p>


---

## 🧬 **Pré-Requisitos**

Instale antes de rodar:

| Ferramenta | Link |
|---------|------|
| Docker Desktop | https://www.docker.com/products/docker-desktop |
| Node.js (v18+) | https://nodejs.org/en/download |
| Angular CLI | `npm install -g @angular/cli` |
| Git | https://git-scm.com/downloads |

---

## 🐳 **Rodando o Projeto (Back + DB + Rabbit)**

```bash
git clone https://github.com/Gian-UC/ApiConsultaDeFrete.git
cd ApiConsultaDeFrete
docker-compose up -d --build
```

Serviço	Porta	Acesso:
API	5145 | http://localhost:5145/swagger|

RabbitMQ | 15672	http://localhost:15672  (guest / guest) |

MySQL	|  3306	


## 🔐 **Token Melhor Envio**

Abra docker-compose.yml e coloque seu token:

MelhorEnvio__Token: "SEU_TOKEN_AQUI"

## 🎮 **Rodando o Front-End**

| cd envio-rapido-ui |
| npm install |
| ng serve --open |

Acesse:

http://localhost:4200

<img width="1870" height="927" alt="image" src="https://github.com/user-attachments/assets/d8cdc760-3fbe-405e-9ddc-6bd95c4e9a1a" />


🧪 Exemplo de Requisição
```
{
  "origemCep": "01001000",
  "destinoCep": "80010020",
  "peso": 1,
  "altura": 10,
  "largura": 15,
  "comprimento": 20
}
```

## 🧪 **Testes Unitários**

Os testes utilizam xUnit + Moq.

cd EnvioRapidoApi.Tests
dotnet test

🛑 Parar Toda a Aplicação
docker-compose down


✅ Resumo do que você terá funcionando

| Funcionalidade | Status |
|---------------|--------|
| Cálculo de frete via Melhor Envio | ✅ |
| Persistência de envios no MySQL | ✅ |
| Publicação de mensagens no RabbitMQ | ✅ |
| Front-End consumindo API | ✅ |
| Autenticação JWT | ✅ |
| Containerização completa | ✅ |

