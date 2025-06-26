<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="prestadores.aspx.cs" Inherits="LojaVirtual.prestadores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />
    <script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>
    <style>
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

        .filtros-container {
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 12px;
            margin: 20px auto;
        }

        .prestador-foto {
            width: 100%;
            height: 200px;
            object-fit: cover;
            border-radius: 10px;
        }

        .btn-pesquisa {
            background-color: #28a745;
            color: white;
            padding: 10px;
            border: none;
            border-radius: 6px;
            width: 100%;
            cursor: pointer;
            margin-top: 10px;
        }

            .btn-pesquisa:hover {
                background-color: #218838;
            }

        .card-hover-zoom {
            transition: transform 0.3s ease;
        }

            .card-hover-zoom:hover {
                transform: scale(1.05);
            }
        /* paginacao */
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


        .map-container {
            
            display: flex;
            justify-content: center;
            align-items: center;
            flex-wrap: wrap;
            gap: 20px;
            padding: 20px;
            margin: 0 auto;
            max-width: 1200px;
        }



        /* eesponsividade */
        @media (max-width: 768px) {
            .container {
                flex-direction: column; 
            }

            .map-container {
                width: 100%;
            }
        }

     
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="filtros-container">
        <div class="d-flex flex-column">

            <label>Ordenar por:</label>
            <asp:DropDownList ID="ddlOrdenacao" runat="server" AutoPostBack="true" CssClass="btn btn-sm btn-secondary dropdown-toggle">
                <asp:ListItem Value="nome ASC">Nome (A-Z)</asp:ListItem>
                <asp:ListItem Value="nome DESC">Nome (Z-A)</asp:ListItem>

            </asp:DropDownList>
        </div>

        <div class="d-flex flex-column">
            <label>Localidade:</label>
            <asp:DropDownList ID="ddl_localidade" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AplicarFiltros" CssClass="btn btn-sm btn-secondary dropdown-toggle">
                <asp:ListItem Text="Qualquer Localidade" Value="" />
                <asp:ListItem Text="Lisboa" Value="Lisboa" />
                <asp:ListItem Text="Sintra" Value="Sintra" />
                <asp:ListItem Text="Oeiras" Value="Oeiras" />
                <asp:ListItem Text="Cascais" Value="Cascais" />
                <asp:ListItem Text="Amadora" Value="Amadora" />
            </asp:DropDownList>
        </div>

        <div class="d-flex flex-column">
            <label>Raio:</label>
            <asp:DropDownList ID="ddl_raio" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AplicarFiltros" CssClass="btn btn-sm btn-secondary dropdown-toggle">
                <asp:ListItem Text="10 km" Value="10" />
                <asp:ListItem Text="20 km" Value="20" />
                <asp:ListItem Text="50 km" Value="50" />
                <asp:ListItem Text="100 km" Value="100" />
            </asp:DropDownList>
        </div>
        <%--      <<%--div class="d-flex flex-row">
            <label>Preço Máximo (€):</label>
            <asp:DropDownList ID="ddl_preco" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AplicarFiltros" CssClass="btn btn-sm btn-secondary dropdown-toggle">
                <asp:ListItem Text="Sem Limite" Value="" />
                <asp:ListItem Text="Até €10" Value="10" />
                <asp:ListItem Text="Até €20" Value="20" />
                <asp:ListItem Text="Até €50" Value="50" />
            </asp:DropDownList>
        </div>--%>
        <div class="d-flex flex-column">
            <label>Avaliação Mínima:</label>
            <asp:DropDownList ID="ddl_avaliacao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AplicarFiltros" CssClass="btn btn-sm btn-secondary dropdown-toggle">
                <asp:ListItem Text="Todas" Value="" />
                <asp:ListItem Text="★ e acima" Value="1" />
                <asp:ListItem Text="★★ e acima" Value="2" />
                <asp:ListItem Text="★★★ e acima" Value="3" />
                <asp:ListItem Text="★★★★ e acima" Value="4" />
                <asp:ListItem Text="★★★★★" Value="5" />
            </asp:DropDownList>
        </div>

        <div class="d-flex flex-column">
            <label>Disponibilidade:</label>
            <asp:DropDownList ID="ddl_disponibilidade" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AplicarFiltros" CssClass="btn btn-sm btn-secondary dropdown-toggle">
                <asp:ListItem Text="Todos os Dias" Value="" />
                <asp:ListItem Text="Segunda a Sexta" Value="Semana" />
                <asp:ListItem Text="Fins de Semana" Value="FDS" />
            </asp:DropDownList>
        </div>
    </div>

    <section class="container">
        <asp:ListView ID="lv_prestadores" runat="server" OnPagePropertiesChanging="lv_prestadores_PagePropertiesChanging">
            <ItemTemplate>
                <div class="card overflow-hidden card-hover-zoom d-flex" style="width: 15rem; height: 29rem;">
                    <asp:Image ID="img_foto" runat="server" ImageUrl='<%# Eval("FotoBase64") %>' CssClass="-img-top object-fit-cover"
                        Style="height: 200px;" />
                    <div class="card-body flex-grow-0 p-0">
                        <h6 class="card-title p-3 pb-0"><%# Eval("nome") %></h6>
                    </div>
                    <hr class="m-2" />
                    <div class="card-body flex-grow-1 flex-column d-flex pt-0">
                        <div class="d-flex flex-grow-1 flex-column">
                            <p style="font-size: 12px;" class="lh-1 d-inline-block text-truncate" title="<%# Eval("email") %>"><b>Email:</b> <%# Eval("email") %></p>
                            <p style="font-size: 12px;" class="lh-1 d-inline-block text-truncate"><b>Localidade:</b> <%# Eval("localidade") %></p>
                            <p style="font-size: 12px;" class="lh-1 d-inline-block text-truncate" title="<%# Eval("disponibilidade") %>"><b>Disponibilidade:</b> <%# Eval("disponibilidade") %></p>
                            <p style="font-size: 12px;" class="lh-1 d-inline-block text-truncate"><b>Experiência:</b> <%# Eval("experiencia") %></p>
                            <p style="font-size: 12px;" class="lh-1 d-inline-block text-truncate"><b>Avaliação:</b> <%# Eval("nota_media", "{0:N1}") %> ★</p>
                        </div>
                        <asp:LinkButton ID="btn_Contactar" runat="server" Text="Contactar" CssClass="btn btn-sm btn-primary" OnClientClick='<%# Eval("id_utilizador", "abrirModal({0}); return false;") %>' />
                        <%--<button type="button" class="btn btn-sm btn-primary" onclick='abrirModal(<%# Eval("id_utilizador") %>)'>Contactar</button>--%>
                    </div>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </section>
    <!-- hiddenfield para aceder a localizacao em tempo real-->
    <asp:HiddenField ID="hiddenLat" runat="server" />
    <asp:HiddenField ID="hiddenLon" runat="server" />
    <!--hiddenfield para pegar id do prestador-->
    <asp:HiddenField ID="hf_idPrestador" runat="server" />

    <asp:Button ID="btn_geolocate" runat="server" OnClick="btn_geolocate_Click" Style="display: none;" />


    <section class="pagination">
        <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lv_prestadores" PageSize="8">
            <Fields>
                <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowLastPageButton="False" ButtonCssClass="btn btn-outline-primary" />
                <asp:NumericPagerField ButtonType="Button" NextPreviousButtonCssClass="btn btn-outline-primary" CurrentPageLabelCssClass="btn btn-outline-primary" NumericButtonCssClass="btn btn-outline-primary" />
            </Fields>
        </asp:DataPager>
    </section>

    <section class="map-container">
        <div id="map"></div>
    </section>

    <div id="modalContactar" class="modal fade" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Contactar PetSitter</h5>
                <span class="fechar" onclick="fecharModal()">&times;</span>
            </div>

            <div class="modal-body">
                <asp:HiddenField ID="hf_idUtilizador" runat="server" />
                <asp:TextBox ID="tb_msg" runat="server" TextMode="MultiLine" Rows="5"
                    CssClass="form-control" placeholder="Escreva aqui sua mensagem..."></asp:TextBox>
            </div>

            <div class="modal-footer">
                <asp:Button ID="btnEnviarMensagem" runat="server" Text="Enviar"
                    CssClass="btn btn-primary" OnClick="btnEnviarMensagem_Click" />
            </div>
        </div>
    </div>
