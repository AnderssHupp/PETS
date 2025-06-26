using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LojaVirtual
{
    public partial class detalhes_anuncio : System.Web.UI.Page
    {
        public int id_anuncio, id_utilizador;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["user_id"] != null)
                {
                    id_utilizador = Convert.ToInt32(Session["user_id"]);
                }
                else
                {
                    Response.Redirect("login.aspx");
                }
                if (Request.QueryString["id_anuncio"] != null)
                {
                    int id = Convert.ToInt32(Request.QueryString["id_anuncio"]);
                    ViewState["id_anuncio"] = id;
                    CarregarDetalhes(id);
                }

            }
            else if (ViewState["id_anuncio"] != null)
            {
                int id = Convert.ToInt32(ViewState["id_anuncio"]);
                CarregarDetalhes(id);
            }
        }

        private void CarregarDetalhes(int id_anuncio)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);


            string query = @"SELECT a.id_anuncio, a.titulo, a.id_cliente, a.cod_tipoServico, tp.nome_servico as tipo_servico, a.descricao, a.localidade, a.preco, a.data_publi, a.cod_status,p.id_pet, p.nome_pet, p.foto, p.especie, p.raca, p.idade, p.peso, p.observacoes 
                FROM Anuncios a 
                INNER JOIN Pets p ON a.id_pet = p.id_pet 
                INNER JOIN TipoServico tp ON a.cod_TipoServico = tp.cod_TipoServico
                WHERE  id_anuncio = @id_anuncio";

            SqlCommand myCommand = new SqlCommand(query, myConn);
            myCommand.Parameters.AddWithValue("@id_anuncio", id_anuncio);

            myConn.Open();
            SqlDataReader reader = myCommand.ExecuteReader();

            if (reader.Read())
            {
                lbl_titulo.Text = reader["titulo"].ToString();
                lbl_nome.Text = reader["nome_pet"].ToString();
                lbl_especie.Text = reader["especie"].ToString();
                lbl_raca.Text = reader["raca"].ToString();
                lbl_idade.Text = reader["idade"].ToString();
                lbl_peso.Text = reader["peso"].ToString();
                lbl_observacoes.Text = reader["observacoes"].ToString();
                //lbl_tipo.Text = reader["tipo_servico"].ToString();
                lbl_descricao.Text = reader["descricao"].ToString();
                lbl_local.Text = reader["localidade"].ToString();
                lbl_preco.Text = Convert.ToDecimal(reader["preco"]).ToString("N2");

              
                int id_pet = Convert.ToInt32(reader["id_pet"]); 
                if (id_pet > 0)
                {
                    CarregarFotosCarousel(id_anuncio);
                }
            }
            reader.Close();
            myConn.Close();
        }

        private void CarregarFotosCarousel(int id_anuncio)
        {
            List<string> imagensBase64 = new List<string>();
            string connStr = ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // pegar a imagem principal do pet
                string queryPrincipal = @"SELECT p.Foto, p.id_pet FROM Anuncios a 
                                  INNER JOIN Pets p ON a.id_pet = p.id_pet 
                                  WHERE a.id_anuncio = @id_anuncio";

                SqlCommand cmd1 = new SqlCommand(queryPrincipal, conn);
                cmd1.Parameters.AddWithValue("@id_anuncio", id_anuncio);
                SqlDataReader dr1 = cmd1.ExecuteReader();

                int id_pet = -1;
                if (dr1.Read())
                {
                    id_pet = Convert.ToInt32(dr1["id_pet"]);

                    if (dr1["Foto"] != DBNull.Value)
                    {
                        byte[] img = (byte[])dr1["Foto"];
                        imagensBase64.Add("data:image/jpeg;base64," + Convert.ToBase64String(img));
                    }
                }
                dr1.Close();

                //pegar fotos extras do FotosPets
                if (id_pet > 0)
                {
                    string queryExtras = "SELECT foto FROM FotosPets WHERE id_pet = @id_pet";
                    SqlCommand cmd2 = new SqlCommand(queryExtras, conn);
                    cmd2.Parameters.AddWithValue("@id_pet", id_pet);
                    SqlDataReader dr2 = cmd2.ExecuteReader();

                    while (dr2.Read())
                    {
                        if (dr2["foto"] != DBNull.Value)
                        {
                            byte[] img = (byte[])dr2["foto"];
                            imagensBase64.Add("data:image/jpeg;base64," + Convert.ToBase64String(img));
                        }

                    }
                    dr2.Close();
                }

                // bind no Repeater
                DataTable dt = new DataTable();
                dt.Columns.Add("FotoBase64", typeof(string));
                foreach (var base64 in imagensBase64)
                {
                    dt.Rows.Add(base64);
                }

                rpt_carousel.DataSource = dt;
                rpt_carousel.DataBind();

                conn.Close();
            }
        }

        protected void btnEnviarCandidatura_Click(object sender, EventArgs e)
        {
            int id_anuncio = Convert.ToInt32(hf_idAnuncio.Value);
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand("candidatar", myConn);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.AddWithValue("@id_anuncio", id_anuncio);
            myCommand.Parameters.AddWithValue("@id_utilizador", id_utilizador);
            myCommand.Parameters.AddWithValue("@mensagem", tb_msg.Text);
            myCommand.Parameters.AddWithValue("@data_candidatura", DateTime.Now);
            myCommand.Parameters.AddWithValue("@cod_status", 5);

            SqlParameter retorno = new SqlParameter("@retorno", SqlDbType.Int);
            retorno.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(retorno);

            myConn.Open();
            myCommand.ExecuteNonQuery();
            int respostaSP = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);
            myConn.Close();

            if (respostaSP == -1)
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Já se candidatou a este anúncio.'); fecharModal();", true);
            else
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Candidatura enviada com sucesso.'); fecharModal();", true);

            tb_msg.Text = "";

        }
    }
}