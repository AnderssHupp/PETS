<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="recuperar_pw.aspx.cs" Inherits="LojaVirtual.recuperar_pw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .recuperar-container {
            max-width: 400px;
            margin: 30px auto;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            background-color: #fff;
        }

            .recuperar-container h1 {
                font-size: 24px;
                margin-bottom: 20px;
                text-align: center;
            }

            .recuperar-container .form-control {
                margin-bottom: 10px;
            }

            .recuperar-container .btn-primary {
                width: 100%;
                margin-top: 10px;
            }

            .recuperar-container .alert {
                margin-top: 15px;
                text-align: center;
            }

  
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">

    <div class="recuperar-container">
        <h1>Recuperar Palavra-Passe</h1>

        <div class="form-group">
            <label for="tb_email">Email:</label>
            <asp:TextBox ID="tb_email" runat="server" CssClass="form-control" Placeholder="Digite seu email"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_email"
                ErrorMessage="Campo obrigatório" ForeColor="Red" CssClass="text-danger">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Email inválido"
                ControlToValidate="tb_email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                ForeColor="Red" CssClass="text-danger">*</asp:RegularExpressionValidator>
        </div>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" />

        <asp:Button ID="btn_recuperar" runat="server" OnClick="btn_recuperar_Click" Text="Recuperar"
            CssClass="btn btn-primary" />

        <asp:Label ID="lbl_mensagem" runat="server"></asp:Label>
    </div>

</asp:Content>
