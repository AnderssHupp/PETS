using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace LojaVirtual
{
    public partial class criar_anuncio : System.Web.UI.Page
    {
        public int id_utilizador;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["user_id"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    VerificarPets();
                    id_utilizador = Convert.ToInt32(Session["user_id"]);    
                }

            }
        }

        private void VerificarPets()
        {
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = "SELECT id_pet, nome_pet FROM Pets WHERE id_utilizador = @id";
                SqlCommand cmd = new SqlCommand(query, myConn);
                cmd.Parameters.AddWithValue("@id", id_utilizador);
                myConn.Open();
                int count = (int)cmd.ExecuteScalar(); // Retorna o número de pets

                if (count == 0)
                {
                    // Nenhum pet cadastrado, redireciona
                    lbl_msgPet.Visible = true;
                    btnCadastrarPet.Visible = true;
                    panelFormulario.Visible = false;
                }

            }
            

        }

        public class NominatimResult
        {
            public string lat { get; set; }
            public string lon { get; set; }
        }

        public async System.Threading.Tasks.Task<(double, double)> ObterCoordenadasAsync(string localidade)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("PetSitterApp/1.0");

                string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(localidade)}&format=json&limit=1";

                var response = await client.GetStringAsync(url);
                var resultado = JsonConvert.DeserializeObject<List<NominatimResult>>(response);

                if (resultado != null && resultado.Count > 0)
                {
                    double lat = double.Parse(resultado[0].lat, System.Globalization.CultureInfo.InvariantCulture);
                    double lon = double.Parse(resultado[0].lon, System.Globalization.CultureInfo.InvariantCulture);
                    return (lat, lon);
                }

                return (0, 0); // fallback se não encontrar
            }
        }
        protected async void btn_publicar_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                int idPet = Convert.ToInt32(ddl_pets.SelectedValue);
                string titulo = tb_titulo.Text;
                int cod_tipoServico = Convert.ToInt32(ddl_servicos.SelectedItem.Value);

                string descricao = tb_descricao.Text;
                string localidade = ddl_localidade.SelectedItem.Text;
                decimal preco = Convert.ToDecimal(tb_preco.Text);
                DateTime dataNecessidade = DateTime.Parse(tb_data.Text);
                TimeSpan horaNecessidade = TimeSpan.Parse(tb_hora.Text);
                int cod_status = 1;
                // obter coordenadas da localidade via OpenStreetMap

                var (latitude, longitude) = await ObterCoordenadasAsync(localidade);

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("publicar_anuncio", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //parametros
                    cmd.Parameters.AddWithValue("@id_cliente", Convert.ToInt32(Session["user_id"]));
                    cmd.Parameters.AddWithValue("@id_pet", idPet);
                    cmd.Parameters.AddWithValue("@titulo", titulo);
                    cmd.Parameters.AddWithValue("@cod_tipoServico", cod_tipoServico);
                    cmd.Parameters.AddWithValue("@descricao", descricao);
                    cmd.Parameters.AddWithValue("@localidade", localidade);
                    cmd.Parameters.AddWithValue("@latitude", latitude);
                    cmd.Parameters.AddWithValue("@longitude", longitude);
                    cmd.Parameters.AddWithValue("@data_nece", dataNecessidade.Date);
                    cmd.Parameters.AddWithValue("@hora_nece", horaNecessidade);
                    cmd.Parameters.AddWithValue("@preco", preco);
                    cmd.Parameters.AddWithValue("@data_publi", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@cod_status", cod_status);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                lbl_mensagem.Text = "Anúncio publicado com sucesso!";
            }
            catch (Exception ex)
            {
                lbl_mensagem.Text = "Erro ao publicar anúncio: " + ex.Message + "<br/>Stack: " + ex.StackTrace;
            }

        }


        protected void btnCadastrarPet_Click(object sender, EventArgs e)
        {
            Response.Redirect("inserir_pet.aspx");
        }
    }
}