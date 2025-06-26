<%@ Page Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="inserir_produtos.aspx.cs" Inherits="LojaVirtual.inserir_produtos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Inserir Produtos</title>

    <style>
      
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">
    <div class="container my-5 d-flex justify-content-center flex-column align-items-center">
        <h1 class="text-center mb-4">Cadastro de Produtos</h1>

        <div class="card shadow-lg p-4 w-100" style="max-width:650px;"> 
            <div class="row mb-3">
                <label class="col-sm-2 col-form-label">Nome:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="tb_nome" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>

            <div class="row mb-3">
                <label class="col-sm-2 col-form-label">Preço:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="tb_preco" runat="server" CssClass="form-control" TextMode="SingleLine"></asp:TextBox>
                </div>
            </div>

            <div class="row mb-3">
                <label class="col-sm-2 col-form-label">Stock:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="tb_stock" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                </div>
            </div>

            <div class="row mb-3">
                <label class="col-sm-2 col-form-label">Categoria:</label>
                <div class="col-sm-10">
                    <asp:DropDownList ID="ddl_categoria" runat="server" DataSourceID="SqlDataSource1" DataTextField="nome" DataValueField="id_categoria" CssClass="form-select">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row mb-3">
                <label class="col-sm-2 col-form-label">Imagem:</label>
                <div class="col-sm-10">
                    <asp:FileUpload ID="file_upload_img" runat="server" CssClass="form-control-file" />
                </div>
            </div>

            <div class="button-group d-flex justify-content-center" style="gap:10px">
                <asp:Button ID="btn_voltar" runat="server" OnClick="btn_voltar_Click" Text="Voltar" CssClass="btn btn-primary px-4" />
                <asp:Button ID="btn_inserir" runat="server" OnClick="btn_inserir_Click" Text="Inserir Produto" CssClass="btn btn-primary px-4" />
            </div>
            <div class="text-center mt-3">
                <asp:Label ID="lbl_mensagem" runat="server"></asp:Label>
            </div>
        </div>
    </div>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:LojaVirtualConnectionString %>"
        SelectCommand="SELECT [nome], [id_categoria] FROM [Categorias]"></asp:SqlDataSource>
</asp:Content>
