using LojaVirtual.Servicos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LojaVirtual
{
    public partial class dashboard_cliente : System.Web.UI.Page
    {
        public int userId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user_id"] == null)
            {
                Response.Redirect("login.aspx");
            }
            else
            {
                userId = Convert.ToInt32(Session["user_id"]);
            }
            if (!IsPostBack)
            {
                CarregarHistoricoServicos(2, rpServicosAvaliar);
                CarregarFotoPerfil();
                CarregarMeusAnuncios();
                CarregarPets();
                CarregarUsers();
                CarregarMeiosPagamentos();
                CarregarCandidaturas();
            }


            // atualiza as mensagens após envio ou mudança de destinatário
            if (ViewState["destinatario"] != null)
            {
                CarregarMensagens();
            }

        }
        private void CarregarFotoPerfil()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = $"SELECT id_utilizador, nome, foto_perfil FROM Utilizadores  WHERE id_utilizador = {userId}";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id_cliente", userId);
                conn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lbl_nome.Text = dr["nome"].ToString();
                    //mostrar a foto                                 //converter para um byte array

                    byte[] imagemBytes = dr["foto_perfil"] as byte[];
                    if (imagemBytes != null)
                    {
                        img_foto.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String((byte[])dr["foto_perfil"]);

                    }
                    else
                    {
                        // Se n tiver ft coloca imagem padrao
                        img_foto.ImageUrl = "/images/avatar.jpg";
                    }

                }


            }

        }
        protected void btn_novoAnuncio_Click(object sender, EventArgs e)
        {
            Response.Redirect("criar_anuncio.aspx");
        }
        private void CarregarMeusAnuncios()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = @"SELECT a.id_anuncio, a.titulo, a.id_cliente,a.id_pet, a.cod_TipoServico, a.descricao, a.localidade, a.latitude, a.longitude, a.preco, a.data_nece, a.hora_nece, a.cod_status, p.nome_pet, p.Foto, p.id_pet, tp.nome_servico AS TipoServico, s.nome_status AS status 
                   FROM Anuncios a 
                    INNER JOIN Pets p ON a.id_pet = p.id_pet 
                    INNER JOIN TipoServico tp ON a.cod_TipoServico = tp.cod_TipoServico 
                    INNER JOIN Status s ON a.cod_status = s.cod_status 
                    WHERE a.id_cliente = @id_cliente";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id_cliente", userId);


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

                lvAnuncios.DataSource = dt;
                lvAnuncios.DataBind();
            }
        }

        private void CarregarPets()
        {
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = @"SELECT id_pet, nome_pet, especie, raca, idade, peso, observacoes, Foto FROM Pets WHERE id_utilizador = @id_cliente";
                SqlCommand cmd = new SqlCommand(query, myConn);
                cmd.Parameters.AddWithValue("@id_cliente", userId);

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
        protected void lvAnuncios_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            lvAnuncios.EditIndex = -1;
            CarregarMeusAnuncios();
        }
        protected void lvAnuncios_ItemCommand(object source, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "VerCandidaturas")
            {
                //panel_anuncios.Style["display"] = "none";
                //panel_candidaturas.Style["display"] = "block";
                //int idAnuncio = Convert.ToInt32(e.CommandArgument);
                //ViewState["anuncioSelecionado"] = idAnuncio;
                CarregarCandidaturas();

                

            }
            else if (e.CommandName == "Remover")
            {
                int idAnuncio = Convert.ToInt32(e.CommandArgument);
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Anuncios WHERE id_anuncio = @id", conn);
                    cmd.Parameters.AddWithValue("@id", idAnuncio);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

            }
                
            CarregarMeusAnuncios();
        }
        protected void lvAnuncios_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvAnuncios.EditIndex = e.NewEditIndex;

            CarregarMeusAnuncios(); //rebind
        }

        //  para atualiza um item
        protected void lvAnuncios_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            int idAnuncio = Convert.ToInt32(lvAnuncios.DataKeys[e.ItemIndex].Value);

            // obter valores dos controles
            string novoTitulo = ((TextBox)lvAnuncios.Items[e.ItemIndex].FindControl("tbTitulo")).Text;
            string novoCodTipoServico = ((DropDownList)lvAnuncios.Items[e.ItemIndex].FindControl("ddl_servico")).SelectedValue;
            string novaDescricao = ((TextBox)lvAnuncios.Items[e.ItemIndex].FindControl("tbDescricao")).Text;
            string novaData = ((TextBox)lvAnuncios.Items[e.ItemIndex].FindControl("tbData")).Text;
            string novaHora = ((TextBox)lvAnuncios.Items[e.ItemIndex].FindControl("tbHora")).Text;
            string novoLocal = ((TextBox)lvAnuncios.Items[e.ItemIndex].FindControl("tbLocalidade")).Text;
            string novoPreco = ((TextBox)lvAnuncios.Items[e.ItemIndex].FindControl("tbPreco")).Text;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
                {
                    string query = "UPDATE Anuncios SET titulo = @titulo, cod_tipoServico = @codTS,  descricao = @descricao, data_nece = @data, hora_nece = @hora, localidade = @localidade, preco = @preco WHERE id_anuncio = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.Add("@titulo", SqlDbType.VarChar).Value = novoTitulo;
                    cmd.Parameters.Add("@codTS", SqlDbType.Int).Value = novoCodTipoServico;
                    cmd.Parameters.Add("@descricao", SqlDbType.Text).Value = novaDescricao;
                    cmd.Parameters.Add("@data", SqlDbType.Date).Value = novaDescricao;
                    cmd.Parameters.Add("@hora", SqlDbType.Time).Value = novaDescricao;
                    cmd.Parameters.Add("@localidade", SqlDbType.VarChar).Value = novaDescricao;
                    cmd.Parameters.Add("@preco", SqlDbType.Decimal).Value = novaDescricao;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = idAnuncio;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                lvAnuncios.EditIndex = -1; // sair do modo de edit
                CarregarMeusAnuncios();
            }
            catch (Exception ex)
            {
                // tratar erro
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                    $"alert('Erro ao atualizar: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }

        protected void lvAnuncios_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvAnuncios.EditIndex = -1;
            CarregarMeusAnuncios();
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
            lvAnuncios.EditIndex = -1;
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
            string nome = ((TextBox)item.FindControl("tbNomePet")).Text;
            string especie = ((TextBox)item.FindControl("tbEspecie")).Text;
            string raca = ((TextBox)item.FindControl("tbRaca")).Text;
            int idade = int.Parse(((TextBox)item.FindControl("tbIdade")).Text);
            double peso = double.Parse(((TextBox)item.FindControl("tbPeso")).Text);
            string obs = ((TextBox)item.FindControl("tbObs")).Text;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string sql = @"UPDATE Pets SET nome_pet = @nome, especie = @especie, raca = @raca,
                       idade = @idade, peso = @peso, observacoes = @obs WHERE id_pet = @id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@especie", especie);
                    cmd.Parameters.AddWithValue("@raca", raca);
                    cmd.Parameters.AddWithValue("@idade", idade);
                    cmd.Parameters.AddWithValue("@peso", peso);
                    cmd.Parameters.AddWithValue("@obs", obs);
                    cmd.Parameters.AddWithValue("@id", idPet);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            lvPets.EditIndex = -1;
            CarregarPets();
        }
        //protected void rpCandidaturas_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    int id_candidatura = Convert.ToInt32(e.CommandArgument);
        //    if (e.CommandName == "Aceitar")
        //    {
        //        SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);


        //        SqlCommand myCmd = new SqlCommand("UPDATE Candidaturas SET cod_status = 7 WHERE id_candidatura = @id_candidatura", myConn);
        //        myCmd.Parameters.AddWithValue("@id_candidatura", id_candidatura);
        //        myConn.Open();
        //        myCmd.ExecuteNonQuery();

        //        // obter info da candidatura (prestador e anuncio)
        //        int idPrestador = 0;
        //        int idCliente = 0;
        //        int idAnuncio = 0;
        //        string mensagem = "";
        //        string descricao = "";
        //        string titulo = "";
        //        decimal total = 0;
        //        DateTime dataServico = DateTime.MinValue;
        //        TimeSpan horaServico = TimeSpan.Zero;

        //        SqlCommand cmdInfo = new SqlCommand(@"SELECT c.id_utilizador, c.id_anuncio, c.mensagem, a.titulo, a.descricao, a.preco, a.data_nece, a.hora_nece, a.id_cliente 
        //        FROM Candidaturas c
        //        INNER JOIN Anuncios a ON c.id_anuncio = a.id_anuncio
        //        WHERE c.id_candidatura = @id_candidatura", myConn);
        //        cmdInfo.Parameters.AddWithValue("@id_candidatura", id_candidatura);
        //        SqlDataReader dr = cmdInfo.ExecuteReader();

        //        if (dr.Read())
        //        {
        //            idPrestador = Convert.ToInt32(dr["id_utilizador"]);
        //            idAnuncio = Convert.ToInt32(dr["id_anuncio"]);
        //            titulo = dr["titulo"].ToString();
        //            descricao = dr["descricao"].ToString();
        //            mensagem = dr["mensagem"].ToString();
        //            total = Convert.ToDecimal(dr["preco"]);
        //            dataServico = Convert.ToDateTime(dr["data_nece"]);
        //            horaServico = (TimeSpan)dr["hora_nece"];
        //            idCliente = Convert.ToInt32(dr["id_cliente"]);
        //        }
        //        dr.Close();

        //        // cancela as outras candidaturas do mesmo anuncio
        //        SqlCommand cmdCancelar = new SqlCommand(@"
        //        UPDATE Candidaturas SET cod_status = 6
        //        WHERE id_anuncio = @id_anuncio AND id_candidatura != @id_candidatura", myConn);
        //        cmdCancelar.Parameters.AddWithValue("@id_anuncio", idAnuncio);
        //        cmdCancelar.Parameters.AddWithValue("@id_candidatura", id_candidatura);
        //        cmdCancelar.ExecuteNonQuery();

        //        // criar agendamento
        //        SqlCommand cmdAgendamento = new SqlCommand(@"
        //        INSERT INTO Agendamentos (id_cliente, id_prestador, id_anuncio,hora_inicio, hora_fim, cod_status, data_servico, data_criacao)
        //        VALUES (@id_cliente, @id_prestador, @id_anuncio, @hora_inicio, @hora_fim, 8, @data_servico, GETDATE());
        //        SELECT CAST(SCOPE_IDENTITY() AS INT);", myConn);

        //        cmdAgendamento.Parameters.AddWithValue("@id_cliente", idCliente);
        //        cmdAgendamento.Parameters.AddWithValue("@id_prestador", idPrestador);
        //        cmdAgendamento.Parameters.AddWithValue("@id_anuncio", idAnuncio);
        //        cmdAgendamento.Parameters.AddWithValue("@hora_inicio", horaServico);
        //        cmdAgendamento.Parameters.AddWithValue("@hora_fim", horaServico.Add(new TimeSpan(1, 0, 0)));
        //        cmdAgendamento.Parameters.AddWithValue("@data_servico", dataServico);


        //        int idAgendamento = Convert.ToInt32(cmdAgendamento.ExecuteScalar());

        //        DateTime inicioEvento = dataServico.Add(horaServico);
        //        DateTime fimEvento = inicioEvento.Add(new TimeSpan(1, 0, 0));

        //        GoogleCalendarServico.CriarEvento(idPrestador, titulo, $"Reserva de serviço com {idCliente}", inicioEvento, fimEvento);

        //        // criar servico
        //        SqlCommand cmdServico = new SqlCommand(@"
        //        INSERT INTO Servicos (id_agendamento, titulo, descricao, total, cod_status, data_criacao)
        //        VALUES (@id_agendamento, @titulo, @descricao, @total, @cod_status, GETDATE())", myConn);

        //        cmdServico.Parameters.AddWithValue("@id_agendamento", idAgendamento);
        //        cmdServico.Parameters.AddWithValue("@titulo", titulo);
        //        cmdServico.Parameters.AddWithValue("@descricao", descricao);
        //        cmdServico.Parameters.AddWithValue("@total", total);
        //        cmdServico.Parameters.AddWithValue("@cod_status", 8);


        //        cmdServico.ExecuteNonQuery();
        //        lbl_msg.Text = "Candidatura aceite. Agendamento e serviço criados.";

        //        string emailPrestador = "";
        //        string nomePrestador = "";
        //        string emailCliente = "";
        //        string nomeCliente = "";

        //        SqlCommand cmdEmails = new SqlCommand(@"
        //            SELECT 
        //            (SELECT email FROM Utilizadores WHERE id_utilizador = @idPrestador) AS emailPrestador,
        //            (SELECT nome FROM Utilizadores WHERE id_utilizador = @idPrestador) AS nomePrestador,
        //            (SELECT email FROM Utilizadores WHERE id_utilizador = @idCliente) AS emailCliente,
        //            (SELECT nome FROM Utilizadores WHERE id_utilizador = @idCliente) AS nomeCliente", myConn);

        //        cmdEmails.Parameters.AddWithValue("@idPrestador", idPrestador);
        //        cmdEmails.Parameters.AddWithValue("@idCliente", idCliente);

        //        using (SqlDataReader drEmails = cmdEmails.ExecuteReader())
        //        {
        //            if (drEmails.Read())
        //            {
        //                emailPrestador = drEmails["emailPrestador"].ToString();
        //                nomePrestador = drEmails["nomePrestador"].ToString();
        //                emailCliente = drEmails["emailCliente"].ToString();
        //                nomeCliente = drEmails["nomeCliente"].ToString();
        //            }
        //        }
        //        myConn.Close();

        //        // MENSAGENS PRESONALIZADAS
        //        string mensagemPrestador = $"Olá {nomePrestador}, você tem um novo serviço agendado com {nomeCliente} para o dia {dataServico:dd/MM/yyyy} às {horaServico}.";
        //        string mensagemCliente = $"Olá {nomeCliente}, sua candidatura foi aceita! {nomePrestador} irá atendê-lo em {dataServico:dd/MM/yyyy} às {horaServico}.";

        //        EnviarEmail(emailPrestador, mensagemPrestador);
        //        EnviarEmail(emailCliente, mensagemCliente);
        //    }

        //    CarregarCandidaturas(Convert.ToInt32(ViewState["anuncioSelecionado"]));
        //}
        private static void EnviarEmail(string destino, string mensagem)
        {
            try
            {
                // Simulação de envio de e-mail
                MailMessage email = new MailMessage("andersson.hupp.31591@formandos.cinel.pt", destino);
                SmtpClient servidor = new SmtpClient();  // configure no Web.config se necessário

                email.Subject = "Lembrete de Serviço";
                email.Body = mensagem;
                email.IsBodyHtml = true; //se quer add html ou nao(se não vai ser so texto)
                servidor.Host = "smtp-mail.outlook.com"; //smtp.office365.com
                servidor.Port = 587;

                servidor.Credentials = new NetworkCredential("andersson.hupp.31591@formandos.cinel.pt", "@Cerveja24");

                servidor.EnableSsl = true;

                servidor.Send(email);
            }
            catch (Exception ex)
            {
                // Logar ou registrar o erro
                // Ex: salvar em um log de falha ou enviar para o admin
                Console.WriteLine("Erro ao enviar e-mail: " + ex.Message);

            }
        }

        protected void CarregarMeiosPagamentos()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = @"SELECT id_cartao, nome_titular,num_cartao, validade, principal FROM CartoesClientes WHERE id_cliente = @id_cliente";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id_cliente", userId);
                conn.Open();
                rptCartoes.DataSource = cmd.ExecuteReader();
                rptCartoes.DataBind();
            }
        }
        protected void rptCartoes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id_cartao = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "DefinirPrincipal")
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
                {
                    string sqlClear = "UPDATE Cartoes SET principal = 0 WHERE id_cliente = @idCliente";
                    SqlCommand cmdClear = new SqlCommand(sqlClear, conn);
                    cmdClear.Parameters.AddWithValue("@idCliente", userId);
                    cmdClear.ExecuteNonQuery();

                    // agora marcar o selecionado
                    string sqlSet = "UPDATE Cartoes SET principal = 1 WHERE id_cartao = @id";
                    SqlCommand cmdSet = new SqlCommand(sqlSet, conn);

                    cmdSet.Parameters.AddWithValue("@id", id_cartao);
                    conn.Open();
                    cmdSet.ExecuteNonQuery();

                }

            }
            else if (e.CommandName == "Remover")
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
                {
                    string query = @"DELETE FROM CartoesClientes WHERE id_cartao = @id_cartao";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_cartao", id_cartao);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            CarregarMeiosPagamentos();
        }
        private void CarregarCandidaturas()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = @"
                    SELECT c.id_candidatura, c.id_anuncio, u.nome AS nome_prestador, c.mensagem, a.titulo 
                    FROM Candidaturas c 
                    INNER JOIN Utilizadores u ON c.id_utilizador = u.id_utilizador
                    INNER JOIN Anuncios a ON c.id_anuncio = a.id_anuncio
                    WHERE a.id_cliente = @id_cliente AND c.cod_status = '5'";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id_cliente", userId);

                conn.Open();
                rpCandidaturas.DataSource = cmd.ExecuteReader();
                rpCandidaturas.DataBind();
            }

            
        }
       
        //PARTE DO CHAT 
        public void CarregarUsers()
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);

            SqlCommand myCmd = new SqlCommand();
            myCmd.Connection = myConn;
            myCmd.Parameters.AddWithValue("@id", userId);

            string query = @"SELECT DISTINCT 
                CASE 
                    WHEN m.id_remetente = @id THEN m.id_destinatario
                    ELSE m.id_remetente
                END AS id_contato,
                u.nome,u.telemovel,u.foto_perfil
            FROM Mensagens m
            JOIN Utilizadores u ON u.id_utilizador = 
                CASE 
                    WHEN m.id_remetente = @id THEN m.id_destinatario
                    ELSE m.id_remetente
                 END
            WHERE @id IN (m.id_remetente, m.id_destinatario)";
            myCmd.CommandText = query;

            myConn.Open();

            DataTable dt = new DataTable();
            using (SqlDataAdapter da = new SqlDataAdapter(myCmd))
            {
                da.Fill(dt);
            }
            dt.Columns.Add("FotoBase64", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                if (row["foto_perfil"] != DBNull.Value)
                {
                    byte[] fotoBytes = (byte[])row["foto_perfil"];
                    string base64 = Convert.ToBase64String(fotoBytes);
                    row["FotoBase64"] = "data:image/jpeg;base64," + base64;
                }
                //else
                //{
                //    row["FotoBase64"] = "https://via.placeholder.com/50x50?text=User";
                //}
            }


            if (dt.Rows.Count > 0)
            {
                ViewState["destinatario"] = Convert.ToString(dt.Rows[0].ItemArray[0]);
                CarregarMensagens();
            }

            rp_usuarios.DataSource = dt;
            rp_usuarios.DataBind();
            myConn.Close();

        }
        protected void rp_usuarios_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Selecionar")
            {
                ViewState["destinatario"] = Convert.ToString(e.CommandArgument);
                CarregarMensagens();
            }
        }
        protected void CarregarMensagens()
        {

            if (ViewState["destinatario"] == null)
            {
                // evita erro e permite testes sem destinatário ainda selecionado
                rp_mensagens.DataSource = null;
                rp_mensagens.DataBind();
                return;
            }
            int destinatarioId = Convert.ToInt32(ViewState["destinatario"]);
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = @"
                SELECT m.*,u.nome AS nome_remetente
                FROM Mensagens m
                LEFT JOIN Utilizadores u ON u.id_utilizador = m.id_remetente
                WHERE 
                    (m.id_remetente = @id1  AND m.id_destinatario = @id2 )
                    OR
                    (m.id_remetente = @id2 AND m.id_destinatario = @id1 )
                ORDER BY data_envio ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id1", userId);

                cmd.Parameters.AddWithValue("@id2", destinatarioId);


                conn.Open();
                rp_mensagens.DataSource = cmd.ExecuteReader();
                rp_mensagens.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "clearMessageBox", "window.clearTextArea();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "scrollToBottom", "window.scrollChatToTheBottom();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "focusOnTextArea", "window.focusOnTextArea();", true);
            }
        }

        protected void btn_enviar_Click(object sender, EventArgs e)
        {
            if (ViewState["destinatario"] == null)
                return;
            int destinatarioId = Convert.ToInt32(ViewState["destinatario"]);


            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Mensagens (id_remetente, id_destinatario, mensagem, data_envio) VALUES (@r, @d, @msg, GETDATE())", conn);
                cmd.Parameters.AddWithValue("@r", userId);
                cmd.Parameters.AddWithValue("@d", destinatarioId);
                string mensagem = Request.Form["tb_mensagem"];

                cmd.Parameters.AddWithValue("@msg", mensagem.Trim());

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            //tb_mensagem.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "clearMessageBox", "document.getElementById('tb_mensagem').value = '';", true);

            CarregarMensagens();
        }

        protected void btn_msgs_Click(object sender, EventArgs e)
        {
            panelReservas.Visible = false;
            panel_anuncios.Visible = false;
            panelCandidatura.Visible = false;
            panelPets.Visible = false;
            panelMeiosPagamentos.Visible = false;
            panel_msgs.Visible = true;
        }

        protected void btn_pet_Click(object sender, EventArgs e)
        {
            Response.Redirect("inserir_pet.aspx");
        }

        protected void btnAnuncios_Click(object sender, EventArgs e)
        {
            panelReservas.Visible = false;
            panel_msgs.Visible = false;
            panelCandidatura.Visible = true;
            panelPets.Visible = false;
            panelMeiosPagamentos.Visible = false;
            panel_anuncios.Visible = true;
        }

        protected void btnHistorico_Click(object sender, EventArgs e)
        {
            panel_msgs.Visible = false;
            panelCandidatura.Visible = false;
            panel_anuncios.Visible = false;
            panelPets.Visible = false;
            panelMeiosPagamentos.Visible = false;
            pnlNovoCartao.Visible = false;
            panelReservas.Visible = true;
        }

        protected void btnPerfil_Click(object sender, EventArgs e)
        {
            Response.Redirect("perfil.aspx");
        }

        protected void btnPets_Click(object sender, EventArgs e)
        {
            panel_msgs.Visible = false;
            panelCandidatura.Visible = false;
            panel_anuncios.Visible = false;
            panelReservas.Visible = false;
            panelMeiosPagamentos.Visible = false;
            panelPets.Visible = true;
        }

        protected void btnPagamentos_Click(object sender, EventArgs e)
        {
            panel_msgs.Visible = false;
            //panel_candidaturas.Visible = false;
            panel_anuncios.Visible = false;
            panelReservas.Visible = false;
            panelPets.Visible = false;
            pnlNovoCartao.Visible = false;
            rptCartoes.Visible = true;
            btnAddCartao.Visible = true;
            panelMeiosPagamentos.Visible = true;
        }

        protected void rbSelecionar_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selectedRadio = (RadioButton)sender;
            if (selectedRadio.Checked)
            {
                // Obter o ID do cartão selecionado
                string idCartao = selectedRadio.ToolTip;

                // Desmarcar todos os outros radio buttons
                foreach (RepeaterItem item in rptCartoes.Items)
                {
                    RadioButton rb = (RadioButton)item.FindControl("rbSelecionar");
                    if (rb != selectedRadio)
                    {
                        rb.Checked = false;
                    }
                }
            }
        }

        protected void btnAddCartao_Click(object sender, EventArgs e)
        {
            panel_msgs.Visible = false;
            panelCandidatura.Visible = false;
            panel_anuncios.Visible = false;
            panelReservas.Visible = false;
            panelPets.Visible = false;

            rptCartoes.Visible = false;
            btnAddCartao.Visible = false;
            pnlNovoCartao.Visible = true;
        }

        protected void CarregarHistoricoServicos(int cod_status, Repeater rpt)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = @"
                    SELECT  s.id_servico, s.titulo, s.descricao, s.total, s.data_criacao, a.id_agendamento, a.id_prestador
                    FROM Servicos s
                    INNER JOIN Agendamentos a ON s.id_agendamento = a.id_agendamento 
                    WHERE a.id_cliente = @id_cliente AND s.cod_status = @cod_status";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id_cliente", userId);
                cmd.Parameters.AddWithValue("@cod_status", Convert.ToInt32(cod_status));
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rpt.DataSource = dt;
                rpt.DataBind();
            }
        }

        public void rpServicosAvaliar_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Avaliar")
            {
                string[] argumentos = e.CommandArgument.ToString().Split(';');
                int idAvaliado = Convert.ToInt32(argumentos[0]);
                int idServico = Convert.ToInt32(argumentos[1]);
                int idAvaliador = Convert.ToInt32(Session["user_id"]);

                DropDownList ddlNota = (DropDownList)e.Item.FindControl("ddlNota");
                TextBox tbComentario = (TextBox)e.Item.FindControl("tbComentario");
                int nota = Convert.ToInt32(ddlNota.SelectedValue);
                string comentario = tbComentario.Text.Trim();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Avaliacoes (id_avaliador, id_avaliado, id_servico, nota, aprovado) VALUES (@idAvaliador, @idAvaliado, @idServico, @nota, 0)", conn);
                    cmd.Parameters.AddWithValue("@idAvaliador", idAvaliador);
                    cmd.Parameters.AddWithValue("@idAvaliado", idAvaliado);
                    cmd.Parameters.AddWithValue("@idServico", idServico);
                    cmd.Parameters.AddWithValue("@nota", nota);
                    cmd.Parameters.AddWithValue("@comentario", comentario);
                    cmd.ExecuteNonQuery();

                    SqlCommand updateCmd = new SqlCommand(@"
                        UPDATE Servicos SET cod_status = 9 WHERE id_servico = @idServico", conn);
                    updateCmd.Parameters.AddWithValue("@idServico", idServico);
                    updateCmd.ExecuteNonQuery();
                }

                // recarrega
                CarregarHistoricoServicos(2, rpServicosAvaliar);
            }
        }

        public void btnConcluidos_Click(object sender, EventArgs e)
        {
            btnAvaliar.CssClass = "nav-link";
            btnConcluidos.CssClass = "nav-link active";

            panelAvaliar.Visible = false;
            panelConcluidos.Visible = true;
            CarregarHistoricoServicos(3, rpServicosConcluidos);
        }

        protected void btnAvaliar_Click(object sender, EventArgs e)
        {
            btnAvaliar.CssClass = "nav-link active";
            btnConcluidos.CssClass = "nav-link";
            panelAvaliar.Visible = true;
            panelConcluidos.Visible = false;
            if (!IsPostBack) // isso evita rebind desnecessário
            {
                CarregarHistoricoServicos(2, rpServicosAvaliar);
            }
        }

        protected void btnRecusar_Click(object sender, EventArgs e)
        {

        }

        protected void btnAceitar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            RepeaterItem item = (RepeaterItem)btn.NamingContainer;
            int id_candidatura = Convert.ToInt32(btn.CommandArgument);

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString);


                SqlCommand myCmd = new SqlCommand("UPDATE Candidaturas SET cod_status = 7 WHERE id_candidatura = @id_candidatura", myConn);
                myCmd.Parameters.AddWithValue("@id_candidatura", id_candidatura);
                myConn.Open();
                myCmd.ExecuteNonQuery();

                // obter info da candidatura (prestador e anuncio)
                int idPrestador = 0;
                int idCliente = 0;
                int idAnuncio = 0;
                string mensagem = "";
                string descricao = "";
                string titulo = "";
                decimal total = 0;
                DateTime dataServico = DateTime.MinValue;
                TimeSpan horaServico = TimeSpan.Zero;

                SqlCommand cmdInfo = new SqlCommand(@"SELECT c.id_utilizador, c.id_anuncio, c.mensagem, a.titulo, a.descricao, a.preco, a.data_nece, a.hora_nece, a.id_cliente 
                FROM Candidaturas c
                INNER JOIN Anuncios a ON c.id_anuncio = a.id_anuncio
                WHERE c.id_candidatura = @id_candidatura", myConn);
                cmdInfo.Parameters.AddWithValue("@id_candidatura", id_candidatura);
                SqlDataReader dr = cmdInfo.ExecuteReader();

                if (dr.Read())
                {
                    idPrestador = Convert.ToInt32(dr["id_utilizador"]);
                    idAnuncio = Convert.ToInt32(dr["id_anuncio"]);
                    titulo = dr["titulo"].ToString();
                    descricao = dr["descricao"].ToString();
                    mensagem = dr["mensagem"].ToString();
                    total = Convert.ToDecimal(dr["preco"]);
                    dataServico = Convert.ToDateTime(dr["data_nece"]);
                    horaServico = (TimeSpan)dr["hora_nece"];
                    idCliente = Convert.ToInt32(dr["id_cliente"]);
                }
                dr.Close();

                // cancela as outras candidaturas do mesmo anuncio
                SqlCommand cmdCancelar = new SqlCommand(@"
                UPDATE Candidaturas SET cod_status = 6
                WHERE id_anuncio = @id_anuncio AND id_candidatura != @id_candidatura", myConn);
                cmdCancelar.Parameters.AddWithValue("@id_anuncio", idAnuncio);
                cmdCancelar.Parameters.AddWithValue("@id_candidatura", id_candidatura);
                cmdCancelar.ExecuteNonQuery();

                // criar agendamento
                SqlCommand cmdAgendamento = new SqlCommand(@"
                INSERT INTO Agendamentos (id_cliente, id_prestador, id_anuncio,hora_inicio, hora_fim, cod_status, data_servico, data_criacao)
                VALUES (@id_cliente, @id_prestador, @id_anuncio, @hora_inicio, @hora_fim, 8, @data_servico, GETDATE());
                SELECT CAST(SCOPE_IDENTITY() AS INT);", myConn);

                cmdAgendamento.Parameters.AddWithValue("@id_cliente", idCliente);
                cmdAgendamento.Parameters.AddWithValue("@id_prestador", idPrestador);
                cmdAgendamento.Parameters.AddWithValue("@id_anuncio", idAnuncio);
                cmdAgendamento.Parameters.AddWithValue("@hora_inicio", horaServico);
                cmdAgendamento.Parameters.AddWithValue("@hora_fim", horaServico.Add(new TimeSpan(1, 0, 0)));
                cmdAgendamento.Parameters.AddWithValue("@data_servico", dataServico);


                int idAgendamento = Convert.ToInt32(cmdAgendamento.ExecuteScalar());

                DateTime inicioEvento = dataServico.Add(horaServico);
                DateTime fimEvento = inicioEvento.Add(new TimeSpan(1, 0, 0));

                GoogleCalendarServico.CriarEvento(idPrestador, titulo, $"Reserva de serviço com {idCliente}", inicioEvento, fimEvento);

                // criar servico
                SqlCommand cmdServico = new SqlCommand(@"
                INSERT INTO Servicos (id_agendamento, titulo, descricao, total, cod_status, data_criacao)
                VALUES (@id_agendamento, @titulo, @descricao, @total, @cod_status, GETDATE())", myConn);

                cmdServico.Parameters.AddWithValue("@id_agendamento", idAgendamento);
                cmdServico.Parameters.AddWithValue("@titulo", titulo);
                cmdServico.Parameters.AddWithValue("@descricao", descricao);
                cmdServico.Parameters.AddWithValue("@total", total);
                cmdServico.Parameters.AddWithValue("@cod_status", 8);


                cmdServico.ExecuteNonQuery();
                lbl_msg.Text = "Candidatura aceite. Agendamento e serviço criados.";

                string emailPrestador = "";
                string nomePrestador = "";
                string emailCliente = "";
                string nomeCliente = "";

                SqlCommand cmdEmails = new SqlCommand(@"
                    SELECT 
                    (SELECT email FROM Utilizadores WHERE id_utilizador = @idPrestador) AS emailPrestador,
                    (SELECT nome FROM Utilizadores WHERE id_utilizador = @idPrestador) AS nomePrestador,
                    (SELECT email FROM Utilizadores WHERE id_utilizador = @idCliente) AS emailCliente,
                    (SELECT nome FROM Utilizadores WHERE id_utilizador = @idCliente) AS nomeCliente", myConn);

                cmdEmails.Parameters.AddWithValue("@idPrestador", idPrestador);
                cmdEmails.Parameters.AddWithValue("@idCliente", idCliente);

                using (SqlDataReader drEmails = cmdEmails.ExecuteReader())
                {
                    if (drEmails.Read())
                    {
                        emailPrestador = drEmails["emailPrestador"].ToString();
                        nomePrestador = drEmails["nomePrestador"].ToString();
                        emailCliente = drEmails["emailCliente"].ToString();
                        nomeCliente = drEmails["nomeCliente"].ToString();
                    }
                }
                myConn.Close();

                // MENSAGENS PRESONALIZADAS
                string mensagemPrestador = $"Olá {nomePrestador}, você tem um novo serviço agendado com {nomeCliente} para o dia {dataServico:dd/MM/yyyy} às {horaServico}.";
                string mensagemCliente = $"Olá {nomeCliente}, sua candidatura foi aceita! {nomePrestador} irá atendê-lo em {dataServico:dd/MM/yyyy} às {horaServico}.";

                EnviarEmail(emailPrestador, mensagemPrestador);
                EnviarEmail(emailCliente, mensagemCliente);

            //CarregarCandidaturas(Convert.ToInt32(ViewState["anuncioSelecionado"]));
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            // Encerrar a autenticação com Google (caso esteja autenticado)
            HttpContext.Current.GetOwinContext().Authentication.SignOut();


            Response.Redirect("login.aspx");
        }
    }
    

}
