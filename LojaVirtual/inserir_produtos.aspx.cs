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
    public partial class inserir_produtos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_inserir_Click(object sender, EventArgs e)
        {
            //vai apanhar o strem do ficheiro selecionado
            Stream imgStream = file_upload_img.PostedFile.InputStream;

            //apanhar o content type
            //string ct = file_ipload_omg.PostedFile.ContentType;
            //dados binarios
            //
            int tamanho = file_upload_img.PostedFile.ContentLength;//vai dizer o comprimento do ficheiro selecionado antes de colocar no array

            //array de byte
            byte[] imgBinaryData = new byte[tamanho];

            //conversao do img stream
            //vai la ler o img e carrega o array desde a pos 0 ate o comprimento 
            imgStream.Read(imgBinaryData, 0, tamanho);

            //chamar uma conexão da bd

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);

            SqlCommand myCommand = new SqlCommand();


            //comando de ir na bd buscar as storedprocedure
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.AddWithValue("@nome", tb_nome.Text);
            myCommand.Parameters.AddWithValue("@preco", tb_preco.Text);
            myCommand.Parameters.AddWithValue("@stock", tb_stock.Text);
            myCommand.Parameters.AddWithValue("@id_categoria", ddl_categoria.SelectedValue);
            myCommand.Parameters.AddWithValue("@imagem", imgBinaryData);


            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            ////tipo de parametro
            valor.Direction = ParameterDirection.Output;
            ////define o tipo do valor
            valor.SqlDbType = SqlDbType.Int;

            //parametro final
            myCommand.Parameters.Add(valor);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "inserir_produto";
            myCommand.Connection = myConn;
         

            myConn.Open();

            myCommand.ExecuteNonQuery();

            int respostaSP = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);

            myConn.Close();

            if (respostaSP == 1)
            {
                lbl_mensagem.Text = "Produto inserido com sucesso!";

                
            }
            else
            {
                lbl_mensagem.Text = "Produto já EXISTE!";
            }
           
        }

        protected void btn_voltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("gestao_produtos.aspx");
        }
    }
}