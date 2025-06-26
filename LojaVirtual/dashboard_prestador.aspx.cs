using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using LojaVirtual.Servicos;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace LojaVirtual
{
    public partial class agenda : System.Web.UI.Page
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

                CarregarReservas(8);
                CarregarFotoPerfil();
                CarregarAgenda();
                CarregarUsers();
                CarregarMeiosPagamentos();
                CarregarServicosConcluidos();
                if (Convert.ToBoolean(Session["googleUser"]) == true)
                {
                    SincronizarAgendamentosComGoogleCalendar(userId);

                }
            }
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
                cmd.Parameters.AddWithValue("@id_utilizador", userId);
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
                        // Se não tiver foto, opcional: colocar imagem padrão
                        img_foto.ImageUrl = "/images/avatar.jpg";
                    }

                }

            }

        }
        private void CarregarReservas(int cod_status)
        {
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = @"SELECT a.data_servico, a.hora_inicio, a.hora_fim, u.nome AS nome_cliente
                FROM Agendamentos a
                INNER JOIN Utilizadores u ON a.id_cliente = u.id_utilizador
                WHERE a.id_prestador = @id AND a.cod_status = @status
                ORDER BY a.data_servico DESC";

                SqlCommand cmd = new SqlCommand(query, myConn);
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.Parameters.AddWithValue("@status", Convert.ToInt32(cod_status));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rp_atuais.DataSource = dt;
                rp_atuais.DataBind();
            }
        }
        private void CarregarAgenda()
        {
            int prestadorId = Convert.ToInt32(Session["user_id"]);      
            string disponibilidade = ObterDisponibilidade(prestadorId);
            var disponiveis = GerarDisponibilidade(disponibilidade);
            //string accessToken = ObterAccessToken(prestadorId);
            var agendamentos = ObterAgendamentos(prestadorId);
            var eventosGoogle = ObterEventosGoogleCalendar(userId);

            
            var todosEventos = eventosGoogle.Concat(agendamentos).ToList();
            ViewState["json_agenda"] = JsonConvert.SerializeObject(todosEventos);
            ViewState["json_disponiveis"] = JsonConvert.SerializeObject(disponiveis);

        }
    
        public void SincronizarAgendamentosComGoogleCalendar(int id)
        {
            var agendamentos = ObterAgendamentosParaSync(userId);

            foreach (var ag in agendamentos)
            {
                GoogleCalendarServico.CriarEvento(userId, ag.Titulo, ag.Descricao, ag.Inicio, ag.Fim);
            }
        }
        public List<(string Titulo, string Descricao, DateTime Inicio, DateTime Fim)> ObterAgendamentosParaSync(int idPrestador)
        {
            var lista = new List<(string, string, DateTime, DateTime)>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT a.data_servico, a.hora_inicio, a.hora_fim, s.titulo, s.descricao
            FROM Agendamentos a
            JOIN Servicos s ON s.id_agendamento = a.id_agendamento
            WHERE a.id_prestador = @id AND a.cod_status = 8", conn);

                cmd.Parameters.AddWithValue("@id", idPrestador);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var data = Convert.ToDateTime(dr["data_servico"]);
                        var inicio = data.Add((TimeSpan)dr["hora_inicio"]);
                        var fim = data.Add((TimeSpan)dr["hora_fim"]);
                        string titulo = dr["titulo"].ToString();
                        string descricao = dr["descricao"].ToString();

                        lista.Add((titulo, descricao, inicio, fim));
                    }
                }
            }

            return lista;
        }

      
        public List<object> ObterEventosGoogleCalendar(int id_utilizador)
        {
            var tokens = GoogleCalendarServico.ObterTokensDoBanco(userId);
            if (tokens == null) return new List<object>();

            // Renova token se expirado
            if (tokens.Value.Expiration <= DateTimeOffset.UtcNow)
            {
                var novoTokens = GoogleCalendarServico.RenovarToken(tokens.Value.RefreshToken);
                if (novoTokens == null) return new List<object>();

                GoogleCalendarServico.SalvarTokenAtualizado(userId, novoTokens.Value.AccessToken, novoTokens.Value.Expiration);
                tokens = (novoTokens.Value.AccessToken, tokens.Value.RefreshToken, novoTokens.Value.Expiration);
            }

            var credential = GoogleCredential
                .FromAccessToken(tokens.Value.AccessToken)
                .CreateScoped(CalendarService.Scope.Calendar);

            //var credential = GoogleCredential.FromAccessToken(accessToken);

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "LojaVirtualApp"
            });

            List<object> eventos = new List<object>();
            var request = service.Events.List("primary");
            request.TimeMin = DateTime.UtcNow;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 20;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            var events = request.Execute();

            foreach (var ev in events.Items)
            {
                eventos.Add(new
                {
                    title = ev.Summary,
                    start = ev.Start.DateTime?.ToString("yyyy-MM-ddTHH:mm:ss") ?? ev.Start.Date,
                    end = ev.End.DateTime?.ToString("yyyy-MM-ddTHH:mm:ss") ?? ev.End.Date,
                    color = "#4285f4" // azul do Google
                });
            }

            return eventos;
        }
        private string ObterDisponibilidade(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT disponibilidade FROM PrestadorInfo WHERE id_utilizador = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                return (cmd.ExecuteScalar() ?? "Todos os dias").ToString();
            }
        }

        public List<object> GerarDisponibilidade(string disponibilidade)
        {
            List<object> lista = new List<object>();
            DateTime hoje = DateTime.Today;
            DateTime fim = hoje.AddDays(30);

            for (DateTime d = hoje; d <= fim; d = d.AddDays(1))
            {
                DayOfWeek dw = d.DayOfWeek;
                bool add = false;
                if (disponibilidade == "Todos os dias")
                {
                    add = true;
                }
                else if (disponibilidade == "Segunda a Sexta")
                {
                    add = dw >= DayOfWeek.Monday && dw <= DayOfWeek.Friday;
                }
                else if (disponibilidade == "Fins de Semana e Feriados")
                {
                    add = dw == DayOfWeek.Saturday || dw == DayOfWeek.Sunday;
                }

                if (add)
                {
                    lista.Add(new
                    {
                        title = "Disponível",
                        start = d.ToString("yyyy-MM-dd"),
                        display = "background",
                        backgroundColor = "#d4edda" //verde claro
                    });
                }
            }
            return lista;
        }
        public List<object> ObterAgendamentos(int idPrestador)
        {
            List<object> eventos = new List<object>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT a.data_servico, a.hora_inicio, a.hora_fim, u.nome
            FROM Agendamentos a
            INNER JOIN Utilizadores u ON u.id_utilizador = a.id_cliente
            WHERE a.id_prestador = @id AND a.cod_status = '8'
        ", conn);
                cmd.Parameters.AddWithValue("@id", idPrestador);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DateTime data = Convert.ToDateTime(dr["data_servico"]);
                        TimeSpan horaInicio = (TimeSpan)dr["hora_inicio"];
                        TimeSpan horaFim = (TimeSpan)dr["hora_fim"];
                        string nomeCliente = dr["nome"].ToString();

                        eventos.Add(new
                        {
                            title = $"Reserva: {nomeCliente}",
                            start = data.Add(horaInicio).ToString("yyyy-MM-ddTHH:mm:ss"),
                            end = data.Add(horaFim).ToString("yyyy-MM-ddTHH:mm:ss"),
                            color = "#ffc107"
                        });
                    }
                }
            }
            return eventos;
        }
        public void btn_atuais_Click(object sender, EventArgs e)
        {
            btn_atuais.CssClass = "nav-link active";
            btn_pendentes.CssClass = "nav-link";
            btn_concluidas.CssClass = "nav-link";
            CarregarReservas(8);//agendado
            UpdatePanelReservas.Update();

        }

        public void btn_pendentes_Click(object sender, EventArgs e)
        {
            btn_atuais.CssClass = "nav-link";
            btn_pendentes.CssClass = "nav-link active";
            btn_concluidas.CssClass = "nav-link";
            CarregarReservas(5);
            UpdatePanelReservas.Update();

        }

        public void btn_concluidas_Click(object sender, EventArgs e)
        {
            btn_atuais.CssClass = "nav-link";
            btn_pendentes.CssClass = "nav-link";
            btn_concluidas.CssClass = "nav-link active";
            CarregarReservas(3);
            UpdatePanelReservas.Update();

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
                lbl_msg.Visible = (rptCartoes.Items.Count == 0);
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

        protected void CarregarServicosConcluidos()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = @"
            SELECT s.id_servico, s.id_agendamento, s.titulo, s.descricao, s.total, s.data_criacao
            FROM Servicos s
            INNER JOIN Agendamentos a ON a.id_agendamento = s.id_agendamento
            WHERE a.id_prestador = @id_prestador
            ORDER BY s.data_criacao DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id_prestador", userId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                rpServicosConcluidos.DataSource = reader;
                rpServicosConcluidos.DataBind();

                lbl_msgS.Visible = rpServicosConcluidos.Items.Count == 0;
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

            panel_agenda.Visible = false;
            panel_reservas.Visible = false;
            panelMeiosPagamentos.Visible = false;
            btnAddCartao.Visible = false;
            panelServicos.Visible = false;
            panel_msgs.Visible = true;
        }

        protected void btnAgenda_Click(object sender, EventArgs e)
        {
            panel_reservas.Visible = false;
            panel_msgs.Visible = false;
            panelServicos.Visible = false;
            panelMeiosPagamentos.Visible = false;
            btnAddCartao.Visible = false;
            panel_agenda.Visible = true;
        }

        protected void btnPerfil_Click(object sender, EventArgs e)
        {
            Response.Redirect("perfil.aspx");
        }

        protected void btnReservas_Click(object sender, EventArgs e)
        {
            panel_agenda.Visible = false;
            panel_msgs.Visible = false;
            panelServicos.Visible = false;
            btnAddCartao.Visible = false;
            panelMeiosPagamentos.Visible = false;
            panel_reservas.Visible = true;
        }

        protected void btnHistorico_Click(object sender, EventArgs e)
        {

            panel_agenda.Visible = false;
            panel_reservas.Visible = false;
            panel_msgs.Visible = false;
            panelMeiosPagamentos.Visible = false;
            btnAddCartao.Visible = false;
            panelServicos.Visible = true;
        }

        protected void btnPagamentos_Click(object sender, EventArgs e)
        {
            panel_agenda.Visible = false;
            panel_reservas.Visible = false;
            panel_msgs.Visible = false;
            panelServicos.Visible = false;
            pnlNovoCartao.Visible = false;
            panelMeiosPagamentos.Visible = true;
            rptCartoes.Visible = true;
            btnAddCartao.Visible = true;
        }

        protected void btnAddCartao_Click(object sender, EventArgs e)
        {
            panel_agenda.Visible = false;
            panel_reservas.Visible = false;
            panel_msgs.Visible = false;
            panelServicos.Visible = false;
            rptCartoes.Visible = false;
            btnAddCartao.Visible = false;
            lbl_msg.Visible = false;
            pnlNovoCartao.Visible = true;
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