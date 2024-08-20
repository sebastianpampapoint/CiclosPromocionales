using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CiclosPromocionales.Layouts.CiclosPromocionales
{
    public partial class CicloPromocionalEnvioProveedor : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 idCicloPromocional = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            iCicloPromocional.Value = idCicloPromocional.ToString();
            vCargarDatos(idCicloPromocional);
            ArmarComboProveedor(idCicloPromocional);
        }


        public void vCargarDatos(Int32 idCicloPromocional)
        {
            string sEstado = "";
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lCicloPromocional = web.Lists["Ciclo Promocional"];

                    SPListItem itmCicloPromocional = lCicloPromocional.GetItemById(idCicloPromocional);
                    if (itmCicloPromocional != null)
                    {
                        txtTitulo.Text = itmCicloPromocional["Title"].ToString();
                        String strDetalle = "";
                        if (itmCicloPromocional["Detalle"] != null) { strDetalle = itmCicloPromocional["Detalle"].ToString(); }
                        txtDescripcion.Text = strDetalle;
                        sEstado = itmCicloPromocional["Estado"].ToString();

                    }
                }
            }
        }

        public void vCargarPiezas(Int32 idCicloPromocional, Int32 idProveedor)
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lCicloPromocional = web.Lists["Piezas"];
                    SPQuery qryTareas = new SPQuery();
                    String strQuery = "";
                    qryTareas = new SPQuery(lCicloPromocional.Views["Todos los elementos"]);
                    String sOrden = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='{1}' /></OrderBy>", "ID", "False");
                    strQuery = "<And><And><And><Eq><FieldRef Name='Ciclo' LookupId='TRUE' /><Value Type='Lookup'>" + idCicloPromocional.ToString() + "</Value></Eq><Eq><FieldRef Name='Proveedor' LookupId='TRUE' /><Value Type='Lookup'>" + idProveedor.ToString() + "</Value></Eq></And><Eq><FieldRef Name='Enviado_x0020_Proveedor' /><Value Type='Choice'>0</Value></Eq></And><Eq><FieldRef Name='Estado_x0020_Documento' /><Value Type='Choice'>Aprobado</Value></Eq></And>";


                    if (!string.IsNullOrEmpty(strQuery))
                    {
                        strQuery = "<Where>" + strQuery + "</Where>";
                    }
                    if (!string.IsNullOrEmpty(sOrden))
                    {
                        strQuery = strQuery + sOrden;
                    }

                    qryTareas.Query = strQuery;
                    qryTareas.RowLimit = 500;

                    if (lCicloPromocional.GetItems(qryTareas).Count != 0)
                    {
                        System.Data.DataTable tempTbl = lCicloPromocional.GetItems(qryTareas).GetDataTable();
                        PiezasGridView.DataSource = tempTbl;
                        PiezasGridView.PageIndex = 1;
                        PiezasGridView.PageSize = 20;
                        PiezasGridView.DataBind();
                        PiezasGridView.Font.Size = 1;

                        pnlListaPiezas.Visible = true;
                        btnExportar.Enabled = true;

                    }
                    else
                    {
                        pnlListaPiezas.Visible = false;
                        btnExportar.Enabled = false;
                    }
                    

                }
            }
        }

        public void vCargarPiezasEnviadas(Int32 idCicloPromocional, Int32 idProveedor)
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lCicloPromocional = web.Lists["Piezas"];
                    SPQuery qryTareas = new SPQuery();
                    String strQuery = "";
                    qryTareas = new SPQuery(lCicloPromocional.Views["Todos los elementos"]);
                    String sOrden = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='{1}' /></OrderBy>", "ID", "False");
                    strQuery = "<And><And><Eq><FieldRef Name='Ciclo' LookupId='TRUE' /><Value Type='Lookup'>" + idCicloPromocional.ToString() + "</Value></Eq><Eq><FieldRef Name='Proveedor' LookupId='TRUE' /><Value Type='Lookup'>" + idProveedor.ToString() + "</Value></Eq></And><Eq><FieldRef Name='Enviado_x0020_Proveedor' /><Value Type='Choice'>1</Value></Eq></And>";


                    if (!string.IsNullOrEmpty(strQuery))
                    {
                        strQuery = "<Where>" + strQuery + "</Where>";
                    }
                    if (!string.IsNullOrEmpty(sOrden))
                    {
                        strQuery = strQuery + sOrden;
                    }

                    qryTareas.Query = strQuery;
                    qryTareas.RowLimit = 500;

                    if (lCicloPromocional.GetItems(qryTareas).Count != 0)
                    {
                        System.Data.DataTable tempTbl = lCicloPromocional.GetItems(qryTareas).GetDataTable();
                        PiezasEnviadasView.DataSource = tempTbl;
                        PiezasEnviadasView.PageIndex = 1;
                        PiezasEnviadasView.PageSize = 20;
                        PiezasEnviadasView.DataBind();
                        PiezasEnviadasView.Font.Size = 1;
                        pnlListaPiezasEnviadas.Visible = true;

                    }
                    else
                    {
                        pnlListaPiezasEnviadas.Visible = false;
                    }


                }
            }
        }

        protected void ArmarComboProveedor(Int32 idCicloPromocional)
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lCicloPromocional = web.Lists["Piezas"];
                    SPQuery qryTareas = new SPQuery();
                    String strQuery = "";
                    qryTareas = new SPQuery(lCicloPromocional.Views["Todos los elementos"]);
                    String sOrden = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='{1}' /></OrderBy>", "ID", "False");
                    strQuery = "<Eq><FieldRef Name='Ciclo' LookupId='TRUE' /><Value Type='Lookup'>" + idCicloPromocional.ToString() + "</Value></Eq>";


                    if (!string.IsNullOrEmpty(strQuery))
                    {
                        strQuery = "<Where>" + strQuery + "</Where>";
                    }
                    if (!string.IsNullOrEmpty(sOrden))
                    {
                        strQuery = strQuery + sOrden;
                    }

                    qryTareas.Query = strQuery;
                    qryTareas.RowLimit = 500;

                    if (lCicloPromocional.GetItems(qryTareas).Count != 0)
                    {
                        SPListItemCollection sPListItemCollection = lCicloPromocional.GetItems(qryTareas);
                        foreach (SPListItem itmPieza in sPListItemCollection)
                        {
                            if (itmPieza["Proveedor"] != null)
                            {

                                SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmPieza["Proveedor"] as String);
                                String idProveedor = lkvTipoPieza.LookupId.ToString();

                                Boolean bExiste = false;

                                foreach (ListItem listItem in ddlProveedor.Items)
                                {
                                    if (listItem.Value == idProveedor) bExiste = true;
                                }

                                if (bExiste == false) { 
                                ListItem itmProveedor = new ListItem();
                                itmProveedor.Value = lkvTipoPieza.LookupId.ToString();
                                itmProveedor.Text = lkvTipoPieza.LookupValue.ToString();
                                ddlProveedor.Items.Add(itmProveedor);
                                }
                            }
                        }



                    }

                }
            }
        }

        protected string RemoveCharacters(object String)
        {
            string s1 = String.ToString();
            string newString = Regex.Replace(s1, @"#[\d]\d+([,;\s]+\d+)*;", " ");
            newString = Regex.Replace(newString, "#", " ");
            return newString.ToString();
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalPiezas.aspx?ID=" + iCicloPromocional.Value.ToString() + "&Origen=C");
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            Boolean bProcesado = true;
            Int32 iProceso = 0;

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lCicloPromocional = web.Lists["Procesar Proveedor"];

                    String strPiezas = "";

                    SPListItem itmCicloPromocional = lCicloPromocional.AddItem();
                    SPFieldLookupValueCollection values = new SPFieldLookupValueCollection();
                    foreach (GridViewRow gwPieza in PiezasGridView.Rows )
                    {
                        CheckBox cb = (CheckBox)gwPieza.Cells[9].Controls[1]; // (CheckBox)gwPieza.FindControl("ProductSelector");
                        if (cb.Checked) {
                            strPiezas = strPiezas + gwPieza.Cells[0].Text.ToString() + ";";
                            SPFieldLookupValue sPFieldLookupValue = new SPFieldLookupValue();
                            sPFieldLookupValue.LookupId = Convert.ToInt32(gwPieza.Cells[0].Text.ToString());
                            values.Add(sPFieldLookupValue);
                        }

                    }

                    
                    itmCicloPromocional["Title"] = iCicloPromocional.Value.ToString();
                    itmCicloPromocional["Proveedor"] = ddlProveedor.SelectedValue;
                    itmCicloPromocional["Piezas"] = values;
                    itmCicloPromocional["Ciclo"] = iCicloPromocional.Value.ToString();
                    itmCicloPromocional.Update();

                    iProceso = itmCicloPromocional.ID;



                }
            }

            if (bProcesado == true)
            {

                Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/ConfirmarEnvioProveedor.aspx?ID=" + iProceso.ToString() + "&IDCiclo=" + iCicloPromocional.Value.ToString());
            }
        }

        protected void ddlProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            vCargarPiezas(Convert.ToInt32(iCicloPromocional.Value.ToString()), Convert.ToInt32(ddlProveedor.SelectedValue.ToString()));
            vCargarPiezasEnviadas(Convert.ToInt32(iCicloPromocional.Value.ToString()), Convert.ToInt32(ddlProveedor.SelectedValue.ToString()));
        }
    }
}
