using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LojaVirtual.loja;


namespace LojaVirtual
{

    public partial class Carrinho : System.Web.UI.Page
    {
        public decimal total = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)  // garante que o Repeater so e carregado na primeira vez
            {

                CarregarCarrinho();
                AtualizarTotal();
            }
        }


        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id_produto = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "aumentar")
            {
                AtualizarQuantidade(id_produto, 1);
            }
            else if (e.CommandName == "diminuir")
            {
                AtualizarQuantidade(id_produto, -1);
            }
            else if (e.CommandName == "excluir")
            {
                RemoverItemCarrinho(id_produto);
            }

            CarregarCarrinho();
            //Repeater1.DataBind();
            AtualizarTotal();
        }
        private void CarregarCarrinho()
        {
            int? id_utilizador = Session["user_id"] as int?;
            DataTable dt = new DataTable();
            if (id_utilizador.HasValue)
            {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString))
                {
                    conn.Open();
                    string query = @"
                    SELECT C.id_produto, P.nome, C.quantidade, P.preco, P.imagem, (C.quantidade * P.preco) AS total
                    FROM Carrinho C
                    INNER JOIN Produtos P ON C.id_produto = P.id_produto
                    WHERE C.id_utilizador = @id_utilizador";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_utilizador", id_utilizador);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            dt.Load(reader);
                            Repeater1.DataSource = dt;
                            Repeater1.DataBind();
                        }
                    }
                }
            }
            else if (Session["session_id"] != null)
            {

                // utilizador n logado, carrinhotemporario
                string sessionId = Session["session_id"].ToString();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT CT.id_produto, P.nome, CT.quantidade, P.preco, P.imagem, (CT.quantidade * P.preco) AS total
                        FROM CarrinhoTemp CT
                        INNER JOIN Produtos P ON CT.id_produto = P.id_produto
                        WHERE CT.session_id = @session_id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@session_id", sessionId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            dt.Load(reader);
                            Repeater1.DataSource = dt;
                            Repeater1.DataBind();
                        }
                    }
                }
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    Repeater1.DataSource = dt;
                    Repeater1.DataBind();
                }
                else
                {
                    lbl_mensagem.Text = "O carrinho está vazio!";
                    Repeater1.DataSource = null;
                    Repeater1.DataBind();
                }

            }
           

        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // acessa o id do produto, nome, preco e quantidade
                DataRowView dr = (DataRowView)e.Item.DataItem;

                Label lbl_nome = (Label)e.Item.FindControl("lbl_nome");
                if (lbl_nome != null)
                {
                    lbl_nome.Text = dr["nome"].ToString();
                }

                // define os valores diretamente nas labels e controles
                Label lbl_quantidade = (Label)e.Item.FindControl("lbl_quantidade");
                if (lbl_quantidade != null)
                {
                    lbl_quantidade.Text = dr["quantidade"].ToString();
                }
                Label lbl_preco = (Label)e.Item.FindControl("lbl_preco");
                if (lbl_preco != null)
                {
                    lbl_preco.Text = "€ " + dr["preco"].ToString();
                }

                System.Web.UI.WebControls.Image imgProduto = (System.Web.UI.WebControls.Image)e.Item.FindControl("Image_produto");
                // verifica se o campo da imagem n está nulo
                if (dr["imagem"] != DBNull.Value && dr["imagem"] is byte[])
                {
                    byte[] imagemBytes = (byte[])dr["imagem"];
                    string imgUrl = "data:image/jpeg;base64," + Convert.ToBase64String(imagemBytes);

                    // define a URL da imagem no controle
                    if (imgProduto != null)
                    {
                        imgProduto.ImageUrl = imgUrl;
                    }
                }
            }
        }

        protected decimal AtualizarTotal()
        {
            decimal total = 0;

            foreach (RepeaterItem item in Repeater1.Items)
            {
                Label lbl_preco = (Label)item.FindControl("lbl_preco");
                Label lbl_quantidade = (Label)item.FindControl("lbl_quantidade");

                if (lbl_preco != null && lbl_quantidade != null)
                {
                    string precoTexto = lbl_preco.Text.Replace("€", "").Trim();
                    decimal preco = decimal.Parse(precoTexto, System.Globalization.NumberStyles.Currency);
                    int quantidade = int.Parse(lbl_quantidade.Text);
                    total += preco * quantidade;
                }
            }

            if (Session["cod_perfil"] != null && Session["cod_perfil"].ToString() == "3")
            {
                decimal totalComDesconto = total * 0.8m;

                lbl_total.Text = $"<span style='text-decoration: line-through; color: red;'>€ {total:N2}</span> " +
                                 $"<span style='color: green; font-weight: bold;'>€ {totalComDesconto:N2}</span>";
            }
            else
            {
                lbl_total.Text = $"€ {total:N2}";
            }

            if (total == 0)
            {
                lbl_mensagem.Text = "O carrinho esta vazio!";
            }

            return total;  // retorna o valor total calculado
        }


        protected void AtualizarQuantidade(int id_produto, int quantidade)
        {
            int? id_utilizador = Session["user_id"] as int?;
            if (id_utilizador.HasValue)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString))
                {
                    conn.Open();

                    // verifica a quantidade atual do produto no carrinho
                    string selectQuery = "SELECT quantidade FROM Carrinho WHERE id_produto = @id_produto";
                    SqlCommand selectCmd = new SqlCommand(selectQuery, conn);

                    selectCmd.Parameters.AddWithValue("@id_produto", id_produto);

                    int quantidadeAtual = Convert.ToInt32(selectCmd.ExecuteScalar());

                    // calcula a nova quantidade
                    int novaQuantidade = quantidadeAtual + quantidade;

                    // se a nova quantidade for menor que 1, remove o item do carrinho
                    if (novaQuantidade < 1)
                    {
                        RemoverItemCarrinho(id_produto);
                    }
                    else
                    {
                        // atualiza a quantidade no banco de dados
                        string updateQuery = "UPDATE Carrinho SET quantidade = @quantidade WHERE id_produto = @id_produto";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@quantidade", novaQuantidade);

                        updateCmd.Parameters.AddWithValue("@id_produto", id_produto);
                        updateCmd.ExecuteNonQuery();
                    }

                    CarregarCarrinho();
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString))
                {
                    conn.Open();

                    // verifica a quantidade atual do produto no carrinho
                    string selectQuery = "SELECT quantidade FROM CarrinhoTemp WHERE id_produto = @id_produto";
                    SqlCommand selectCmd = new SqlCommand(selectQuery, conn);

                    selectCmd.Parameters.AddWithValue("@id_produto", id_produto);

                    int quantidadeAtual = Convert.ToInt32(selectCmd.ExecuteScalar());

                    // calcula a nova quantidade
                    int novaQuantidade = quantidadeAtual + quantidade;

                    // se a nova quantidade for menor que 1, remove o item do carrinho
                    if (novaQuantidade < 1)
                    {
                        RemoverItemCarrinho(id_produto);
                    }
                    else
                    {
                        // atualiza a quantidade no banco de dados
                        string updateQuery = "UPDATE CarrinhoTemp SET quantidade = @quantidade WHERE id_produto = @id_produto";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@quantidade", novaQuantidade);

                        updateCmd.Parameters.AddWithValue("@id_produto", id_produto);
                        updateCmd.ExecuteNonQuery();
                    }

                    CarregarCarrinho();
                }
            }
        }

        protected void RemoverItemCarrinho(int id_produto)
        {
            int? id_utilizador = Session["user_id"] as int?;
            if (id_utilizador.HasValue)
            {
                SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);


                myConn.Open();
                string query = "DELETE FROM Carrinho WHERE id_produto = @id_produto";
                SqlCommand myCommand = new SqlCommand(query, myConn);

                myCommand.Parameters.AddWithValue("@id_produto", id_produto);

                myCommand.ExecuteNonQuery();


                CarregarCarrinho();
            }
            else
            {
                SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);


                myConn.Open();
                string query = "DELETE FROM CarrinhoTemp WHERE id_produto = @id_produto";
                SqlCommand myCommand = new SqlCommand(query, myConn);

                myCommand.Parameters.AddWithValue("@id_produto", id_produto);

                myCommand.ExecuteNonQuery();


                CarregarCarrinho();
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            if (Session["user_id"] == null)
            {
                Response.Redirect("login.aspx?redirect=checkout.aspx");
            }
            else
            {
                //logado
                string id_utilizador = Session["user_id"].ToString();
                decimal totalCompra = AtualizarTotal();
                if (Session["cod_perfil"] != null && Session["cod_perfil"].ToString() == "3") //verfica qual o cod_perfil
                {
                    decimal totalComDesconto = totalCompra * 0.8m;
                    Response.Redirect($"checkout.aspx?total={totalComDesconto:N2}");
                }
                else
                {
                    Response.Redirect($"checkout.aspx?total={totalCompra:N2}");
                }
            }

        }
    }
}