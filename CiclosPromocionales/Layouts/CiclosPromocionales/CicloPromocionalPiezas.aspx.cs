using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

namespace CiclosPromocionales.Layouts.CiclosPromocionales
{
    public class AttachmentsData
    {

        public string Title { get; set; }
        public string AttachmentTitle { get; set; }
        public string AttachmentURL { get; set; }
    }
    public partial class CicloPromocionalPiezas : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 idCicloPromocional = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            iCicloPromocional.Value = idCicloPromocional.ToString();
            vCargarDatos(idCicloPromocional);
            vCargarPiezas(idCicloPromocional);

            //btnGuardar.Enabled = true;
            btnActualizar.Enabled = false;
            btnEliminar.Enabled = false;

            if (!Page.IsPostBack)
            {
                vCargarComboProducto();
                vCargarComboTipoMaterial();
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
                    if (itmCicloPromocional != null) { 
                        txtTitulo.Text = itmCicloPromocional["Title"].ToString();
                        String strDetalle = "";
                        if (itmCicloPromocional["Detalle"] != null) { strDetalle = itmCicloPromocional["Detalle"].ToString(); }
                        txtDescripcion.Text = strDetalle;
                        
                        listarAdjuntos("Ciclo Promocional", idCicloPromocional);

                        if (itmCicloPromocional["Cotización"] != null)
                        {
                            sEstadoCotizacion.Value = itmCicloPromocional["Cotización"].ToString();
                            if (itmCicloPromocional["Cotización"].ToString() != "Pendiente")
                            {
                                btnEnviarCotizar.Enabled = false;
                                btnGuardar.Enabled = false;
                            }
                            txtEstadoCotización.Text = itmCicloPromocional["Cotización"].ToString();

                            SPList lCotizacion = web.Lists["Cotizaciones"];
                            SPQuery queryFile = new SPQuery();

                            queryFile.Query = string.Concat("<Where><Eq><FieldRef Name='Ciclo_x0020_Promocional' LookupId='TRUE'/>", "<Value Type='Lookup'>", idCicloPromocional, "</Value></Eq></Where>");

                            SPListItemCollection item = lCotizacion.GetItems(queryFile);

                            foreach (SPListItem fileName in item)
                            {
                                string fileurl = (string)fileName["EncodedAbsUrl"];

                                
                                    //txtNombreDocumento.Text =  @"<a href=' " + fileurl + "'>" + itmDocumento.File.Name.ToString() + "</a><br/>";
                                    hDocumento.Text = fileName.File.Name.ToString();
                                    hDocumento.NavigateUrl = fileurl;

                                }


                        }

                        if (itmCicloPromocional["Cotiza"].ToString().ToUpper() == "TRUE")
                        {
                            txtCotiza.Text = "Sí";
                        } else
                        {
                            txtCotiza.Text = "No";
                        }
  

                        if (itmCicloPromocional["Estado"] != null)
                        {
                            if (itmCicloPromocional["Estado"].ToString() != "Borrador")
                            {
                                btnIniciarProceso.Enabled = false;
                            }
                        }
                    }
                }
            }


        }

        public void vCargarComboProducto()
        {
            llbProducto.Items.Clear();

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lConfigTipoPieza = web.Lists["Producto - Línea"];
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
                            ListItem itmCheck = new ListItem();
                            itmCheck.Value = itmConfig.ID.ToString();
                            itmCheck.Text = itmConfig.Title.ToString();
                            llbProducto.Items.Add(itmCheck);
                        }


                    }
                }
            }
        }

        public void vCargarComboTipoMaterial()
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lConfigTipoPieza = web.Lists["Tipo Pieza"];
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
                            ddlTipoPieza.Items.Add(itmTipoMaterial);
                        }

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
                        System.Data.DataTable tempTbl = lCicloPromocional.GetItems(qryTareas).GetDataTable();
                        PiezasGridView.DataSource = tempTbl;
                        PiezasGridView.PageIndex = 1;
                        PiezasGridView.PageSize = 100;
                        PiezasGridView.DataBind();
                        PiezasGridView.Font.Size = 1;


                        
                    } 
                    else
                    {
                        btnIniciarProceso.Enabled = false;
                        btnEnviarCotizar.Enabled = false;
                        btnPanelTareas.Enabled = false;

                        btnEnviarProveedor.Enabled = false;
                    }
                }
            }
        }


        protected void GridViewPiezas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = PiezasGridView.Rows[index];
            if (e.CommandName == "EditarProducto")
            {

                LimpiarPanelMateriales();
                SPList lstList;
                lstList = SPContext.Current.Web.Lists["Piezas"];
                SPListItem itmMaterial;
                iMaterial.Value = row.Cells[0].Text;
                
                itmMaterial = lstList.GetItemById(Convert.ToInt32(iMaterial.Value));

                if (itmMaterial != null)
                {
                    txtPieza.Text = itmMaterial.Title.ToString();
                    if (itmMaterial["Código SAP"] is null) { txtCodigoSAP.Text = ""; } else { txtCodigoSAP.Text = itmMaterial["Código SAP"].ToString(); }
                    if (itmMaterial["Cantidad"] is null) { txtCantidad.Text = ""; } else { txtCantidad.Text = itmMaterial["Cantidad"].ToString(); }
                    if (itmMaterial["Comentarios"] is null) { txtComentarios.Text = ""; } else { txtComentarios.Text = itmMaterial["Comentarios"].ToString(); }
                    if (itmMaterial["DM"] is null) { ddlResponsable.SelectedValue = "NO"; } else { ddlResponsable.SelectedValue = itmMaterial["DM"].ToString(); }
                    if (itmMaterial["Tipo Pieza"] is null) {
                        ddlTipoPieza.SelectedValue = "0";
                        hdnTipoPieza.Value = "0";

                    } else {
                        SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmMaterial["Tipo Pieza"] as String);
                        ddlTipoPieza.SelectedValue = lkvTipoPieza.LookupId.ToString();
                        hdnTipoPieza.Value = lkvTipoPieza.LookupId.ToString();
                    }

                    SPFieldLookupValueCollection strProducto = itmMaterial["Producto"] as SPFieldLookupValueCollection;
                    foreach (SPFieldLookupValue iProducto in strProducto)
                    {
                        foreach (ListItem xProducto in llbProducto.Items)
                        {
                            if (xProducto.Value == iProducto.LookupId.ToString())
                            {
                                //xProducto.Selected = true;
                                llbProductoSeleccionado.Items.Add(xProducto);
                                llbProducto.Items.Remove(xProducto);
                                break;
                            }
                        }
                    }
                }

                btnGuardar.Enabled = false;
                btnActualizar.Enabled = true;
                btnEliminar.Enabled = true;
                if (sEstadoCotizacion.Value != "Pendiente" && sEstadoCotizacion.Value != "No Aplica")
                {
                    btnEliminar.Enabled = false;
                
                    ddlTipoPieza.Enabled = false;
                }                

            } else
            {
                if (e.CommandName == "VerDetalle")
                {
                    Int32 idCicloPromocional = 0;
                    idCicloPromocional = Convert.ToInt32(Request["ID"]);
                    String idMaterial = row.Cells[0].Text;

                    Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalDetallePieza.aspx?ID=" + idCicloPromocional + "&IDPieza=" + idMaterial);

                }

                if (e.CommandName == "VerTareas")
                {
                    Int32 idCicloPromocional = 0;
                    idCicloPromocional = Convert.ToInt32(Request["ID"]);
                    String idMaterial = row.Cells[0].Text;

                    Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalTareas.aspx?ID=" + idCicloPromocional + "&IDPieza=" + idMaterial);

                }

            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (bValidarDatos() == true)
            {
                SPList lstList;
                lstList = SPContext.Current.Web.Lists["Piezas"];

                SPListItem addListMaterial;

                addListMaterial = lstList.AddItem();


                SPFieldLookupValueCollection strProducto = new SPFieldLookupValueCollection();
                foreach (ListItem xProducto in llbProductoSeleccionado.Items)
                {
                        strProducto.Add(new SPFieldLookupValue(xProducto.Value));
                }
                addListMaterial["Ciclo"] = iCicloPromocional.Value;
                addListMaterial["Producto"] = strProducto;
                addListMaterial["Title"] = txtPieza.Text;
                addListMaterial["Código SAP"] = txtCodigoSAP.Text.ToString().TrimEnd();
                addListMaterial["Cantidad"] = txtCantidad.Text;
                addListMaterial["Comentarios"] = txtComentarios.Text;
                addListMaterial["DM"] = ddlResponsable.SelectedValue.ToString();
                if (ddlTipoPieza.SelectedItem.Value.ToString() != "0")
                {
                    addListMaterial["Tipo Pieza"] = ddlTipoPieza.SelectedItem.Value.ToString();

                }
                else
                {
                    addListMaterial["Tipo Pieza"] = null;
                }

                addListMaterial.Update();

                Int32 idCicloPromocional = 0;
                idCicloPromocional = Convert.ToInt32(Request["ID"]);
                Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/Procesando.aspx?ID=" + idCicloPromocional + "&Pg=CP");
            }
        }

        protected Boolean bValidarDatos()
        {
            Boolean bValidar = true;

            bValidar = false;
            foreach (ListItem xProducto in llbProductoSeleccionado.Items)
            {
                    bValidar = true;
            }
            if (bValidar == false)
            {
                lblProductoPieza.ForeColor = System.Drawing.Color.Red;
            }

            if (txtPieza.Text.Trim() == "") { bValidar = false; lblPieza.ForeColor = System.Drawing.Color.Red; }
            if (txtCantidad.Text.Trim() == "") { bValidar = false; lblCantidad.ForeColor = System.Drawing.Color.Red; }
            if (ddlTipoPieza.SelectedItem.Value.ToString() == "0") { bValidar = false; lblTipoPieza.ForeColor = System.Drawing.Color.Red; }


                return bValidar;

        }

        protected void LimpiarPanelMateriales()
        {
            iMaterial.Value = "0";
            txtPieza.Text = "";
            txtCodigoSAP.Text = "";
            txtCantidad.Text = "";
            txtComentarios.Text = "";
            ddlTipoPieza.SelectedValue = "0";
            ddlResponsable.SelectedValue = "NO";
            llbProductoSeleccionado.Items.Clear();

            vCargarComboProducto();


            if (sEstadoCotizacion.Value != "Pendiente" && sEstadoCotizacion.Value != "No Aplica")
            {
                btnGuardar.Enabled = true;
            }

            
            btnActualizar.Enabled = false;
            btnEliminar.Enabled = false;

            vCargarPiezas(Convert.ToInt32(Request["ID"]));
        }

        protected string RemoveCharacters(object String)
        {
            string s1 = String.ToString();
            string newString = Regex.Replace(s1, @"#[\d]\d+([,;\s]+\d+)*;", " ");
            newString = Regex.Replace(newString, "#", " ");
            return newString.ToString();
        }

        protected string ConvertFromHexToColor(object Detalle)
        {
            string strColor = "white";
            if (Detalle != "")
            {
                strColor = "green";
            }

            return strColor;
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            if (bValidarDatos() == true)
            {
                Int32 idMaterial = Convert.ToInt32(iMaterial.Value);
                SPList lstList;
                lstList = SPContext.Current.Web.Lists["Piezas"];

                SPListItem addListMaterial;

                addListMaterial = lstList.GetItemById(idMaterial);


                SPFieldLookupValueCollection strProducto = new SPFieldLookupValueCollection();
                foreach (ListItem xProducto in llbProductoSeleccionado.Items)
                {
                    strProducto.Add(new SPFieldLookupValue(xProducto.Value));
                }
                addListMaterial["Ciclo"] = iCicloPromocional.Value;
                addListMaterial["Producto"] = strProducto;
                addListMaterial["Title"] = txtPieza.Text;
                addListMaterial["Código SAP"] = txtCodigoSAP.Text.ToString().TrimEnd();
                addListMaterial["Cantidad"] = txtCantidad.Text;
                addListMaterial["Comentarios"] = txtComentarios.Text;
                addListMaterial["DM"] = ddlResponsable.SelectedValue.ToString();
                if (ddlTipoPieza.SelectedItem.Value.ToString() != "0")
                {
                    addListMaterial["Tipo Pieza"] = ddlTipoPieza.SelectedItem.Value.ToString();
                }
                else
                {
                    addListMaterial["Tipo Pieza"] = null;
                }

                if (hdnTipoPieza.Value != ddlTipoPieza.SelectedItem.Value.ToString())
                {
                    addListMaterial["Detalle"] = "";
                    addListMaterial["Opción 2"] = "";
                    addListMaterial["Opción 3"] = "";
                }

                addListMaterial.Update();
                System.Threading.Thread.Sleep(1500);

                LimpiarPanelMateriales();

                Int32 idCicloPromocional = 0;
                idCicloPromocional = Convert.ToInt32(Request["ID"]);
                Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/Procesando.aspx?ID=" + idCicloPromocional + "&Pg=CP");
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {

                Int32 idMaterial = Convert.ToInt32(iMaterial.Value);
                SPList lstList;
                lstList = SPContext.Current.Web.Lists["Piezas"];

                SPListItem addListMaterial;

                addListMaterial = lstList.GetItemById(idMaterial);
                
                addListMaterial.Delete();

                LimpiarPanelMateriales();

            Int32 idCicloPromocional = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/Procesando.aspx?ID=" + idCicloPromocional + "&Pg=CP");


        }

        protected void btnAdjuntar_Click(object sender, EventArgs e)
        {
            AdjuntarDocumento();
            listarAdjuntos("Ciclo Promocional", Convert.ToInt32(iCicloPromocional.Value));
        }

        protected void listarAdjuntos(String strLista, Int32 idElemento)
        {
            //if (!Page.IsPostBack)
            //{
            try
            {

                //cellAdjuntosAvisoPago.Visible = true;
                gridAdjuntoCiclo.DataSource = getAttachmentsData(strLista, idElemento);
                gridAdjuntoCiclo.DataBind();

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
                    lstList = SPContext.Current.Web.Lists["Ciclo Promocional"];
                    SPListItem itmAdjunto;
                    itmAdjunto = lstList.GetItemById(Convert.ToInt32(iCicloPromocional.Value.ToString()));
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
                GridViewRow row = gridAdjuntoCiclo.Rows[index];
                SPList lstList = SPContext.Current.Web.Lists["Ciclo Promocional"];
                SPListItem itmAdjunto;
                itmAdjunto = lstList.GetItemById(Convert.ToInt32(iCicloPromocional.Value));
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
                listarAdjuntos("Ciclo Promocional", Convert.ToInt32(iCicloPromocional.Value));
            }
            catch (Exception ex)
            {
                //Errores.Text = Errores.Text + " " + ex.Message;

            }

        }

        protected void btnIniciarProceso_Click(object sender, EventArgs e)
        {
            Boolean bProcesado = true;

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lCicloPromocional = web.Lists["Procesamiento"];

                    SPListItem itmCicloPromocional = lCicloPromocional.AddItem();
                    itmCicloPromocional["Title"] = iCicloPromocional.Value.ToString();
                    itmCicloPromocional["Acción"] = "Iniciar Ciclo Promocional";
                    itmCicloPromocional["Identificador"] = iCicloPromocional.Value.ToString();
                    itmCicloPromocional.Update();

                }
            }

            if (bProcesado == true)
            {

                Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalPiezas.aspx?ID=" + iCicloPromocional.Value.ToString() + "&Origen=C");
            }
        }



        protected void btnEnviarCotizar_click(object sender, EventArgs e)
        {
            Int32 idCicloPromocional = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/Cotizar.aspx?ID=" + idCicloPromocional);

        }

        protected void btnPanelTareas_Click(object sender, EventArgs e)
        {
            Int32 idCicloPromocional = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalTareas.aspx?ID=" + idCicloPromocional);

        }

        protected void btnEnviarProveedor_Click(object sender, EventArgs e)
        {
            Int32 idCicloPromocional = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalEnvioProveedor.aspx?ID=" + idCicloPromocional);

        }

        protected void btnSacarProducto_Click(object sender, EventArgs e)
        {
            int iTotalElementos = llbProductoSeleccionado.Items.Count - 1;

            for (int i = iTotalElementos; i >= 0; i--)
            {
                ListItem xProducto = llbProductoSeleccionado.Items[i];

                if (xProducto.Selected == true)
                {
                    llbProducto.Items.Add(xProducto);
                    llbProductoSeleccionado.Items.Remove(xProducto);
                }
            }

            if (iMaterial.Value != "")
            {
                btnGuardar.Enabled = false;
                btnActualizar.Enabled = true;
            }
            else
            {
                btnGuardar.Enabled = true;
                btnActualizar.Enabled = false;
            }
        }

        protected void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            int iTotalElementos = llbProducto.Items.Count - 1;

            for (int i = iTotalElementos; i >= 0; i--)
            {
                ListItem xProducto = llbProducto.Items[i];
            
                if (xProducto.Selected == true)
                {
                    llbProductoSeleccionado.Items.Add(xProducto);
                    llbProducto.Items.Remove(xProducto);
                }
            }
            
            if (iMaterial.Value != "")
            {
                btnGuardar.Enabled = false;
                btnActualizar.Enabled = true;
            } else
            {
                btnGuardar.Enabled = true;
                btnActualizar.Enabled = false;
            }

        }

        protected void btnGenerarExcel_Click(object sender, EventArgs e)
        {
            Boolean bProcesado = true;

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lCicloPromocional = web.Lists["Procesamiento"];

                    SPListItem itmCicloPromocional = lCicloPromocional.AddItem();
                    itmCicloPromocional["Title"] = iCicloPromocional.Value.ToString();
                    itmCicloPromocional["Acción"] = "Descarga Datos";
                    itmCicloPromocional["Identificador"] = iCicloPromocional.Value.ToString();
                    itmCicloPromocional["Mensaje"] = "";
                    itmCicloPromocional.Update();

                }
            }

            if (bProcesado == true)
            {
                
                Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalPiezas.aspx?ID=" + iCicloPromocional.Value.ToString() + "&Origen=C");
            }
        }

        protected void PiezasGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strDetalle = e.Row.Cells[13].Text.ToString();// as TextBox).Text;
                if (strDetalle != "" && strDetalle != "&nbsp;") { 
                    e.Row.Cells[11].BackColor = Color.LightGreen;
                    //e.Row.Cells[0].ForeColor  = Color.LightGreen;
                    e.Row.Cells[13].Text = "";
                }

            }
            
        }
    }
}
