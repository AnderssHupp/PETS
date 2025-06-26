using iTextSharp.text.pdf;
using iTextSharp.text;
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
using System.Net.Mail;
using System.Net;
using System.Threading;

namespace LojaVirtual
{
    public partial class Checkout : System.Web.UI.Page
    {
        public int userId;
        public string userEmail = "";
        public SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["user_id"] == null)
                {
                    Response.Redirect("login.aspx");
                }
               
                string totalCompra = Request.QueryString["total"];
                if (!string.IsNullOrEmpty(totalCompra))
                {
                    lblTotal.Text = "€ " + totalCompra;
                }
                CarregarMorada();
                CarregarDadosPagamento();

            }
        }
        private void CarregarDadosPagamento()
        {
            userId = Convert.ToInt32(Session["user_id"]);
            string query = $"SELECT id_cliente, nome_titular, num_cartao, validade, cvv FROM CartoesClientes Where id_cliente = {userId}";
            SqlCommand myCommand = new SqlCommand(query, myConn);

            myCommand.Parameters.AddWithValue("@id_cliente", userId);
            myConn.Open();
            SqlDataReader dr = myCommand.ExecuteReader();
            if (dr.Read())
            {
                lbl_nomeTitular.Text = dr["nome_titular"].ToString();
                lbl_num.Text = dr["num_cartao"].ToString();
                lbl_validade.Text = dr["validade"].ToString();
                lbl_cvv.Text = dr["cvv"].ToString();
                pnl_info.Visible = false;    // Exibe os dados salvos

                pnlNovoCartao.Visible = false;
            }
            else
            {
                pnl_info.Visible = false;
                pnlNovoCartao.Visible = false;
            }
            myConn.Close();
        }
        private void CarregarMorada()
        {
            userId = Convert.ToInt32(Session["user_id"]);
            string query = $"SELECT id_utilizador, morada, concelho, distrito, cp FROM Enderecos Where id_utilizador = {userId}";
            SqlCommand myCommand = new SqlCommand(query, myConn);

            myCommand.Parameters.AddWithValue("@id_utilizador", userId);
            myConn.Open();
            SqlDataReader dr = myCommand.ExecuteReader();
            if (dr.Read())
            {
                lbl_morada.Text = dr["morada"].ToString();
                lbl_concelho.Text = dr["concelho"].ToString();
                lbl_distrito.Text = dr["distrito"].ToString();
                lbl_cp.Text = dr["cp"].ToString();

                pnl_moradaR.Visible = true;
                pnlOutraMorada.Visible = false;  // oculta os campos de nova morada
            }
            else
            {
                chkOutraMorada.Visible = false;
                pnlOutraMorada.Visible = true;

            }
            myConn.Close();
        }

        protected void rbCartao_CheckedChanged(object sender, EventArgs e)
        {
            userId = Convert.ToInt32(Session["user_id"]);
            bool cartaoSelcionado = rbCartao.Checked;
            pnl_mbway.Visible = false;
            if (cartaoSelcionado)
            {
                string query = $"SELECT nome_titular, num_cartao, validade, cvv FROM CartoesClientes WHERE id_cliente = {userId}";
                SqlCommand myCommand = new SqlCommand(query, myConn);
                myCommand.Parameters.AddWithValue("@id_cliente", userId);

                myConn.Open();
                SqlDataReader dr = myCommand.ExecuteReader();
                if (dr.Read())
                {
                    // se existir cartao, mostra o painel com os dados do cartão
                    pnl_info.Visible = true;
                    pnlNovoCartao.Visible = false;
                    lbl_nomeTitular.Text = dr["nome_titular"].ToString();
                    lbl_num.Text = dr["num_cartao"].ToString();
                    lbl_validade.Text = dr["validade"].ToString();
                    lbl_cvv.Text = dr["cvv"].ToString();

                }
                else
                {
                    // Se n, mostra o painel para adicionar novo cartao
                    pnl_info.Visible = false;
                    pnlNovoCartao.Visible = true;
                    chkOutraMorada.Checked = true;
                }
            }
            else
            {
                pnl_info.Visible = false;
                pnlNovoCartao.Visible = false;

            }

            upWizard.Update();

        }
        protected void rbMBWay_CheckedChanged(object sender, EventArgs e)
        {
            pnl_mbway.Visible = rbMBWay.Checked;
            pnl_info.Visible = false;
            pnlNovoCartao.Visible = false;
            upWizard.Update();
        }
        protected void chkOutraMorada_CheckedChanged(object sender, EventArgs e)
        {
            pnlOutraMorada.Visible = chkOutraMorada.Checked;
            upWizard.Update();
        }
        protected void btnNext1_Click(object sender, EventArgs e)
        {
            // Passa para o painel de pagamento
            pnlEntrega.Visible = false;
            pnl_moradaR.Visible = false;
            pnlPagamento.Visible = true;
        }

        protected void btnBack1_Click(object sender, EventArgs e)
        {
            // Volta para o painel de entrega
            pnlPagamento.Visible = false;
            pnlEntrega.Visible = true;
            pnl_moradaR.Visible = true;
            //chkOutraMorada.Checked = false;
        }

        protected void btnNext2_Click(object sender, EventArgs e)
        {
            // Força a atualização da morada
            //upWizard.Update();
            // Verifica se o usuário escolheu uma nova morada

            if (chkOutraMorada.Checked == true)
            {
                lblResumoMorada.Text = tb_novaMorada.Text + "<br />" +
                                       tb_NovoConcelho.Text + "<br />" +
                                       tb_NovoDistrito.Text + "<br />" +
                                       tb_CodigoPostal.Text;

            }
            else
            {
                lblResumoMorada.Text = lbl_morada.Text + "<br />" +
                                       lbl_concelho.Text + "<br />" +
                                       lbl_distrito.Text + "<br />" +
                                       lbl_cp.Text;
            }


            lblPagamento.Text = rbCartao.Checked ? "Cartão de Crédito" : "MB WAY";
            lblTotal.Text = lblTotal.Text;

            upWizard.Update();
            // Passa para o painel de confirmação
            pnlPagamento.Visible = false;
            pnlConfirmacao.Visible = true;
        }


        protected void btnBack2_Click(object sender, EventArgs e)
        {
            // Volta para o painel de pagamento
            pnlConfirmacao.Visible = false;
            pnlPagamento.Visible = true;
        }

        protected void cb_novoCartao_CheckedChanged(object sender, EventArgs e)
        {
            pnlNovoCartao.Visible = cb_novoCartao.Checked;
            upWizard.Update();
        }

        private string GerarFaturaPDF()
        {

            string userName = "";
            string userNif = "";
            userId = Convert.ToInt32(Session["user_id"]);

            string query = "SELECT nome, email, nif FROM Utilizadores WHERE id_utilizador = @id_utilizador";
            SqlCommand myCommand = new SqlCommand(query, myConn);
            myCommand.Parameters.AddWithValue("@id_utilizador", userId);

            myConn.Open();
            SqlDataReader dr = myCommand.ExecuteReader();
            if (dr.Read())
            {
                userName = dr["nome"].ToString();
                userEmail = dr["email"].ToString();
                userNif = dr["nif"].ToString();
            }
            myConn.Close();

            string filePath = Server.MapPath("~/Faturas/Fatura_" + userId + ".pdf");

            if (!Directory.Exists(Server.MapPath("~/Faturas/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Faturas/"));
            }

            Document doc = new Document(PageSize.A4);
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

            // Adicionar cabeçalho da fatura
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            Paragraph title = new Paragraph("Fatura de Compra", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            doc.Add(title);
            doc.Add(new Paragraph("\n"));

            // Dados do cliente
            doc.Add(new Paragraph("Nome: " + userName));
            doc.Add(new Paragraph("E-mail: " + userEmail));
            doc.Add(new Paragraph("NIF: " + userNif));
            doc.Add(new Paragraph("Morada: " + lblResumoMorada.Text));
            doc.Add(new Paragraph("\n"));

            // Método de pagamento
            doc.Add(new Paragraph("Método de Pagamento: " + lblPagamento.Text));
            doc.Add(new Paragraph("\n"));

            // Total da compra
            Font totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
            doc.Add(new Paragraph("Total: " + lblTotal.Text, totalFont));

            doc.Close();

            return filePath;
        }
        public void EnviarMail(string filePath)
        {
            MailMessage email = new MailMessage();
            SmtpClient servidor = new SmtpClient();

            try
            {
                email.From = new MailAddress("andersson.hupp.31591@formandos.cinel.pt");

                email.To.Add(userEmail); //diferença entre ambos é que o to pode ser para varios destinatarios

                email.Subject = "Fatura - Compra";

                email.IsBodyHtml = true; //se quer add html ou nao(se não vai ser so texto)

                email.Body = "<p>Obrigado por escolher a gente!</p>" +
                    "<p>Segue em anexo a fatura da sua compra</p>";

                Attachment faturaAnexo = new Attachment(filePath);
                email.Attachments.Add(faturaAnexo);

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
        protected void AtualizarStock(int id_utilizador)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);
            try
            {
                myConn.Open();
                
                SqlCommand myCommand = new SqlCommand();
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "atualizar_stock";
                myCommand.Connection = myConn;

                myCommand.Parameters.AddWithValue("@id_utilizador", id_utilizador);

                myCommand.ExecuteNonQuery();

                myConn.Close();
            }
            catch (Exception ex)
            {
                // Tratamento de erro (opcional)
                Response.Write("Erro ao atualizar stock: " + ex.Message);
            }
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            userId = Convert.ToInt32(Session["user_id"]);


            AtualizarStock(userId);


            EnviarMail(GerarFaturaPDF());


            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Compra realizada com sucesso!');", true);
            

            Response.Redirect("loja.aspx");
        }
    }
}