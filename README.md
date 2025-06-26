# 🐾 PetWorld

**PetWorld** é uma aplicação web desenvolvida em **ASP.NET Core MVC** que tem como objetivo facilitar a comunicação entre pessoas que possuem animais de estimação e prestadores de serviços (Pet Sitters). A aplicação permite gestão de anúncios, agendamentos, perfis, avaliações, marketplace de produtos, e muito mais.

## 🚀 Funcionalidades Principais

- 📋 Cadastro de usuários com diferentes perfis: Cliente, Prestador (Pet Sitter) e Administrador.
- 🐶 Registro de pets associados ao cliente.
- 📢 Criação e gestão de anúncios de serviços (por parte dos clientes).
- 📅 Agendamentos de serviços com gestão de agenda e sincronização com Google Calendar.
- 📦 Marketplace de produtos para pets.
- 📬 Sistema de mensagens internas entre usuários.
- 🌟 Sistema de avaliações para prestadores.
- 📂 Upload de fotos de perfil e imagens de produtos.
- 🔍 Filtros por geolocalização (Open Street Maps).
- 📱 Integração com WhatsApp API para comunicação direta.
- ⏰ Notificações e lembretes automáticos com Hangfire.

## 🧰 Tecnologias Utilizadas

- ASP.NET Core MVC (.NET 8)
- SQL Server
- Bootstrap
- HTML5 / CSS3 / JavaScript
- Google Calendar API
- Open Street Maps
- WhatsApp API
- Hangfire (Agendamento de tarefas/notificações)
- Autenticação via OAuth do Google com Cookies + Claims Identity

## ⚙️ Instalação e Execução

1. Clone o repositório:
```bash
git clone https://github.com/AnderssHupp/PetWorld.git
