using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LojaVirtual
{
    public partial class produto_detahe : System.Web.UI.Page
    {
        public int id_produto;
        protected void Page_Load(object sender, EventArgs e)
        {
            string userId = Session["user_id"]?.ToString();
         
            if (Request.QueryString["id_produto"] == null)
            {
                Response.Redirect("loja.aspx");
            }
            else
            {

                id_produto = Convert.ToInt32(Request.QueryString["id_produto"]);
                CarregarDetalhesProduto(id_produto);
            }

        }

        protected void CarregarDetalhesProduto(int id_produto)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);


            string query = $"SELECT p.id_produto, p.nome, p.preco, p.imagem, c.descricao  FROM Produtos p INNER JOIN Categorias c ON p.id_categoria = c.id_categoria WHERE id_produto = {id_produto}";
            SqlCommand myCommand = new SqlCommand(query, myConn);
            myCommand.Parameters.AddWithValue("id_produto", id_produto);

            myConn.Open();
            SqlDataReader dr = myCommand.ExecuteReader();
            if (dr.Read())
            {
                lbl_nome.Text = dr["nome"].ToString();
                lbl_descricao.Text = dr["descricao"].ToString();
                img_produto.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String((byte[])dr["imagem"]);

                // Obter o preço original
                decimal precoOriginal = Convert.ToDecimal(dr["preco"]);

                // Verifica se o usuário é revendedor 
                if (Session["cod_perfil"] != null && Session["cod_perfil"].ToString() == "3")
                {
                    decimal precoComDesconto = precoOriginal * 0.8m; // 20% de desconto

                    lbl_preco.Text = $"<span style='text-decoration: line-through; color: red;'>{precoOriginal:C}</span> " +
                                     $"<span style='color: green; font-weight: bold;'>{precoComDesconto:C}</span>";
                }
                else
                {
                    lbl_preco.Text = precoOriginal.ToString("C");
                }

            }
            myConn.Close();
        }

        protected void btn_voltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("loja.aspx");
        }

        protected void btn_add_Click(object sender, EventArgs e)
        {
       
            int? id_utilizador = ObterIdUtilizador(); // retorna null se n tiver logado
            int quantidade = 1;

            if (id_utilizador.HasValue)
            {
                // logado, add ao carrinho oficial
                using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("add_carrinho", myConn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("@id_produto", id_produto);
                        myCommand.Parameters.AddWithValue("@id_utilizador", id_utilizador);
                        myCommand.Parameters.AddWithValue("@quantidade", quantidade);

                        myConn.Open();
                        myCommand.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                // nao logado add ao CarrinhoTemporario
                Session["session_id"] = Session.SessionID;
                string sessionId = Session["session_id"].ToString();
                using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("INSERT INTO CarrinhoTemp (session_id, id_produto, quantidade) VALUES (@session_id, @id_produto, @quantidade)", myConn))
                    {
                        myCommand.Parameters.AddWithValue("@session_id", sessionId);
                        myCommand.Parameters.AddWithValue("@id_produto", id_produto);
                        myCommand.Parameters.AddWithValue("@quantidade", quantidade);

                        myConn.Open();
                        myCommand.ExecuteNonQuery();
                    }
                }
            }


            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Produto adicionado ao carrinho!');", true);
        }

        private int? ObterIdUtilizador()
        {
            if (Session["user_id"] != null)
            {
                return Convert.ToInt32(Session["user_id"]);
            }

            return null; // n logado

        }
    }
}