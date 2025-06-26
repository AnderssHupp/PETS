<%@ Page Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="ativacao_conta.aspx.cs" Inherits="LojaVirtual.ativacao_conta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .container-ativacao {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            height: 80vh;
            text-align: center;
            background-color: #f8f9fa;
            border-radius: 10px;
            padding: 40px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

            .container-ativacao h1 {
                color: #28a745;
                font-size: 2rem;
                margin-bottom: 10px;
            }

            .container-ativacao p {
                font-size: 1.2rem;
                color: #6c757d;
                margin-bottom: 20px;
            }

        .btn-voltar {
            padding: 12px 20px;
            background-color: #28a745;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            transition: background-color 0.3s;
            width: 100%;
            max-width: 200px;
            font-size: 16px;
        }

            .btn-voltar:hover {
               background-color: #218838;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">
    <div class="container-ativacao">
        <h1>Conta ativada com sucesso! 🎉</h1>
        <p>Agora você pode acessar sua conta e aproveitar todos os recursos.</p>
        <a href="login.aspx" class="btn-voltar">Ir para o Login</a>
    </div>
</asp:Content>
