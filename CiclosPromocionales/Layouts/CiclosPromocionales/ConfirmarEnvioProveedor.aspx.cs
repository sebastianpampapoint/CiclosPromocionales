using System;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CiclosPromocionales.Layouts.CiclosPromocionales
{
    public partial class ConfirmarEnvioProveedor : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 idProceso = 0;
            idProceso = Convert.ToInt32(Request["ID"]);
            iProceso.Value = idProceso.ToString();
            iCicloPromocional.Value = Request["IDCiclo"];

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    if (Page.IsPostBack != true) {
                        SPList lCicloPromocional = web.Lists["Procesar Proveedor"];


                        StringBuilder strPiezas = new StringBuilder();

                        SPListItem itmCicloPromocional = lCicloPromocional.GetItemById(idProceso);

                        SPFieldLookupValue lkvProveedor = new SPFieldLookupValue(itmCicloPromocional["Proveedor"] as String);
                        Int32 iProveedor = lkvProveedor.LookupId;


                        SPList lProveedores = web.Lists["Proveedores"];
                        SPListItem itmProveedor = lProveedores.GetItemById(iProveedor);
                        String strUsuario = "";
                        String strClave = "";

                        if (itmProveedor["Usuario"] != null) { strUsuario = itmProveedor["Usuario"].ToString(); }
                        if (itmProveedor["Clave"] != null) { strClave = itmProveedor["Clave"].ToString(); }

                        StringBuilder strMensaje = new StringBuilder();

                        txtMensaje.RichText = true;
                    SPList lMensajes = web.Lists["Configuración Mensajes"];
                    SPListItemCollection sPListItemCollection = lMensajes.GetItems();
                    foreach (SPListItem sPListItem in sPListItemCollection)
                    {
                            if (sPListItem["Tipo mensaje"].ToString() == "Envío Proveedor")
                                strMensaje.Append(sPListItem["Mensaje"].ToString());
                            //strMensaje.AppendLine(sPListItem["Mensaje"].ToString());
                    }

                        strMensaje.Replace("ValorUsuario", strUsuario);
                        strMensaje.Replace("ValorClave", strClave);

                        //strMensaje.AppendLine("");
                        //strMensaje.AppendLine("Usuario:  " + strUsuario);
                        //strMensaje.AppendLine("Clave  :  " + strClave);
                        //strMensaje.AppendLine("");
                        //strMensaje.AppendLine("Materiales a procesar:  " );

                        SPFieldLookupValueCollection flPiezas = itmCicloPromocional["Piezas"] as SPFieldLookupValueCollection;
                        foreach (SPFieldLookupValue gwPieza in flPiezas)
                    {
                            strPiezas.AppendLine(" - " + gwPieza.LookupValue.ToString() + "<br />");

                        }

                        strMensaje.Replace("ValorMateriales", strPiezas.ToString());

                        txtMensaje.Text = strMensaje.ToString();

                    }

                }
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalPiezas.aspx?ID=" + iCicloPromocional.Value.ToString() + "&Origen=C");
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            Boolean bProcesado = true;

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lCicloPromocional = web.Lists["Procesamiento"];

                    String strPiezas = "";
                    
     

                    SPListItem itmCicloPromocional = lCicloPromocional.AddItem();
                    itmCicloPromocional["Title"] = iProceso.Value.ToString();
                    itmCicloPromocional["Acción"] = "Envío Proveedor";
                    itmCicloPromocional["Identificador"] = iProceso.Value.ToString();
                    itmCicloPromocional["Mensaje"] = txtMensaje.Text;
                    itmCicloPromocional.Update();

                }
            }

            if (bProcesado == true)
            {

                Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalPiezas.aspx?ID=" + iCicloPromocional.Value.ToString() + "&Origen=C");
            }
        }
    }
}
