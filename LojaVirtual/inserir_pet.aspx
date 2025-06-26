<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="inserir_pet.aspx.cs" Inherits="LojaVirtual.inserir_pet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-container {
            max-width: 600px;
            width: 95%;
            margin: 40px auto;
            padding: 24px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #f9f9f9;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.05);
        }

        h2 {
            text-align: center;
            margin-bottom: 24px;
            color: #333;
        }

        .message {
            text-align: center;
            margin-top: 10px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">
    <div class="form-container">
        <%--div class="d-flex p-5" style="justify-content: center;">--%>
        <%--<div class="card shadow-lg w-75 d-flex" style="align-items:center;">--%>
        <div class="form-group w-100 p-4" style="max-width: 600px;">
            <h2>Cadastrar Pet</h2>

            <div class="form-group">
                <label>Nome:</label>
                <asp:TextBox ID="tb_nome" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label>Espécie:</label>
                <asp:TextBox ID="tb_especie" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label>Raça:</label>
                <asp:TextBox ID="tb_raca" runat="server" CssClass="form-control" />

            </div>

            <div class="form-group">
                <label>Idade:</label>
                <asp:TextBox ID="tb_idade" runat="server" TextMode="Number" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label>Peso (kg):</label>
                <asp:TextBox ID="tb_peso" runat="server" TextMode="Number" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label>Observações:</label>
                <asp:TextBox ID="tb_obs" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" />
            </div>

            <div class="form-group ">
                <label class="form-label">Fotos:</label>
                <asp:FileUpload ID="FileUpload_fotos" runat="server" AllowMultiple="true" CssClass="form-control-file" />
                <small>Você pode selecionar várias imagens ao mesmo tempo (CTRL + clique)</small>
            </div>        
            <asp:Button ID="btn_salvar" runat="server" Text="Cadastrar Pet" CssClass="btn btn-primary" OnClick="btn_salvar_Click" />

            <asp:Label ID="lbl_mensagem" runat="server" CssClass="message" />
        </div>
    </div>
</asp:Content>
