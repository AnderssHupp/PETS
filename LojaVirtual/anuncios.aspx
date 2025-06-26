<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="anuncios.aspx.cs" Inherits="LojaVirtual.anuncios" %>

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

        .anuncio-item {
            width: 30%;
            padding: 12px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #fafafa;
        }

        .ordenacao_pesquisa-container {
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 12px;
            margin: 20px auto;
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

        .modal-candidatura {
            display: none;
            position: fixed;
            z-index: 999;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
        }

        .modal-conteudo {
            background: white;
            margin: 10% auto;
            padding: 20px;
            width: 90%;
            max-width: 500px;
            border-radius: 10px;
            position: relative;
        }

        .fechar {
            position: absolute;
            top: 10px;
            right: 15px;
            font-size: 22px;
            cursor: pointer;
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

        .card-hover-zoom {
            transition: transform 0.3s ease;
        }

            .card-hover-zoom:hover {
                transform: scale(1.05);
            }

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
    <div class="ordenacao_pesquisa-container">
        <div class="d-flex flex-column">
            <label>Ordenar por:</label>
            <asp:DropDownList ID="ddl_servicos" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AplicarFiltros" CssClass="btn btn-sm btn-secondary dropdown-toggle"></asp:DropDownList>
        </div>
        <div class="d-flex flex-column">
            <label>Localidade:</label>
            <asp:DropDownList ID="ddl_localidade" runat="server" OnSelectedIndexChanged="AplicarFiltros" CssClass="btn btn-sm btn-secondary dropdown-toggle">
                <asp:ListItem Text="Qualquer Localidade" Value="" />
                <asp:ListItem Text="Lisboa" Value="Lisboa" />
                <asp:ListItem Text="Amadora" Value="Amadora" />
                <asp:ListItem Text="Sintra" Value="Sintra" />
                <asp:ListItem Text="Oeiras" Value="Oeiras" />
                <asp:ListItem Text="Cascais" Value="Cascais" />
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
        <div class="d-flex flex-column">
            <label>Preço:</label>
            <asp:DropDownList ID="ddl_preco" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AplicarFiltros" CssClass="btn btn-sm btn-secondary dropdown-toggle">
                <asp:ListItem Text="Qualquer Preço" Value="" />
                <asp:ListItem Text="Até €10" Value="10" />
                <asp:ListItem Text="Até €20" Value="20" />
                <asp:ListItem Text="Até €50" Value="50" />
            </asp:DropDownList>
        </div>
    </div>
    <section class="container">


        <asp:ListView ID="lv_anuncios" runat="server" OnPagePropertiesChanging="lv_anuncios_PagePropertiesChanging" OnItemCommand="lv_anuncios_ItemCommand">
            <ItemTemplate>

                <%-- <div class="card overflow-hidden card-hover-zoom" style="width: 18rem;">--%>
                <div class="card overflow-hidden card-hover-zoom d-flex" style="width: 15rem; height: 29rem;">
                    <asp:ImageButton ID="ImagePet" runat="server"
                        ImageUrl='<%# Eval("FotoBase64") %>'
                        CommandName="VerDetalhes"
                        CommandArgument='<%# Eval("id_anuncio") %>'
                        OnCommand="ImagePet_Command"
                        CssClass="-img-top object-fit-cover"
                        Style="height: 200px;" />


                    <div class="card-body flex-grow-0 p-0">
                        <h6 class="card-title p-3 pb-0"><%# Eval("titulo") %>
                        </h6>
                    </div>
                    <hr class="m-2" />
                    <div class="card-body flex-grow-1 flex-column d-flex pt-0">
                        <div class="d-flex flex-grow-1 flex-column">
                            <%--<li class="list-group-item"><b>Tipo:</b> <%# Eval("tipo_servico") %></li>--%>
                            <p style="font-size: 12px;" class="lh-1 d-inline-block text-truncate" title="<%# Eval("descricao") %>"><b>Descrição:</b> <%# Eval("descricao") %></p>
                            <p style="font-size: 12px;" class="lh-1 d-inline-block text-truncate" title="<%# Eval("data_nece") %>"><b>Data do Serviço:</b> <%# Eval("data_nece", "{0:dd/MM/yyyy}") %></p>
                            <p style="font-size: 12px;" class="lh-1 d-inline-block text-truncate" title="<%# Eval("hora_nece") %>"><b>Hora:</b> <%# Eval("hora_nece", @"{0:hh\:mm}") %></p>


                            <%--<div class="card-body">--%>
                            <p style="font-size: 12px;" class="lh-1 d-inline-block text-truncate" title="<%# Eval("localidade") %>"><b>Local: </b><%# Eval("localidade") %></p>
                            <p style="font-size: 12px;" class="lh-1 d-inline-block text-truncate" title="<%# Eval("preco") %>"><b>Preço: </b>€<%# Eval("preco", "{0:N2}") %></p>
                            <button type="button" class="btn btn-primary" onclick='abrirModal(<%# Eval("id_anuncio") %>)'>Candidatar-se</button>
                        </div>
                    </div>
                    </div>
            </ItemTemplate>
        </asp:ListView>
    </section>
    <!-- hiddenfield para aceder a localizacao em tempo real-->
    <asp:HiddenField ID="hiddenLat" runat="server" />
    <asp:HiddenField ID="hiddenLon" runat="server" />
    <asp:Button ID="btn_geolocate" runat="server" OnClick="btn_geolocate_Click" Style="display: none;" />

    <div id="modalCandidatura" class="modal-candidatura">
        <div class="modal-conteudo">
            <span class="fechar" onclick="fecharModal()">&times;</span>
            <h3>Candidatar-se ao anúncio</h3>
            <asp:HiddenField ID="hf_idAnuncio" runat="server" />
            <asp:TextBox ID="tb_msg" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control" placeholder="Escreva aqui sua mensagem..."></asp:TextBox>
            <br />
            <asp:Button ID="btnEnviarCandidatura" runat="server" Text="Enviar" CssClass="btn-pesquisa" OnClick="btnEnviarCandidatura_Click" />
        </div>
    </div>

    <section class="pagination">
        <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lv_anuncios" PageSize="10">
            <Fields>
                <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowLastPageButton="False" ButtonCssClass="btn btn-outline-primary" />
                <asp:NumericPagerField ButtonType="Button" NextPreviousButtonCssClass="btn btn-outline-primary" CurrentPageLabelCssClass="btn btn-outline-primary" NumericButtonCssClass="btn btn-outline-primary" />
            </Fields>
        </asp:DataPager>
    </section>

    <section class="map-container">
        <div id="map"></div>
    </section>
    <script>
        window.onload = function () {
            const localidade = document.getElementById('<%= ddl_localidade.ClientID %>');
            localidade.style.display = "none";
            localidade.style.display = "none";

            // evita novo postback se ja tem coordenadas
            if (document.getElementById('<%= hiddenLat.ClientID %>').value !== '') {
                return;
            }

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(success, error);
            }

            function success(position) {
                const lat = position.coords.latitude;
                const lon = position.coords.longitude;

                document.getElementById('<%= hiddenLat.ClientID %>').value = lat;
                document.getElementById('<%= hiddenLon.ClientID %>').value = lon;

                // aciona postback automatico
                __doPostBack('<%= btn_geolocate.UniqueID %>', '');
            }

            function error(err) {
                console.warn("Erro ao obter localização: ", err.message);
                // mostra filtro manual por localidade
                localidade.style.display = "inline-block";
            }
        }

        function initMap(dados) {
            const map = L.map('map').setView([38.736946, -9.142685], 10); // Lisboa como padrão

            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                maxZoom: 18,
                attribution: '&copy; OpenStreetMap contributors'
            }).addTo(map);

            dados.forEach(a => {
                const marker = L.marker([a.lat, a.lon]).addTo(map);
                marker.bindPopup(`<strong>${a.titulo}</strong><br>${a.descricao}<br>${a.localidade}`);
            });

            if (dados.length > 0) {
                const bounds = dados.map(a => [a.lat, a.lon]);
                map.fitBounds(bounds);
            }
        }

        function abrirModal(idAnuncio) {
            document.getElementById('<%= hf_idAnuncio.ClientID %>').value = idAnuncio;
            document.getElementById('modalCandidatura').style.display = 'block';
        }

        function fecharModal() {
            document.getElementById('modalCandidatura').style.display = 'none';
        }

    </script>
</asp:Content>