</div>

    <!--localizacao-->
    <script>
        window.onload = function () {
            if (document.getElementById('<%= hiddenLat.ClientID %>').value !== '') return;

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(success, error);
            }

            function success(position) {
                const lat = position.coords.latitude;
                const lon = position.coords.longitude;

                document.getElementById('<%= hiddenLat.ClientID %>').value = lat;
                document.getElementById('<%= hiddenLon.ClientID %>').value = lon;

                __doPostBack('<%= btn_geolocate.UniqueID %>', '');
            }

            function error(err) {
                console.warn("Erro ao obter localização:", err.message);
            }
        }
        function initMap(dados) {
            const map = L.map('map').setView([38.736946, -9.142685], 10); // Lisboa como padrão

            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                maxZoom: 18,
                attribution: '&copy; OpenStreetMap contributors'
            }).addTo(map);

            dados.forEach(p => {
                const marker = L.marker([p.lat, p.lon]).addTo(map);
                marker.bindPopup(`<strong>${p.nome}</strong><br>${p.localidade}`);
            });

            if (dados.length > 0) {
                const bounds = dados.map(p => [p.lat, p.lon]);
                map.fitBounds(bounds);
            }
        }

        let modalInstance;

        function abrirModal(idPrestador) {
            document.getElementById('<%= hf_idPrestador.ClientID %>').value = idPrestador;
            
            const modalElement = document.getElementById('modalContactar');
            modalInstance = new bootstrap.Modal(modalElement);
            modalInstance.show();
        }

        function fecharModal() {
            if (modalInstance) {
                modalInstance.hide();
            } else {
                // fallback caso o modal tenha sido aberto de outra forma
                const fallbackModal = bootstrap.Modal.getInstance(document.getElementById('modalContactar'));
                if (fallbackModal) fallbackModal.hide();
            }
        }
    </script>

</asp:Content>
