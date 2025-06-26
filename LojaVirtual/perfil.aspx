<%@ Page Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="perfil.aspx.cs" Inherits="LojaVirtual.perfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Perfil</title>
    <style>
        .perfil-container {
            max-width: 800px;
            margin: 30px auto;
            padding: 20px;
            background-color: #f4f4f4;
            border-radius: 12px;
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1);
        }

        h1, h2 {
            text-align: center;
            margin-bottom: 20px;
            color: #333;
        }

        .secao {
            margin-bottom: 30px;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.05);
        }

        .form-group {
            margin-bottom: 15px;
        }

        label {
            font-weight: bold;
            margin-bottom: 5px;
            display: block;
            color: #555;
        }

        input[type="text"], input[type="password"], input[type="email"], input[type="date"] {
            width: 100%;
            padding: 10px;
            margin: 5px 0;
            border-radius: 8px;
            border: 1px solid #ddd;
            font-size: 16px;
        }
        /*
        button {
            margin: 10px 0;
            padding: 12px;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            transition: background-color 0.3s;
            width: 100%;
        }*/

        /* button:hover {
                background-color: #0056b3;
            }

        .btn-adicionar {
            background-color: #28a745;
        }

            .btn-adicionar:hover {
                background-color: #218838;
            }

        .btn-remover {
            background-color: #dc3545;
            margin-left: 10px;
        }

            .btn-remover:hover {
                background-color: #c82333;
            }*/

        .mensagem-sucesso {
            color: green;
            margin-top: 10px;
        }

        .mensagem-erro {
            color: red;
            margin-top: 10px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <asp:Label ID="lbl_boasvindas" runat="server" Text=""></asp:Label>
    <div class="perfil-container">
        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
            <contenttemplate>
                <div class="secao">
                    <!--  Dados -->
                    <h2>Dados Pessoais</h2>
                    <div class="form-group">
                        <label>Nome Completo:</label>
                        <asp:TextBox ID="tb_nome" runat="server" />
                        <asp:RequiredFieldValidator ID="rfv_nome" runat="server" ControlToValidate="tb_nome"
                            ErrorMessage="Nome Completo é obrigatório" ForeColor="Red" ValidationGroup="DadosPessoais" />
                    </div>

                    <div class="form-group">
                        <label>Email:</label>
                        <asp:TextBox ID="tb_email" runat="server" TextMode="Email" />

                        <asp:RequiredFieldValidator ID="rfv_email" runat="server" ControlToValidate="tb_email"
                            ErrorMessage="Email é obrigatório" ForeColor="Red" ValidationGroup="DadosPessoais" />
                    </div>

                    <div class="form-group">
                        <label>Telemóvel:</label>
                        <asp:TextBox ID="tb_telemovel" runat="server" />

                        <asp:RequiredFieldValidator ID="rfv_telemovel" runat="server" ControlToValidate="tb_telemovel"
                            ErrorMessage="Telemóvel é obrigatório" ForeColor="Red" ValidationGroup="DadosPessoais" />
                    </div>

                    <div class="form-group">
                        <label>NIF:</label>
                        <asp:TextBox ID="tb_nif" runat="server" />

                        <asp:RequiredFieldValidator ID="rfv_nif" runat="server" ControlToValidate="tb_nif"
                            ErrorMessage="NIF é obrigatório" ForeColor="Red" ValidationGroup="DadosPessoais" />
                    </div>

                    <div class="form-group">
                        <label>Data de Nascimento:</label>
                        <asp:TextBox ID="tb_dataNascimento" runat="server" TextMode="Date" />

                        <asp:RequiredFieldValidator ID="rfv_dataNascimento" runat="server" ControlToValidate="tb_dataNascimento"
                            ErrorMessage="Data de Nascimento é obrigatória" ForeColor="Red" ValidationGroup="DadosPessoais" />
                    </div>
                    
                    <asp:Button ID="btn_AlterarDados" runat="server" Text="Guardar" class="btn btn-primary" OnClick="btn_AlterarDados_Click" ValidationGroup="DadosPessoais" ></asp:Button>
                    <asp:Label ID="lbl_msgDados" runat="server"></asp:Label>
                </div>

                <!--  Palavra-Passe -->
                <div class="secao">
                    <h2>Alterar Palavra-Passe</h2>
                    <div class="form-group">
                        <label>Password Atual:</label>
                        <asp:TextBox ID="tb_pw_atual" runat="server" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tb_pw_atual" ErrorMessage="Campo obrigatório" ForeColor="Red" ValidationGroup="PalavraPasse" />
                    </div>
                    <div class="form-group">
                        <label>Nova Password:</label>
                        <asp:TextBox ID="tb_pw_nova" runat="server" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_pw_nova" ErrorMessage="Campo obrigatório" ForeColor="Red" ValidationGroup="PalavraPasse" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tb_pw_nova" ErrorMessage="Password fraca" ForeColor="Black" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$" ValidationGroup="PalavraPasse" />
                    </div>
                    <div class="form-group">
                        <label>Confirmar Nova Password:</label>
                        <asp:TextBox ID="tb_confirmarPw" runat="server" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tb_confirmarPw" ErrorMessage="Campo obrigatório" ForeColor="Red" ValidationGroup="PalavraPasse" />
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="tb_confirmarPw"
                            ControlToCompare="tb_pw_nova" ErrorMessage="As palavras-passe não coincidem" ForeColor="Red" ValidationGroup="PalavraPasse" />
                    </div>
                    
                    <asp:Button ID="btn_alterarPw" runat="server" Text="Alterar Palavra-Passe" class="btn btn-primary" OnClick="btn_alterarPw_Click" ValidationGroup="PalavraPasse" />
                     <asp:Label ID="lbl_mensagem" runat="server"></asp:Label>

                <!-- Moradas de Entrega -->
                <div class="secao">
                    <h2>Morada de Entrega</h2>
                    <div class="form-group">
                        <label>Morada:</label>
                        <asp:TextBox ID="tb_morada" runat="server" />
                    </div>
                    <div class="form-group">
                        <label>Concelho:</label>
                        <asp:TextBox ID="tb_concelho" runat="server" />
                    </div>
                    <div class="form-group">
                        <label>Distrito:</label>
                        <asp:TextBox ID="tb_distrito" runat="server" />
                    </div>
                    <div class="form-group">
                        <label>Código Postal:</label>
                        <asp:TextBox ID="tb_cp" runat="server" />
                    </div>
                    <asp:Button ID="btn_AlterarMorada" class="btn btn-primary" runat="server" Text="Guardar" OnClick="btn_AlterarMorada_Click" CausesValidation="false"></asp:Button>
                    <asp:Label ID="lbl_msgMorada" runat="server"></asp:Label>
                </div>
                <triggers>
                    <asp:PostBackTrigger ControlID="btn_AlterarDados" />
                    <asp:AsyncPostBackTrigger runat="server" ControlID="btn_AlterarMorada" EventName="Click" />
                </triggers>
            </contenttemplate>
        </asp:UpdatePanel>
        <!-- btn de Terminar Sessao -->
        <div class="text-right">
            <asp:Button ID="btn_Logout" runat="server" Text="Terminar Sessão" class="btn btn-primary" CausesValidation="false" OnClick="btn_Logout_Click" />
        </div>
    </div>

</asp:Content>
