using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LojaVirtual
{
    public partial class Criar_ContaUni : System.Web.UI.Page
    {
        public string nome;
        public bool userGoogle;

        protected void Page_Load(object sender, EventArgs e)
        {

            userGoogle = Session["email_google"] != null && Session["nome_google"] != null;
            if (!IsPostBack)
            {
                if (userGoogle)
                {
                    userGoogle = true;
                    tb_nome_cliente.Text = tb_nome_prestador.Text = Session["nome_google"].ToString();
                    tb_email_cliente.Text = tb_email_prestador.Text = Session["email_google"].ToString();
                    tb_nome_cliente.ReadOnly = tb_email_cliente.ReadOnly = true;
                    tb_nome_prestador.ReadOnly = tb_email_prestador.ReadOnly = true;
                    tb_pass_prestador.Text = "";
                    // desativa campo de senha
                    panel_password.Visible = false;
                    panel_passP.Visible = false;
                }
            }
            //ScriptManager scriptManager = ScriptManager.GetCurrent(this);
            //scriptManager.RegisterPostBackControl(btn_criar_cliente);
            this.DataBind();
        }

        protected void btn_next_step1_Click(object sender, EventArgs e)
        {
            panel_step1.Visible = false;

            if (rbl_tipoConta.SelectedValue == "cliente")
            {
                panel_step2_cliente.Visible = true;
            }
            else if (rbl_tipoConta.SelectedValue == "prestador")
            {
                panel_step2_prestador.Visible = true;
            }
        }

        protected void btn_voltar_Click(object sender, EventArgs e)
        {
            panel_step2_cliente.Visible = false;
            panel_step2_prestador.Visible = false;
            panel_step1.Visible = true;
            rbl_tipoConta.ClearSelection();
        }

        protected async void btn_criar_cliente_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);
                SqlCommand myCommand = new SqlCommand();
                string localidade = ddl_localidade.SelectedItem.Text;
                var (latitude, longitude) = await ObterCoordenadasAsync(localidade);

                myCommand.Parameters.AddWithValue("@nome", tb_nome_cliente.Text);
                nome = tb_email_cliente.Text;
                myCommand.Parameters.AddWithValue("@email", tb_email_cliente.Text);
                myCommand.Parameters.AddWithValue("@password", EncryptString(tb_pass_cliente.Text));
                myCommand.Parameters.AddWithValue("@telemovel", tb_tel_cliente.Text);
                //myCommand.Parameters.AddWithValue("@nif", tb_nif_cliente.Text);

                myCommand.Parameters.AddWithValue("@localidade", localidade);
                myCommand.Parameters.AddWithValue("@latitude", latitude);
                myCommand.Parameters.AddWithValue("@longitude", longitude);

                if (FileUpload_cliente.HasFile)
                {
                    Stream imgStream = FileUpload_cliente.PostedFile.InputStream;
                    int tamanho = FileUpload_cliente.PostedFile.ContentLength;

                    byte[] imgBinaryData = new byte[tamanho];
                    imgStream.Read(imgBinaryData, 0, tamanho);
                    myCommand.Parameters.AddWithValue("@foto_perfil", imgBinaryData);

                }
                else
                {
                    myCommand.Parameters.AddWithValue("@foto_perfil", DBNull.Value);
                }

                myCommand.Parameters.AddWithValue("@cod_perfil", 2); // cliente
                myCommand.Parameters.AddWithValue("@data_criacao", DateTime.UtcNow);

                SqlParameter valor = new SqlParameter();
                valor.ParameterName = "@retorno";
                valor.Direction = ParameterDirection.Output;
                valor.SqlDbType = SqlDbType.Int;
                myCommand.Parameters.Add(valor);

                //comando de ir na bd buscar as storedprocedure
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "inserir_cliente";


                myCommand.Connection = myConn;

                myConn.Open();

                myCommand.ExecuteNonQuery();

                int respostaSP = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);

                myConn.Close();

                if (respostaSP == 1)
                {
                    lbl_mensagem.Text = "Utilizador criado com sucesso, no entanto terá de ativar sua conta através do email recebido!";
                    EnviarMail();

                    //Response.Redirect("login.aspx");

                }
                else
                {
                    lbl_mensagem.Text = "Utilizador já EXISTE!";
                }
            }
            catch (Exception ex)
            {
                lbl_mensagem.Text = "Erro:" + ex.Message;
            }
        }

        protected async void btn_criar_prestador_Click(object sender, EventArgs e)
        {
            try
            {
                string experiencia = ddl_experiencia.SelectedValue;
                string disponibilidade = ddl_disponibilidade.SelectedValue;
                string localidade = ddl_localidadeP.SelectedItem.Text;
                var (latitude, longitude) = await ObterCoordenadasAsync(localidade);

                SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);
                SqlCommand myCommand = new SqlCommand();

                myCommand.Parameters.AddWithValue("@nome", tb_nome_prestador.Text);
                nome = tb_nome_prestador.Text;
                myCommand.Parameters.AddWithValue("@email", tb_email_prestador.Text);
                if (userGoogle == true)
                {
                    myCommand.Parameters.AddWithValue("@password", "google_password");
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@password", EncryptString(tb_pass_prestador.Text));
                }
                myCommand.Parameters.AddWithValue("@telemovel", tb_tel_prestador.Text);

                myCommand.Parameters.AddWithValue("@experiencia", experiencia);
                myCommand.Parameters.AddWithValue("@disponibilidade", disponibilidade);
                myCommand.Parameters.AddWithValue("@localidade", localidade);
                myCommand.Parameters.AddWithValue("@latitude", latitude);
                myCommand.Parameters.AddWithValue("@longitude", longitude);
                myCommand.Parameters.AddWithValue("@cod_perfil", 3); // prestador
                if (FileUpload_prestador.HasFile)
                {
                    Stream imgStream = FileUpload_prestador.PostedFile.InputStream;
                    int tamanho = FileUpload_prestador.PostedFile.ContentLength;

                    byte[] imgBinaryData = new byte[tamanho];
                    imgStream.Read(imgBinaryData, 0, tamanho);
                    myCommand.Parameters.AddWithValue("@foto_perfil", imgBinaryData);

                }
                else
                {
                    myCommand.Parameters.AddWithValue("@foto_perfil", DBNull.Value);
                }
                    myCommand.Parameters.AddWithValue("@iban", tb_iban.Text);
                myCommand.Parameters.AddWithValue("@data_criacao", DateTime.UtcNow);


                SqlParameter valor = new SqlParameter();
                valor.ParameterName = "@retorno";
                ////tipo de parametro
                valor.Direction = ParameterDirection.Output;
                ////define o tipo do valor
                valor.SqlDbType = SqlDbType.Int;

                //parametro final
                myCommand.Parameters.Add(valor);

                //comando de ir na bd buscar as storedprocedure
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "inserir_prestador";


                myCommand.Connection = myConn;

                myConn.Open();

                myCommand.ExecuteNonQuery();

                int respostaSP = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);

                myConn.Close();

                if (respostaSP == 1)
                {
                    lbl_msg.Text = "Pet Sitter cadastrado com sucesso!!No entanto terá de ativar sua conta através do email recebido!";

                    EnviarMail();

                    //Response.Redirect("login.aspx");
                }
                else
                {
                    lbl_msg.Text = "Pet Sitter já EXISTE!";
                }
            }
            catch (Exception ex)
            {
                lbl_msg.Text = "Erro:" + ex.Message;
            }
        }
        public async Task<(double, double)> ObterCoordenadasAsync(string localidade)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("PetWorld/1.0");
                string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(localidade)}&format=json&limit=1";

                var response = await client.GetStringAsync(url);
                var resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NominatimResult>>(response);

                if (resultado != null && resultado.Count > 0)
                {
                    double lat = double.Parse(resultado[0].lat, System.Globalization.CultureInfo.InvariantCulture);
                    double lon = double.Parse(resultado[0].lon, System.Globalization.CultureInfo.InvariantCulture);
                    return (lat, lon);
                }

                return (0, 0);
            }
        }

        public class NominatimResult
        {
            public string lat { get; set; }
            public string lon { get; set; }
        }

        public static string EncryptString(string Message)
        {
            string Passphrase = "cinel";
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = System.Security.Cryptography.CipherMode.ECB;
            TDESAlgorithm.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            try
            {
                System.Security.Cryptography.ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            string enc = Convert.ToBase64String(Results);
            enc = enc.Replace("+", "KKKKKM");
            enc = enc.Replace("/", "JJJJJM");
            enc = enc.Replace("\\", "IIIIIM");
            return enc;
        }
        public void EnviarMail()
        {
            MailMessage email = new MailMessage();
            SmtpClient servidor = new SmtpClient();

            try
            {
                email.From = new MailAddress("yourEmail@email.com");

                //email.To.Add(tb_email.Text); //diferença entre ambos e que o to pode ser para varios destinatarios

                email.Subject = "Email de ativação da conta";

                email.IsBodyHtml = true; //se quer add html ou nao(se não vai ser so texto)

                email.Body = "Obrigado por se ter registrado no nosso site <br/> Para ativar a conta clique <a href='https://localhost:44389/ativacao_conta.aspx?user=" + EncryptString(nome) + "'>aqui</a>";

                servidor.Host = "smtp-mail.outlook.com"; //smtp.office365.com
                servidor.Port = 587;

                servidor.Credentials = new NetworkCredential("yourEmail@email.com", "YourP@ssword");

                servidor.EnableSsl = true;

                servidor.Send(email);

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }


    }
}
