using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace LojaVirtual.Servicos
{
    public class ServicosLembretes
    {
        public static void EnviarLembretesHoje()
        {

            // Lógica para buscar clientes com serviços recorrentes
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                conn.Open();
                string query = @"SELECT u.email, u.nome, p.nome_pet, ts.nome_servico, a.data_servico
                                 FROM Agendamentos a
                                 INNER JOIN Anuncios an ON a.id_anuncio = an.id_anuncio 
                                 INNER JOIN Utilizadores u ON an.id_cliente = u.id_utilizador
                                 INNER JOIN Pets p ON an.id_pet = p.id_pet
                                 INNER JOIN TipoServico ts ON an.cod_TipoServico = ts.cod_TipoServico
                                 WHERE CAST (a.data_servico AS DATE) = CAST(GETDATE() AS DATE) AND a.cod_status = 8";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string email = reader["email"].ToString();
                    string nome = reader["nome"].ToString();
                    string pet = reader["nome_pet"].ToString();
                    string servico = reader["nome_servico"].ToString();
                    DateTime data = Convert.ToDateTime(reader["data_servico"]);

                    EnviarEmail(email, $"Olá {nome}, lembrete: {servico} para {pet} em {data:dd/MM/yyyy}.");
                }
            }
        }

        public static void EnviarLembretesRecorrente()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                conn.Open();

                // Seleciona último serviço realizado por cliente, pet e tipo de serviço (banho, tosquia, vacinação)
                string query = @"
                    SELECT 
                        u.email, u.nome, p.nome_pet, ts.nome_servico,
                        MAX(a.data_servico) AS ultima_data_servico
                    FROM Agendamentos a
                    INNER JOIN Anuncios an ON a.id_anuncio = an.id_anuncio 
                    INNER JOIN Utilizadores u ON an.id_cliente = u.id_utilizador
                    INNER JOIN Pets p ON an.id_pet = p.id_pet
                    INNER JOIN TipoServico ts ON an.cod_TipoServico = ts.cod_TipoServico
                    WHERE a.cod_status = 7
                    AND ts.nome_servico IN ('Banho', 'Tosquia', 'Vacinação')
                    GROUP BY u.email, u.nome, p.nome_pet, ts.nome_servico";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                DateTime hoje = DateTime.Today;

                while (reader.Read())
                {
                    string email = "anderssonhupp@gmail.com";/*reader["email"].ToString();*/
                    string nome = reader["nome"].ToString();
                    string pet = reader["nome_pet"].ToString();
                    string servico = reader["nome_servico"].ToString();
                    DateTime ultimaData = Convert.ToDateTime(reader["ultima_data_servico"]);

                    bool enviarLembrete = false;
                    string mensagem = "";

                    switch (servico)
                    {
                        case "Banho":
                            // banho 30 dias)
                            if ((hoje - ultimaData).TotalDays >= 30)
                            {
                                enviarLembrete = true;
                                mensagem = $"Olá {nome}, está na hora do banho para {pet}! Último banho realizado em {ultimaData:dd/MM/yyyy}.";
                            }
                            break;

                        case "Tosquia":
                            // Tosquia: a cada 6 meses 
                            if ((hoje - ultimaData).TotalDays >= 180)
                            {
                                enviarLembrete = true;
                                mensagem = $"Olá {nome}, está na hora da tosquia para {pet}! Última tosquia realizada em {ultimaData:dd/MM/yyyy}.";
                            }
                            break;

                        case "Vacinação":
                            // Vacinação: anual (365 dias)
                            if ((hoje - ultimaData).TotalDays >= 365)
                            {
                                enviarLembrete = true;
                                mensagem = $"Olá {nome}, está na hora da vacinação anual para {pet}! Última vacinação realizada em {ultimaData:dd/MM/yyyy}.";
                            }
                            break;
                    }

                    if (enviarLembrete)
                    {
                        EnviarEmail(email, mensagem);
                    }
                }
            }
        }

        private static void EnviarEmail(string destino, string mensagem)
        {
            try
            {

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
    }

}
