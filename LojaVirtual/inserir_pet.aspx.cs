using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LojaVirtual
{
    public partial class inserir_pet : System.Web.UI.Page
    {
        private int id_utilizador;
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
            }
        }

        protected void btn_salvar_Click(object sender, EventArgs e)
        {

            byte[] primeiraFoto = null;

            // se tiver pelo menos uma foto, define a 1 como principal
            if (FileUpload_fotos.HasFiles)
            {
                HttpPostedFile fotoPrincipal = FileUpload_fotos.PostedFiles[0];
                primeiraFoto = new byte[fotoPrincipal.ContentLength];
                fotoPrincipal.InputStream.Read(primeiraFoto, 0, fotoPrincipal.ContentLength);
            }

            //chamar uma conexão da bd

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);

            SqlCommand myCommand = new SqlCommand();


            //comando de ir na bd buscar as storedprocedure
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.AddWithValue("@id_utilizador", id_utilizador);
            myCommand.Parameters.AddWithValue("@nome_pet", tb_nome.Text);
            myCommand.Parameters.AddWithValue("@especie", tb_especie.Text);
            myCommand.Parameters.AddWithValue("@raca", tb_raca.Text);
            myCommand.Parameters.AddWithValue("@idade", Convert.ToInt32(tb_idade.Text));
            myCommand.Parameters.AddWithValue("@peso", Convert.ToDecimal(tb_peso.Text));
            myCommand.Parameters.AddWithValue("@observacoes", tb_obs.Text);
            myCommand.Parameters.AddWithValue("@foto", (object)primeiraFoto ?? DBNull.Value);

            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            ////tipo de parametro
            valor.Direction = ParameterDirection.Output;
            ////define o tipo do valor
            valor.SqlDbType = SqlDbType.Int;

            //parametro final
            myCommand.Parameters.Add(valor);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "inserir_pet";
            myCommand.Connection = myConn;

            myConn.Open();

            myCommand.ExecuteNonQuery();


            int idPetCriado = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);

            if (idPetCriado > 0)
            {
                // Inserir fotos extras na tabela FotosPets
                foreach (HttpPostedFile file in FileUpload_fotos.PostedFiles)
                {
                    byte[] img = new byte[file.ContentLength];
                    file.InputStream.Read(img, 0, img.Length);

                    SqlCommand cmdFoto = new SqlCommand("INSERT INTO FotosPets (id_pet, foto, nome_arquivo) VALUES (@id_pet, @foto, @nome)", myConn);
                    cmdFoto.Parameters.AddWithValue("@id_pet", idPetCriado);
                    cmdFoto.Parameters.AddWithValue("@foto", img);
                    cmdFoto.Parameters.AddWithValue("@nome", Path.GetFileName(file.FileName));
                    cmdFoto.ExecuteNonQuery();
                }

                lbl_mensagem.Text = "Pet cadastrado com sucesso!";
            }
            else
            {
                lbl_mensagem.Text = "Pet já foi registrado!";
            }

            myConn.Close();



        }
    }
}
