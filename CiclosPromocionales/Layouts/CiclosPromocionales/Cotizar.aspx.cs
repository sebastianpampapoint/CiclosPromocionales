using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using System.IO;
using System.Web.UI;
using OfficeOpenXml;
using System.IO;

namespace CiclosPromocionales.Layouts.CiclosPromocionales
{
    public partial class Cotizar : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 idCicloPromocional = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            iCicloPromocional.Value = idCicloPromocional.ToString();
            vCargarDatos(idCicloPromocional);
            vCargarPiezas(idCicloPromocional);
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

                    SPList lMensajes = web.Lists["Configuración Mensajes"];
                    SPListItemCollection sPListItemCollection = lMensajes.GetItems();
                    foreach (SPListItem sPListItem in sPListItemCollection)
                    {
                        if (sPListItem["Tipo mensaje"].ToString() == "Alta Cotización")
                            txtMensaje.Text = sPListItem["Mensaje"].ToString();
                    }
                }
            }
        }

        public void vCargarPiezas(Int32 idCicloPromocional)
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
                        SPListItemCollection itemColl = null;
                        itemColl = lCicloPromocional.GetItems(qryTareas);
                        foreach (SPListItem itmTarea in itemColl)
                        {
                            vCargarPiezasDetalle(itmTarea.ID);
                        }

                    }
                }
            }
        }

        public void vCargarPiezasDetalle(Int32 idCicloPromocionalPieza)
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    Int32 iTipoPieza = 0;
                    SPList lCicloPromocionalPieza = web.Lists["Piezas"];

                    SPListItem itmCicloPromocionalPieza = lCicloPromocionalPieza.GetItemById(idCicloPromocionalPieza);
                    if (itmCicloPromocionalPieza != null)
                    {
                        vAgregarFilaBlanco();
                        vAgregarFilaBlanco();
                        //txtPieza.Text = itmCicloPromocionalPieza["Title"].ToString();
                        //txtCantidad.Text = itmCicloPromocionalPieza["Cantidad"].ToString();
                        //txtDepartamentoMedico.Text = itmCicloPromocionalPieza["DM"].ToString();

                        String txtProducto = "";

                        SPFieldLookupValueCollection strProducto = itmCicloPromocionalPieza["Producto"] as SPFieldLookupValueCollection;
                        foreach (SPFieldLookupValue iProducto in strProducto)
                        {

                            if (txtProducto == "")
                            {
                                txtProducto = iProducto.LookupValue.ToString();
                            }
                            else
                            {
                                txtProducto = txtProducto + "; " + iProducto.LookupValue.ToString();
                            }
                        }



                        SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmCicloPromocionalPieza["Tipo Pieza"] as String);

                        //txtTipoMaterial.Text = lkvTipoPieza.LookupValue.ToString();
                        iTipoPieza = lkvTipoPieza.LookupId;

                        TableRow tblRowFila0 = new TableRow();
                        TableCell tblCellColumna0 = new TableCell();
                        tblCellColumna0.Text = "Producto:";
                        tblCellColumna0.Font.Bold = true;
                        TableCell tblCellColumna01 = new TableCell();
                        tblCellColumna01.Text = txtProducto;
                        tblCellColumna01.ColumnSpan = 3;
                        tblRowFila0.Cells.Add(tblCellColumna0);
                        tblRowFila0.Cells.Add(tblCellColumna01);
                        tblMateriales.Rows.Add(tblRowFila0);


                        TableRow tblRowFila1 = new TableRow();
                        TableCell tblCellColumna1 = new TableCell();
                        tblCellColumna1.Text = "Material";
                        tblCellColumna1.Font.Bold = true;
                        TableCell tblCellColumna2 = new TableCell();
                        tblCellColumna2.Text = lkvTipoPieza.LookupValue.ToString();
                        TableCell tblCellColumna3 = new TableCell();
                        tblCellColumna3.Text = "Cantidad";
                        tblCellColumna3.Font.Bold = true;
                        TableCell tblCellColumna4 = new TableCell();
                        tblCellColumna4.Text = itmCicloPromocionalPieza["Cantidad"].ToString(); 
                        tblRowFila1 = new TableRow();
                        tblRowFila1.Cells.Add(tblCellColumna1);
                        tblRowFila1.Cells.Add(tblCellColumna2);
                        tblRowFila1.Cells.Add(tblCellColumna3);
                        tblRowFila1.Cells.Add(tblCellColumna4);

                        tblMateriales.Rows.Add(tblRowFila1);



                        if (itmCicloPromocionalPieza["Detalle"] != null)
                        {

                            String strAlternativo1 = "-";
                            String strAlternativo2 = "-";

                            if (itmCicloPromocionalPieza["Opción 2"] != null)
                            {
                                strAlternativo1 = itmCicloPromocionalPieza["Opción 2"].ToString();
                            }

                            if (itmCicloPromocionalPieza["Opción 3"] != null)
                            {
                                strAlternativo2 = itmCicloPromocionalPieza["Opción 3"].ToString();
                            }

                            //vArmarPanelDetalle(iTipoPieza, itmCicloPromocionalPieza["Detalle"].ToString(), itmCicloPromocionalPieza["Opción 2"].ToString(), itmCicloPromocionalPieza["Opción 3"].ToString());
                            vArmarPanelDetalle(iTipoPieza, itmCicloPromocionalPieza["Detalle"].ToString(), strAlternativo1, strAlternativo2);
                        }
                        else
                        {
                            vArmarPanelDetalle(iTipoPieza, "-", "-", "-");
                            btnExportar.Enabled = false;
                            lblMensajeError.Text = "No se puede envir a cotizar porque hay materiales pendientes de configurar.";
                        }
                    }
                }
            }


        }

        public void vArmarPanelDetalle(Int32 iTipoPieza, String strAuxXML, String strAuxXMLAlternativo1, String strAuxXMLAlternativo2)
        {
            Int32 iFila = 1;

            TableRow tblRowFila1 = new TableRow();
            TableCell tblCellCabeceraCampo = new TableCell();
            TableCell tblCellDetalle = new TableCell();
            TableCell tblCellAlternativa1 = new TableCell();
            TableCell tblCellAlternativa2 = new TableCell();
            tblRowFila1 = new TableRow();

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lConfigTipoPieza = web.Lists["Configuración Tipo Pieza"];
                    SPQuery qryTareas = new SPQuery();
                    String strQuery = "";
                    qryTareas = new SPQuery(lConfigTipoPieza.Views["Todos los elementos"]);
                    String sOrden = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='{1}' /></OrderBy>", "ID", "True");
                    strQuery = "<Eq><FieldRef Name='Tipo_x0020_Pieza' LookupId='TRUE' /><Value Type='Lookup'>" + iTipoPieza.ToString() + "</Value></Eq>";


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

                    if (lConfigTipoPieza.GetItems(qryTareas).Count != 0)
                    {

                        tblRowFila1 = new TableRow();
                        tblCellCabeceraCampo = new TableCell();
                        tblCellCabeceraCampo.Text = "-";
                        tblCellDetalle = new TableCell();
                        if (strAuxXML != "-") tblCellDetalle.Text = "Detalle";
                        tblCellAlternativa1 = new TableCell();
                        if (strAuxXMLAlternativo1 != "-")
                        {
                            tblCellAlternativa1.Text = "Alternativo 1";
                        } else
                        {
                            tblCellAlternativa1.Text = "-";
                        }
                        
                        tblCellAlternativa2 = new TableCell();
                        if (strAuxXMLAlternativo2 != "-")
                        {
                            tblCellAlternativa2.Text = "Alternativo 2";
                        } else
                        {
                            tblCellAlternativa2.Text = "-";
                        }
                        

                        tblRowFila1.Cells.Add(tblCellCabeceraCampo);
                        tblRowFila1.Cells.Add(tblCellDetalle);
                        tblRowFila1.Cells.Add(tblCellAlternativa1);
                        tblRowFila1.Cells.Add(tblCellAlternativa2);

                        tblMateriales.Rows.Add(tblRowFila1);


                        SPListItemCollection lColConfigTipoPieza = lConfigTipoPieza.GetItems(qryTareas);
                        foreach (SPListItem itmConfig in lColConfigTipoPieza)
                        {
                            tblRowFila1 = new TableRow();
                            tblCellCabeceraCampo = new TableCell();
                            tblCellCabeceraCampo.Text = itmConfig.Title.ToString();

                            String txtValorDefault = itmConfig["Valor Default"].ToString();

                            tblCellDetalle = new TableCell();
                            if (strAuxXML != "-") tblCellDetalle.Text = vCargarDetalle(strAuxXML, itmConfig.ID);
                            tblCellAlternativa1 = new TableCell();
                            if (strAuxXMLAlternativo1 != "-") { 
                                tblCellAlternativa1.Text = vCargarDetalle(strAuxXMLAlternativo1, itmConfig.ID);
                            } else
                            {
                                tblCellAlternativa1.Text = "-";
                            }
                            tblCellAlternativa2 = new TableCell();
                            if (strAuxXMLAlternativo2 != "-") {
                                tblCellAlternativa2.Text = vCargarDetalle(strAuxXMLAlternativo2, itmConfig.ID);
                            }
                            else
                            {
                                tblCellAlternativa2.Text = "-";
                            }
                            tblCellCabeceraCampo.BorderStyle = BorderStyle.Solid;
                            tblCellCabeceraCampo.BorderWidth = 1;
                            tblCellCabeceraCampo.BorderColor = System.Drawing.Color.Black;

                            tblCellDetalle.BorderStyle = BorderStyle.Solid;
                            tblCellDetalle.BorderWidth = 1;
                            tblCellDetalle.BorderColor = System.Drawing.Color.Black;

                            tblCellAlternativa1.BorderStyle = BorderStyle.Solid;
                            tblCellAlternativa1.BorderWidth = 1;
                            tblCellAlternativa1.BorderColor = System.Drawing.Color.Black;

                            tblCellAlternativa2.BorderStyle = BorderStyle.Solid;
                            tblCellAlternativa2.BorderWidth = 1;
                            tblCellAlternativa2.BorderColor = System.Drawing.Color.Black;

                            tblRowFila1.Cells.Add(tblCellCabeceraCampo);
                            tblRowFila1.Cells.Add(tblCellDetalle);
                            tblRowFila1.Cells.Add(tblCellAlternativa1);
                            tblRowFila1.Cells.Add(tblCellAlternativa2);

                            tblMateriales.Rows.Add(tblRowFila1);
                        }

                    }
                }
            }

        }

        public string vCargarDetalle(String strAuxXML, Int32 idElemento)
        {
            String strAux = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strAuxXML);
            XmlNode xnList = xmlDoc.SelectSingleNode("/DetalleMaterial/Detalle[@ID='" + idElemento.ToString() + "']");
            strAux = xnList.InnerText.ToString();

            return strAux;
        }

        public void vAgregarFilaBlanco()
        {
            TableRow tblRowFila1 = new TableRow();
            TableCell tblCellColumna1 = new TableCell();
            tblCellColumna1.Text = "";
            TableCell tblCellColumna2 = new TableCell();
            tblCellColumna2.Text = "";
            TableCell tblCellColumna3 = new TableCell();
            tblCellColumna3.Text = "";
            TableCell tblCellColumna4 = new TableCell();
            tblCellColumna4.Text = "";
            tblRowFila1 = new TableRow();
            tblRowFila1.Cells.Add(tblCellColumna1);
            tblRowFila1.Cells.Add(tblCellColumna2);
            tblRowFila1.Cells.Add(tblCellColumna3);
            tblRowFila1.Cells.Add(tblCellColumna4);

            tblMateriales.Rows.Add(tblRowFila1);
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

                    SPListItem itmCicloPromocional = lCicloPromocional.AddItem();
                    itmCicloPromocional["Title"] = iCicloPromocional.Value.ToString();
                    itmCicloPromocional["Acción"] = "Alta Cotización";
                    itmCicloPromocional["Identificador"] = iCicloPromocional.Value.ToString();
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
