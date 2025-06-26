<%@ Page Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="loja.aspx.cs" Inherits="LojaVirtual.loja" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Loja Virtual</title>
    <style>
        /* Container da loja */
        .container {
            display: flex;
            justify-content: center;
            align-items: center;
            flex-wrap: wrap;
            gap: 20px;
            padding: 20px;
            margin: 0 auto;
            max-width: 1200px;
        }

        /* Estilo do card do produto */
        /* .card {
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            align-items: center;
            width: 18rem;
            background-color: #fff;
            border-radius: 12px;
            padding: 16px;
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1);
            transition: transform 0.3s, box-shadow 0.3s;
        }

            .card:hover {
                transform: translateY(-5px);
                box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
            }

            .card img, .image-button {
                border-radius: 8px;
                width: 100%;
                height: auto;
                max-height: 180px;
                object-fit: cover;
                margin-bottom: 12px;
                transition: transform 0.3s, box-shadow 0.3s;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            }

                .image-button:hover, .card img:hover {
                    transform: scale(1.05);
                    box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
                }

            .card label {
                font-weight: bold;
                margin: 4px 0;
                color: #333;
                font-size: 16px;
            }*/


        /* Botão do carrinho */
        .btn-carrinho {
            margin-top: 8px;
            padding: 8px 16px;
            background-color: #28a745;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            transition: background-color 0.3s;
            width: 100%;
        }

            .btn-carrinho:hover {
                background-color: #218838;
            }

        /* Botão de pesquisa */
        .btn-pesquisa {
            padding: 8px 16px;
            margin-left: 8px;
            background-color: #28a745;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

            .btn-pesquisa:hover {
                background-color: #218838;
            }

        /* Ordenação e pesquisa */
        .ordenacao_pesquisa-container {
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 12px;
            margin: 20px auto;
        }

        .textbox {
            padding: 8px;
            border-radius: 8px;
            border: 1px solid #ddd;
            font-size: 14px;
            outline: none;
            transition: border-color 0.3s;
            width: 200px;
        }

        .texbox:focus {
            border-color: #007bff;
        }

        select {
            padding: 8px;
            border-radius: 8px;
            border: 1px solid #ddd;
            font-size: 14px;
            outline: none;
            transition: border-color 0.3s;
        }

            select:focus {
                border-color: #007bff;
            }

        /* Paginação */
        .pagination {
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 8px;
            margin: 20px auto;
        }

            .pagination a, .pagination button {
                padding: 8px 16px;
                margin: 0 4px;
                background-color: #007bff;
                color: white;
                border: none;
                border-radius: 8px;
                cursor: pointer;
                transition: background-color 0.3s;
                text-decoration: none;
            }

                .pagination a:hover, .pagination button:hover {
                    background-color: #0056b3;
                }

            .pagination .current-page {
                background-color: #0056b3;
                color: white;
                font-weight: bold;
            }

        .preco-riscado {
            text-decoration: line-through;
            color: red;
            margin-right: 10px;
            font-size: 14px;
        }

        .preco-desconto {
            font-weight: bold;
            color: green;
            font-size: 16px;
        }

        .preco-normal {
            font-weight: bold;
            color: black;
            font-size: 16px;
        }


        /* Responsividade */
        @media (max-width: 768px) {
            .container {
                flex-direction: column;
                align-items: center;
            }

            .card {
                width: 90%;
            }

            .ordenacao_pesquisa-container, .pagination {
                flex-direction: column;
                gap: 8px;
            }

                .pagination button, .pagination a {
                    width: 100px;
                }
        }
    </style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Main_Content" runat="server">



    <!-- Dropdown para ordenacao -->
    <div class="ordenacao_pesquisa-container">
        <label>Ordenar por:</label>
        <asp:DropDownList ID="ddlOrdenacao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOrdenacao_SelectedIndexChanged" CssClass="btn btn-sm btn-secondary dropdown-toggle">
            <asp:ListItem Value="nome ASC">Nome (A-Z)</asp:ListItem>
            <asp:ListItem Value="nome DESC">Nome (Z-A)</asp:ListItem>
            <asp:ListItem Value="preco ASC">Preço (Menor para Maior)</asp:ListItem>
            <asp:ListItem Value="preco DESC">Preço (Maior para Menor)</asp:ListItem>
        </asp:DropDownList>

        <!-- campo de Pesquisa -->
        <div class="input-group-sm w-auto">
            <asp:TextBox ID="tb_pesquisa" runat="server" placeholder="Buscar produto..." CssClass="form-control-sm w-auto"></asp:TextBox>
        </div>
        <asp:Button ID="btn_pesquisa" runat="server" Text="Pesquisar" CssClass="btn btn-outline-secondary" OnClick="btn_pesquisa_Click" />

        
    </div>

    <section class="container">
        <asp:ListView ID="ListView1" runat="server" OnPagePropertiesChanging="ListView1_PagePropertiesChanging">
            <ItemTemplate>
                <%--<div class="card">--%>
                <div class="card overflow-hidden card-hover-zoom d-flex" style="width: 15rem; height: 22rem;">
                    <asp:ImageButton ID="Image_produto" runat="server"
                        ImageUrl='<%# Eval("ImagemBase64") %>'
                        CommandName="VerDetalhes"
                        CommandArgument='<%# Eval("id_produto") %>'
                        OnCommand="Image_produto_Command"
                        CssClass="-img-top object-fit-cover"
                        Style="height: 220px; width: 100%" />


                    <hr class="m-2" />
                    <div class="d-flex justify-content-center" >
                        <asp:Label ID="lbl_nome" runat="server" Text='<%# Eval("nome") %>' CssClass="product-name"></asp:Label>
                    </div>
                    <div class="card-body flex-grow-1 flex-column d-flex pt-0">
                        <div class="d-flex flex-grow-1 flex-column" style="align-items: center;">

                            <asp:Label ID="lbl_preco" runat="server" Text='<%# Eval("preco", "{0:C}") %>' CssClass="preco-riscado" Visible='<%# Session["cod_perfil"] != null && Session["cod_perfil"].ToString() == "3" %>'></asp:Label>

                            <asp:Label ID="lbl_precoDesconto" runat="server" Text='<%# Eval("preco_desconto") %>' CssClass="preco-desconto" Visible='<%# Session["cod_perfil"] != null && Session["cod_perfil"].ToString() == "3" %>'></asp:Label>

                            <asp:Label ID="lbl_precoNormal" runat="server" Text='<%# Eval("preco", "{0:C}") %>' CssClass="preco-normal" Visible='<%# Session["cod_perfil"] == null || Session["cod_perfil"].ToString() != "3" %>'></asp:Label>


                            <asp:Button ID="btn_adicionar" runat="server" Text="Adicionar ao Carrinho" CssClass="btn btn-sm btn-primary mt-3" OnClick="btn_adicionar_Click" CommandArgument='<%# Eval("id_produto") %>' />

                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </section>
    <!-- Paginação -->
    <section class="pagination">
        <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="18">
            <Fields>
                <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowLastPageButton="False" ButtonCssClass="btn btn-outline-primary" />
                <asp:NumericPagerField ButtonType="Button" NextPreviousButtonCssClass="btn btn-outline-primary" CurrentPageLabelCssClass="btn btn-outline-primary" NumericButtonCssClass="btn btn-outline-primary" />
            </Fields>
        </asp:DataPager>
    </section>

</asp:Content>
