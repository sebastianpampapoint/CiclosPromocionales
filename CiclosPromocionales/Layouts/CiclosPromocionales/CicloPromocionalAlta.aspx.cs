using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CiclosPromocionales.Layouts.CiclosPromocionales
{
    public partial class CicloPromocionalAlta : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 idDocument = 0;
            idDocument = Convert.ToInt32(Request["ID"]);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)

        {
            Boolean bProcesado = true;
            Int32 iCicloPromocional = 0;

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lCicloPromocional = web.Lists["Ciclo Promocional"];

                    Boolean bCotiza = true;

                    if (ddlCotiza.SelectedValue == "NO") { bCotiza = false; }

                    SPListItem itmCicloPromocional = lCicloPromocional.AddItem();
                    itmCicloPromocional["Title"] = txtTitulo.Text;
                    itmCicloPromocional["Detalle"] = txtDescripcion.Text;
                    itmCicloPromocional["Cotiza"] = bCotiza;
                    itmCicloPromocional["Estado"] = "Borrador";
                    if (bCotiza == false)
                    {
                        itmCicloPromocional["Cotización"] = "No Aplica";
                    }
                    itmCicloPromocional.Update();

                    iCicloPromocional = itmCicloPromocional.ID;

                    itmCicloPromocional["Ver"] = @"<p><a href='" + SPContext.Current.Site.Url  + "/_layouts/15/CiclosPromocionales/CicloPromocionalPiezas.aspx?ID=" + iCicloPromocional + "&Origen=E'><img alt='Ver' src='/SiteAssets/dashboard.png' style = 'margin: 0px; width: 16px; '/></a></p>";
                    itmCicloPromocional["Estado"] = "Borrador";
                    itmCicloPromocional.Update();

                    System.Threading.Thread.Sleep(5000);
                }
            }

            if (bProcesado == true) { 

                Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalPiezas.aspx?ID=" + iCicloPromocional + "&Origen=A");
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

        }
    }
}
