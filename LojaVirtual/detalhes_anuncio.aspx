<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="detalhes_anuncio.aspx.cs" Inherits="LojaVirtual.detalhes_anuncio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        .carousel img {
            max-height: 500px;
            object-fit: cover;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="container my-5 d-flex justify-content-center ">
        <div class="row shadow rounded-4 p-4 bg-white w-100">

            <div class="col-md-6 mb-4">
                <h2>
                    <asp:Label ID="lbl_titulo" runat="server" /></h2>
                <p>
                    <label><strong>Nome:</strong></label>
                    <asp:Label ID="lbl_nome" runat="server" />
                </p>
                <p>
                    <label><strong>Espécie:</strong> </label>
                    <asp:Label ID="lbl_especie" runat="server" />
                </p>
                <p>
                    <label><strong>Raça:</strong> </label>
                    <asp:Label ID="lbl_raca" runat="server" />
                </p>
                <p>
                    <label><strong>Peso(kg):</strong></label>
                    <asp:Label ID="lbl_peso" runat="server" />
                </p>
                <p>
                    <label><strong>Idade:</strong> </label>
                    <asp:Label ID="lbl_idade" runat="server" />
                </p>
                <p>
                    <label><strong>Observações:</strong> </label>
                    <asp:Label ID="lbl_observacoes" runat="server" />
                </p>
                <%-- <p>
                    <label><strong>Tipo de serviço:</strong> </label>
                    <asp:Label ID="lbl_tipo" runat="server" />
                </p>--%>
                <p>
                    <label><strong>Descrição:</strong> </label>
                    <asp:Label ID="lbl_descricao" runat="server" />
                </p>
                <p>
                    <label><strong>Localidade:</strong> </label>
                    <asp:Label ID="lbl_local" runat="server" />
                </p>
                <p>
                    <label><strong>Preço: €</strong> </label>
                    <asp:Label ID="lbl_preco" runat="server" />
                </p>

                <div class="mt-3">
                    <button type="button" class="btn btn-primary" onclick='abrirModal(<%# Eval("id_anuncio") %>)'>Candidatar-se</button>
                    <asp:Button ID="btn_voltar" runat="server" Text="Voltar" CssClass="btn btn-primary" />
                </div>
                <asp:Label ID="lbl_msg" runat="server"></asp:Label>
            </div>

            <!-- carrossel de imagens -->
            <div class="col-md-6">
                <div id="carouselFotosPet" class="carousel slide" data-bs-ride="carousel">
                    <div class="carousel-inner">
                        <asp:Repeater ID="rpt_carousel" runat="server">
                            <ItemTemplate>
                                <div class='carousel-item <%# (Container.ItemIndex == 0 ? "active" : "") %>'>
                                    <img src='<%# Eval("FotoBase64") %>' class="d-block w-100 rounded" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <button class="carousel-control-prev" type="button" data-bs-target="#carouselFotosPet" data-bs-slide="prev">
                        <span class="carousel-control-prev-icon"></span>
                    </button>
                    <button class="carousel-control-next" type="button" data-bs-target="#carouselFotosPet" data-bs-slide="next">
                        <span class="carousel-control-next-icon"></span>
                    </button>
                </div>
            </div>
        </div>
    </div>
    <div id="modalCandidatura" class="modal fade" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Candidatar-se ao anúncio</h5>
                    <span class="fechar" onclick="fecharModal()">&times;</span>
                </div>

                <div class="modal-body">
                    <asp:HiddenField ID="hf_idAnuncio" runat="server" />
                    <asp:TextBox ID="tb_msg" runat="server" TextMode="MultiLine" Rows="5"
                        CssClass="form-control" placeholder="Escreva aqui sua mensagem..."></asp:TextBox>
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnEnviarCandidatura" runat="server" Text="Enviar"
                        CssClass="btn btn-primary" OnClick="btnEnviarCandidatura_Click" />
                </div>
            </div>
        </div>
    </div>
    <script>
        let modalInstance;

        function abrirModal(idAnuncio) {
            document.getElementById('<%= hf_idAnuncio.ClientID %>').value = idAnuncio;

            const modalElement = document.getElementById('modalCandidatura');
            modalInstance = new bootstrap.Modal(modalElement);
            modalInstance.show();
        }

        function fecharModal() {
            if (modalInstance) {
                modalInstance.hide();
            } else {
                // fallback caso o modal tenha sido aberto de outra forma
                const fallbackModal = bootstrap.Modal.getInstance(document.getElementById('modalCandidatura'));
                if (fallbackModal) fallbackModal.hide();
            }
        }

    </script>
</asp:Content>
