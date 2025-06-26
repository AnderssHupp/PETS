<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="checkout.aspx.cs" Inherits="LojaVirtual.Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .morada-card {
            background-color: #f9f9f9;
            padding: 16px;
            margin-top: 10px;
            border-radius: 10px;
            border-left: 4px solid #28a745;
            transition: box-shadow 0.3s;
        }

        .card:hover {
            box-shadow: 0 6px 12px rgba(0,0,0,0.1);
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">

    <section class="container my-5 d-flex justify-content-center">
        <div class="card shadow p-4" style="width: 60%;">
            <h2 class="text-center mb-4">Checkout</h2>

            <asp:ScriptManager runat="server" />

            <asp:UpdatePanel ID="upWizard" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <!-- ENTREGA -->
                    <asp:Panel ID="pnlEntrega" runat="server" Visible="true" CssClass="mb-4">
                        <h4 class="mb-3">1. Entrega</h4>

                        <div class="morada-card">
                            <asp:Panel ID="pnl_moradaR" runat="server" Visible="false" CssClass="mb-3">
                                <p>
                                    <strong>Morada:</strong>
                                    <asp:Label ID="lbl_morada" runat="server" />
                                </p>
                                <p>
                                    <strong>Concelho:</strong>
                                    <asp:Label ID="lbl_concelho" runat="server" />
                                </p>
                                <p>
                                    <strong>Distrito:</strong>
                                    <asp:Label ID="lbl_distrito" runat="server" />
                                </p>
                                <p>
                                    <strong>Código Postal:</strong>
                                    <asp:Label ID="lbl_cp" runat="server" />
                                </p>
                            </asp:Panel>
                        </div>
                        <div class="form-check mb-3 mt-3">
                            <asp:CheckBox ID="chkOutraMorada" runat="server" CssClass="form-check-input " AutoPostBack="true" OnCheckedChanged="chkOutraMorada_CheckedChanged" />
                            <label class="form-check-label">
                                Entregar em outra morada
                            </label>
                        </div>
                        <asp:Panel ID="pnlOutraMorada" runat="server" Visible="false" CssClass="row g-3 mb-3">
                            <div class="col-md-6">
                                <asp:TextBox ID="tb_novaMorada" runat="server" placeholder="Nova Morada" CssClass="form-control" />
                            </div>
                            <div class="col-md-6">
                                <asp:TextBox ID="tb_NovoConcelho" runat="server" placeholder="Concelho" CssClass="form-control" />
                            </div>
                            <div class="col-md-6">
                                <asp:TextBox ID="tb_NovoDistrito" runat="server" placeholder="Distrito" CssClass="form-control" />
                            </div>
                            <div class="col-md-6">
                                <asp:TextBox ID="tb_CodigoPostal" runat="server" placeholder="Código Postal" CssClass="form-control" />
                            </div>
                        </asp:Panel>


                        <asp:Button ID="btnNext1" runat="server" Text="Próximo" OnClick="btnNext1_Click" CssClass="btn btn-primary" />
                    </asp:Panel>

                    <!-- PAGAMENTO -->
                    <asp:Panel ID="pnlPagamento" runat="server" Visible="false" CssClass="mb-4">
                        <h4 class="mb-3">2. Pagamento</h4>

                        <div class="form-check mb-2">
                            <asp:RadioButton ID="rbCartao" runat="server" GroupName="Pagamento" AutoPostBack="true" OnCheckedChanged="rbCartao_CheckedChanged" CssClass="form-check-input" InputAttributes-CssClass="form-check-input" />
                            <label class="form-check-label">
                                Cartão de Crédito
                            </label>
                        </div>

                        <asp:Panel ID="pnl_info" runat="server" Visible="false" CssClass="mb-3">
                            <div class="morada-card">
                                <p>
                                    <strong>Nome:</strong>
                                    <asp:Label ID="lbl_nomeTitular" runat="server" />
                                </p>
                                <p>
                                    <strong>Número:</strong>
                                    <asp:Label ID="lbl_num" runat="server" />
                                </p>
                                <p>
                                    <strong>Validade:</strong>
                                    <asp:Label ID="lbl_validade" runat="server" />
                                </p>
                                <p>
                                    <strong>CVV:</strong>
                                    <asp:Label ID="lbl_cvv" runat="server" />
                                </p>
                                <div class="form-check">
                                    <asp:CheckBox ID="cb_novoCartao" runat="server" CssClass="form-check-input me-2" AutoPostBack="true" OnCheckedChanged="cb_novoCartao_CheckedChanged" />

                                    <label class="form-check-label">
                                        Adicionar outro cartão
                                    </label>
                                </div>
                            </div>
                        </asp:Panel>

                        <!-- Novo Cartão -->
                        <asp:Panel ID="pnlNovoCartao" runat="server" Visible="false" CssClass="row g-3 mb-3">
                            <div class="col-md-6">
                                <asp:TextBox ID="tb_nomeTitular" runat="server" placeholder="Nome Titular" CssClass="form-control" />
                            </div>
                            <div class="col-md-6">
                                <asp:TextBox ID="tb_numCartao" runat="server" placeholder="Número do Cartão" CssClass="form-control" />
                            </div>
                            <div class="col-md-6">
                                <asp:TextBox ID="tb_validade" runat="server" placeholder="Validade (MM/AA)" CssClass="form-control" />
                            </div>
                            <div class="col-md-6">
                                <asp:TextBox ID="tb_cvv" runat="server" placeholder="CVV" CssClass="form-control" />
                            </div>
                        </asp:Panel>


                        <div class="form-check mb-3">
                            <asp:RadioButton ID="rbMBWay" runat="server" GroupName="Pagamento" CssClass="form-check-input" AutoPostBack="true" OnCheckedChanged="rbMBWay_CheckedChanged" />
                            <label class="form-check-label">
                                MB WAY
                            </label>

                        </div>

                        <asp:Panel ID="pnl_mbway" runat="server" Visible="false" CssClass="mb-3">
                            <asp:TextBox ID="tb_num" runat="server" placeholder="Número Telemóvel" CssClass="form-control" />
                        </asp:Panel>

                        <div class="d-flex justify-content-between">
                            <asp:Button ID="btnBack1" runat="server" Text="Voltar" CssClass="btn btn-outline-secondary" OnClick="btnBack1_Click" />
                            <asp:Button ID="btnNext2" runat="server" Text="Próximo" CssClass="btn btn-primary" OnClick="btnNext2_Click" />
                        </div>
                    </asp:Panel>

                    <!-- CONFIRMAÇÃO -->
                    <asp:Panel ID="pnlConfirmacao" runat="server" Visible="false" CssClass="mb-3">
                        <h4 class="mb-3">3. Confirmação</h4>
                        <div class="morada-card">
                            <p>
                                <strong>Total:</strong>
                                <asp:Label ID="lblTotal" runat="server" CssClass="text-success fw-bold" />
                            </p>
                            <p>
                                <strong>Forma de pagamento:</strong>
                                <asp:Label ID="lblPagamento" runat="server" />
                            </p>
                            <p>
                                <strong>Morada de entrega:</strong>
                                <asp:Label ID="lblResumoMorada" runat="server" />
                            </p>

                        </div>
                        <div class="d-flex justify-content-between mt-4">
                            <asp:Button ID="btnBack2" runat="server" Text="Voltar" CssClass="btn btn-outline-secondary" OnClick="btnBack2_Click" />
                            <asp:Button ID="btnFinalizar" runat="server" Text="Finalizar Compra" CssClass="btn btn-success px-4" OnClick="btnFinalizar_Click" />
                        </div>
                    </asp:Panel>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </section>
</asp:Content>
