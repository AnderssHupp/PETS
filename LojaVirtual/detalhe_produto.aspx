<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="detalhe_produto.aspx.cs" Inherits="LojaVirtual.produto_detahe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Produto detalhe</title>
    <style>
        /* Container de detalhes do produto */
        .product-details {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            text-align: center;
            margin: 40px auto;
            padding: 30px;
            width: 90%;
            max-width: 600px;
            background-color: #fff;
            border-radius: 12px;
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1);
            transition: transform 0.3s, box-shadow 0.3s;
        }

            .product-details:hover {
                transform: translateY(-5px);
                box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
            }

            /* Imagem do produto */
            .product-details img {
                max-width: 100%;
                height: auto;
                border-radius: 12px;
                margin: 20px 0;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                transition: transform 0.3s;
            }

                .product-details img:hover {
                    transform: scale(1.05);
                }

            /* Título do produto */
            .product-details h2 {
                font-size: 28px;
                margin-bottom: 12px;
                color: #333;
            }

            /* Preço e descrição */
            .product-details p {
                font-size: 18px;
                margin: 6px 0;
                color: #555;
            }

            /* Botão de Voltar */
            .product-details button {
                padding: 12px 20px;
                margin-top: 20px;
                background-color: #007bff;
                color: white;
                border: none;
                border-radius: 8px;
                cursor: pointer;
                transition: background-color 0.3s;
                font-size: 16px;
                width: 100%;
                max-width: 200px;
            }

                .product-details button:hover {
                    background-color: #0056b3;
                }

        .button-group {
            display: flex;
            justify-content: center;
            gap: 12px;
            margin-top: 20px;
            width: 100%;
        }

        .btn-carrinho {
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

            .btn-carrinho:hover {
                background-color: #218838;
            }

        .btn-voltar {
            padding: 12px 20px;
            background-color: #007bff;
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
                background-color: #0056b3;
            }

        .preco-normal {
            font-weight: bold;
            color: black;
            font-size: 16px;
        }

        /* Responsividade */
        @media (max-width: 768px) {
            .product-details {
                width: 100%;
                padding: 20px;
            }

                .product-details h2 {
                    font-size: 24px;
                }

                .product-details p {
                    font-size: 16px;
                }

                .product-details button {
                    font-size: 14px;
                    padding: 10px;
                }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">
    <div class="product-details">
        <h2>
            <asp:Label ID="lbl_nome" runat="server"></asp:Label></h2>
        <asp:Image ID="img_produto" runat="server" />
        <p>Preço: <asp:Label ID="lbl_preco" runat="server" CssClass="preco-normal"></asp:Label></p>
        <p><b>Descrição: </b><asp:Label ID="lbl_descricao" runat="server"></asp:Label></p>
        <p><b>Informações:</b> <asp:Label ID="lbl_info" runat="server"> Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</asp:Label></p>
        <div class="button-group">
            <asp:Button ID="btn_voltar" runat="server" Text="Voltar" OnClick="btn_voltar_Click" CssClass="btn btn-outline-secondary" />
            <asp:Button ID="btn_add" runat="server" Text="Adicionar ao carrinho" CssClass="btn btn-primary" OnClick="btn_add_Click" />
        </div>
    </div>

</asp:Content>
