using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

namespace LojaVirtual
{
    public partial class perfil : System.Web.UI.Page
    {
        public int idUtilizador;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["user_id"] != null)
                {
                    idUtilizador = Convert.ToInt32(Session["user_id"]);

                }
                else
                {
                    Response.Redirect("login.aspx");
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SELECT nome, email, telemovel FROM utilizadores WHERE id_utilizador = @id_utilizador", conn))
                {
                    cmd.Parameters.AddWithValue("@id_utilizador", idUtilizador);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        tb_nome.Text = reader["nome"].ToString();
                        tb_email.Text = reader["email"].ToString();
                        tb_telemovel.Text = reader["telemovel"].ToString();
                        //tb_nif.Text = reader["nif"].ToString();
                        //tb_dataNascimento.Text = Convert.ToDateTime(reader["data_nascimento"]).ToString("yyyy-MM-dd");
                    }
                    reader.Close();
                }

                //buscar a morada registrada

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SELECT morada, concelho, distrito, cp FROM Enderecos WHERE id_utilizador = @id_utilizador", conn))
                {
                    cmd.Parameters.AddWithValue("@id_utilizador", idUtilizador);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        tb_morada.Text = reader["morada"].ToString();
                        tb_concelho.Text = reader["concelho"].ToString();
                        tb_distrito.Text = reader["distrito"].ToString();
                        tb_cp.Text = reader["cp"].ToString();

                    }
                    reader.Close();
                }


            }
        }



        protected void btn_AlterarDados_Click(object sender, EventArgs e)
        {

            idUtilizador = Convert.ToInt32(Session["user_id"]);
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);


            SqlCommand myCommand = new SqlCommand();


            myCommand.Parameters.AddWithValue("@id_utilizador", idUtilizador); //quem é o user
            myCommand.Parameters.AddWithValue("@nome", tb_nome.Text);
            myCommand.Parameters.AddWithValue("@email", tb_email.Text);
            myCommand.Parameters.AddWithValue("@telemovel", Convert.ToInt32(tb_telemovel.Text));
            //myCommand.Parameters.AddWithValue("@nif", Convert.ToInt32(tb_nif.Text));
            //myCommand.Parameters.AddWithValue("@data_nascimento", Convert.ToDateTime(tb_dataNascimento.Text));

            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            valor.Direction = ParameterDirection.Output;
            valor.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(valor);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "alterar_dados";


            myCommand.Connection = myConn;

            myConn.Open();

            myCommand.ExecuteNonQuery();

            int respostaSP = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);


            myConn.Close();

            if (respostaSP == 1)
            {
                lbl_msgDados.Text = "Dados alterados com sucesso!!";

            }
            else
            {
                lbl_msgDados.Text = "Erro ao atualizar os dados!!";
            }

        }


        protected void btn_alterarPw_Click(object sender, EventArgs e)
        {
            idUtilizador = Convert.ToInt32(Session["user_id"]);
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);


            SqlCommand myCommand = new SqlCommand();

            myCommand.Parameters.AddWithValue("@id_utilizador", idUtilizador); //quem e o user
            myCommand.Parameters.AddWithValue("@pw_atual", EncryptString(tb_pw_atual.Text)); //pw atual
            myCommand.Parameters.AddWithValue("@pw_nova", EncryptString(tb_pw_nova.Text));//pw nova

            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            valor.Direction = ParameterDirection.Output;
            valor.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(valor);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "alterar_pw";


            myCommand.Connection = myConn;

            myConn.Open();

            myCommand.ExecuteNonQuery();

            int respostaSP = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);


            myConn.Close();

            if (respostaSP == 1)
            {
                lbl_mensagem.Text = "Palavra-passe alterada com sucesso!!";

            }
            else
            {
                lbl_mensagem.Text = "Palavra-passe atual inválida!!";
            }
        }

        protected void btn_AlterarMorada_Click(object sender, EventArgs e)
        {
            idUtilizador = Convert.ToInt32(Session["user_id"]);
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);


            SqlCommand myCommand = new SqlCommand();


            myCommand.Parameters.AddWithValue("@id_utilizador", idUtilizador); //quem e o user

            myCommand.Parameters.AddWithValue("@morada", tb_morada.Text);
            myCommand.Parameters.AddWithValue("@concelho", tb_concelho.Text);
            myCommand.Parameters.AddWithValue("@distrito", tb_distrito.Text);
            myCommand.Parameters.AddWithValue("@cp", tb_cp.Text);

            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            valor.Direction = ParameterDirection.Output;
            valor.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(valor);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "alterar_morada";


            myCommand.Connection = myConn;

            myConn.Open();

            myCommand.ExecuteNonQuery();

            int respostaSP = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);


            myConn.Close();

            if (respostaSP == 1)
            {
                lbl_msgMorada.Text = "Morada alterada com sucesso!!";

            }
            else
            {
                lbl_msgMorada.Text = "Morada inserida com sucesso!!";
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

        protected void btn_Logout_Click(object sender, EventArgs e)
        {
            //limpa as variaveis sessions
            Session.Clear();
            Session.Abandon();

            // Encerrar a autenticação com Google (caso esteja autenticado)
            HttpContext.Current.GetOwinContext().Authentication.SignOut();


            Response.Redirect("login.aspx");
        }
    }
}