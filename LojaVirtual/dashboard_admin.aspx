<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="dashboard_admin.aspx.cs" Inherits="LojaVirtual.dashboard_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body {
            background-color: #f4f6f9;
            font-family: 'Segoe UI', sans-serif;
        }

        .admin-container {
            display: flex;
            flex-wrap: wrap;
            gap: 24px;
            margin: 30px auto;
            padding: 0 15px;
            max-width: 1200px;
        }

        .area {
            background: #fff;
            min-height: 580px;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
        }

        .sidebar {
            background-color: #ffffff;
            border-radius: 12px;
            padding: 20px;
            width: 100%;
            max-width: 240px;
            box-shadow: 0 0 10px rgba(0,0,0,0.05);
            text-align: center;
            flex-shrink: 0;
        }

            .sidebar h4 {
                margin-bottom: 20px;
                font-weight: 600;
                color: #333;
            }

        .user-foto {
            width: 96px;
            height: 96px;
            border-radius: 50%;
            object-fit: cover;
            margin-bottom: 12px;
            border: 3px solid #28a745;
        }

        .btn-msgs, .btn-logout {
            display: block;
            margin: 8px 0;
            padding: 10px;
            text-align: left;
            border-radius: 8px;
            font-weight: 500;
            transition: 0.2s ease-in-out;
            text-decoration: none;
            border: none;
            background: #f8f9fa;
            color: #333;
        }

            .btn-msgs:hover {
                background-color: #d4edda;
                color: #155724;
            }

        .btn-logout {
            background-color: #fff0f0;
            color: #dc3545;
        }

            .btn-logout:hover {
                background-color: #f8d7da;
                color: #721c24;
            }

        .conteudo-painel {
            flex: 1;
            display: flex;
            flex-direction: column;
            gap: 30px;
        }
        /*.admin-sidebar {
            background-color: #f5f5f5;
            padding: 20px;
            border-radius: 12px;
        }

            .admin-sidebar a, link-button {
                display: block;
                margin-bottom: 12px;
                color: #333;
                font-weight: bold;
                text-decoration: none;
            }*/

        /* .admin-content {
            background-color: #fff;
            padding: 20px;
            border-radius: 12px;
        }*/
        /*.area {
            background: #fff;
            min-height: 580px;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
        }*/


        .stat-card {
            display: inline-block;
            width: 180px;
            height: 100px;
            margin-right: 20px;
            background: #e9f5ec;
            border-radius: 10px;
            padding: 15px;
            font-size: 16px;
        }

        .card-title {
            font-weight: 600;
            color: #28a745;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="admin-container">
        <!-- Sidebar -->
        <div class="sidebar">
            <h4>🔧 Administração</h4>
            <asp:LinkButton ID="LinkButtonPainelGeral" runat="server" CssClass="btn-msgs" OnClick="LinkButtonPainelGeral_Click">📊 Painel Geral</asp:LinkButton>
            <asp:LinkButton ID="LinkButtonServices" runat="server" CssClass="btn-msgs" PostBackUrl="https://localhost:44389/hangfire">🔄 Serviços Recorrentes</asp:LinkButton>
            <asp:LinkButton ID="LinkButtonUsers" runat="server" CssClass="btn-msgs" OnClick="LinkButtonUsers_Click">👥 Utilizadores</asp:LinkButton>
            <asp:LinkButton ID="LinkButtonProdutos" runat="server" CssClass="btn-msgs" OnClick="LinkButtonProdutos_Click">🛒 Produtos</asp:LinkButton>
            <asp:LinkButton ID="LinkButtonAvaliacoes" runat="server" CssClass="btn-msgs" OnClick="LinkButtonAvaliacoes_Click">⭐ Avaliações</asp:LinkButton>
            <asp:LinkButton ID="LinkButtonSuporte" runat="server" CssClass="btn-msgs" PostBackUrl="https://dashboard.tawk.to">📩 Suporte</asp:LinkButton>
            <asp:LinkButton ID="btnLogout" runat="server" CssClass="btn-logout" OnClick="btnLogout_Click">
<i class="bi bi-box-arrow-right"></i> Logout
            </asp:LinkButton>
        </div>


        <!-- Conteúdo -->
        <div class="conteudo-painel">
            <asp:UpdatePanel ID="UpdatePanelConteudo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="panelEstatisticas" runat="server" Visible="true">
                        <div class="admin-content">
                            <h3>📊 Estatísticas Gerais</h3>
                            <div class="stat-card">
                                Clientes:
                    <asp:Label ID="lblClientes" runat="server" />
                            </div>
                            <div class="stat-card">
                                Prestadores:
                    <asp:Label ID="lblPrestadores" runat="server" />
                            </div>
                            <div class="stat-card">
                                Serviços:
                    <asp:Label ID="lblServicos" runat="server" />
                            </div>
                            <div class="stat-card">
                                Produtos:
                    <asp:Label ID="lblProdutos" runat="server" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="panelUtilizadores" runat="server" Visible="false">

                        <h3 class="text-center mb-4">👥 Gestão de Utilizadores</h3>

                        <div class="row">
                            <asp:ListView ID="lv_utilizadores" runat="server" OnItemCommand="lv_utilizadores_ItemCommand" OnPagePropertiesChanging="lv_utilizadores_PagePropertiesChanging">
                                <LayoutTemplate>
                                    <div class="table-responsive">
                                        <table class="table table-hover table-bordered align-middle">
                                            <thead class="table-dark">
                                                <tr>
                                                    <th>ID</th>
                                                    <th>Nome</th>
                                                    <th>Email</th>
                                                    <th>Perfil</th>
                                                    <th>Telemóvel</th>
                                                    <th>Status</th>
                                                    <th>Ações</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                            </tbody>
                                        </table>
                                    </div>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td class="fw-bold"><%# Eval("id_utilizador") %> </td>
                                        <td><%# Eval("nome") %> </td>
                                        <td><%# Eval("email") %></td>
                                        <td><%# Eval("nome_perfil") %></td>
                                        <td><%# Eval("telemovel") %></td>
                                        <td>

                                            <span class='<%# Convert.ToBoolean(Eval("ativo")) ? "text-success" : "text-danger" %>'>
                                                <%# Convert.ToBoolean(Eval("ativo")) ? "✔ Ativo" : "❌ Inativo" %>
                                            </span>
                                        </td>

                                        <td>
                                            <div class="btn-group w-100 mt-2">
                                                <asp:Button ID="btn_ativar" runat="server" CommandName="Ativar" CommandArgument='<%# Eval("id_utilizador") %>' Visible='<%# !Convert.ToBoolean(Eval("ativo")) %>'
                                                    CssClass="btn btn-sm btn-success" Text="Ativar" />
                                                <asp:Button ID="btn_desativar" runat="server" CommandName="Desativar" CommandArgument='<%# Eval("id_utilizador") %>' Visible='<%# !Convert.ToBoolean(Eval("ativo")) %>'
                                                    CssClass="btn btn-sm btn-warning" Text="Desativar" />
                                                <asp:Button ID="btn_delete" runat="server" CommandName="Excluir" CommandArgument='<%# Eval("id_utilizador") %>'
                                                    CssClass="btn btn-sm btn-danger" Text="Excluir" />
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td colspan="7" class="text-center text-muted">Nenhum produto encontrado.</td>
                                    </tr>
                                </EmptyDataTemplate>
                            </asp:ListView>
                            <div class="d-flex justify-content-center mt-3">
                                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lv_utilizadores" PageSize="7" CssClass="pagination">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="true" ShowLastPageButton="true" />
                                    </Fields>
                                </asp:DataPager>
                            </div>
                        </div>


                        <div class="alert alert-info text-center mt-3" role="alert">
                            <asp:Label ID="lbl_mensagem" runat="server" CssClass="mb-0"></asp:Label>
                        </div>
                    </asp:Panel>

                    <!--Painel produtos-->
                    <asp:Panel ID="panelProdutos" runat="server" Visible="false">

                        <div class="container mt-4">
                            <h3 class="text-center mb-4">Gestão de Produtos</h3>
                            <div class="d-flex justify-content-end mb-3">
                                <asp:Button ID="btn_inserir" runat="server" Text="Inserir Novo Produto" CssClass="btn btn-outline-dark" OnClick="btn_inserir_Click" />
                            </div>

                            <asp:ListView ID="lv_produtos" runat="server"
                                OnItemEditing="lv_produtos_ItemEditing"
                                OnItemUpdating="lv_produtos_ItemUpdating"
                                OnItemCanceling="lv_produtos_ItemCanceling"
                                OnItemDataBound="lv_produtos_ItemDataBound" DataKeyNames="id_produto">

                                <LayoutTemplate>
                                    <div class="table-responsive">
                                        <table class="table table-hover table-bordered align-middle">
                                            <thead class="table-dark">
                                                <tr>
                                                    <th>ID</th>
                                                    <th>Nome</th>
                                                    <th>Preço</th>
                                                    <th>Stock</th>
                                                    <th>Categoria</th>
                                                    <th>Imagem</th>
                                                    <th>Ações</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                            </tbody>
                                        </table>
                                    </div>
                                </LayoutTemplate>

                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("id_produto") %></td>
                                        <td><%# Eval("nome_produto") %></td>
                                        <td><%# Eval("preco") %></td>
                                        <td><%# Eval("stock") %></td>
                                        <td><%# Eval("nome_categoria") %></td>
                                        <td>
                                            <asp:Image ID="img_produto" runat="server" ImageUrl='<%# "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("imagem")) %>' Width="80" Height="80" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnEditar" runat="server" CommandName="Edit" CommandArgument='<%# Eval("id_produto") %>' CssClass="btn btn-sm btn-primary">Editar</asp:LinkButton>
                                            <asp:LinkButton ID="btnExcluir" runat="server" CommandName="Excluir" CommandArgument='<%# Eval("id_produto") %>' CssClass="btn btn-sm btn-danger">Excluir</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>

                                <EditItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id_produto") %>' /></td>
                                        <td>
                                            <asp:TextBox ID="tb_nome" runat="server" Text='<%# Bind("nome_produto") %>' CssClass="form-control" /></td>
                                        <td>
                                            <asp:TextBox ID="tb_preco" runat="server" Text='<%# Bind("preco") %>' CssClass="form-control" /></td>
                                        <td>
                                            <asp:TextBox ID="tb_stock" runat="server" Text='<%# Bind("stock") %>' CssClass="form-control" /></td>
                                        <td>
                                            <asp:DropDownList ID="ddl_categoria" runat="server" DataSourceID="SqlDataSource2" DataTextField="nome" DataValueField="id_categoria" SelectedValue='<%# Bind("id_categoria") %>' CssClass="form-select" />
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fu_imagem" runat="server" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnAtualizar" runat="server" CommandName="Update" CommandArgument='<%# Eval("id_produto") %>' CssClass="btn btn-success btn-sm">Salvar</asp:LinkButton>
                                            <asp:LinkButton ID="btnCancelar" runat="server" CommandName="Cancel" CssClass="btn btn-warning btn-sm">Cancelar</asp:LinkButton>
                                        </td>
                                    </tr>
                                </EditItemTemplate>

                                <EmptyDataTemplate>
                                    <tr>
                                        <td colspan="7" class="text-center text-muted">Nenhum produto encontrado.</td>
                                    </tr>
                                </EmptyDataTemplate>
                            </asp:ListView>
                            <div class="d-flex justify-content-center mt-3 gap-2">
                                <asp:Button ID="btnAnterior" runat="server" Text="◀ Anterior" OnClick="btnAnterior_Click" CssClass="btn btn-outline-primary" />
                                <asp:Label ID="lblPagina" runat="server" CssClass="align-self-center" />
                                <asp:Button ID="btnProxima" runat="server" Text="Próxima ▶" OnClick="btnProxima_Click" CssClass="btn btn-outline-primary" />
                            </div>

                        </div>
                        <div class="alert alert-info text-center mt-3" role="alert">
                            <asp:Label ID="lbl_msg" runat="server" CssClass="mb-0"></asp:Label>
                        </div>
                    </asp:Panel>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lv_produtos" />
                    <%-- <asp:AsyncPostBackTrigger ControlID="DataPager2" />--%>
                </Triggers>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <!-- Lista de avaliações, botões de moderação -->
                    <asp:Panel ID="panelAvaliacoes" runat="server" Visible="false">
                        <h3>⭐ Moderação de Avaliações</h3>
                        <asp:Repeater ID="rpAvaliacoes" runat="server">
                            <ItemTemplate>
                                <div class="mb-3 p-3 border rounded">
                                    <p><b>Nome Avaliador:</b> <%# Eval("nome_avaliador") %></p>
                                    <p><b>Nome Avaliado:</b> <%# Eval("nome_avaliado") %></p>
                                    <p><b>Nota:</b> <%# Eval("nota") %> ★</p>
                                    <p><b>Comentário:</b> <%# Eval("comentario") %></p>
                                    <asp:Button ID="btnAprovar" runat="server" CommandName="Aprovar" CommandArgument='<%# Eval("id_avaliacao") %>' Text="✅ Aprovar" CssClass="btn btn-success btn-sm" />
                                    <asp:Button ID="btnRecusar" runat="server" CommandName="Recusar" CommandArgument='<%# Eval("id_avaliacao") %>' Text="❌ Remover" CssClass="btn btn-danger btn-sm" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </asp:Panel>

                    <!-- tickets de suporte  -->
                    <asp:Panel ID="panelSuporte" runat="server" Visible="false">

                        <h3>📩 Gestão de Tickets de Suporte</h3>
                        <asp:Repeater ID="rpSuporte" runat="server">
                            <ItemTemplate>
                                <div class="mb-3 p-3 border rounded">
                                    <p><b>Usuário:</b> <%# Eval("nome_usuario") %></p>
                                    <p><b>Assunto:</b> <%# Eval("assunto") %></p>
                                    <p><b>Mensagem:</b> <%# Eval("mensagem") %></p>
                                    <p><b>Data:</b> <%# Eval("data_envio", "{0:dd/MM/yyyy HH:mm}") %></p>
                                    <asp:Button ID="btnResponder" runat="server" CommandName="Responder" CommandArgument='<%# Eval("id_ticket") %>' Text="Responder" CssClass="btn btn-outline-primary btn-sm" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
            ConnectionString="<%$ ConnectionStrings:LojaVirtualConnectionString %>"
            SelectCommand="SELECT id_categoria, nome FROM Categorias"></asp:SqlDataSource>
</asp:Content>
