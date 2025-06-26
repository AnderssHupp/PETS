<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="criar_anuncio.aspx.cs" Inherits="LojaVirtual.criar_anuncio" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Criar Anúncio</title>
    <style>
        /*body {
            font-family: Arial, sans-serif;
        }*/

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

        /*      .form-group {
            margin-bottom: 20px;
        }

        .form-group label {
            display: block;
            margin-bottom: 6px;
            font-weight: 500;
        }

        .form-group input,
        .form-group textarea,
        .form-group select {
            width: 100%;
            padding: 10px 12px;
            font-size: 16px;
            border: 1px solid #ccc;
            border-radius: 6px;
            box-sizing: border-box;
        }

        .btn {
            width: 100%;
            background-color: #28a745;
            color: white;
            padding: 12px;
            font-size: 16px;
            border: none;
            cursor: pointer;
            border-radius: 6px;
            transition: background-color 0.3s ease;
        }

        .btn:hover {
            background-color: #218838;
        }*/

        .message {
            text-align: center;
            font-weight: bold;
            margin-top: 20px;
            color: #d63333;
        }

        @media screen and (max-width: 480px) {
            .form-group label {
                font-size: 14px;
            }

            .form-group input,
            .form-group textarea,
            .form-group select {
                font-size: 14px;
                padding: 8px 10px;
            }

            .btn {
                padding: 10px;
                font-size: 15px;
            }

            h2 {
                font-size: 20px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">
    <div class="form-container">

        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:Label ID="lbl_msgPet" runat="server" Text="Você ainda não cadastrou nenhum pet." ForeColor="Red" Visible="false" CssClass="message" />

                    <asp:Button ID="btnCadastrarPet" runat="server" Text="Cadastrar Pet"
                        CssClass="btn btn-primary" Visible="false" OnClick="btnCadastrarPet_Click" />

                <asp:Panel ID="panelFormulario" runat="server" Visible="true">
                    <h2>Criar Novo Anúncio</h2>

                    <div class="form-group">
                        <label>Título:</label>
                        <asp:TextBox ID="tb_titulo" runat="server" CssClass="form-control" />
                    </div>

                    <div class="form-group">
                        <label>Tipo de Serviço</label>
                        <asp:DropDownList ID="ddl_servicos" runat="server" DataSourceID="SqlDataSource1" DataTextField="nome_servico" DataValueField="cod_tipoServico" CssClass="form-select">
                        </asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:LojaVirtualConnectionString %>' SelectCommand="SELECT * FROM [TipoServico]"></asp:SqlDataSource>
                    </div>
                    <div class="form-group">
                        <label>Pet</label>
                        <asp:DropDownList ID="ddl_pets" runat="server" DataSourceID="SqlDataSource2" DataTextField="nome_pet" DataValueField="id_pet" CssClass="form-select">
                        </asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:LojaVirtualConnectionString %>' SelectCommand="SELECT * FROM [Pets] WHERE id_utilizador = @id_utilizador">
                            <SelectParameters>
                                <asp:SessionParameter Name="id_utilizador" SessionField="user_id" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div class="form-group">
                        <label>Descrição:</label>
                        <asp:TextBox ID="tb_descricao" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" />
                    </div>

                    <div class="form-group">
                        <label>Localidade:</label>
                        <asp:DropDownList ID="ddl_localidade" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Lisboa" />
                            <asp:ListItem Text="Sintra" />
                            <asp:ListItem Text="Cascais" />
                            <asp:ListItem Text="Oeiras" />
                            <asp:ListItem Text="Outros" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Data de necessidade do serviço:</label>
                        <asp:TextBox ID="tb_data" runat="server" TextMode="Date" CssClass="form-control" />
                    </div>

                    <div class="form-group">
                        <label>Hora de início prevista:</label>
                        <asp:TextBox ID="tb_hora" runat="server" TextMode="Time" CssClass="form-control" />
                    </div>

                    <div class="form-group">
                        <label>Preço (€):</label>
                        <asp:TextBox ID="tb_preco" runat="server" TextMode="Number" CssClass="form-control" />
                    </div>

                    <asp:Button ID="btn_publicar" runat="server" Text="Publicar Anúncio" CssClass="btn btn-primary" OnClick="btn_publicar_ClickAsync" />
                </asp:Panel>
                <asp:Label ID="lbl_mensagem" runat="server" CssClass="message" />

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
