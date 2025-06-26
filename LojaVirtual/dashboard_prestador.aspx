<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="dashboard_prestador.aspx.cs" Inherits="LojaVirtual.agenda" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- FullCalendar -->
    <link href="https://cdn.jsdelivr.net/npm/fullcalendar@6.1.8/index.global.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/fullcalendar@6.1.8/index.global.min.js"></script>


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



        .servicos-area {
            background: #fff;
            min-height: 580px;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
        }

        .conteudo-painel {
            flex: 1;
            display: flex;
            flex-direction: column;
            gap: 30px;
        }

        /*.painel-centro {
            display: flex;
            flex-wrap: wrap;*/
        /*  gap: 20px;*/
        /*width: 100%;
        }*/

        .agenda-wrapper,
        .reservas-wrapper {
            width: 100%;
        }

        #panel_reservas, #panel_agenda {
            width: 100%;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
            min-width: 320px;
        }

        /* === Sidebar === */
        .sidebar h4 {
            margin-bottom: 20px;
            font-weight: 600;
            color: #333;
        }

        .user-photo {
            width: 100px;
            height: 100px;
            border-radius: 50%;
            object-fit: cover;
            border: 3px solid #28a745;
            margin-bottom: 12px;
        }



        .reserva-card, .cartao-item {
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

        #calendar {
            margin-top: 20px;
        }

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
            transform: scale(1.05);
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

        .meios-pagamento-panel {
            background-color: #fff;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
            margin-bottom: 20px;
        }

        input[type="text"], input[type="number"], input[type="password"] {
            width: 80%;
            padding: 10px;
            margin: 8px 0;
            border-radius: 8px;
            border: 1px solid #ccc;
            box-shadow: inset 0 2px 4px rgba(0, 0, 0, 0.1);
            transition: border-color 0.3s ease, box-shadow 0.3s ease;
        }

            input[type="text"]:focus, input[type="number"]:focus, input[type="password"]:focus {
                border-color: #6200ea;
                box-shadow: inset 0 2px 6px rgba(98, 0, 234, 0.3);
                outline: none;
            }

        /*.cartao-item {
            display: flex;
            align-items: center;
            gap: 15px;
            padding: 10px 0;
            border-bottom: 1px solid #eee;*/

        @keyframes slideIn {
            from {
                transform: translateY(10px);
                opacity: 0;
            }

            to {
                transform: translateY(0);
                opacity: 1;
            }
        }

        @keyframes fadeIn {
            from {
                opacity: 0;
            }

            to {
                opacity: 1;
            }
        }

        @media (max-width: 1024px) {
            .dashboard-layout {
                display: flex;
                flex-direction: column;
                gap: 20px;
            }

            .sidebar,
            .servicos-area {
                position: static;
                width: 100%;
            }

            .reservas-container {
                flex-direction: column;
            }

            .agenda-area,
            .reservas-area {
                width: 100%;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div class="painel-container">
        <div class="sidebar">
            <asp:Image ID="img_foto" runat="server" CssClass="user-photo" />
            <h4>
                <asp:Label ID="lbl_nome" runat="server"></asp:Label></h4>

            <asp:LinkButton ID="btn_msgs" runat="server" OnClick="btn_msgs_Click" CssClass="btn-msgs"><img src="icones/mensagens.png" style="width:16px;height:16px;" /> Mensagens</asp:LinkButton>
            <asp:LinkButton ID="btnPerfil" runat="server" CssClass="btn-msgs" OnClick="btnPerfil_Click">
<img src="icones/usuario-de-perfil.png" style="width:16px;height:16px;" /> Perfil
            </asp:LinkButton>

            <asp:LinkButton ID="btnReservas" runat="server" OnClick="btnReservas_Click" CssClass="btn-msgs">
<img src="icones/booked.png" style="width:16px;height:16px;" /> Reservas
            </asp:LinkButton>
            <asp:LinkButton ID="btnAgenda" runat="server" OnClick="btnAgenda_Click" CssClass="btn-msgs">
<img src="icones/calendar.png" style="width:16px;height:16px;" /> Agenda
            </asp:LinkButton>

            <asp:LinkButton ID="btnHistorico" runat="server" OnClick="btnHistorico_Click" CssClass="btn-msgs">
<img src="icones/historia.png" style="width:16px;height:16px;"/> Histórico de Serviços
            </asp:LinkButton>

            <asp:LinkButton ID="btnPagamentos" runat="server" OnClick="btnPagamentos_Click" CssClass="btn-msgs">
<img src="icones/credit-card.png"  style="width:16px;height:16px;"/> Meios de Pagamentos
            </asp:LinkButton>

            <asp:LinkButton ID="btnLogout" runat="server" OnClick="btnLogout_Click" CssClass="btn-logout">
<i class="bi bi-box-arrow-right"></i> Logout
            </asp:LinkButton>

        </div>

        <!--Conteudo central-->
        <div class="conteudo-painel">

            <asp:UpdatePanel ID="UpdatePanelReservas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="area">
                        <div class="reservas-wrapper">
                            <asp:Panel ID="panel_reservas" runat="server" Visible="true">
                                <h3>Reservas</h3>

                                <div class="agenda-tabs">
                                    <ul class="nav nav-tabs">
                                        <li class="nav-item">
                                            <asp:Button ID="btn_atuais" runat="server" Text="Reservas Atuais" OnClick="btn_atuais_Click" CssClass="nav-link active" />
                                        </li>
                                        <li class="nav-item">
                                            <asp:Button ID="btn_pendentes" runat="server" Text="Pendentes" OnClick="btn_pendentes_Click" CssClass="nav-link" />
                                        </li>
                                        <li class="nav-item">
                                            <asp:Button ID="btn_concluidas" runat="server" Text="Concluídas" OnClick="btn_concluidas_Click" CssClass="nav-link" />
                                        </li>
                                    </ul>
                                </div>

                                <div>
                                    <asp:Repeater ID="rp_atuais" runat="server">
                                        <ItemTemplate>
                                            <div class="reserva-card">
                                                <b>Data:</b> <%# Eval("data_servico", "{0:dd/MM/yyyy}") %><br />
                                                <b>Hora:</b> <%# Eval("hora_inicio") %> - <%# Eval("hora_fim") %><br />
                                                <b>Cliente:</b> <%# Eval("nome_cliente") %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>

                            </asp:Panel>
                        </div>

                        <div class="agenda-wrapper">
                            <asp:Panel ID="panel_agenda" runat="server" Visible="false">
                                <div class="agenda-area">
                                    <h3 style="text-align: center;">Agenda</h3>

                                    <div id="calendar"></div>

                                </div>
                            </asp:Panel>
                        </div>

                        <!--Painel mensagens-->
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

                                                <asp:Button ID="btn_enviar" runat="server" Type="submit" Text="Enviar" OnClick="btn_enviar_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>

                        <asp:Panel ID="panelMeiosPagamentos" runat="server" Visible="false">

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
                            <asp:Label ID="lbl_msg" runat="server" Text="Nenhum meio de pagamento cadastrado."
                                CssClass="text-danger" Visible="false" />

                            <!-- novo cartão -->
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
                        </asp:Panel>
                        <!--servicos-->
                        <asp:Panel ID="panelServicos" runat="server" Visible="false">

                            <h3>Histórico de Serviços</h3>
                            <asp:Label ID="lbl_msgS" runat="server" Text="Não há histórico de serviços disponiveis" CssClass="text-danger" Visible="false" />
                            <asp:Repeater ID="rpServicosConcluidos" runat="server">
                                <ItemTemplate>
                                    <div class="reserva-card">
                                        <b>Serviço:</b> <%# Eval("titulo") %><br />
                                        <b>Data:</b> <%# Eval("data_criacao", "{0:dd/MM/yyyy}") %><br />
                                        <b>Total:</b> €<%# Eval("total", "{0:N2}") %>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                        </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn_atuais" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btn_pendentes" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btn_concluidas" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

        </div>
        <script type="text/javascript">
            function handleKey(e) {
                if (e.key === "Enter" && !e.shiftKey) {
                    e.preventDefault(); // impede quebra de linha
                    document.getElementById('<%= btn_enviar.ClientID %>').click(); // envia
                }

            }
        </script>
        <script>
           
            document.addEventListener('DOMContentLoaded', function () {
                var calendarEl = document.getElementById('calendar');
                var calendar = new FullCalendar.Calendar(calendarEl, {
                    initialView: 'dayGridMonth',
                    height: 500,
                    headerToolbar: {
                        left: 'prev,next today',
                        center: 'title',
                        right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
                    },
                    events: [
                        // disponibilidades em verde claro 
                        ...<%= ViewState["json_disponiveis"] %>,
                        // agendamentos reais
                        ...<%= ViewState["json_agenda"] %>]
                });
                calendar.render();
            })
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
        </script>
</asp:Content>
