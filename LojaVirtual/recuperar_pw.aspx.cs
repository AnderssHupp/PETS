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
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace LojaVirtual
{
    public partial class recuperar_pw : System.Web.UI.Page
    {
        public string novaPw;
        protected void Page_Load(object sender, EventArgs e)
        {
          
           
        }

        protected void btn_recuperar_Click(object sender, EventArgs e)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand();
            novaPw = GerarNovaPalavraPasse();

            myCommand.Parameters.AddWithValue("@email", tb_email.Text);
            myCommand.Parameters.AddWithValue("@pw_nova", EncryptString(novaPw));

            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";

            valor.Direction = ParameterDirection.Output;
            valor.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(valor);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "recuperar_pw";

            myCommand.Connection = myConn;

            myConn.Open();

            myCommand.ExecuteNonQuery();

            int respostaSP = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);

            myConn.Close();

            if (respostaSP == 1)
            {
                lbl_mensagem.Text = "Palavra-passe recuperada com sucesso!!";
                EnviarMail();
            }
            else if (respostaSP == 2)
            {
                lbl_mensagem.Text = "Conta inativa ou utilizador fora de validade!!";
            }
            else
            {
                lbl_mensagem.Text = "Utilizador incorretos!!";
            }

        }
        private string GerarNovaPalavraPasse()
        {
            const int tamanho = 8;
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < tamanho; i++)
            {
                sb.Append(caracteres[random.Next(caracteres.Length)]);
            }

            return sb.ToString();
        }

        public void EnviarMail()
        {
            MailMessage email = new MailMessage();
            SmtpClient servidor = new SmtpClient();

            try
            {
                email.From = new MailAddress("andersson.hupp.31591@formandos.cinel.pt");

                email.To.Add(tb_email.Text);

                email.Subject = "Email de recuperação da conta";

                email.IsBodyHtml = true;

                email.Body = $"A sua nova password: {novaPw} ";

                servidor.Host = "smtp-mail.outlook.com"; //smtp.office365.com
                servidor.Port = 587;

                servidor.Credentials = new NetworkCredential("andersson.hupp.31591@formandos.cinel.pt", "@Cerveja24");

                servidor.EnableSsl = true;

                servidor.Send(email);

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
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