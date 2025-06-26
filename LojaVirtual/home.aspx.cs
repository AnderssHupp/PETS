using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LojaVirtual
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_serSitter_Click(object sender, EventArgs e)
        {
            Response.Redirect("criar_conta.aspx");
        }
        

        protected void btn_buscarSitter_Click(object sender, EventArgs e)
        {
            Response.Redirect("prestadores.aspx");
        }
    }
    
}