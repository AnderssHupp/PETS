using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;

namespace LojaVirtual
{
    public partial class prestadores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarPrestadores();
            }
        }
        protected void CarregarPrestadores()
        {
            string localidade = ddl_localidade.SelectedValue;
            //string precoMax = ddl_preco.SelectedValue;
            string avaliacaoMin = ddl_avaliacao.SelectedValue;
            string disponibilidade = ddl_disponibilidade.SelectedValue;
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);
            StringBuilder query = new StringBuilder(@"SELECT u.id_utilizador, u.nome, u.email, u.latitude, u.longitude, u.localidade, u.foto_perfil,pi.disponibilidade,pi.experiencia, AVG(a.nota) AS nota_media 
                FROM Utilizadores u 
                INNER JOIN PrestadorInfo pi ON u.id_utilizador = pi.id_utilizador
                LEFT JOIN Avaliacoes a ON u.id_utilizador = a.id_avaliado 
                WHERE u.cod_perfil = 3");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = myConn;
            //filtros dinamicos
            if (!string.IsNullOrEmpty(localidade))
            {
                query.Append(" AND localidade = @localidade");
                cmd.Parameters.AddWithValue("@localidade", localidade);
            }
            //if (!string.IsNullOrEmpty(precoMax))
            //{
            //    query.Append(" AND preco_medio <= @precoMax");
            //    cmd.Parameters.AddWithValue("@precoMax", Convert.ToDecimal(precoMax));
            //}

            if (!string.IsNullOrEmpty(disponibilidade))
            {
                if (disponibilidade == "Semana")
                {
                    query.Append(" AND pi.disponibilidade IN ('Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta')");
                }
                else if (disponibilidade == "FDS")
                {
                    query.Append(" AND pi.disponibilidade IN ('Sábado', 'Domingo')");
                }
            }
            query.Append(@"
            GROUP BY 
                u.id_utilizador, u.nome, u.email, u.localidade, u.latitude, u.longitude,u.foto_perfil, pi.disponibilidade,pi.experiencia");

            if (!string.IsNullOrEmpty(avaliacaoMin))
            {
                query.Append(" HAVING AVG(a.nota) >= @avaliacaoMin");
                cmd.Parameters.AddWithValue("@avaliacaoMin", Convert.ToInt32(avaliacaoMin));
            }
            //passa o comando de acordo com o filtro
            cmd.CommandText = query.ToString();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            //carregar a foto_perfil
            dt.Columns.Add("FotoBase64", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                if (row["foto_perfil"] != DBNull.Value)
                {
                    byte[] fotoBytes = (byte[])row["foto_perfil"];
                    string base64 = Convert.ToBase64String(fotoBytes);
                    row["FotoBase64"] = "data:image/jpeg;base64," + base64;
                }
                else
                {
                    // imagem padrão
                    row["FotoBase64"] = "https://images.assetsdelivery.com/compings_v2/tanyastock/tanyastock1608/tanyastock160801788.jpg";
                }
            }

            //utilizar geolocalizacao para os filtros
            double? userLat = null, userLon = null;
            if (Session["Latitude"] != null && Session["Longitude"] != null)
            {
                userLat = Convert.ToDouble(Session["Latitude"], CultureInfo.InvariantCulture);
                userLon = Convert.ToDouble(Session["Longitude"], CultureInfo.InvariantCulture);
            }
            DataTable dtFinal = new DataTable();

            if (userLat != null && userLon != null)
            {
                int raioSelecionado = 10; // valor padrao
                if (!string.IsNullOrEmpty(ddl_raio.SelectedValue))
                    raioSelecionado = Convert.ToInt32(ddl_raio.SelectedValue);

                // filtra por raio
                List<DataRow> prestadoresProximos = new List<DataRow>();

                foreach (DataRow row in dt.Rows)
                {
                    if (row["Latitude"] != DBNull.Value && row["Longitude"] != DBNull.Value)
                    {
                        double prestLat = Convert.ToDouble(row["Latitude"]);
                        double prestLon = Convert.ToDouble(row["Longitude"]);
                        double distancia = CalcularDistancia(userLat.Value, userLon.Value, prestLat, prestLon);

                        if (distancia <= raioSelecionado)
                        {
                            prestadoresProximos.Add(row);
                        }
                    }
                }
                dtFinal = dt.Clone();
                foreach (var row in prestadoresProximos)
                {
                    dtFinal.ImportRow(row);
                }
                // filtro manual por localidade se n ha localização
            }
            else
            if (!string.IsNullOrEmpty(localidade))
            {
                var resultados = dt.AsEnumerable()
                    .Where(r => r["localidade"].ToString().IndexOf(localidade, StringComparison.OrdinalIgnoreCase) >= 0);

                dtFinal = resultados.Any() ? resultados.CopyToDataTable() : dt.Clone();
            }
            else
            {
                dtFinal = dt; // sem filtro mostra todos
            }

            string json = GerarJsonPrestadores(dtFinal);
            ClientScript.RegisterStartupScript(this.GetType(), "mapa", $"initMap({json});", true);

            lv_prestadores.DataSource = dtFinal;
            lv_prestadores.DataBind();

        }
        public string GerarJsonPrestadores(DataTable dt)
        {
            var prestadores = dt.AsEnumerable()
                .Where(row => row["latitude"] != DBNull.Value && row["longitude"] != DBNull.Value)
                .Select(row => new
                {
                    nome = row["nome"].ToString(),
                    localidade = row["localidade"].ToString(),
                    lat = Convert.ToDouble(row["latitude"]),
                    lon = Convert.ToDouble(row["longitude"])
                }).ToList();

            return Newtonsoft.Json.JsonConvert.SerializeObject(prestadores);
        }
        protected void AplicarFiltros(object sender, EventArgs e)
        {
            CarregarPrestadores();
        }

        protected void lv_prestadores_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            CarregarPrestadores();
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

        protected void btn_geolocate_Click(object sender, EventArgs e)
        {
            if (Session["Latitude"] == null && Session["Longitude"] == null)
            {
                if (!string.IsNullOrEmpty(hiddenLat.Value) && !string.IsNullOrEmpty(hiddenLon.Value))
                {
                    Session["Latitude"] = hiddenLat.Value;
                    Session["Longitude"] = hiddenLon.Value;

                    CarregarPrestadores();
                }
            }
        }
        protected void btn_Contactar_Click(object sender, EventArgs e)
        {
            if (Session["user_id"] == null)
            {
                Response.Redirect($"login.aspx");

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "abrirModal", "abrirModal();", true);
            }
        }

        protected void btnEnviarMensagem_Click(object sender, EventArgs e)
        {
            if (Session["user_id"] != null)
            {

                int id_utilizador = Convert.ToInt32(Session["user_id"]);
                int id_destinatario = Convert.ToInt32(hf_idPrestador.Value);

                SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);


                SqlCommand myCmd = new SqlCommand("INSERT INTO Mensagens (id_remetente, id_destinatario, mensagem, data_envio) VALUES (@r,@d, @msg, GETDATE())", myConn);
                myCmd.Parameters.AddWithValue("@r", id_utilizador);
                myCmd.Parameters.AddWithValue("@d", id_destinatario);
                myCmd.Parameters.AddWithValue("@msg", tb_msg.Text.Trim());

                myConn.Open();

                int linhasAfetadas = myCmd.ExecuteNonQuery();
                myConn.Close();


                if (linhasAfetadas > 0)
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Mensagem enviada com sucesso.'); fecharModal();", true);
                else
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('error'); fecharModal();", true);

                tb_msg.Text = "";
            }
            else
            {
                Response.Redirect("login.aspx");
            }
        }
    }
}

