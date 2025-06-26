<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="LojaVirtual.login_utilizador" MasterPageFile="~/Template.Master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Login</title>
    <style>
     

        h1 {
            margin-bottom: 20px;
            color: #333;
        }

   
        .link-recuperar {
            color: #007bff;
            text-decoration: none;
            font-size: 14px;
        }

            .link-recuperar:hover {
                text-decoration: underline;
            }

        .erro-validacao {
            color: red;
            font-size: 12px;
        }

        .mensagem {
            margin-top: 12px;
            color: red;
        }

        .centralizado {
            text-align: center;
        }

        .btn-google {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            width: 100%;
            padding: 10px;
            margin: 12px 0;
            background-color: white;
            color: #555;
            border: 1px solid #ddd;
            border-radius: 8px;
            cursor: pointer;
            transition: background-color 0.3s;
            font-weight: bold;
        }

            .btn-google:hover {
                background-color: #f5f5f5;
            }

        .google-icon {
            width: 20px;
            height: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">

    <div class="d-flex w-100 p-5" style="justify-content:center"; >
       <section class="d-flex" style="flex-direction:column;">       
            <h1>Login</h1>
            <div class="form-group">
            <label>Email:</label>
            <asp:TextBox ID="tb_email" runat="server" TextMode="Email" CssClass="form-control"></asp:TextBox>

            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_email" ErrorMessage="Campo obrigatório" CssClass="erro-validacao" ValidationGroup="LoginValidation">*</asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
            <label >Password:</label>
            <asp:TextBox ID="tb_pw" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>

            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_pw" ErrorMessage="Campo obrigatório" CssClass="erro-validacao" ValidationGroup="LoginValidation">*</asp:RequiredFieldValidator>
            </div>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="erro-validacao" ValidationGroup="LoginValidation" />
            
            <asp:Button ID="BtnEntrar" runat="server" OnClick="BtnEntrar_Click" Text="Entrar" CssClass="btn btn-primary" ValidationGroup="LoginValidation" />
    

            <asp:Label ID="lbl_mensagem" runat="server" CssClass="mensagem"></asp:Label>
            <asp:HyperLink ID="Link_recuperar_pw" runat="server" CssClass="link-recuperar" NavigateUrl="~/recuperar_pw.aspx">Recuperar palavra passe e/ou utilizador.</asp:HyperLink>
            <asp:HyperLink ID="Link_CriarConta" runat="server" CssClass="link-recuperar" NavigateUrl="~/criar_conta.aspx">Ainda não és cliente? Crie sua conta!</asp:HyperLink>
           
            <p class="centralizado">OU</p>
            <button id="btn_google" runat="server" onserverclick="btn_google_Click1"
                class="btn-google" type="submit">
                <svg class="google-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 48 48">
                    <path fill="#4285F4" d="M24 9.5c3.1 0 5.7 1.1 7.8 2.9l5.8-5.8C33.8 3.9 29.2 2 24 2 14.7 2 7.1 8.3 4.3 16.7l6.9 5.4c1.6-4.8 6.1-8.3 11.8-8.3z" />
                    <path fill="#34A853" d="M24 44c5.8 0 10.7-2 14.2-5.3l-6.7-5.3c-1.8 1.2-4.1 1.9-6.6 1.9-5.6 0-10.3-3.7-12-8.8l-6.9 5.3C7.3 38.9 15.1 44 24 44z" />
                    <path fill="#FBBC05" d="M44 24c0-1.6-.2-3.2-.5-4.7H24v9.1h11.4c-.8 2.3-2.2 4.2-4.1 5.6l6.7 5.3C42.2 35.4 44 30 44 24z" />
                    <path fill="#EA4335" d="M11.2 28.7c-1-3-1-6.3 0-9.3L4.3 14C.8 20.3.8 27.7 4.3 34l6.9-5.3z" />
                </svg>
                Entrar com Google
            </button>
        </section>
    </div>
</asp:Content>
