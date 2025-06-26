using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Google.Apis.Auth.OAuth2.Web.AuthorizationCodeWebApp;

namespace LojaVirtual
{
    public partial class login_utilizador : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            bool googleUser;
            // verifica se houve retorno da autenticacao do google
            if (Request.IsAuthenticated)
            {
                var identity = HttpContext.Current.GetOwinContext().Authentication.User.Identity as ClaimsIdentity;
                var context = HttpContext.Current.GetOwinContext();
                var authResult = context.Authentication.AuthenticateAsync("ExternalCookie");

                string email = identity?.FindFirst(ClaimTypes.Email)?.Value;
                string nome = identity?.FindFirst(ClaimTypes.Name)?.Value;

                string accessToken = identity?.FindFirst("access_token")?.Value;
                string refreshToken = identity?.FindFirst("refresh_token")?.Value;
                var expires = identity?.FindFirst("expires")?.Value;


                //  capturar o nome
                if (string.IsNullOrEmpty(nome))
                {
                    nome = identity?.FindFirst("name")?.Value;
                }


                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(nome))
                {
                    int id_utilizador, cod_perfil;

                    SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);

                    string query = "SELECT id_utilizador, cod_perfil FROM Utilizadores WHERE email = @email ";

                    SqlCommand myCommand = new SqlCommand(query, myConn);

                    myCommand.Parameters.AddWithValue("@email", email);
                    myConn.Open();
                    SqlDataReader dr = myCommand.ExecuteReader();

