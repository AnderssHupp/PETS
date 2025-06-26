<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="dashboard_cliente.aspx.cs" Inherits="LojaVirtual.dashboard_cliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css">

    <style>
        body {
            background-color: #f4f6f9;
            font-family: 'Segoe UI', sans-serif;
        }

        .painel-container {
            display: flex;
            flex-wrap: wrap;
            gap: 24px;
            margin: 30px auto;
            padding: 0 15px;
            max-width: 1200px;
        }

        .meios-pagamento-panel {
            background-color: #fff;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
            margin-bottom: 20px;
        }

        .cartao-item {
            background-color: #f9f9f9;
            padding: 16px;
            margin-top: 10px;
            border-radius: 10px;
            border-left: 4px solid #28a745;
            transition: box-shadow 0.3s;
        }

        .reserva-card:hover {
            box-shadow: 0 6px 12px rgba(0,0,0,0.1);
        }

        .anuncios-area, .candidaturas-area {
            background: #fff;
            min-height: 580px;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
        }

        .grid-anuncios {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
            gap: 16px;
        }

        .anuncio-card {
            border: 1px solid #e1e1e1;
            border-radius: 8px;
            padding: 16px;
            text-align: center;
            transition: all 0.3s ease-in-out;
            background: #f9f9f9;
        }

            .anuncio-card:hover {
                transform: translateY(-5px);
                box-shadow: 0 6px 16px rgba(0, 0, 0, 0.08);
            }

            .anuncio-card .btn-painel {
                margin: 4px;
                background-color: #007bff;
                color: white;
                border: none;
                padding: 6px 12px;
                border-radius: 6px;
            }

                .anuncio-card .btn-painel:hover {
                    background-color: #0056b3;
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
            gap: 20px;
        }

        .acoes-rapidas .btn {
            margin-right: 10px;
            background-color: #28a745;
            border: none;
            color: white;
        }

            .acoes-rapidas .btn:hover {
                background-color: #218838;
            }

        .card {
            transition: transform 0.2s;
        }

            .card:hover {
                transform: translateY(-5px);
                box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
            }


        .text-truncate-multiline {
            display: -webkit-box;
            -webkit-line-clamp: 3;
            -webkit-box-orient: vertical;
            overflow: hidden;
            text-overflow: ellipsis;
        }


        @media (max-width: 576px) {
            .row-cols-1 > * {
                flex: 0 0 100%;
                max-width: 100%;
            }
        }


        .card {
            min-height: 500px;
        }


        .card-edit {
            border: 2px solid #ffc107 !important;
            box-shadow: 0 0 10px rgba(255, 193, 7, 0.3);
        }


        .edit-scroll-container {
            overflow-y: auto;
            max-height: 300px;
            padding-right: 5px;
        }


            .edit-scroll-container::-webkit-scrollbar {
                width: 5px;
            }

            .edit-scroll-container::-webkit-scrollbar-thumb {
                background-color: #ffc107;
                border-radius: 10px;
            }


        .image-thumb {
            width: 100%;
            height: 150px;
            object-fit: cover;
            border-radius: 8px;
            margin-bottom: 10px;
        }


        .candidatura-card {
            background-color: #eef9f0;
            border: 1px solid #d4edda;
            border-radius: 8px;
            padding: 16px;
            margin-bottom: 16px;
        }

            .candidatura-card button {
                margin-right: 8px;
                background-color: #007bff;
                color: white;
                border: none;
                padding: 6px 12px;
                border-radius: 6px;
            }

            .candidatura-card .btn-recusar {
                background-color: #dc3545;
            }

                .candidatura-card .btn-recusar:hover {
                    background-color: #c82333;
                }

        .anuncios-grid-container {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(18rem, 1fr));
            gap: 1rem;
            width: 100%;
        }
        /* Chat */
        .container {
            display: flex;
            text-align: center;
        }

        .chat-users {
            width: 280px;
            min-height: 500px;
            background-color: #f8f9fa;
            padding: 15px;
            border-right: 1px solid #ddd;
            overflow-y: auto;
        }

            .chat-users a {
                display: block;
                padding: 12px;
                margin-bottom: 8px;
                background-color: #fff;
                border-radius: 8px;
                font-weight: 600;
                cursor: pointer;
                text-decoration: none;
                color: #333;
                transition: all 0.3s;
            }

                .chat-users a:hover,
                .chat-users a.active {
                    background-color: #d4edda;
                    color: #155724;
                }

        .chat-content {
            flex: 1;
            display: flex;
            flex-direction: column;
            background: #e9ecef;
            padding: 16px;
        }

        .chat-messages {
            flex: 1;
            overflow-y: auto;
            border-radius: 8px;
            display: flex;
            flex-direction: column;
            gap: 12px;
            animation: fadeIn 0.5s;
            padding: 10px;
            background-color: #f8f9fa;
            border: 1px solid #ddd;
            border-radius: 8px;
            margin-bottom: 10px;
        }

        .message {
            display: inline-block;
            padding: 10px;
            border-radius: 12px;
            word-wrap: break-word;
            border: 1px solid #ccc;
            max-width: 70%; /* Limita o tamanho para mensagens longas */
            min-width: 50px; /* Evita balões minúsculos */
            background-color: #f1f1f1;
        }

        .message-wrapper {
            margin: 10px 0;
            clear: both;
            display: inline-block;
            max-width: 100%;
        }

            .message-wrapper.sent {
                float: right;
                text-align: right;
            }

            .message-wrapper.received {
                float: left;
                text-align: left;
            }

        .nome-remetente {
            font-weight: bold;
            margin-bottom: 4px;
            color: #555;
        }



        .message-wrapper.sent .message {
            background-color: #dcf8c6;
            border-color: #c2e6b8;
        }

        .message-wrapper.received .message {
            background-color: #ffffff;
        }

        .chat-user-card {
            display: flex;
            align-items: center;
            justify-content: space-between;
            background: #fff;
            padding: 8px 12px;
            border-radius: 8px;
            margin-bottom: 10px;
            transition: background-color 0.3s;
            border: 1px solid #eee;
        }

            .chat-user-card:hover {
                background-color: #e2f5e9;
            }

        .user-info {
            display: flex;
            align-items: center;
            text-decoration: none;
        }

        .user-photo {
            width: 40px;
            height: 40px;
            border-radius: 50%;
            object-fit: cover;
            margin-right: 10px;
            border: 1px solid #ccc;
        }

        .user-name {
            font-weight: 600;
            color: #333;
        }

        .user-link {
            text-decoration: none;
            color: inherit;
            flex-grow: 1;
        }

        .whatsapp-link img {
            width: 24px;
            height: 24px;
            opacity: 0.9;
        }

        .whatsapp-link:hover img {
            opacity: 1;
            /*transform: scale(1.05);*/
        }

        .chat-input {
            display: flex;
            gap: 10px;
        }

            .chat-input textarea,
            .chat-input input[type="text"] {
                flex-grow: 1;
                border-radius: 8px;
                padding: 10px;
                border: 1px solid #ccc;
            }

            .chat-input button,
            .chat-input input[type="submit"] {
                padding: 10px 16px;
                border-radius: 8px;
                border: none;
                background-color: #28a745;
                color: white;
                cursor: pointer;
                transition: background-color 0.3s;
            }

                .chat-input button:hover {
                    background-color: #218838;
                }

        @media (max-width: 992px) {
            .painel-container {
                flex-direction: column;
            }

            .sidebar {
                width: 100%;
                max-width: none;
            }

            .chat-container {
                flex-direction: column;
                height: auto;
            }

            .chat-users {
                width: 100%;
                border-right: none;
                border-bottom: 1px solid #ccc;
            }
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="painel-container">
        <div class="sidebar">
            <asp:Image ID="img_foto" runat="server" CssClass="user-foto" />
            <h4>
                <asp:Label ID="lbl_nome" runat="server"></asp:Label></h4>
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btn_msgs_Click" CssClass="btn-msgs"><img src="icones/mensagens.png" style="width:16px;height:16px;" /> Mensagens</asp:LinkButton>

            <asp:LinkButton ID="btnPerfil" runat="server" OnClick="btnPerfil_Click" CssClass="btn-msgs">
    <img src="icones/usuario-de-perfil.png" style="width:16px;height:16px;" /> Perfil
            </asp:LinkButton>

            <asp:LinkButton ID="btnAnuncios" runat="server" OnClick="btnAnuncios_Click" CssClass="btn-msgs">
    <img src="icones/anuncio.png" style="width:16px;height:16px;" /> Anuncios
            </asp:LinkButton>

            <asp:LinkButton ID="btnPets" runat="server" CssClass="btn-msgs" OnClick="btnPets_Click">
    <img src="icones/patas (1).png" style="width:16px;height:16px;" /> Meus Pets
            </asp:LinkButton>

            <asp:LinkButton ID="btnHistorico" runat="server" OnClick="btnHistorico_Click" CssClass="btn-msgs">
    <img src="icones/historia.png" style="width:16px;height:16px;"/> Histórico de Serviços
            </asp:LinkButton>

            <asp:LinkButton ID="btnPagamentos" runat="server" CssClass="btn-msgs" OnClick="btnPagamentos_Click">
    <img src="icones/credit-card.png"  style="width:16px;height:16px;"/> Meios de Pagamentos
            </asp:LinkButton>

            <asp:LinkButton ID="btnLogout" runat="server" CssClass="btn-logout" OnClick="btnLogout_Click">
    <i class="bi bi-box-arrow-right"></i> Logout
            </asp:LinkButton>
        </div>
        <div class="conteudo-painel">
            <div class="acoes-rapidas">
                <asp:Button ID="btn_novoAnuncio" CssClass="btn btn-secondary" runat="server" Text="➕ Criar Novo Anúncio" OnClick="btn_novoAnuncio_Click" />
                <asp:Button ID="btn_pet" runat="server" CssClass="btn btn-secondary" Text="➕ Cadastrar Pet" OnClick="btn_pet_Click" />
            </div>


            <!--anuncios -->
            <div class="anuncios-area">
                <div>
                    <asp:UpdatePanel ID="upAnuncios" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="panel_anuncios" runat="server" CssClass="d-block">
                                <h3>Meus Anúncios</h3>

                                <div class="d-grid">
                                    <asp:ListView ID="lvAnuncios" runat="server" OnPagePropertiesChanging="lvAnuncios_PagePropertiesChanging" OnItemCommand="lvAnuncios_ItemCommand" OnItemEditing="lvAnuncios_ItemEditing"
                                        OnItemUpdating="lvAnuncios_ItemUpdating"
                                        OnItemCanceling="lvAnuncios_ItemCanceling" DataKeyNames="id_anuncio">

                                        <LayoutTemplate>
                                            <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 row-cols-xl-4 g-4">
                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                            </div>
                                        </LayoutTemplate>

                                        <ItemTemplate>

                                            <%--<div class="card overflow-hidden card-hover-zoom d-flex ms-3" style="width: 20rem; height: 40rem;">--%>
                                            <div class="col" style="width: 18rem;">
                                                <div class="card h-100 w-100">
                                                    <%--<div class="d-flex w-100 h-100" style="flex-direction: column">--%>
                                                    <asp:Image ID="ImagePet" runat="server"
                                                        ImageUrl='<%# Eval("ImagemBase64") %>'
                                                        CommandArgument='<%# Eval("id_anuncio") %>'
                                                        CssClass="-img-top object-fit-cover"
                                                        Style="height: 200px;" />

                                                    <div class="card-body d-flex flex-column">
                                                        <h5 class="card-title"><%# Eval("titulo") %></h5>

                                                        <div class="card-text mb-2">
                                                            <p class="mb-1"><small class="text-muted"><b>Tipo Serviço:</b> <%# Eval("TipoServico") %></small></p>
                                                            <p class="mb-1 text-truncate" title='<%# Eval("descricao") %>'><small><b>Descrição:</b> <%# Eval("descricao") %></small></p>
                                                            <p class="mb-1"><small><b>Data:</b> <%# Eval("data_nece", "{0:dd/MM/yyyy}") %></small></p>
                                                            <p class="mb-1"><small><b>Hora:</b> <%# Eval("hora_nece", @"{0:hh\:mm}") %></small></p>
                                                            <p class="mb-1 text-truncate" title='<%# Eval("localidade") %>'><small><b>Local:</b> <%# Eval("localidade") %></small></p>
                                                            <p class="mb-1"><small><b>Preço:</b> €<%# Eval("preco", "{0:N2}") %></small></p>
                                                        </div>
                                                        <div class="mt-auto">
                                                            <div class="d-grid gap-2 d-md-flex justify-content-md-between">
                                                                <asp:LinkButton
                                                                    ID="btnEditar"
                                                                    runat="server"
                                                                    CommandName="Edit"
                                                                    CommandArgument='<%# Eval("id_anuncio") %>'
                                                                    CssClass="btn btn-sm btn-outline-warning mb-2">
                                                        <img src="icones/editar-codigo.png" style="width:16px;height:16px;" /> Editar
                                                                </asp:LinkButton>

                                                                <asp:LinkButton ID="btnRemover" runat="server" CommandName="Remover" CommandArgument='<%# Eval("id_anuncio") %>' CssClass="btn btn-sm btn-outline-danger mb-2"><img src="icones/excluir.png" style="width:16px;height:16px;"/> Remover</asp:LinkButton>
                                                            </div>
                                                            <%--  <asp:Button ID="btnVerCandidaturas" runat="server" Text="Ver Candidaturas" CommandName="VerCandidaturas" CommandArgument='<%# Eval("id_anuncio") %>' CssClass="btn btn-sm btn-outline-secondary w-100 mt-2" />--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>

                                        <EditItemTemplate>
                                            <!-- mode edit -->
                                            <div class="col" style="width: 18rem;">
                                                <div class="card h-100 w-100 border-warning">
                                                    <asp:Image ID="ImagePetEdit" runat="server"
                                                        ImageUrl='<%# Eval("ImagemBase64") %>'
                                                        CssClass="card-img-top img-fluid"
                                                        Style="height: 200px; object-fit: cover;" />
                                                    <%--<asp:FileUpload ID="fileNewFoto" />--%>

                                                    <div class="card-body d-flex flex-column" style="overflow-y: auto; max-height: 300px;">
                                                        <div class="mb-2">
                                                            <label class="form-label">Título</label>
                                                            <asp:TextBox ID="tbTitulo" runat="server"
                                                                Text='<%# Bind("titulo") %>'
                                                                CssClass="form-control" />
                                                        </div>

                                                        <div class="mb-2">
                                                            <label class="form-label small">Tipo de Serviço</label>
                                                            <asp:DropDownList ID="ddl_servicos" runat="server"
                                                                DataSourceID="SqlDataSource1"
                                                                DataTextField="nome_servico"
                                                                DataValueField="cod_tipoServico"
                                                                CssClass="form-select"
                                                                SelectedValue='<%# Bind("cod_TipoServico") %>'>
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:LojaVirtualConnectionString %>' SelectCommand="SELECT * FROM [TipoServico]"></asp:SqlDataSource>
                                                        </div>

                                                        <div class="mb-2">
                                                            <label class="form-label small">Descrição</label>
                                                            <asp:TextBox ID="tbDescricao" runat="server"
                                                                Text='<%# Bind("descricao") %>'
                                                                TextMode="MultiLine"
                                                                Rows="2"
                                                                CssClass="form-control form-control-sm" />
                                                        </div>

                                                        <div class="row g-2 mb-3">
                                                            <div class="col-md-6">
                                                                <label class="form-label">Data</label>
                                                                <asp:TextBox ID="tbData" runat="server"
                                                                    Text='<%# Bind("data_nece", "{0:yyyy-MM-dd}") %>'
                                                                    TextMode="Date"
                                                                    CssClass="form-control" />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <label class="form-label">Hora</label>
                                                                <asp:TextBox ID="tbHora" runat="server"
                                                                    Text='<%# Bind("hora_nece") %>'
                                                                    TextMode="Time"
                                                                    CssClass="form-control" />
                                                            </div>
                                                        </div>

                                                        <div class="mb-3">
                                                            <label class="form-label">Localidade</label>
                                                            <asp:TextBox ID="tbLocalidade" runat="server"
                                                                Text='<%# Bind("localidade") %>'
                                                                CssClass="form-control" />
                                                        </div>

                                                        <div class="mb-3">
                                                            <label class="form-label">Preço (€)</label>
                                                            <asp:TextBox ID="tbPreco" runat="server"
                                                                Text='<%# Bind("preco", "{0:N2}") %>'
                                                                TextMode="Number"
                                                                step="0.01"
                                                                CssClass="form-control" />
                                                        </div>
                                                        <div class="d-flex" style="justify-content: space-around">
                                                            <asp:LinkButton ID="btnAtualizar" runat="server" CommandName="Update" CommandArgument='<%# Eval("id_anuncio") %>'><img src="icones/confirme.png" style="width:20px;height:20px;" /></asp:LinkButton>

                                                            <asp:LinkButton ID="btnCancelar" runat="server" CommandName="Cancel"><img src="icones/cancelar.png" style="width:20px;height:20px;" /></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </EditItemTemplate>
                                    </asp:ListView>
                                </div>
                                <section class="d-flex justify-content-center mt-3">
                                    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lvAnuncios" PageSize="3">
                                        <Fields>
                                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowLastPageButton="False" ButtonCssClass="btn btn-outline-primary" />
                                            <asp:NumericPagerField ButtonType="Button" NextPreviousButtonCssClass="btn btn-outline-primary" CurrentPageLabelCssClass="btn btn-outline-primary" NumericButtonCssClass="btn btn-outline-primary" />
                                        </Fields>
                                    </asp:DataPager>
                                </section>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lvAnuncios" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="uP" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="panelCandidatura" runat="server" Visible="false">
                                <div class="mt-3">
                                    <h3>Candidaturas Recebidas</h3>

                                    <asp:Repeater ID="rpCandidaturas" runat="server">
                                        <ItemTemplate>
                                            <div class="candidatura-card">
                                                <p><b>Prestador:</b> <%# Eval("nome_prestador") %></p>
                                                <p><b>Mensagem:</b> <%# Eval("mensagem") %></p>
                                                <p><b>Titulo:</b> <%# Eval("titulo") %></p>

                                                <asp:LinkButton
                                                    ID="btnAceitar"
                                                    runat="server"
                                                    Text="Aceitar"
                                                    CssClass="btn btn-success"
                                                    OnClick="btnAceitar_Click"
                                                    CommandArgument='<%# Eval("id_candidatura") %>' />

                                                <asp:Button
                                                    ID="btnRecusar"
                                                    runat="server"
                                                    Text="Recusar"
                                                    CssClass="btn btn-danger"
                                                    OnClick="btnRecusar_Click"
                                                    CommandArgument='<%# Eval("id_candidatura") %>' />
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                    <asp:Label ID="lbl_msg" runat="server" />
                                    <%--<asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" CssClass="btn btn-secondary" />--%>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>



                <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:Panel ID="panel_msgs" runat="server" Visible="false">
                            <div class="anuncios-area p-0 m-0">
                                <div class="container p-0 m-0">

                                    <div class="chat-users">
                                        <asp:Repeater ID="rp_usuarios" runat="server" OnItemCommand="rp_usuarios_ItemCommand">
                                            <ItemTemplate>
                                                <div class="chat-user-card">
                                                    <asp:LinkButton ID="btn_usuario" runat="server" CommandName="Selecionar" CommandArgument='<%# Eval("id_contato") %>' CssClass="user-link">
                                            <div class="user-info">
                                            <img src='<%# Eval("FotoBase64") %>' alt="Foto"
                                                                    class="user-photo" />
                                            <span class="user-name"><%# Eval("nome") %></span>
                                                </div>
                                                    </asp:LinkButton>

                                                    <%-- WhatsApp Button --%>
                                                    <a href='<%# "https://wa.me/351" + Eval("telemovel") %>' target="_blank" title="Conversar via WhatsApp">
                                                        <img src="https://img.icons8.com/color/24/whatsapp.png" alt="WhatsApp" />
                                                    </a>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div class="d-flex flex-grow-1" style="max-height: 500px;">

                                        <!-- conversa -->
                                        <div class="chat-content">
                                            <div id="mensagensBox" class="chat-messages">
                                                <asp:Repeater ID="rp_mensagens" runat="server">
                                                    <ItemTemplate>

                                                        <div class='message-wrapper <%# (Eval("id_remetente").ToString() == Session["user_id"].ToString()) ? "sent" : "received" %>'>
                                                            <div class='nome-remetente'>
                                                                <b><%# Eval("nome_remetente") %>:</b>
                                                            </div>
                                                            <div class='message'>
                                                                <%# Eval("mensagem") %>
                                                                <small><i><%# Eval("data_envio", "{0:HH:mm}") %></i></small>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>

                                            <div class="chat-input">
                                                <textarea id="tb_mensagem" name="tb_mensagem" rows="2" onkeydown="handleKey(event)"></textarea>
                                                <%--<asp:TextBox ID="tb_mensagem" runat="server" TextMode="MultiLine" Rows="2" />--%>
                                                <asp:Button ID="btn_enviar" runat="server" Type="submit" Text="Enviar" OnClick="btn_enviar_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                        </asp:Panel>
                        <!--MEUS PETS-->
                        <asp:Panel ID="panelPets" runat="server" Visible="false">
                            <div class="anuncios-area">
                                <h3>Meus Pets</h3>
                                <asp:ListView ID="lvPets" runat="server"
                                    DataKeyNames="id_pet" OnPagePropertiesChanging="lvPets_PagePropertiesChanging"
                                    OnItemEditing="lvPets_ItemEditing"
                                    OnItemUpdating="lvPets_ItemUpdating"
                                    OnItemCanceling="lvPets_ItemCanceling"
                                    OnItemCommand="lvPets_ItemCommand">

                                    <LayoutTemplate>
                                        <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 row-cols-xl-4 g-4">
                                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                        </div>
                                    </LayoutTemplate>

                                    <ItemTemplate>
                                        <div class="col" style="width: 18rem;">
                                            <div class="card h-100 w-100">
                                                <asp:Image ID="imgPet" runat="server"
                                                    ImageUrl='<%# Eval("ImagemBase64") %>'
                                                    CssClass="card-img-top object-fit-cover"
                                                    Style="height: 200px;" />

                                                <div class="card-body">
                                                    <h5 class="card-title"><%# Eval("nome_pet") %></h5>
                                                    <p><b>Espécie:</b> <%# Eval("especie") %></p>
                                                    <p><b>Raça:</b> <%# Eval("raca") %></p>
                                                    <p><b>Idade:</b> <%# Eval("idade") %> anos</p>
                                                    <p><b>Peso:</b> <%# Eval("peso") %> kg</p>
                                                    <p><b>Observações:</b> <%# Eval("observacoes") %></p>


                                                    <div class="d-flex justify-content-center gap-2">
                                                        <asp:LinkButton
                                                            ID="btnEditar2"
                                                            runat="server"
                                                            CommandName="Edit"
                                                            CommandArgument='<%# Eval("id_pet") %>'
                                                            CssClass="btn btn-sm btn-outline-warning mb-2">
                                                    <img src="icones/editar-codigo.png" style="width:16px;height:16px;" /> Editar
                                                        </asp:LinkButton>

                                                        <asp:LinkButton ID="btnExcluir" runat="server" CommandName="Remover" CommandArgument='<%# Eval("id_pet") %>' CssClass="btn btn-sm btn-outline-danger mb-2"><img src="icones/excluir.png" style="width:16px;height:16px;"/> Excluir</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="col" style="width: 18rem;">
                                            <div class="card h-100 w-100 border-warning">
                                                <asp:Image ID="imgPetEdit" runat="server"
                                                    ImageUrl='<%# Eval("ImagemBase64") %>'
                                                    CssClass="card-img-top object-fit-cover"
                                                    Style="height: 200px;" />

                                                <div class="card-body d-flex flex-column" style="overflow-y: auto; max-height: 300px;">
                                                    <div class="mb-2">
                                                        <label>Nome:</label>
                                                        <asp:TextBox ID="tbNomePet" runat="server" Text='<%# Bind("nome_pet") %>' CssClass="form-control" />
                                                    </div>
                                                    <div class="mb-2">
                                                        <label>Espécie:</label>
                                                        <asp:TextBox ID="tbEspecie" runat="server" Text='<%# Bind("especie") %>' CssClass="form-control" />
                                                    </div>
                                                    <div class="mb-2">
                                                        <label>Raça:</label>
                                                        <asp:TextBox ID="tbRaca" runat="server" Text='<%# Bind("raca") %>' CssClass="form-control" />
                                                    </div>
                                                    <div class="mb-2">
                                                        <label>Idade:</label>
                                                        <asp:TextBox ID="tbIdade" runat="server" Text='<%# Bind("idade") %>' TextMode="Number" CssClass="form-control" />
                                                    </div>
                                                    <div class="mb-2">
                                                        <label>Peso:</label>
                                                        <asp:TextBox ID="tbPeso" runat="server" Text='<%# Bind("peso") %>' TextMode="Number" CssClass="form-control" />
                                                    </div>
                                                    <div class="mb-2">
                                                        <label>Observações:</label>
                                                        <asp:TextBox ID="tbObs" runat="server" Text='<%# Bind("observacoes") %>' TextMode="MultiLine" Rows="2" CssClass="form-control" />
                                                    </div>
                                                    <div class="d-flex" style="justify-content: space-around">
                                                        <asp:LinkButton ID="btnSalvar" runat="server" CommandName="Update" CommandArgument='<%# Eval("id_pet") %>'><img src="icones/confirme.png" style="width:20px;height:20px;" /></asp:LinkButton>

                                                        <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel"><img src="icones/cancelar.png" style="width:20px;height:20px;" /></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        </div>
                                    </EditItemTemplate>
                                    <EmptyDataTemplate>

                                        <div class="text-center text-muted">Nenhum pet cadastrado.</div>

                                    </EmptyDataTemplate>
                                </asp:ListView>
                                <section class="d-flex justify-content-center mt-3">
                                    <asp:DataPager ID="DataPager2" runat="server" PagedControlID="lvPets" PageSize="3">
                                        <Fields>
                                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowLastPageButton="False" ButtonCssClass="btn btn-outline-primary" />
                                            <asp:NumericPagerField ButtonType="Button" NextPreviousButtonCssClass="btn btn-outline-primary" CurrentPageLabelCssClass="btn btn-outline-primary" NumericButtonCssClass="btn btn-outline-primary" />
                                        </Fields>
                                    </asp:DataPager>
                                </section>
                            </div>

                        </asp:Panel>
                        <!--Meios pagamentos-->
                        <asp:Panel ID="panelMeiosPagamentos" runat="server" Visible="false">
                            <div class="anuncios-area">
                                <h3 class="card-title mb-2">Meios de Pagamento</h3>
                                <asp:Repeater ID="rptCartoes" runat="server" Visible="true" OnItemCommand="rptCartoes_ItemCommand">
                                    <ItemTemplate>
                                        <div class="cartao-item">
                                            <div class="form-check">
                                                <asp:RadioButton ID="rbSelecionar" runat="server" GroupName="Cartoes"
                                                    AutoPostBack="true"
                                                    CssClass="form-check-input"
                                                    OnCheckedChanged="rbSelecionar_CheckedChanged"
                                                    ToolTip='<%# Eval("id_cartao") %>'
                                                    Checked='<%# Convert.ToBoolean(Eval("principal")) %>' />
                                            </div>

                                            <div class="flex-grow-1">
                                                <div class="card-title d-flex" style="flex-direction: column; align-items: flex-start;">
                                                    <span class="fw-bold"><%# Eval("nome_titular") %></span>
                                                    <span class="text-muted">**** **** **** <%# Eval("num_cartao").ToString().Substring(Eval("num_cartao").ToString().Length - 4) %></span>
                                                    <small class="text-muted">Validade: <%# Eval("validade") %></small>
                                                </div>

                                            </div>
                                            <div class="d-flex  gap-2 mt-2 ">
                                                <asp:Button ID="btnDefinirPrincipal" runat="server" Text="Definir como Principal"
                                                    CssClass="btn btn-sm btn-outline-primary" CommandName="DefinirPrincipal" CommandArgument='<%# Eval("id_cartao") %>' />
                                                <asp:Button ID="btnRemover" runat="server" Text="Remover"
                                                    CssClass="btn btn-sm btn-outline-danger"
                                                    CommandArgument='<%# Eval("id_cartao") %>' CommandName="Remover" />
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>


                                <!-- Painel para adicionar novo cartão -->
                                <asp:Panel ID="pnlNovoCartao" runat="server" Visible="false" CssClass="mt-4 p-4 border rounded">
                                    <h5 class="mb-4">Adicionar Novo Cartão</h5>
                                    <div class="row g-3">
                                        <div class="col-md-12">
                                            <label class="form-label">Nome do Titular</label>
                                            <asp:TextBox ID="tb_nomeTitular" runat="server"
                                                placeholder="Nome como consta no cartão"
                                                CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-8">
                                            <label class="form-label">Número do Cartão</label>
                                            <asp:TextBox ID="tb_numCartao" runat="server"
                                                placeholder="0000 0000 0000 0000"
                                                CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label class="form-label">Validade</label>
                                            <asp:TextBox ID="tb_validade" runat="server"
                                                placeholder="MM/AA"
                                                CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label class="form-label">CVV</label>
                                            <asp:TextBox ID="tb_cvv" runat="server"
                                                placeholder="123"
                                                CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-12">
                                            <asp:Button ID="btnAdd" runat="server" Text="Adicionar"
                                                CssClass="btn btn-secondary" Visible="true" />
                                        </div>
                                    </div>
                                </asp:Panel>


                                <div class="card-footer  mt-2">
                                    <asp:Button ID="btnAddCartao" runat="server" Text="Adicionar Novo Cartão"
                                        CssClass="btn btn-secondary"
                                        Visible="true" OnClick="btnAddCartao_Click" />
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="rpCandidaturas" EventName="ItemCommand" />

                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="updateReservas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <!--historico reservas-->
                        <asp:Panel ID="panelReservas" runat="server" Visible="false">
                            <div class="anuncios-area">
                                <h3>Histórico de Serviços</h3>

                                <div class="agenda-tabs">
                                    <ul class="nav nav-tabs">
                                        <li class="nav-item">
                                            <asp:Button ID="btnAvaliar" runat="server" Text="Serviços por avaliar" OnClick="btnAvaliar_Click" CssClass="nav-link active" />
                                        </li>

                                        <li class="nav-item">
                                            <asp:Button ID="btnConcluidos" runat="server" Text="Serviços Concluídos" OnClick="btnConcluidos_Click" CssClass="nav-link" />
                                        </li>
                                    </ul>
                                </div>

                                <div class="reserva-card mb-3 p-3 border rounded bg-light">
                                    <asp:Panel ID="panelAvaliar" runat="server" Visible="true">

                                        <asp:Repeater ID="rpServicosAvaliar" runat="server" OnItemCommand="rpServicosAvaliar_ItemCommand">
                                            <ItemTemplate>
                                                <b>Serviço:</b> <%# Eval("titulo") %><br />
                                                <b>Data:</b> <%# Eval("data_criacao", "{0:dd/MM/yyyy}") %><br />
                                                <b>Total:</b> €<%# Eval("total", "{0:N2}") %><br />


                                                <asp:Label ID="lblNota" runat="server" Text="Avalie o prestador (1 a 5):" CssClass="form-label d-block"></asp:Label>
                                                <asp:DropDownList ID="ddlNota" runat="server" CssClass="form-select form-select-sm w-auto d-inline-block me-2">
                                                    <asp:ListItem Text="1" Value="1" />
                                                    <asp:ListItem Text="2" Value="2" />
                                                    <asp:ListItem Text="3" Value="3" />
                                                    <asp:ListItem Text="4" Value="4" />
                                                    <asp:ListItem Text="5" Value="5" />
                                                </asp:DropDownList>
                                                <div class="form-group">
                                                    <label class="form-label d-block">Comentário:</label>
                                                    <asp:TextBox ID="tbComentario" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <asp:Button ID="btnAvaliar" runat="server" Text="Avaliar" CommandName="Avaliar" CssClass="btn btn-sm btn-success" CommandArgument='<%# Eval("id_prestador") + ";" + Eval("id_servico") %>' />
                                                </br>
                                                </br>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </asp:Panel>

                                    <%--<label>Não há histórico de serviços disponiveis</label>--%>

                                    <asp:Panel ID="panelConcluidos" runat="server" Visible="false">
                                        <asp:Repeater ID="rpServicosConcluidos" runat="server">
                                            <ItemTemplate>
                                                <b>Serviço:</b> <%# Eval("titulo") %><br />
                                                <b>Data:</b> <%# Eval("data_criacao", "{0:dd/MM/yyyy}") %><br />
                                                <b>Total:</b> €<%# Eval("total", "{0:N2}") %><br />
                                                </br>
                                        </br>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </asp:Panel>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAvaliar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        </div>




        <%-- Para a text area do input --%>
        <script type="text/javascript">
            function handleKey(e) {
                if (e.key === "Enter" && !e.shiftKey) {
                    e.preventDefault(); // impede quebra de linha
                    document.getElementById('<%= btn_enviar.ClientID %>').click(); // envia
                }

            }
        </script>

        <script>
            window.scrollChatToTheBottom = function () {
                const box = document.getElementById("mensagensBox")
                box.scrollTop = box.scrollHeight;
            }

            window.onload = function () {
                scrollChatToTheBottom();
            }

            window.clearTextArea = function () {
                document.getElementById('tb_mensagem').value = '';
            }

            window.focusOnTextArea = function () {
                setTimeout(() => {
                    document.getElementById('tb_mensagem').focus();
                }, 300)
            }


            function adjustEditCardHeight() {
                const editCards = document.querySelectorAll('.lvAnuncios .card-edit');
                editCards.forEach(card => {
                    const originalHeight = card.dataset.originalHeight;
                    if (originalHeight) {
                        card.style.minHeight = originalHeight;
                    }
                });
            }

            // Executar após cada postback
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                adjustEditCardHeight();
            });
        </script>
</asp:Content>
