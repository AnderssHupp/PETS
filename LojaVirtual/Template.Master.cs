using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LojaVirtual
{
    public partial class Template : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string perfilUrl = GetDashboardUrlPorPerfil();
                litDashboardLink.Text = $"<a href='{perfilUrl}' class='nav-link'><i class='fa fa-user-circle'></i> Perfil</a>";
            }
        }

        private string GetDashboardUrlPorPerfil()
        {
            
            int cod_perfil = Convert.ToInt32(Session["cod_perfil"]);

            switch (cod_perfil)
            {
                case 1: return "dashboard_admin.aspx";
                case 2: return "dashboard_cliente.aspx";
                case 3: return "dashboard_prestador.aspx";
                default: return "login.aspx"; // fallback
            }
        }
    }
}