                    if (dr.Read())
                    {
                        id_utilizador = Convert.ToInt32(dr["id_utilizador"]);
                        cod_perfil = Convert.ToInt32(dr["cod_perfil"]);

                        if (cod_perfil == 3)
                        {
                            googleUser = true;
                            Session["googleUser"] = googleUser;
                            if (!string.IsNullOrEmpty(expires) && long.TryParse(expires, out long expiresUnix))
                            {
                                DateTimeOffset expiresAt = DateTimeOffset.FromUnixTimeSeconds(expiresUnix);
                                SalvarTokenGoogle(id_utilizador, accessToken, refreshToken, expiresAt);
                            }
                            else
                            {
                                lbl_mensagem.Text = "Erro ao converter a data de expiração.";
                            }

                            Session["user_id"] = id_utilizador;
                            Session["nome_utilizador"] = nome;
                            Session["cod_perfil"] = cod_perfil;
                            lbl_mensagem.Text = "login com sucesso";
                            Response.Redirect("loja.aspx");
                        }
                    }
                    else
                    {
                        // utilizador ainda n tem conta entao salva dados temporariamente e redireciona
                        Session["email_google"] = email;
                        Session["nome_google"] = nome;
                        Response.Redirect("criar_conta.aspx"); // pagina onde o utilizador completa o cadastro
                    }

                }
                else
                {
                    lbl_mensagem.Text = "Erro ao obter informações da conta Google.";
                }
            }
        }


        public void SalvarTokenGoogle(int id_utilizador, string accessToken, string refreshToken, DateTimeOffset expires)
        {
            try
            {

                SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);

                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM TokensGoogle WHERE id_utilizador = @id", myConn);
                checkCmd.Parameters.AddWithValue("@id", id_utilizador);
                myConn.Open();
                int exists = (int)checkCmd.ExecuteScalar();

                string query;
                if (exists > 0)
                {
                    query = "UPDATE TokensGoogle SET access_token = @access_token, refresh_token = @refresh_token, expires_at = @expires_at WHERE id_utilizador = @id_utilizador";
                }
                else
                {
                    query = "INSERT INTO TokensGoogle (id_utilizador, access_token, refresh_token, expires_at) VALUES (@id_utilizador, @access_token, @refresh_token, @expires_at)";
                }
                SqlCommand myCommand = new SqlCommand(query, myConn);

                myCommand.Parameters.AddWithValue("@id_utilizador", id_utilizador);
                myCommand.Parameters.AddWithValue("@access_token", accessToken);
                myCommand.Parameters.AddWithValue("@refresh_token", refreshToken);
                myCommand.Parameters.AddWithValue("@expires_at", expires);

                myCommand.ExecuteNonQuery();
                myConn.Close();
            }
            catch (Exception ex)
            {
                lbl_mensagem.Text += ex.Message;
            }

        }
        protected void BtnEntrar_Click(object sender, EventArgs e)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand();
            myCommand.Parameters.AddWithValue("@email", tb_email.Text);
            myCommand.Parameters.AddWithValue("@password", EncryptString(tb_pw.Text));

            //retorno
            SqlParameter valorRetorno = new SqlParameter("@retorno", SqlDbType.Int);
            valorRetorno.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(valorRetorno);

            // parametro para o id do utilizador
            SqlParameter valorId = new SqlParameter("@id_utilizador", SqlDbType.Int);
            valorId.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(valorId);

            //paramentro pro cod perfil
            SqlParameter valorPerfil = new SqlParameter("@cod_perfil", SqlDbType.Int);
            valorPerfil.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(valorPerfil);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "login";
            myCommand.Connection = myConn;

            myConn.Open();

            myCommand.ExecuteNonQuery();

            int respostaSP = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);
            int id_utilizador = Convert.ToInt32(myCommand.Parameters["@id_utilizador"].Value);
            int cod_perfil = Convert.ToInt32(myCommand.Parameters["@cod_perfil"].Value);

            myConn.Close();

            if (respostaSP == 1 && id_utilizador != -1)
            {
                Session["user_id"] = id_utilizador;
                Session["cod_perfil"] = cod_perfil; //armazenar o cod

                //tranferir ites do carrinho temp 
                TransferirCarrinho(id_utilizador);

                //verficar qual o perfil
                if (cod_perfil == 1)
                {
                    Response.Redirect("loja.aspx");
                    lbl_mensagem.Text = "Admin logado com sucesso!!";
                }
                else //utilizador 
                {
                    Response.Redirect("loja.aspx");
                    lbl_mensagem.Text = "Utilizador logado com sucesso!!";

                }
            }
            else if (respostaSP == -1)
            {
                lbl_mensagem.Text = "A sua conta não esta ativa! Por favor verifique seu email.";
            }
            else
            {
                lbl_mensagem.Text = "Utilizador ou palavra-passe incorretos!!";
            }

        }

        protected void btn_google_Click1(object sender, EventArgs e)
        {
            var properties = new AuthenticationProperties { RedirectUri = "/login.aspx" };
           
            //properties.Dictionary.Add("prompt", "consent");
            //properties.Dictionary.Add("access_type", "offline");
            HttpContext.Current.GetOwinContext().Authentication.Challenge(properties, new[] { "Google",
                "https://www.googleapis.com/auth/calendar"
                });

        }

        private void TransferirCarrinho(int id_utilizador)
        {
            string sessionId = Session["session_id"]?.ToString();
            if (string.IsNullOrEmpty(sessionId)) return;

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString))
            {
                myConn.Open();
                string selectQuery = "SELECT id_produto, quantidade FROM CarrinhoTemp WHERE session_id = @session_id";
                using (SqlCommand selectCmd = new SqlCommand(selectQuery, myConn))
                {
                    selectCmd.Parameters.AddWithValue("@session_id", sessionId);

                    using (SqlDataReader dr = selectCmd.ExecuteReader())
                    {
                        //lista para carregar os produtos
                        List<(int id_produto, int quantidade)> produtos = new List<(int, int)>();
                        while (dr.Read())
                        {
                            int id_produto = Convert.ToInt32(dr["id_produto"]);
                            int quantidade = Convert.ToInt32(dr["quantidade"]);
                            produtos.Add((id_produto, quantidade));
                        }
                        dr.Close();
                        // inserir produtos no carrinho
                        foreach (var produto in produtos)
                        {
                            using (SqlCommand myCommand = new SqlCommand("add_carrinho", myConn))
                            {
                                myCommand.CommandType = CommandType.StoredProcedure;
                                myCommand.Parameters.AddWithValue("@id_produto", produto.id_produto);
                                myCommand.Parameters.AddWithValue("@id_utilizador", id_utilizador);
                                myCommand.Parameters.AddWithValue("@quantidade", produto.quantidade);
                                myCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    string deleteQuery = "DELETE FROM CarrinhoTemp WHERE session_id = @session_id";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, myConn))
                    {
                        deleteCmd.Parameters.AddWithValue("@session_id", sessionId);
                        deleteCmd.ExecuteNonQuery();
                    }
                }
            }
        }
        public static string EncryptString(string Message)
        {
            string Passphrase = "cinel";
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string

            string enc = Convert.ToBase64String(Results);
            enc = enc.Replace("+", "KKKKKM");
            enc = enc.Replace("/", "JJJJJM");
            enc = enc.Replace("\\", "IIIIIM");
            return enc;
        }

    }
}