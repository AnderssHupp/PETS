using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LojaVirtual
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                CarregarPets();
            }
        }


        private void CarregarPets()
        {
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = @"SELECT id_pet, nome_pet, especie, raca, idade, peso, observacoes, Foto FROM Pets WHERE id_utilizador = @id_cliente";
                SqlCommand cmd = new SqlCommand(query, myConn);
                cmd.Parameters.AddWithValue("@id_cliente", 1);

                SqlDataAdapter dtAdpt = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dtAdpt.Fill(dt);

                // add uma coluna nova para imagem base64
                dt.Columns.Add("ImagemBase64", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    byte[] imagemBytes = row["Foto"] as byte[];
                    if (imagemBytes != null)
                    {
                        string base64String = Convert.ToBase64String(imagemBytes);
                        row["ImagemBase64"] = $"data:image/jpeg;base64,{base64String}";
                    }

                }

                lvPets.DataSource = dt;
                lvPets.DataBind();
            }
        }
        protected void lvPets_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Remover")
            {
                int idPet = Convert.ToInt32(e.CommandArgument);
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Pets WHERE id_pet = @id", conn);
                    cmd.Parameters.AddWithValue("@id", idPet);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                CarregarPets();
            }
        }
        protected void lvPets_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager2.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            lvPets.EditIndex = -1;
            CarregarPets();
        }
        protected void lvPets_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvPets.EditIndex = e.NewEditIndex;
            CarregarPets();
        }

        protected void lvPets_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvPets.EditIndex = -1;
            CarregarPets();
        }

        protected void lvPets_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            int idPet = Convert.ToInt32(lvPets.DataKeys[e.ItemIndex].Value);
            ListViewItem item = lvPets.Items[e.ItemIndex];

            FileUpload fuFotos = (FileUpload)item.FindControl("fuFotos");
            byte[] primeiraFoto = null;

           

            string nome = ((TextBox)item.FindControl("tbNomePet")).Text;
            string especie = ((TextBox)item.FindControl("tbEspecie")).Text;
            string raca = ((TextBox)item.FindControl("tbRaca")).Text;
            int idade = int.Parse(((TextBox)item.FindControl("tbIdade")).Text);
            double peso = double.Parse(((TextBox)item.FindControl("tbPeso")).Text);
            string obs = ((TextBox)item.FindControl("tbObs")).Text;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                conn.Open();

                if (fuFotos != null && fuFotos.HasFiles)
                {
                    HttpPostedFile uploadedFile = fuFotos.PostedFile;
                    primeiraFoto = new byte[uploadedFile.ContentLength];
                    uploadedFile.InputStream.Read(primeiraFoto, 0, uploadedFile.ContentLength);
                }

                // Atualizar pet
                string sql = @"UPDATE Pets SET nome_pet = @nome, especie = @especie, raca = @raca,
                        idade = @idade, peso = @peso, observacoes = @obs, foto = @foto WHERE id_pet = @id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@especie", especie);
                    cmd.Parameters.AddWithValue("@raca", raca);
                    cmd.Parameters.AddWithValue("@idade", idade);
                    cmd.Parameters.AddWithValue("@peso", peso);
                    cmd.Parameters.AddWithValue("@obs", obs);
                    cmd.Parameters.AddWithValue("@foto", (object)primeiraFoto ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", idPet);
                    cmd.ExecuteNonQuery();
                }

                // Inserir mais fotos
                if (fuFotos != null && fuFotos.HasFiles)
                {
                    foreach (HttpPostedFile file in fuFotos.PostedFiles)
                    {
                        byte[] img = new byte[file.ContentLength];
                        file.InputStream.Read(img, 0, img.Length);

                        SqlCommand cmdFoto = new SqlCommand("INSERT INTO FotosPets (id_pet, foto, nome_arquivo) VALUES (@id_pet, @foto, @nome)", conn);
                        cmdFoto.Parameters.AddWithValue("@id_pet", idPet);
                        cmdFoto.Parameters.AddWithValue("@foto", img);
                        cmdFoto.Parameters.AddWithValue("@nome", Path.GetFileName(file.FileName));
                        cmdFoto.ExecuteNonQuery();
                    }
                   
                }
            }

            lvPets.EditIndex = -1;
            CarregarPets();
        }
    }
}