using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CiclosPromocionales.Layouts.CiclosPromocionales
{
    public partial class Procesando : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);

            if (Request["Pg"] == "CP")
            {
                Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalPiezas.aspx?ID=" + Request["ID"] + "&Origen=E");

            }
            // Redirecciono a Tareas x Piezas
            if (Request["Pg"] == "TP")
            {
                Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalTareas.aspx?ID=" + Request["ID"] + "&IDPieza=" + Request["IDPieza"]);

            }

            if (Request["Pg"] == "DP")
            {
                Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalDetallePieza.aspx?ID=" + Request["ID"] + "&IDPieza=" + Request["IDPieza"]);

            }


            Response.Redirect(SPContext.Current.Site.Url );



        }
    }
}
