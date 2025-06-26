<%@ Page Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="carrinho.aspx.cs" Inherits="LojaVirtual.Carrinho" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Carrinho</title>

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="Main_Content" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <style>
        .container {
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 16px;
            flex-wrap: wrap;
            gap: 16px;
            margin: 0 auto;
            max-width: 100%;
        }

        .cart {
            width: 90%;
            max-width: 1000px;
            background-color: #fff;
            border-radius: 12px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            padding: 16px;
        }

        .quantity-controls {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 5px;
        }
     

        h1 {
            text-align: center;
            margin-bottom: 16px;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            margin: 0 auto;
        }

        th, td {
            padding: 12px;
            text-align: center;
            border: 1px solid #ddd;
        }

        th {
            background-color: #f2f2f2;
            font-weight: bold;
        }

        td img {
            max-width: 100%;
            height: auto;
            border-radius: 8px;
        }

        .total {
            margin-top: 16px;
            text-align: right;
            font-size: 1.2em;
            font-weight: bold;
        }

        .cart-item {
            display: flex;
            justify-content: flex-start;
            align-items: center;
            margin-bottom: 15px;
            border-bottom: 1px solid #eee;
            padding-bottom: 15px;
        }

        .cart-item-details {
            flex: 1;
            padding: 0 20px;
        }

        .mensagem-vazia {
            color: red;
            font-weight: bold;
            text-align: center;
            margin-top: 20px;
        }

        @media (max-width: 768px) {
            table, th, td {
                font-size: 14px;
            }

            th, td {
                padding: 8px;
            }

            .container {
                padding: 8px;
            }

            .cart {
                width: 100%;
            }
        }
    </style>

    <section class="container my-5">
        <div class="card shadow-sm p-4">
            <h2 class="text-center mb-4">🛒 Carrinho de Compras</h2>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound" OnItemCommand="Repeater1_ItemCommand">
                        <HeaderTemplate>
                            <div class="table-responsive">
                                <table class="table table-bordered align-middle text-center">
                                    <thead class="table-dark">
                                        <tr>
                                            <th style="width: 35%">Produto</th>
                                            <th style="width: 15%">Preço</th>
                                            <th style="width: 50%">Quantidade</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr>
                                <td class="text-start">
                                    <div class="d-flex align-items-center">
                                        <asp:Image ID="Image_produto" runat="server" CssClass="img-thumbnail me-3" Width="80" Height="80" ImageUrl='<%# Eval("imagem") %>' />
                                        <asp:Label ID="lbl_nome" runat="server" CssClass="fw-bold" Text='<%# Eval("nome") %>'></asp:Label>
                                    </div>
                                </td>

                                <td>
                                    <asp:Label ID="lbl_preco" runat="server" CssClass="text-success fw-semibold" Text='<%# Eval("preco", "{0:C}") %>'></asp:Label>
                                </td>

                                <td>
                                    <div class="btn-group" role="group">
                                        <asp:Button ID="btnDiminuir" runat="server" Text="−" CommandName="diminuir" CommandArgument='<%# Eval("id_produto") %>' CssClass="btn btn-outline-secondary" />
                                        <asp:Label ID="lbl_quantidade" runat="server" CssClass="px-3 fw-bold align-self-center" Text='<%# Eval("quantidade") %>'></asp:Label>
                                        <asp:Button ID="btnAumentar" runat="server" Text="+" CommandName="aumentar" CommandArgument='<%# Eval("id_produto") %>' CssClass="btn btn-outline-secondary" />
                                        <asp:Button ID="btnExcluir" runat="server" Text="🗑️" CommandName="excluir" CommandArgument='<%# Eval("id_produto") %>' CssClass="btn btn-outline-danger" />
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>

                        <FooterTemplate>
                            </tbody>
                            </table>
                        </div>
                        </FooterTemplate>
                    </asp:Repeater>

                    <div class="mt-4 d-flex justify-content-between align-items-center">
                        <div>
                            <asp:Label ID="lbl_mensagem" runat="server" CssClass="text-muted"></asp:Label>
                        </div>
                        <div class="text-end">
                            <h5>Total:
                                <asp:Label ID="lbl_total" runat="server" CssClass="text-success fw-bold"></asp:Label></h5>
                            <asp:Button ID="btnCheckout" runat="server" Text="Ir para o Checkout" CssClass="btn btn-primary px-4 mt-2" OnClick="btnCheckout_Click" />
                        </div>
                    </div>
                </ContentTemplate>

                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Repeater1" EventName="ItemCommand" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </section>
</asp:Content>
