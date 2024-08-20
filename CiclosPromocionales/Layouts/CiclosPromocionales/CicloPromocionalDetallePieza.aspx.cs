using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace CiclosPromocionales.Layouts.CiclosPromocionales
{

    public partial class CicloPromocionalDetallePieza : LayoutsPageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    Int32 idCicloPromocional = 0;
                    Int32 idCicloPromocionalPieza = 0;
                    idCicloPromocional = Convert.ToInt32(Request["ID"]);
                    idCicloPromocionalPieza = Convert.ToInt32(Request["IDPieza"]);
                    Int32 iTipoPieza = 0;
                    SPList lCicloPromocionalPieza = web.Lists["Piezas"];
                    SPListItem itmCicloPromocionalPieza = lCicloPromocionalPieza.GetItemById(idCicloPromocionalPieza);
                    SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmCicloPromocionalPieza["Tipo Pieza"] as String);
                    txtTipoMaterial.Text = lkvTipoPieza.LookupValue.ToString();
                    iTipoPieza = lkvTipoPieza.LookupId;
                    vArmarPanelDetalle(iTipoPieza, true);
                }
            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //if (Page.IsPostBack != true)
            //{
                Int32 idCicloPromocional = 0;
                Int32 idCicloPromocionalPieza = 0;
                idCicloPromocional = Convert.ToInt32(Request["ID"]);
                idCicloPromocionalPieza = Convert.ToInt32(Request["IDPieza"]);
                iCicloPromocional.Value = idCicloPromocional.ToString();
                iMaterial.Value = idCicloPromocionalPieza.ToString();
                vCargarComboProveedor();
                vCargarDatos(idCicloPromocional);
                vCargarPiezas(idCicloPromocionalPieza);
            //}
        }

        public void vCargarComboProveedor()
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lConfigTipoPieza = web.Lists["Proveedores"];
                    SPQuery qryTareas = new SPQuery();
                    String strQuery = "";
                    qryTareas = new SPQuery(lConfigTipoPieza.Views["Todos los elementos"]);
                    String sOrden = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='{1}' /></OrderBy>", "Title", "True");
                    //strQuery = "<Eq><FieldRef Name='Tipo_x0020_Pieza' LookupId='TRUE' /><Value Type='Lookup'>" + iTipoPieza.ToString() + "</Value></Eq>";


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
                            ListItem itmTipoMaterial = new ListItem();
                            itmTipoMaterial.Value = itmConfig.ID.ToString();
                            itmTipoMaterial.Text = itmConfig.Title.ToString();
                            ddlProveedor.Items.Add(itmTipoMaterial);
                        }

                    }
                }
            }
        }

        public void vCargarDatos(Int32 idCicloPromocional)
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lCicloPromocional = web.Lists["Ciclo Promocional"];

                    SPListItem itmCicloPromocional = lCicloPromocional.GetItemById(idCicloPromocional);
                    if (itmCicloPromocional != null)
                    {
                        txtTitulo.Text = itmCicloPromocional["Title"].ToString();
                    }
                }
            }


        }

        public void vCargarPiezas(Int32 idCicloPromocionalPieza)
        {

            if (Page.IsPostBack != true)
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.RootWeb)
                    {
                        Int32 iTipoPieza = 0;
                        SPList lCicloPromocionalPieza = web.Lists["Piezas"];
                        String strTituloA1 = "";
                        String strTituloA2 = "";
                        String strCantidadA1 = "";
                        String strCantidadA2 = "";

                        SPListItem itmCicloPromocionalPieza = lCicloPromocionalPieza.GetItemById(idCicloPromocionalPieza);
                        if (itmCicloPromocionalPieza != null)
                        {

                            txtPieza.Text = itmCicloPromocionalPieza["Title"].ToString();
                            txtCantidad.Text = itmCicloPromocionalPieza["Cantidad"].ToString();
                            txtDepartamentoMedico.Text = itmCicloPromocionalPieza["DM"].ToString();
                            txtEstadoDocumento.Text = itmCicloPromocionalPieza["Estado Documento"].ToString();
                            txtEstadoCotizacion.Text = itmCicloPromocionalPieza["Estado"].ToString();

                            SPFieldLookupValueCollection strProducto = itmCicloPromocionalPieza["Producto"] as SPFieldLookupValueCollection;
                            foreach (SPFieldLookupValue iProducto in strProducto)
                            {
                                llbProducto.Items.Add(iProducto.LookupValue.ToString());
                            }

                            SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmCicloPromocionalPieza["Tipo Pieza"] as String);

                            txtTipoMaterial.Text = lkvTipoPieza.LookupValue.ToString();
                            iTipoPieza = lkvTipoPieza.LookupId;
                            if (itmCicloPromocionalPieza["Opción 2 Enviar"] != null)
                            {
                                if (itmCicloPromocionalPieza["Opción 2 Enviar"].ToString() == "True") chbAlternativo1.Checked = true;
                            }
                            if (itmCicloPromocionalPieza["Opción 3 Enviar"] != null)
                            {
                                if (itmCicloPromocionalPieza["Opción 3 Enviar"].ToString() == "True") chbAlternativo2.Checked = true;
                            }

                            if (itmCicloPromocionalPieza["Opción 2 Titulo"] != null)
                            {
                                strTituloA1 = itmCicloPromocionalPieza["Opción 2 Titulo"].ToString();
                            }
                            if (itmCicloPromocionalPieza["Opción 2 Cantidad"] != null)
                            {
                                strCantidadA1 = itmCicloPromocionalPieza["Opción 2 Cantidad"].ToString();
                            }
                            if (itmCicloPromocionalPieza["Opción 3 Titulo"] != null)
                            {
                                strTituloA2 = itmCicloPromocionalPieza["Opción 3 Titulo"].ToString();
                            }
                            if (itmCicloPromocionalPieza["Opción 3 Cantidad"] != null)
                            {
                                strCantidadA2 = itmCicloPromocionalPieza["Opción 3 Cantidad"].ToString();
                            }
                            //}
                            

                            if (itmCicloPromocionalPieza["Detalle"] != null)
                            {
                                // vArmarPanelDetalle(iTipoPieza, true);
                                vCargarDetalle(itmCicloPromocionalPieza["Detalle"].ToString(), 1, itmCicloPromocionalPieza["Title"].ToString(), itmCicloPromocionalPieza["Cantidad"].ToString());
                            }
                            else
                            {
                                vCargarDetalle("Default", 1, itmCicloPromocionalPieza["Title"].ToString(), itmCicloPromocionalPieza["Cantidad"].ToString());
                            }
                            if (chbAlternativo1.Checked)
                            {


                                if (itmCicloPromocionalPieza["Opción 2"] != null)
                                {
                                    vCargarDetalle(itmCicloPromocionalPieza["Opción 2"].ToString(), 2, strTituloA1, strCantidadA1);
                                }
                                else
                                {
                                    vCargarDetalle("Default", 2, strTituloA1, strCantidadA1);
                                }
                            }
                            else
                            {
                                vCargarDetalle("", 2, "", "");
                            }


                            if (chbAlternativo2.Checked)
                            {


                                if (itmCicloPromocionalPieza["Opción 3"] != null)
                                {
                                    vCargarDetalle(itmCicloPromocionalPieza["Opción 3"].ToString(), 3, strTituloA2, strCantidadA2);
                                }
                                else
                                {
                                    vCargarDetalle("Default", 3, strTituloA2, strCantidadA2);
                                }
                            }
                            else
                            {
                                vCargarDetalle("", 3, "", "");
                            }

                            listarAdjuntos("Piezas", Convert.ToInt32(iMaterial.Value));

                            if (itmCicloPromocionalPieza["Estado"] != null)
                            {
                                ddlResultado.SelectedValue = txtEstadoCotizacion.Text;

                            }

                            if (itmCicloPromocionalPieza["Resultado"] != null)
                            {
                                ddlOpcionSeleccionada.SelectedValue = itmCicloPromocionalPieza["Resultado"].ToString();
                                

                            }
                            if (itmCicloPromocionalPieza["Proveedor"] != null)
                            {
                                SPFieldLookupValue lkvProveedor = new SPFieldLookupValue(itmCicloPromocionalPieza["Proveedor"] as String);
                                String idProveedor = lkvProveedor.LookupId.ToString();
                                ddlProveedor.SelectedValue = idProveedor;
                            }

                            if (itmCicloPromocionalPieza["Enviado Proveedor"].ToString() == "1")
                            {                              
                                ddlResultado.Enabled = false;
                                ddlOpcionSeleccionada.Enabled = false;
                                ddlProveedor.Enabled = false;
                                
                            }
                            if (txtEstadoCotizacion.Text != "Pendiente")
                            {
                                chbAlternativo1.Enabled = false;
                                chbAlternativo2.Enabled = false;

                            }

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

            iFila = iFila + 1;
            if (iFila % 2 == 0)
            {
                tblRowFila1.BackColor = System.Drawing.Color.FromName("#8FBC8B");
            }
            else
            {
                tblRowFila1.BackColor = System.Drawing.Color.FromName("#66CDAA");
            }

            tblCellCabeceraCampo = new TableCell();
            tblCellCabeceraCampo.Text = "Material";
            tblCellCabeceraCampo.ID = "ID_Cabecera";
            tblCellDetalle = new TableCell();
            tblCellAlternativa1 = new TableCell();
            tblCellAlternativa2 = new TableCell();

            tblRowFila1.Cells.Add(tblCellCabeceraCampo);
            tblRowFila1.Cells.Add(tblCellDetalle);
            tblRowFila1.Cells.Add(tblCellAlternativa1);
            tblRowFila1.Cells.Add(tblCellAlternativa2);

            TextBox txtNotasCabecera = new TextBox();
            txtNotasCabecera.ID = "Valor_D_Titulo" ;
            txtNotasCabecera.Width = 350;
            txtNotasCabecera.Text = "";
            tblRowFila1.Cells[1].Controls.Add(txtNotasCabecera);

            txtNotasCabecera = new TextBox();
            txtNotasCabecera.ID = "Valor_A1_Titulo";
            txtNotasCabecera.Width = 350;
            txtNotasCabecera.Text = "";
            tblRowFila1.Cells[2].Controls.Add(txtNotasCabecera);

            txtNotasCabecera = new TextBox();
            txtNotasCabecera.ID = "Valor_A2_Titulo";
            txtNotasCabecera.Width = 350;
            txtNotasCabecera.Text = "";
            tblRowFila1.Cells[3].Controls.Add(txtNotasCabecera);


            tblCheckList.Rows.Add(tblRowFila1);

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

            tblCellCabeceraCampo = new TableCell();
            tblCellCabeceraCampo.Text = "Cantidad";
            tblCellCabeceraCampo.ID = "ID_CabeceraCantidad";
            tblCellDetalle = new TableCell();
            tblCellAlternativa1 = new TableCell();
            tblCellAlternativa2 = new TableCell();

            tblRowFila1.Cells.Add(tblCellCabeceraCampo);
            tblRowFila1.Cells.Add(tblCellDetalle);
            tblRowFila1.Cells.Add(tblCellAlternativa1);
            tblRowFila1.Cells.Add(tblCellAlternativa2);

            txtNotasCabecera = new TextBox();
            txtNotasCabecera.ID = "Valor_D_Cantidad";
            txtNotasCabecera.Width = 350;
            txtNotasCabecera.Text = "";
            tblRowFila1.Cells[1].Controls.Add(txtNotasCabecera);

            txtNotasCabecera = new TextBox();
            txtNotasCabecera.ID = "Valor_A1_Cantidad";
            txtNotasCabecera.Width = 350;
            txtNotasCabecera.Text = "";
            tblRowFila1.Cells[2].Controls.Add(txtNotasCabecera);

            txtNotasCabecera = new TextBox();
            txtNotasCabecera.ID = "Valor_A2_Cantidad";
            txtNotasCabecera.Width = 350;
            txtNotasCabecera.Text = "";
            tblRowFila1.Cells[3].Controls.Add(txtNotasCabecera);

            tblCheckList.Rows.Add(tblRowFila1);

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
                        foreach(SPListItem itmConfig in lColConfigTipoPieza)
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
                            tblCellAlternativa1 = new TableCell();
                            tblCellAlternativa2 = new TableCell();
                            
                            tblRowFila1.Cells.Add(tblCellCabeceraCampo);
                            tblRowFila1.Cells.Add(tblCellDetalle);
                            tblRowFila1.Cells.Add(tblCellAlternativa1);
                            tblRowFila1.Cells.Add(tblCellAlternativa2);

                            if (bValorDefault == true)
                            {
                                TextBox txtNotas = new TextBox();
                                txtNotas.ID = "Valor_D_" + itmConfig.ID.ToString();
                                txtNotas.Width = 350;
                                txtNotas.Text = itmConfig["Valor Default"].ToString();
                                tblRowFila1.Cells[1].Controls.Add(txtNotas);

                                txtNotas = new TextBox();
                                txtNotas.ID = "Valor_A1_" + itmConfig.ID.ToString();
                                txtNotas.Width = 350;
                                txtNotas.Text = itmConfig["Valor Default"].ToString();
                                tblRowFila1.Cells[2].Controls.Add(txtNotas);

                                txtNotas = new TextBox();
                                txtNotas.ID = "Valor_A2_" + itmConfig.ID.ToString();
                                txtNotas.Width = 350;
                                txtNotas.Text = itmConfig["Valor Default"].ToString();
                                tblRowFila1.Cells[3].Controls.Add(txtNotas);



                            }

                            tblCheckList.Rows.Add(tblRowFila1);
                        }

                    }
                }
            }
            
        }

        public void vCargarDetalle(String strAuxXML, Int32 iPosicion, string Titulo, string Cantidad)
        {
            int i = 0;
            XmlDocument xmlDoc = new XmlDocument();
            if (strAuxXML != "" && strAuxXML != "Default")
            {
                xmlDoc.LoadXml(strAuxXML);
            }
            foreach (TableRow tblRowTarea in tblCheckList.Rows)
            {
                if (i != 0)
                {
                    if (i == 1)
                    {
                        TextBox txtDetalle = tblRowTarea.Cells[iPosicion].Controls[0] as TextBox;
                        txtDetalle.Text = Titulo;
                    } 

                    if (i == 2)
                    {
                        TextBox txtDetalle = tblRowTarea.Cells[iPosicion].Controls[0] as TextBox;
                        txtDetalle.Text = Cantidad;
                    }

                    if (i > 2)
                    {
                        if (strAuxXML != "" && strAuxXML != "Default") { 
                            
                            String idElemento = tblRowTarea.Cells[0].ID.ToString().Split('_')[1].ToString();
                            XmlNode xnList = xmlDoc.SelectSingleNode("/DetalleMaterial/Detalle[@ID='" + idElemento.ToString() + "']");
                            TextBox txtDetalle = tblRowTarea.Cells[iPosicion].Controls[0] as TextBox;
                            txtDetalle.Text = xnList.InnerText.ToString();
                            txtDetalle.EnableViewState = true;
                            txtDetalle.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        }
                        else
                        {
                            if (strAuxXML != "Default") { 
                            TextBox txtDetalle = tblRowTarea.Cells[iPosicion].Controls[0] as TextBox;
                            txtDetalle.Text = "";
                                txtDetalle.EnableViewState = true;
                                txtDetalle.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

                            }
                        }

                    }

                    
                }
                i = i + 1;
            }
            
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            Int32 idMaterial = Convert.ToInt32(Request["IDPieza"]);
            SPList lstList;
            lstList = SPContext.Current.Web.Lists["Piezas"];

            SPListItem addListMaterial;

            addListMaterial = lstList.GetItemById(idMaterial);


            addListMaterial["Detalle"] = vArmarXML(1);
            if (chbAlternativo1.Checked == true)
            {
                addListMaterial["Opción 2 Enviar"] = "True";
                TextBox txtTitulo = tblCheckList.Rows[1].Cells[2].Controls[0] as TextBox;
                addListMaterial["Opción 2 Titulo"] = txtTitulo.Text.ToString();
                TextBox txtCantidad = tblCheckList.Rows[2].Cells[2].Controls[0] as TextBox;
                addListMaterial["Opción 2 Cantidad"] = txtCantidad.Text.ToString();
                addListMaterial["Opción 2"] = vArmarXML(2);
            } else
            {
                addListMaterial["Opción 2 Enviar"] = "False";
                addListMaterial["Opción 2 Titulo"] = "";
                addListMaterial["Opción 2 Cantidad"] = "";
                addListMaterial["Opción 2"] = "";
            }
            if (chbAlternativo2.Checked == true) {
                addListMaterial["Opción 3 Enviar"] = "True";
                TextBox txtTitulo = tblCheckList.Rows[1].Cells[3].Controls[0] as TextBox;
                addListMaterial["Opción 3 Titulo"] = txtTitulo.Text.ToString();
                TextBox txtCantidad = tblCheckList.Rows[2].Cells[3].Controls[0] as TextBox;
                addListMaterial["Opción 3 Cantidad"] = txtCantidad.Text.ToString();
                addListMaterial["Opción 3"] = vArmarXML(3);
            } else
            {
                addListMaterial["Opción 3 Enviar"] = "False";
                addListMaterial["Opción 3 Titulo"] = "";
                addListMaterial["Opción 3 Cantidad"] = "";
                addListMaterial["Opción 3"] = "";
            }
            

            if (ddlResultado.SelectedValue != "0") {
                addListMaterial["Estado"] = ddlResultado.SelectedValue;
                
            }

            if (ddlOpcionSeleccionada.SelectedValue != "0")
            {
                addListMaterial["Resultado"] = ddlOpcionSeleccionada.SelectedValue;
            }

            if (ddlProveedor.SelectedValue != "0")
            {
                addListMaterial["Proveedor"] = ddlProveedor.SelectedValue;
            }

            if (ddlResultado.SelectedValue == "Rechazado")
            {
                addListMaterial["Proveedor"] = "";
                addListMaterial["Resultado"] = "";
            }

            addListMaterial.Update();


            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/Procesando.aspx?ID=" + Request["ID"] + "&IDPieza=" + Request["IDPieza"] + "&Pg=DP");


        }

        protected String vArmarXML(Int32 iPosicion)
        {
            String AuxXML = "";
            int i = 0;


            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            XmlElement element1 = doc.CreateElement(string.Empty, "DetalleMaterial", string.Empty);
            doc.AppendChild(element1);

            foreach (TableRow tblRowTarea in tblCheckList.Rows)
            {

                if (i >= 3)
                {
                    XmlElement element2 = doc.CreateElement(string.Empty, "Detalle", string.Empty);
                    element2.SetAttribute("ID", tblRowTarea.Cells[0].ID.ToString().Split('_')[1].ToString());
                    TextBox txtDetalle = tblRowTarea.Cells[iPosicion].Controls[0] as TextBox;
                    XmlText txtValorDetalle = doc.CreateTextNode(txtDetalle.Text.ToString());
                    element2.AppendChild(txtValorDetalle);
                    element1.AppendChild(element2);
                }
                i = i + 1;
            }
            
            AuxXML = doc.OuterXml.ToString();

            return AuxXML;
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Int32 idCicloPromocional = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalPiezas.aspx?ID=" + idCicloPromocional + "&Origen=E");
        }

        protected void listarAdjuntos(String strLista, Int32 idElemento)
        {
            //if (!Page.IsPostBack)
            //{
            try
            {

                //cellAdjuntosAvisoPago.Visible = true;
                gridAdjuntoMaterial.DataSource = getAttachmentsData(strLista, idElemento);
                gridAdjuntoMaterial.DataBind();

            }
            catch (Exception ex)
            {
                //Errores.Text = Errores.Text + " - Error Listar Adjuntos: " + ex.Message;
            }
            //}
        }
        public List<AttachmentsData> getAttachmentsData(String strLista, Int32 iElemento)
        {
            List<AttachmentsData> AttachmentsData = new List<AttachmentsData>();

            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                {
                    using (SPWeb currentWeb = site.OpenWeb(webId))
                    {
                        SPList lst = currentWeb.Lists[strLista];
                        SPListItem item = lst.GetItemById(iElemento);

                        if (item["Attachments"].ToString() == "True")
                        {
                            SPAttachmentCollection attachments = item.Attachments;
                            //SPFolder folder = item.Attachments.UrlPrefix;
                            //SPFolder folder = currentWeb.Folders[item.Attachments.UrlPrefix];
                            //SPFolder folder = currentWeb.Folders["Lists"].SubFolders[lst.Title].SubFolders["Attachments"].SubFolders[item.ID.ToString()];

                            foreach (string fileName in item.Attachments)
                            {
                                SPFile file = item.ParentList.ParentWeb.GetFile(
                                item.Attachments.UrlPrefix + fileName);

                                AttachmentsData.Add(new AttachmentsData()
                                {
                                    Title = item["Title"].ToString(),
                                    AttachmentTitle = file.Name.ToString(),
                                    AttachmentURL = currentWeb.Url + "/" + file.Url.ToString()
                                });
                            }
                        }
                        else if (item["Attachments"].ToString() == "False")
                        {
                            /* AttachmentsData.Add(new AttachmentsData()
                             {
                                 Title = item["Title"].ToString(),
                                 AttachmentTitle = "--",
                                 AttachmentURL = Page.Request.Url.ToString() + "#"
                             });*/
                        }
                    }
                }
            });
            return AttachmentsData;
        }
        protected void AdjuntarDocumento()
        {
            try
            {
                if (filUploadAdjunto.FileName != "")
                {
                    SPList lstList;
                    lstList = SPContext.Current.Web.Lists["Piezas"];
                    SPListItem itmAdjunto;
                    itmAdjunto = lstList.GetItemById(Convert.ToInt32(iMaterial.Value.ToString()));
                    itmAdjunto.Attachments.Add(filUploadAdjunto.FileName, filUploadAdjunto.FileBytes);
                    itmAdjunto.UpdateOverwriteVersion();
                }
            }
            catch (Exception ex)
            {
                //Errores.Text = Errores.Text + " - Error Adjuntar: " + ex.Message;
            }
        }
        protected void AvisosPagoAdjuntosGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridAdjuntoMaterial.Rows[index];
                SPList lstList = SPContext.Current.Web.Lists["Piezas"];
                SPListItem itmAdjunto;
                itmAdjunto = lstList.GetItemById(Convert.ToInt32(iMaterial.Value));
                SPAttachmentCollection atcItem;
                atcItem = itmAdjunto.Attachments;
                //Errores.Text = Errores.Text + row.Cells[0].Text;
                if (e.CommandName == "EliminarAdjunto")
                {
                    itmAdjunto.Attachments.Delete(row.Cells[3].Text);
                    itmAdjunto.Update();
                }
                else
                {
                    String sPath;
                    sPath = SPContext.Current.Web.ServerRelativeUrl;
                    sPath = row.Cells[3].Text;

                    SPFile file = SPContext.Current.Web.GetFile(sPath);
                    string filePath = Path.Combine(@"C:\SharePoint", row.Cells[0].Text);

                    byte[] binFile = file.OpenBinary();
                    System.IO.FileStream fs = System.IO.File.Create(filePath);
                    fs.Write(binFile, 0, binFile.Length);
                    fs.Close();



                }
                listarAdjuntos("Piezas", Convert.ToInt32(iMaterial.Value));
            }
            catch (Exception ex)
            {
                //Errores.Text = Errores.Text + " " + ex.Message;

            }

        }

        protected void btnAdjuntar_Click(object sender, EventArgs e)
        {
            AdjuntarDocumento();
            listarAdjuntos("Piezas", Convert.ToInt32(iMaterial.Value));
        }


        protected void btnGuardarDetalle_Click(object sender, EventArgs e)
        {
            Int32 idCicloPromocional = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/TipoMaterialGuardar.aspx?ID=" + idCicloPromocional + "&IDPieza=" + iMaterial.Value);

        }

        protected void btnPanelTareas_Click(object sender, EventArgs e)
        {
            Int32 idCicloPromocional = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalTareas.aspx?ID=" + idCicloPromocional + "&IDPieza=" + iMaterial.Value);

        }




    }
}
