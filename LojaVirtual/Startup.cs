using Hangfire;
using Hangfire.SqlServer;
using LojaVirtual.Servicos;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(LojaVirtual.Startup))]

namespace LojaVirtual
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/login.aspx")
            });

            //app.Use(async (context, next) =>
            //{
            //    // Intercepta erro de OAuth do Google (por ex.: access_denied)
            //    if (context.Request.Path == new PathString("/signin-google") &&
            //        context.Request.Query["error"] == "access_denied")
            //    {
            //        context.Response.Redirect("/login.aspx?error=access_denied");
            //        return; // evita continuar o pipeline
            //    }

            //    await next.Invoke();
            //});
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = System.Configuration.ConfigurationManager.AppSettings["GoogleClientId"],
                ClientSecret = System.Configuration.ConfigurationManager.AppSettings["GoogleClientSecret"],
                CallbackPath = new PathString("/signin-google"),
                Provider = new GoogleOAuth2AuthenticationProvider
                {
                    OnApplyRedirect = context =>
                    {
                        // Forçar o consentimento e o refresh_token
                        context.Response.Redirect(context.RedirectUri + "&access_type=offline&prompt=consent");
                    },
                    OnAuthenticated = context =>
                    {
                        // salvar tokens no AuthenticationProperties
                        context.Identity.AddClaim(new Claim("access_token", context.AccessToken));
                        context.Identity.AddClaim(new Claim("refresh_token", context.RefreshToken ?? ""));

                        if (context.ExpiresIn.HasValue)
                        {
                            var expiresAt = DateTimeOffset.UtcNow.Add(context.ExpiresIn.Value);
                            context.Identity.AddClaim(new Claim("expires", expiresAt.ToUnixTimeSeconds().ToString()));
                        }

                        return Task.FromResult(0);
                    }

                },// incluir escopos do calendar
                SignInAsAuthenticationType = "ApplicationCookie",
                Scope = { "email", "profile", "https://www.googleapis.com/auth/calendar" }

            }); 
        

        

            //HANGFIRE
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("LojaVirtualConnectionString");

            app.UseHangfireServer(); // inicia o servidor
            app.UseHangfireDashboard("/hangfire"); // painel acessivel via /hangfire

            

            // job recorrente
            RecurringJob.AddOrUpdate(
                "lembrete-servico-pet",
                () => ServicosLembretes.EnviarLembretesHoje(),
                Cron.Daily); // executa diariamente
            RecurringJob.AddOrUpdate(
              "lembrete-servico-reccorentes-pet",
              () => ServicosLembretes.EnviarLembretesRecorrente(),
              Cron.Daily);



        }
    }
}
