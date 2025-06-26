<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="LojaVirtual.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">

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

                        <div class="form-group ">
                            <label class="form-label">Fotos:</label>
                            <asp:FileUpload ID="fuFotos" runat="server" AllowMultiple="true" CssClass="form-control-file" />
                            <small>Você pode selecionar várias imagens ao mesmo tempo (CTRL + clique)</small>
                        </div>
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

</asp:Content>
