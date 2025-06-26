using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;

namespace LojaVirtual
{
    public partial class loja : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarProdutos();
            }
        }

        private void CarregarProdutos()
        {
            DataTable dataTable = ObterData();
            ListView1.DataSource = dataTable;
            ListView1.DataBind();
        }

        private DataTable ObterData()
        {
            DataTable dt = new DataTable();
            string ordernar = ddlOrdenacao.SelectedValue ?? "nome ASC";
            string pesquisa = tb_pesquisa.Text.Trim(); // obter o valor da pesquisa

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString))
            {
                string query = "SELECT id_produto, nome, preco, imagem FROM Produtos WHERE nome LIKE @pesquisa ORDER BY " + ordernar;
                using (SqlCommand comando = new SqlCommand(query, con))
                {
                    comando.Parameters.AddWithValue("@pesquisa", "%" + pesquisa + "%"); // filtra pelo nome

                    using (SqlDataAdapter da = new SqlDataAdapter(comando))
                    {
                        da.Fill(dt);
                        dt.Columns.Add("preco_desconto", typeof(string));
                        dt.Columns.Add("ImagemBase64", typeof(string));

                        foreach (DataRow row in dt.Rows)
                        {
                            byte[] imagemBytes = row["imagem"] as byte[];
                            if (imagemBytes != null)
                            {
                                string base64String = Convert.ToBase64String(imagemBytes);
                                row["ImagemBase64"] = $"data:image/png;base64,{base64String}";
                            }

                            if (Session["cod_perfil"] != null && Session["cod_perfil"].ToString() == "3")
                            {
                                decimal precoOriginal = Convert.ToDecimal(row["preco"]);
                                decimal precoComDesconto = precoOriginal * 0.8m;
                                row["preco_desconto"] = precoComDesconto.ToString("C");
                            }
                        }
                    }
                }
            }
            return dt;
        }


        protected void ddlOrdenacao_SelectedIndexChanged(object sender, EventArgs e)
        {

            CarregarProdutos();

        }

        protected void Image_produto_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "VerDetalhes")
            {
                string id_produto = e.CommandArgument.ToString();
                // redireciona para a pagina de detalhes com o ID do produto
                Response.Redirect($"detalhe_produto.aspx?id_produto={id_produto}");
            }
        }
        protected void ListView1_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            // define as novas propriedades da pagina e recarrega os produtos
            DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            CarregarProdutos();
        }

        protected void btn_adicionar_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int id_produto = int.Parse(button.CommandArgument);

            int? id_utilizador = ObterIdUtilizador(); // retorna null se n tiver logado
            int quantidade = 1;

            if (id_utilizador.HasValue)
            {
                // logado, add ao carrinho of
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

        protected void btn_pesquisa_Click(object sender, EventArgs e)
        {
            CarregarProdutos();
        }

        protected void btn_gestaoU_Click(object sender, EventArgs e)
        {
            Response.Redirect("gerir_utilizadores.aspx");
        }
    }
}
