using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Xml;
using System.Web.UI.WebControls;

namespace CiclosPromocionales.Layouts.CiclosPromocionales
{
    public partial class TipoMaterialGuardar : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 idCicloPromocional = 0;
            Int32 idCicloPromocionalPieza = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            idCicloPromocionalPieza = Convert.ToInt32(Request["IDPieza"]);
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    Int32 iTipoPieza = 0;
                    SPList lCicloPromocionalPieza = web.Lists["Piezas"];

                    SPListItem itmCicloPromocionalPieza = lCicloPromocionalPieza.GetItemById(idCicloPromocionalPieza);
                    if (itmCicloPromocionalPieza != null)
                    {

                       

                        SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmCicloPromocionalPieza["Tipo Pieza"] as String);
                        iTipoPieza = lkvTipoPieza.LookupId;

                        vArmarPanelDetalle(iTipoPieza, true);
                        if (itmCicloPromocionalPieza["Detalle"] != null)
                        {
                            vCargarDetalle(itmCicloPromocionalPieza["Detalle"].ToString(), 1);
                        }


                    }
                }
            }
        }


        public void vArmarPanelDetalle(Int32 iTipoPieza, Boolean bValorDefault)
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
                        SPListItemCollection lColConfigTipoPieza = lConfigTipoPieza.GetItems(qryTareas);
                        foreach (SPListItem itmConfig in lColConfigTipoPieza)
                        {
                            tblRowFila1 = new TableRow();
                            iFila = iFila + 1;
                            if (iFila % 2 == 0)
                            {
                                tblRowFila1.BackColor = System.Drawing.Color.FromName("#8FBC8B");
                            }
                            else
                            {
                                tblRowFila1.BackColor = System.Drawing.Color.FromName("#66CDAA");
                            }

                            // Armo el campo para la cabecera y los campos para cada etapa
                            tblCellCabeceraCampo = new TableCell();
                            tblCellCabeceraCampo.Text = itmConfig.Title.ToString();
                            tblCellCabeceraCampo.ID = "ID_" + itmConfig.ID.ToString();
                            tblCellDetalle = new TableCell();


                            tblRowFila1.Cells.Add(tblCellCabeceraCampo);
                            tblRowFila1.Cells.Add(tblCellDetalle);

                            if (bValorDefault == true)
                            {
                                TextBox txtNotas = new TextBox();
                                txtNotas.ID = "Valor_D_" + itmConfig.ID.ToString();
                                txtNotas.Width = 350;
                                txtNotas.Text = itmConfig["Valor Default"].ToString();
                                tblRowFila1.Cells[1].Controls.Add(txtNotas);
                            }

                            tblCheckList.Rows.Add(tblRowFila1);
                        }

                    }
                }
            }

        }

        public void vCargarDetalle(String strAuxXML, Int32 iPosicion)
        {
            int i = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strAuxXML);
            foreach (TableRow tblRowTarea in tblCheckList.Rows)
            {
                if (i != 0)
                {
                    String idElemento = tblRowTarea.Cells[0].ID.ToString().Split('_')[1].ToString();
                    XmlNode xnList = xmlDoc.SelectSingleNode("/DetalleMaterial/Detalle[@ID='" + idElemento.ToString() + "']");
                    TextBox txtDetalle = tblRowTarea.Cells[iPosicion].Controls[0] as TextBox;
                    txtDetalle.Text = xnList.InnerText.ToString();
                }
                i = i + 1;
            }

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Int32 itmTipoMaterial = 0;

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lTipoPieza = web.Lists["Tipo Pieza"];

                    SPListItem itmTipoPieza = lTipoPieza.AddItem();
                    itmTipoPieza["Title"] = txtTitulo.Text;
                    itmTipoPieza.Update();

                    itmTipoMaterial = itmTipoPieza.ID;

                    System.Threading.Thread.Sleep(5000);
                    Int32 i = 0;
                    foreach (TableRow tblRowTarea in tblCheckList.Rows)
                    {

                        if (i != 0)
                        {
                            SPList lConfigTipoPieza = web.Lists["Configuración Tipo Pieza"];

                            SPListItem itmConfigTipoPieza = lConfigTipoPieza.AddItem();
                            itmConfigTipoPieza["Title"] = tblRowTarea.Cells[0].Text.ToString();
                            itmConfigTipoPieza["Tipo Pieza"] = itmTipoMaterial;
                            TextBox txtDetalle = tblRowTarea.Cells[1].Controls[0] as TextBox;
                            itmConfigTipoPieza["Valor Default"] = txtDetalle.Text.ToString();
                            itmConfigTipoPieza.Update();
                        }
                        i = i + 1;
                    }



                }
            }


            


            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            XmlElement element1 = doc.CreateElement(string.Empty, "DetalleMaterial", string.Empty);
            doc.AppendChild(element1);

            


        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Int32 idCicloPromocional = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalPiezas.aspx?ID=" + idCicloPromocional + "&Origen=E");
        }
    }
}
