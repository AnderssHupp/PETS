<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="criar_conta.aspx.cs" Async="true" Inherits="LojaVirtual.Criar_ContaUni" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Criar Conta</title>
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

        #cards {
            display: flex;
            justify-content: center;
            flex-direction: row;
            gap: 20px;
            margin-top: 20px;
        }

        /*   .create-account-container {
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 20px;
            margin: auto;
            width: 100%;
            max-width: 600px;
        }*/

        .perfil-card {
            display: inline-block;
            width: 260px;
            height: 150px;
            margin: 10px;
            padding: 24px;
            border: 2px solid #ddd;
            border-radius: 10px;
            cursor: pointer;
            transition: all 0.2s ease-in-out;
            vertical-align: top;
            background-color: #fafafa;
            text-align: center;
        }

            .perfil-card:hover {
                border-color: #28a745;
                box-shadow: 0 0 8px rgba(40, 167, 69, 0.2);
            }


        .selected {
            border-color: #28a745;
            background-color: #e9fbe9;
        }

        .radio-option {
            display: none;
        }


        .btn {
            width: 100%;
            padding: 12px;
            margin: 12px 0;
            background-color: #28a745;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

            .btn:hover {
                background-color: #218838;
            }

        .step-buttons {
            display: flex;
            justify-content: space-between;
            gap: 12px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="d-flex p-5 justify-content-center">
        <asp:Panel ID="panel_step1" runat="server" Visible="true">
            <section class="d-flex form-group w-100" style="max-width: 600px; align-items: center; flex-direction: column;">
                <h2>Crie sua conta</h2>
                <h4>Junte-se a nós como Cliente ou Cuidador.</h4>

                <asp:RadioButtonList ID="rbl_tipoConta" runat="server" RepeatDirection="Horizontal" CssClass="radio-option">
                    <asp:ListItem Text="Cliente" Value="cliente" />
                    <asp:ListItem Text="Prestador" Value="prestador" />
                </asp:RadioButtonList>

                <div id="cards">
                    <label class="perfil-card" onclick="selecionar('0')">
                        <strong>Cliente</strong><br />
                        Preciso de alguém para cuidar do meu pet
                   
                    </label>
                    <label class="perfil-card" onclick="selecionar('1')">
                        <strong>
                            <img src="icones/patas (1).png" style="width: 16px; height: 16px;" />Pet Sitter</strong><br />
                        Quero prestar serviços
                   
                    </label>
                </div>

                <asp:Button ID="btn_next_step1" runat="server" Text="Próximo" CssClass="btn" OnClick="btn_next_step1_Click" Enabled="false" />
            </section>
        </asp:Panel>

        <asp:Panel ID="panel_step2_cliente" runat="server" Visible="false">
            <div class="form-group w-100" style="max-width: 600px;">

                <h2>Cadastro de Cliente</h2>
                <div class="form-group">
                    <label>Nome:</label>
                    <asp:TextBox ID="tb_nome_cliente" runat="server" CssClass="form-control" />

                </div>
                <div class="form-group">
                    <label>Email:</label>
                    <asp:TextBox ID="tb_email_cliente" runat="server" TextMode="Email" CssClass="form-control" />

                </div>
                <asp:Panel ID="panel_password" Visible="true" runat="server">
                    <div class="form-group">
                        <label>Password:</label>
                        <asp:TextBox ID="tb_pass_cliente" runat="server" TextMode="Password" CssClass="form-control" />
                        <div class="form-text">
                            Your password must be 8-20 characters long, contain letters and numbers, and must not contain spaces, special characters, or emoji.
                        </div>

                    </div>
                </asp:Panel>
                <div class="form-group">
                    <label>Foto:</label>
                    <asp:FileUpload ID="FileUpload_cliente" runat="server" CssClass="form-control-file" />

                </div>
                <div class="form-group">
                    <label>Telemóvel:</label>
                    <asp:TextBox ID="tb_tel_cliente" runat="server" CssClass="form-control" />

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
                    <label>NIF:</label>
                    <asp:TextBox ID="tb_nif_cliente" runat="server" CssClass="form-control" />
                </div>
                <div class="step-buttons">
                    <asp:Button ID="btn_voltar_cliente" runat="server" Text="Voltar" CssClass="btn btn-primary" OnClick="btn_voltar_Click" />
                    <asp:Button ID="btn_criar_cliente" runat="server" Text="Criar Conta Cliente" CssClass="btn btn-primary" OnClick="btn_criar_cliente_Click" />
                </div>
                <asp:Label ID="lbl_mensagem" runat="server" CssClass="mensagem" />

            </div>
        </asp:Panel>

        <asp:Panel ID="panel_step2_prestador" runat="server" Visible="false">
            <div class="form-group w-100" style="max-width: 600px;">

                <h2>Cadastro de Prestador</h2>
                <div class="form-group">
                    <label>Nome:</label>
                    <asp:TextBox ID="tb_nome_prestador" runat="server" CssClass="form-control" />

                </div>
                <div class="form-group">
                    <label>Email:</label>
                    <asp:TextBox ID="tb_email_prestador" runat="server" TextMode="Email" CssClass="form-control" />

                </div>
                <div class="form-group">
                    <asp:Panel ID="panel_passP" runat="server" Visible="true">
                        <label>Password:</label>
                        <asp:TextBox ID="tb_pass_prestador" runat="server" TextMode="Password" CssClass="form-control" />
                        <div class="form-text">
                            Your password must be 8-20 characters long, contain letters and numbers, and must not contain spaces, special characters, or emoji.
                        </div>
                    </asp:Panel>
                </div>
                <div class="form-group">
                    <label>Foto:</label>
                    <asp:FileUpload ID="FileUpload_prestador" runat="server" CssClass="form-control-file" />

                </div>
                <div class="form-group">
                    <label>Telemóvel:</label>
                    <asp:TextBox ID="tb_tel_prestador" runat="server" CssClass="form-control" />

                </div>
                <div class="form-group">
                    <label>Experiência:</label>
                    <asp:DropDownList ID="ddl_experiencia" runat="server" CssClass="form-select">
                        <asp:ListItem>Sem experiência</asp:ListItem>
                        <asp:ListItem>1-2 anos</asp:ListItem>
                        <asp:ListItem>+3 anos</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <br />
                <div class="form-group">
                    <label>Localidade:</label>
                    <asp:DropDownList ID="ddl_localidadeP" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Lisboa" />
                        <asp:ListItem Text="Sintra" />
                        <asp:ListItem Text="Cascais" />
                        <asp:ListItem Text="Oeiras" />
                        <asp:ListItem Text="Outros" />
                    </asp:DropDownList>

                </div>
                <div class="form-group">
                    <label>Disponibilidade:</label>
                    <asp:DropDownList ID="ddl_disponibilidade" runat="server" CssClass="form-select">
                        <asp:ListItem>Todos os dias</asp:ListItem>
                        <asp:ListItem>Segunda a Sexta</asp:ListItem>
                        <asp:ListItem>Fins de Semana e Feriados</asp:ListItem>
                    </asp:DropDownList>

                </div>

                <div class="form-group">
                    <label>IBAN:</label>
                    <asp:TextBox ID="tb_iban" runat="server" CssClass="form-control" />

                </div>
                <div class="step-buttons">
                    <asp:Button ID="btn_voltar_prestador" runat="server" Text="Voltar" CssClass="btn btn-primary" OnClick="btn_voltar_Click" />
                    <asp:Button ID="btn_criar_prestador" runat="server" Text="Criar Conta Prestador" CssClass="btn btn-primary" OnClick="btn_criar_prestador_Click" />
                </div>
                <asp:Label ID="lbl_msg" runat="server" CssClass="mensagem" />

            </div>
        </asp:Panel>

    </div>

 <script type="text/javascript">
     function selecionar(index) {
         const cards = document.querySelectorAll('.perfil-card');
         const radios = document.getElementsByName('<%# rbl_tipoConta.UniqueID %>');
        cards.forEach((card, i) => {
            card.classList.remove("selected");
            if (i == index) {
                card.classList.add("selected");
                radios[i].checked = true;
            }
        });
        document.getElementById('<%# btn_next_step1.ClientID %>').disabled = false;
     }
 </script>

</asp:Content>
