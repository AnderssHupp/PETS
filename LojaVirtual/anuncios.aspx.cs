using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LojaVirtual
{
    public partial class anuncios : System.Web.UI.Page
    {
        public string id_anuncio;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                CarregarTipoServicos();
                CarregarAnuncios();
            }
        }
        protected void lv_anuncios_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            // define as novas propriedades da pagina e recarrega os produtos
            DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            CarregarAnuncios();
        }
        private void CarregarTipoServicos()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT cod_tipoServico, nome_servico FROM TipoServico", conn);
                ddl_servicos.DataSource = cmd.ExecuteReader();
                ddl_servicos.DataTextField = "nome_servico";
                ddl_servicos.DataValueField = "cod_tipoServico";
                ddl_servicos.DataBind();
                ddl_servicos.Items.Insert(0, new ListItem("Todos os serviços", ""));
            }
        }
        protected void AplicarFiltros(object sender, EventArgs e)
        {
            CarregarAnuncios();
        }
        protected void btn_geolocate_Click(object sender, EventArgs e)
        {
            if (Session["Latitude"] == null && Session["Longitude"] == null)
            {
                if (!string.IsNullOrEmpty(hiddenLat.Value) && !string.IsNullOrEmpty(hiddenLon.Value))
                {
                    Session["Latitude"] = hiddenLat.Value;
                    Session["Longitude"] = hiddenLon.Value;

                    CarregarAnuncios();
                }
            }
        }
        public void CarregarAnuncios()
        {
            string tipo = ddl_servicos.SelectedValue;
            string localidade = ddl_localidade.SelectedValue;
            string precoMax = ddl_preco.SelectedValue;

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);

            StringBuilder query = new StringBuilder(@"SELECT a.id_anuncio, a.titulo, a.id_cliente, a.cod_tipoServico, a.descricao, a.localidade, a.latitude, a.longitude, a.preco, a.data_nece, a.hora_nece, a.cod_status, p.nome_pet, p.Foto, tp.cod_TipoServico, tp.nome_servico as tipo_servico 
                FROM Anuncios a 
                INNER JOIN Pets p ON a.id_pet = p.id_pet
                INNER JOIN TipoServico tp ON a.cod_TipoServico = tp.cod_TipoServico
                WHERE cod_status = '1'");

            SqlCommand myCmd = new SqlCommand();

            myCmd.Connection = myConn;

            // filtros dinamicos
            if (!string.IsNullOrEmpty(tipo))
            {
                query.Append(" AND a.cod_tipoServico = @tipo ");
                myCmd.Parameters.AddWithValue("@tipo", Convert.ToInt32(tipo));
            }

            if (!string.IsNullOrEmpty(precoMax))
            {
                query.Append(" AND a.preco <= @preco ");
                myCmd.Parameters.AddWithValue("@preco", Convert.ToDecimal(precoMax));
            }

            myCmd.CommandText = query.ToString();

            SqlDataAdapter dataAdapter = new SqlDataAdapter(myCmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            // add nova coluna para imagem base64
            dt.Columns.Add("FotoBase64", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                byte[] imagemBytes = row["Foto"] as byte[];
                if (imagemBytes != null)
                {
                    string base64String = Convert.ToBase64String(imagemBytes);
                    row["FotoBase64"] = $"data:image/jpeg;base64,{base64String}";
                }
                //else
                //{
                //    row["FotoBase64"] = "";
                //}

            }
            double? lat = null, lon = null;

            if (Session["Latitude"] != null && Session["Longitude"] != null)
            {
                lat = Convert.ToDouble(Session["Latitude"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                lon = Convert.ToDouble(Session["Longitude"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }
            DataTable dtFiltrado = new DataTable();
            if (lat != null && lon != null)
            {
                int raioSelecionado = 10; // valor padrão
                if (!string.IsNullOrEmpty(ddl_raio.SelectedValue))
                    raioSelecionado = Convert.ToInt32(ddl_raio.SelectedValue);

                // filtra os anuncios por distancia

                List<DataRow> anunciosDentroDoRaio = new List<DataRow>();

                foreach (DataRow row in dt.Rows)
                {
                    if (row["Latitude"] != DBNull.Value && row["Longitude"] != DBNull.Value)
                    {
                        double anuncioLat = Convert.ToDouble(row["Latitude"]);
                        double anuncioLon = Convert.ToDouble(row["Longitude"]);
                        double distancia = CalcularDistancia(lat.Value, lon.Value, anuncioLat, anuncioLon);

                        if (distancia <= raioSelecionado)
                        {
                            anunciosDentroDoRaio.Add(row);
                        }
                    }
                }

                dtFiltrado = dt.Clone();

                foreach (DataRow row in anunciosDentroDoRaio)
                {
                    dtFiltrado.ImportRow(row);
                }
            }
            else
            {
                // filtro manual por localidade se n ha localizacao
                if (!string.IsNullOrEmpty(localidade))
                {
                    var resultados = dt.AsEnumerable()
                        .Where(r => r["localidade"].ToString().IndexOf(localidade, StringComparison.OrdinalIgnoreCase) >= 0);

                    dtFiltrado = resultados.Any() ? resultados.CopyToDataTable() : dt.Clone();
                }
                else
                {
                    dtFiltrado = dt; // sem filtro definido mostra todos
                }

                string json = GerarJsonPrestadores(dtFiltrado);
                ClientScript.RegisterStartupScript(this.GetType(), "mapa", $"initMap({json});", true);
            }
            lv_anuncios.DataSource = dtFiltrado;
            lv_anuncios.DataBind();

        }
        public string GerarJsonPrestadores(DataTable dt)
        {
            var anuncios = dt.AsEnumerable()
                .Where(row => row["latitude"] != DBNull.Value && row["longitude"] != DBNull.Value)
                .Select(row => new
                {
                    titulo = row["titulo"].ToString(),
                    descricao = row["descricao"].ToString(),
                    localidade = row["localidade"].ToString(),
                    lat = Convert.ToDouble(row["latitude"]),
                    lon = Convert.ToDouble(row["longitude"])
                }).ToList();

            return Newtonsoft.Json.JsonConvert.SerializeObject(anuncios);
        }
        protected void ImagePet_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "VerDetalhes")
            {
                id_anuncio = e.CommandArgument.ToString();
                // redireciona para a pagina de detalhes com o ID do anuncio
                Response.Redirect($"detalhes_anuncio.aspx?id_anuncio={id_anuncio}");
            }
        }
        public double CalcularDistancia(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371.0; // raio da Terra em km
            var dLat = (lat2 - lat1) * Math.PI / 180.0;
            var dLon = (lon2 - lon1) * Math.PI / 180.0;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }
        protected void lv_anuncios_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "VerDetalhes" || e.CommandName == "Candidatar")
            {
                string id_anuncio = e.CommandArgument.ToString();
                Response.Redirect($"detalhes_anuncio.aspx?id_anuncio={id_anuncio}");
            }
        }

        protected void btnEnviarCandidatura_Click(object sender, EventArgs e)
        {
            int id_anuncio = Convert.ToInt32(hf_idAnuncio.Value);
            int id_utilizador = Convert.ToInt32(Session["user_id"]);

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