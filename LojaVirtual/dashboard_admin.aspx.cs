using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LojaVirtual
{
    public partial class dashboard_admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarEstatisticas();
                CarregarAvaliacoes();
                CarregarTickets();
                CarregarUtilizadores();
                CarregarProdutos();
            }

        }

        protected void LinkButtonPainelGeral_Click(object sender, EventArgs e)
        {
            MostrarPainel(panelEstatisticas);
        }

        protected void LinkButtonUsers_Click(object sender, EventArgs e)
        {
            MostrarPainel(panelUtilizadores);
        }

        protected void LinkButtonAvaliacoes_Click(object sender, EventArgs e)
        {
            MostrarPainel(panelAvaliacoes);
        }

        protected void LinkButtonSuporte_Click(object sender, EventArgs e)
        {
            MostrarPainel(panelSuporte);
        }

        protected void LinkButtonProdutos_Click(object sender, EventArgs e)
        {
            MostrarPainel(panelProdutos);
        }
        private void MostrarPainel(Panel painelAtivo)
        {
            panelEstatisticas.Visible = false;
            panelUtilizadores.Visible = false;
            panelProdutos.Visible = false;
            panelAvaliacoes.Visible = false;
            panelSuporte.Visible = false;

            painelAtivo.Visible = true;
        }

        private void CarregarEstatisticas()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                conn.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("SELECT COUNT(*) FROM Utilizadores WHERE cod_perfil = '2'", conn);
                lblClientes.Text = cmd.ExecuteScalar().ToString();

                cmd = new SqlCommand("SELECT COUNT(*) FROM Utilizadores WHERE cod_perfil = '3'", conn);
                lblPrestadores.Text = cmd.ExecuteScalar().ToString();

                cmd = new SqlCommand("SELECT COUNT(*) FROM Servicos", conn);
                lblServicos.Text = cmd.ExecuteScalar().ToString();

                cmd = new SqlCommand("SELECT COUNT(*) FROM Produtos", conn);
                lblProdutos.Text = cmd.ExecuteScalar().ToString();
            }
        }

        private void CarregarAvaliacoes()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT a.id_avaliacao, a.id_avaliador, a.id_avaliado, u.nome as nome_avaliador, u2.nome as nome_avaliado, a.comentario, a.nota, a.aprovado 
                FROM Avaliacoes a 
                INNER JOIN Utilizadores u  ON a.id_avaliador = u.id_utilizador 
                INNER JOIN Utilizadores u2 ON a.id_avaliado = u2.id_utilizador", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                rpAvaliacoes.DataSource = reader;
                rpAvaliacoes.DataBind();
            }
        }

        private void CarregarTickets()
        {
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            //{
            //    conn.Open();
            //    SqlCommand cmd = new SqlCommand("SELECT id_ticket, assunto, mensagem, status FROM Suporte", conn);
            //    SqlDataReader reader = cmd.ExecuteReader();
            //    rpSuporte.DataSource = reader;
            //    rpSuporte.DataBind();
            //}
        }
        //CODE BEHIND PAINEL UTILIZADORES
        protected void lv_utilizadores_ItemCommand(object source, ListViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            string comando = e.CommandName;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                conn.Open();
                SqlCommand cmd;

                if (comando == "Ativar")
                {
                    cmd = new SqlCommand($"UPDATE Utilizadores SET ativo = 1 WHERE id_utilizador = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    lbl_mensagem.Text = "Utilizador ativado.";
                }
                else if (comando == "Desativar")
                {
                    cmd = new SqlCommand($"UPDATE Utilizadores SET ativo = 0 WHERE id_utilizador = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    lbl_mensagem.Text = "Utilizador desativado.";
                }
                else if (comando == "Excluir")
                {
                    cmd = new SqlCommand($"DELETE FROM Utilizadores WHERE id_utilizador = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    lbl_mensagem.Text = "Utilizador removido.";
                }

            }

            CarregarUtilizadores();

        }

        protected void CarregarUtilizadores()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = @"SELECT u.id_utilizador, u.nome, u.email, u.telemovel, u.ativo, tp.cod_perfil, tp.nome_perfil          
                FROM Utilizadores u
                INNER JOIN TipoPerfil tp ON u.cod_perfil = tp.cod_perfil";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            lv_utilizadores.DataSource = dt;
            lv_utilizadores.DataBind();
        }
        //CODE BEHIND PAINEL PRODUTOS

        public void CarregarProdutos()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = "SELECT p.id_produto, p.nome AS nome_produto, p.preco, p.stock, p.id_categoria, c.nome AS nome_categoria, p.imagem FROM Produtos p INNER JOIN Categorias c ON p.id_categoria = c.id_categoria";

                SqlCommand myCmd = new SqlCommand(query, conn);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(myCmd);
                da.Fill(dt);

                PagedDataSource pds = new PagedDataSource();
                pds.DataSource = dt.DefaultView;
                pds.AllowPaging = true;
                pds.PageSize = TamanhoPagina;
                pds.CurrentPageIndex = PaginaAtual;

                btnAnterior.Enabled = !pds.IsFirstPage;
                btnProxima.Enabled = !pds.IsLastPage;
                lblPagina.Text = $"Página {PaginaAtual + 1} de {pds.PageCount}";
                lv_produtos.DataSource = pds;
                lv_produtos.DataBind();

            }

        }
        protected void lv_produtos_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;
                if (dr != null)
                {
                    Label lblId = e.Item.FindControl("lbl_id") as Label;
                    if (lblId != null && dr.Row.Table.Columns.Contains("id_produto"))
                    {
                        lblId.Text = dr["id_produto"].ToString();
                    }

                    // Também faça o mesmo para os outros controles
                    TextBox tbNome = e.Item.FindControl("tb_nome") as TextBox;
                    if (tbNome != null && dr.Row.Table.Columns.Contains("nome_produto"))
                        tbNome.Text = dr["nome_produto"].ToString();

                    TextBox tbPreco = e.Item.FindControl("tb_preco") as TextBox;
                    if (tbPreco != null && dr.Row.Table.Columns.Contains("preco"))
                        tbPreco.Text = dr["preco"].ToString();

                    TextBox tbStock = e.Item.FindControl("tb_stock") as TextBox;
                    if (tbStock != null && dr.Row.Table.Columns.Contains("stock"))
                        tbStock.Text = dr["stock"].ToString();

                    DropDownList ddlCategoria = e.Item.FindControl("ddl_categoria") as DropDownList;
                    if (ddlCategoria != null && dr.Row.Table.Columns.Contains("id_categoria"))
                        ddlCategoria.SelectedValue = dr["id_categoria"].ToString();

                    Image img = e.Item.FindControl("Image_produto") as Image;
                    if (img != null && dr["imagem"] != DBNull.Value)
                    {
                        byte[] imagem = dr["imagem"] as byte[];
                        if (imagem != null)
                        {
                            img.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(imagem);
                        }
                    }
                }
            }
        }
        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            PaginaAtual--;
            CarregarUtilizadores();
        }

        protected void btnProxima_Click(object sender, EventArgs e)
        {
            PaginaAtual++;
            CarregarUtilizadores();
        }

        protected void btn_inserir_Click(object sender, EventArgs e)
        {
            Response.Redirect("inserir_produtos.aspx");
        }

        protected void lv_utilizadores_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            lv_utilizadores.EditIndex = -1;
            CarregarUtilizadores();
        }

        //protected void lv_produtos_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        //{
        //    DataPager2.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        //    lv_produtos.EditIndex = -1;
        //    CarregarProdutos();
        //}

        public int PaginaAtual
        {
            get => ViewState["PaginaAtual"] != null ? (int)ViewState["PaginaAtual"] : 0;
            set => ViewState["PaginaAtual"] = value;
        }

        const int TamanhoPagina = 7;

        protected void lv_produtos_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lv_produtos.EditIndex = e.NewEditIndex;
            CarregarProdutos(); // método que você deve usar para recarregar os dados do ListView
        }

        protected void lv_produtos_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lv_produtos.EditIndex = -1;
            CarregarProdutos();
        }

        protected void lv_produtos_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            int idProduto = Convert.ToInt32(lv_produtos.DataKeys[e.ItemIndex].Value);
            ListViewItem item = lv_produtos.Items[e.ItemIndex];

            string nome = ((TextBox)item.FindControl("tb_nome")).Text;
            decimal preco = Convert.ToDecimal(((TextBox)item.FindControl("tb_preco")).Text);
            int stock = Convert.ToInt32(((TextBox)item.FindControl("tb_stock")).Text);
            int idCategoria = Convert.ToInt32(((DropDownList)item.FindControl("ddl_categoria")).SelectedValue);
            FileUpload fileUpload = (FileUpload)item.FindControl("fu_imagem");

            byte[] imagemBytes = null;

            if (fileUpload.HasFile)
            {
                imagemBytes = fileUpload.FileBytes;
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                conn.Open();

                string query = @"
            UPDATE Produtos
               SET nome = @nome,
                   preco = @preco,
                   stock = @stock,
                   id_categoria = @id_categoria"
                           + (imagemBytes != null ? ", imagem = @imagem" : "") +
                           " WHERE id_produto = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idProduto);
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@preco", preco);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@id_categoria", idCategoria);
                if (imagemBytes != null)
                    cmd.Parameters.AddWithValue("@imagem", imagemBytes);

                cmd.ExecuteNonQuery();
            }

            lv_produtos.EditIndex = -1;
            CarregarProdutos();
            lbl_msg.Text = "Produto atualizado com sucesso!";
        }


        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            // Encerrar a autenticação com Google (caso esteja autenticado)
            HttpContext.Current.GetOwinContext().Authentication.SignOut();


            Response.Redirect("login.aspx");
        }
    }
}