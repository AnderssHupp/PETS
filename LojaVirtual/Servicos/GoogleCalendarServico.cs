using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace LojaVirtual.Servicos
{
    public class GoogleCalendarServico
    {
        public static void CriarEvento(int id_utilizador, string titulo, string descricao, DateTime inicio, DateTime fim)
        {
            var tokens = ObterTokensDoBanco(id_utilizador);
            if (tokens == null) return;

            // Verificar expiração e renovar se necessário
            if (tokens.Value.Expiration <= DateTimeOffset.UtcNow)
            {
                var novoTokens = RenovarToken(tokens.Value.RefreshToken);
                if (novoTokens == null) return;

                SalvarTokenAtualizado(id_utilizador, novoTokens.Value.AccessToken, novoTokens.Value.Expiration);
                tokens = (novoTokens.Value.AccessToken, tokens.Value.RefreshToken, novoTokens.Value.Expiration);
            }

            var credential = GoogleCredential
                .FromAccessToken(tokens.Value.AccessToken)
                .CreateScoped(CalendarService.Scope.Calendar);

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "LojaVirtual"
            });

            var eventosExistentes = ObterEventosExistentes(service, inicio, fim);
            bool sobreposto = eventosExistentes.Any(ev =>
                ev.Start.DateTime == inicio && ev.End.DateTime == fim);

            if (!sobreposto)
            {
                var novoEvento = new Event()
                {
                    Summary = titulo,
                    Description = descricao,
                    Start = new EventDateTime() { DateTime = inicio },
                    End = new EventDateTime() { DateTime = fim }
                };

                service.Events.Insert(novoEvento, "primary").Execute();
            }
        }

        private static IList<Event> ObterEventosExistentes(CalendarService service, DateTime inicio, DateTime fim)
        {
            var request = service.Events.List("primary");
            request.TimeMin = inicio;
            request.TimeMax = fim;
            request.ShowDeleted = false;
            request.SingleEvents = true;

            var events = request.Execute();
            return events.Items;
        }

        public static (string AccessToken, string RefreshToken, DateTimeOffset Expiration)? ObterTokensDoBanco(int id_utilizador)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ConnectionString;

            using (SqlConnection myConn = new SqlConnection(connectionString))
            {
                string query = @"SELECT access_token, refresh_token, expires_at 
                             FROM TokensGoogle 
                             WHERE id_utilizador = @id_utilizador";

                SqlCommand cmd = new SqlCommand(query, myConn);
                cmd.Parameters.AddWithValue("@id_utilizador", id_utilizador);
                myConn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        string accessToken = dr["access_token"].ToString();
                        string refreshToken = dr["refresh_token"].ToString();
                        DateTimeOffset expiresAt;

                        if (DateTimeOffset.TryParse(dr["expires_at"].ToString(), out expiresAt))
                        {
                            return (accessToken, refreshToken, expiresAt);
                        }
                    }
                }
            }

            return null;
        }

        public static (string AccessToken, DateTimeOffset Expiration)? RenovarToken(string refreshToken)
        {
            var clientId = ConfigurationManager.AppSettings["GoogleClientId"];
            var clientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"];
            var httpClient = new HttpClient();

            var requestContent = new StringContent($"client_id={clientId}&client_secret={clientSecret}&refresh_token={refreshToken}&grant_type=refresh_token",
                Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = httpClient.PostAsync("https://oauth2.googleapis.com/token", requestContent).Result;

            if (response.IsSuccessStatusCode)
            {
                dynamic json = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                string newAccessToken = json.access_token;
                int expiresIn = json.expires_in;
                var newExpiration = DateTimeOffset.UtcNow.AddSeconds(expiresIn);

                return (newAccessToken, newExpiration);
            }

            return null;
        }

        public static void SalvarTokenAtualizado(int id_utilizador, string novoAccessToken, DateTimeOffset novaExpiracao)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LojaVirtualConnectionString"].ToString()))
            {
                string query = @"UPDATE TokensGoogle SET access_token = @access, expires_at = @exp WHERE id_utilizador = @id";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@access", novoAccessToken);
                cmd.Parameters.AddWithValue("@exp", novaExpiracao);
                cmd.Parameters.AddWithValue("@id", id_utilizador);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}