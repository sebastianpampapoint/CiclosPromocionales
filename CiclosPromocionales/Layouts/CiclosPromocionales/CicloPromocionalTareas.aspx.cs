using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.DirectoryServices.AccountManagement;

namespace CiclosPromocionales.Layouts.CiclosPromocionales
{

    public class AttachmentsDataMaterial
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Estado { get; set; }
        public string AttachmentTitle { get; set; }
        public string AttachmentURL { get; set; }
    }
    public partial class CicloPromocionalTareas : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 idCicloPromocional = 0;
            Int32 idCicloPromocionalPieza = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            if (Request["IDPieza"] != null) { 
                idCicloPromocionalPieza = Convert.ToInt32(Request["IDPieza"]);
            }
            iCicloPromocional.Value = idCicloPromocional.ToString();
            iMaterial.Value = idCicloPromocionalPieza.ToString();
            btnIniciarProceso.Visible = false;
            btnAnularProceso.Visible = false;
            vCargarDatos(idCicloPromocional, idCicloPromocionalPieza);

            if (idCicloPromocionalPieza == 0)
            {
                pnlAdjuntar.Visible = false;
            }

        }

        public void vCargarDatos(Int32 idCicloPromocional, Int32 idCicloPromocionalPieza)
        {
            string sEstado = "";
            string sReinicio = "NO";
            string sRechazado = "NO";
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

                    if (sEstado != "Cancelado" && sEstado != "Completado")
                    {
                        if (Page.IsPostBack != true)
                        {
                            if (idCicloPromocionalPieza != 0)
                            {
                                CargarTareaActual(idCicloPromocionalPieza, true);
                                CargarTareasCumplidas(idCicloPromocionalPieza, true);

                            }
                            else
                            {
                                CargarTareaActual(idCicloPromocional, false);
                                CargarTareasCumplidas(idCicloPromocional, false);
                            }

                        }
                    }

                    if (idCicloPromocionalPieza != 0)
                    {
                        CargarTareasCumplidas(idCicloPromocionalPieza, true);
                        SPList lCicloPromocionalMaterial = web.Lists["Piezas"];

                        SPListItem itmCicloPromocionalMaterial = lCicloPromocionalMaterial.GetItemById(idCicloPromocionalPieza);
                        txtMaterial.Text = itmCicloPromocionalMaterial.Title;
                        SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmCicloPromocionalMaterial["Tipo Pieza"] as String);
                        txtTipoMaterial.Text = lkvTipoPieza.LookupValue.ToString();
                        txtDepartamentoMedico.Text = itmCicloPromocionalMaterial["DM"].ToString();
                        sReinicio = itmCicloPromocionalMaterial["Correcciones"].ToString();
                        if (itmCicloPromocionalMaterial["Estado"] != null)
                        {
                            strEstadoCotizacion.Value = itmCicloPromocionalMaterial["Estado"].ToString();
                        }

                    }
                    else
                    {
                        CargarTareasCumplidas(idCicloPromocional, false);
                        divMaterial.Visible = false;
                    }
                }
                listarAdjuntos("Documentos - Materiales", Convert.ToInt32(iMaterial.Value));
                if (bTieneDocumento.Value == "SI")
                {
                    if (strEstadoDocumento.Value == "Borrador")
                    {
                        btnIniciarProceso.Visible = true;
                        lblReinicio.Visible = true;
                        ddlReinicio.Visible = true;


                        if (sReinicio == "SI")
                        {
                            

                            ListItem itmSectorReinicio;

                            if (txtDepartamentoMedico.Text.ToString().ToUpper() == "SI")
                            {
                                itmSectorReinicio = new ListItem();
                                itmSectorReinicio.Value = "Departamento Médico";
                                itmSectorReinicio.Text = "Departamento Médico";
                                ddlReinicio.Items.Add(itmSectorReinicio);
                            }

                            itmSectorReinicio = new ListItem();
                            itmSectorReinicio.Value = "Marketing";
                            itmSectorReinicio.Text = "Marketing";
                            ddlReinicio.Items.Add(itmSectorReinicio);

                            itmSectorReinicio = new ListItem();
                            itmSectorReinicio.Value = "Diseño";
                            itmSectorReinicio.Text = "Diseño";
                            ddlReinicio.Items.Add(itmSectorReinicio);

                        }
                        else
                        {
                            ListItem itmSectorReinicio;

                            if (txtDepartamentoMedico.Text.ToString().ToUpper() == "SI")
                            {
                                itmSectorReinicio = new ListItem();
                                itmSectorReinicio.Value = "Departamento Médico";
                                itmSectorReinicio.Text = "Departamento Médico";
                                ddlReinicio.Items.Add(itmSectorReinicio);
                            }
                            else
                            {
                                itmSectorReinicio = new ListItem();
                                itmSectorReinicio.Value = "Marketing";
                                itmSectorReinicio.Text = "Marketing";
                                ddlReinicio.Items.Add(itmSectorReinicio);
                            }

                            
                        }
                    }

                    if (strEstadoCotizacion.Value == "Rechazado")
                    {
                        btnAnularProceso.Visible = true;
                    }
                }



                if (Funciones_Comunes.IsUserAuthorized("Ciclo Promocional - Diseño") == false)
                {
                    pnlAdjuntarDisenio.Visible = false;
                    btnAdjuntar.Visible = false;
                    btnIniciarProceso.Visible = false;
                }



            }
        }

        protected void CargarTareaActual(Int32 idCicloPromocional, Boolean bPieza)
        {
            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;

            //ddlSeleccioneTarea.Items.Clear();

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lCicloPromocional = web.Lists["Ciclo Promocional"];
                    SPList lBitacora = web.Lists["Bitácora Tareas"];
                    SPList lConfiguracionProceso = web.Lists["Configuración Proceso"];
                    String strOrigen = "T";
                    strOrigen = Request["Origen"];

                    SPQuery queryDA = new SPQuery();
                    if (bPieza == false)
                    {
                        queryDA.Query = string.Concat("<Where><And><And><Or><Membership Type='CurrentUserGroups'><FieldRef Name='Asignado'/></Membership><Eq> <FieldRef Name='Asignado'></FieldRef><Value Type='Integer'><UserID Type='Integer'/></Value></Eq></Or><Eq><FieldRef Name='Ciclo_x0020_Promocional' LookupId='TRUE'/>", "<Value Type='Lookup'>", idCicloPromocional, "</Value></Eq></And><Eq><FieldRef Name='Estado'/><Value Type='String'>Pendiente</Value></Eq></And></Where>");
                    }
                    else
                    {
                        queryDA.Query = string.Concat("<Where><And><And><Or><Membership Type='CurrentUserGroups'><FieldRef Name='Asignado'/></Membership><Eq> <FieldRef Name='Asignado'></FieldRef><Value Type='Integer'><UserID Type='Integer'/></Value></Eq></Or><Eq><FieldRef Name='Ciclo_x0020_Promcional_x0020__x0' LookupId='TRUE'/>", "<Value Type='Lookup'>", idCicloPromocional, "</Value></Eq></And><Eq><FieldRef Name='Estado'/>", "<Value Type='String'>Pendiente</Value></Eq></And></Where>");
                    }

                    //queryDA.Query = string.Concat("<Where><And><Eq><FieldRef Name='Solicitud_x0020_asociada' LookupId='TRUE'/>", "<Value Type='Lookup'>", idDocument, "</Value></Eq><Eq><FieldRef Name='Estado'/>", "<Value Type='String'>Pendiente</Value></Eq></And></Where><OrderBy><FieldRef Name='ID' Ascending='False'/></OrderBy>");

                    SPListItemCollection itemColl = null;
                    itemColl = lBitacora.GetItems(queryDA);
                    if (itemColl.Count > 0)
                    {

                        ListItem itmTarea0 = new ListItem();
                        itmTarea0.Value = "0";
                        itmTarea0.Text = "<-- Seleccione una tarea -->";
                        ddlSeleccioneTarea.Items.Add(itmTarea0);
                        int i = 0;
                        foreach (SPListItem itmTarea in itemColl)
                        {
                            if (bPieza == true || (bPieza == false && itmTarea["Tipo Tarea"].ToString() == "Inicio")) {
                                Int32 iConfiguracionProceso;
                                SPListItem itmConfiguracionProceso;
                                if (itmTarea["Configuracion Tarea"] is null) { iConfiguracionProceso = 0; } else { iConfiguracionProceso = Convert.ToInt32(itmTarea["Configuracion Tarea"].ToString().Split(';')[0]); };
                                if (iConfiguracionProceso != 0)
                                {
                                    itmConfiguracionProceso = lConfiguracionProceso.GetItemById(iConfiguracionProceso);
                                    if (itmConfiguracionProceso["Completa Inicio Proceso"] is null == false)
                                    {
                                        if (itmConfiguracionProceso["Completa Inicio Proceso"].ToString() == "False")
                                        {
                                            ListItem itmTareaActiva = new ListItem();
                                            itmTareaActiva.Value = itmTarea.ID.ToString();
                                            itmTareaActiva.Text = itmTarea.Title.ToString();
                                            ddlSeleccioneTarea.Items.Add(itmTareaActiva);
                                            i = i + 1;
                                        }
                                    }

                                }
                                
                                
                            }
                        }

                        if (i == 1)
                        {
                            ddlSeleccioneTarea.SelectedIndex = 1;
                            CargarTareaEdicion(Convert.ToInt32(ddlSeleccioneTarea.SelectedItem.Value.ToString()));

                        }
                        else
                        {
                            if (i==0)
                            {
                                ddlSeleccioneTarea.Visible = false;
                                pnlBitacoraDocumentoActual.Visible = false;
                            }
                            else
                            {
                                tblDatosTareaActual.Visible = false;
                                tblDatosAprobacionTareaActual.Visible = false;
                                //                            tblAdjuntarDocumento.Visible = false;
                                tblDatosAprobacion.Visible = false;
                            }
                            
                        }

                        

                    }
                    else
                    {

                        ddlSeleccioneTarea.Visible = false;
                        pnlBitacoraDocumentoActual.Visible = false;


                    }

                }
            }
        }
        protected void CargarTareaEdicion(Int32 idTarea)
        {
            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;
            tblDatosTareaActual.Visible = true;
            //tblAdjuntarDocumento.Visible = true;
            IdTareaBitacora.Value = idTarea.ToString();

            tblDatosTareaActual.Visible = true;
            tblDatosAprobacionTareaActual.Visible = true;
            //tblAdjuntarDocumento.Visible = true;
            tblDatosAprobacion.Visible = true;


            //SPSecurity.RunWithElevatedPrivileges(delegate ()
            //{
            using (SPSite site = new SPSite(siteId))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lCicloPromocional = web.Lists["Ciclo Promocional"];
                    SPList lBitacora = web.Lists["Bitácora Tareas"];
                    SPList lConfiguracionProceso = web.Lists["Configuración Proceso"];
                    Int32 iConfiguracionProceso;
                    SPListItem itmConfiguracionProceso;
                    SPListItem itmTarea = lBitacora.GetItemById(idTarea);
                    String strOrigen = "T";
                    strOrigen = Request["Origen"];

                    String strEtapaTarea, strEstadoTarea, strFechaInicio, strFechaFin, strCorrector, strSector;

                    strEtapaTarea = itmTarea.Title.ToString();
                    txtEtapaTarea.Text = strEtapaTarea;
                    if (itmTarea["Estado"] is null) { txtEstadoTarea.Text = ""; } else { txtEstadoTarea.Text = itmTarea["Estado"].ToString(); };
                    if (itmTarea["Fecha Inicio"] is null) { txtFechaInicio.Text = ""; } else { txtFechaInicio.Text = Convert.ToDateTime(itmTarea["Fecha Inicio"].ToString()).ToShortDateString(); };
                    if (itmTarea["Fecha de Fin"] is null) { txtFechaFin.Text = ""; }
                    if (itmTarea["Comentarios"] is null) { txtDatosAprobacion.Text = ""; } else { txtDatosAprobacion.Text = itmTarea["Comentarios"].ToString(); };


                    if (itmTarea["Asignado"] is null) { strCorrector = ""; }
                    else
                    {
                        String strResponsable = "";
                        try
                        {
                            string fieldValue = itmTarea["Asignado"].ToString();
                            SPFieldUserValueCollection users = new SPFieldUserValueCollection(itmTarea.Web, fieldValue);
                            foreach (SPFieldUserValue uv in users)
                            {
                                if (uv != null)
                                {
                                    if (uv.User != null)
                                    {
                                        SPUser user = uv.User;
                                        if (strResponsable != "")
                                        {
                                            strResponsable = strResponsable + "; " + user.Name.ToString();
                                        }
                                        else
                                        {
                                            strResponsable = user.Name.ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (web.Groups[uv.LookupValue] != null)
                                        {
                                            SPGroup sGroup = web.Groups[uv.LookupValue];
                                            if (strResponsable != "")
                                            {
                                                strResponsable = strResponsable + "; " + sGroup.Name.ToString();
                                            }
                                            else
                                            {
                                                strResponsable = sGroup.Name.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch { }

                        strCorrector = strResponsable;
                    };
                    txtCorrector.Text = strCorrector;

                    listarAdjuntosTarea("Bitácora Tareas", Convert.ToInt32(IdTareaBitacora.Value.ToString()));

                    if (itmTarea["Configuracion Tarea"] is null) { iConfiguracionProceso = 0; } else { iConfiguracionProceso = Convert.ToInt32(itmTarea["Configuracion Tarea"].ToString().Split(';')[0]); };
                    if (iConfiguracionProceso != 0)
                    {
                        Boolean bTareaAprobacion = false;
                        itmConfiguracionProceso = lConfiguracionProceso.GetItemById(iConfiguracionProceso);
                        if (itmConfiguracionProceso["Tipo Tarea"] is null == false)
                        {
                            if (itmConfiguracionProceso["Tipo Tarea"].ToString() == "Aprobación")
                            {
                                bTareaAprobacion = true;
                            }
                        }

                        if (itmConfiguracionProceso["Adjunto Obligatorio"] is null)
                        {
                            AdjuntoObligatorio.Value = "NO";
                        }
                        else
                        {

                            if (itmConfiguracionProceso["Adjunto Obligatorio"].ToString() == "False")
                            {
                                AdjuntoObligatorio.Value = "NO";
                            }
                            else
                            {
                                AdjuntoObligatorio.Value = "SI";
                            }
                        }

                        if (bTareaAprobacion == true)
                        {
                            btnCompletar.Visible = false;
                            btnAprobar.Visible = true;
                            btnRechazar.Visible = true;
                        }
                        else
                        {
                            btnCompletar.Visible = true;
                            btnAprobar.Visible = false;
                            btnRechazar.Visible = false;
                        }

                        ddlTareaSiguiente.Items.Clear();
                            if (itmConfiguracionProceso["Tarea Siguiente"] != null)
                            {
                                
                                SPFieldLookupValueCollection spTareaSiguiente = new SPFieldLookupValueCollection(itmConfiguracionProceso["Tarea Siguiente"].ToString());
                                foreach (SPFieldLookupValue spTarea in spTareaSiguiente)
                                {
                                    ListItem itmTareaActiva = new ListItem();
                                    itmTareaActiva.Value = spTarea.LookupId.ToString();
                                    itmTareaActiva.Text = spTarea.LookupValue.ToString();
                                    ddlTareaSiguiente.Items.Add(itmTareaActiva);
                                }

                                if (spTareaSiguiente.Count == 0)
                                {
                                    SPFieldLookupValue spTarea = new SPFieldLookupValue(itmConfiguracionProceso["Tarea Siguiente"].ToString());
                                    {
                                        if (spTarea.LookupValue != null)
                                        {
                                            ListItem itmTareaActiva = new ListItem();
                                            itmTareaActiva.Value = spTarea.LookupId.ToString();
                                            itmTareaActiva.Text = spTarea.LookupValue.ToString();
                                            ddlTareaSiguiente.Items.Add(itmTareaActiva);
                                        }
                                        else {
                                            lblTareaSiguiente.Visible = false;
                                            ddlTareaSiguiente.Visible = false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lblTareaSiguiente.Visible = false;
                                ddlTareaSiguiente.Visible = false;
                            }
                        }


                        


                }
            }
            //});
        }
        protected void CargarTareasCumplidas(Int32 idCicloPromocional, Boolean bPieza)
        {
            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;
            tblHistorialTareas.Rows.Clear();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                {
                    using (SPWeb web = site.RootWeb)
                    {
                        SPList lCicloPromocional = web.Lists["Ciclo Promocional"];
                        SPList lBitacora = web.Lists["Bitácora Tareas"];
                        // itmDocumento = lCicloPromocional.GetItemById(idCicloPromocional);
                        String strOrigen = "T";
                        //strOrigen = Request["Origen"];

                        String strEtapaTarea, strEstadoTarea, strFechaInicio, strFechaFin, strCorrector, strDatosAprobacion, strDatosMensaje, strAdjuntos;

                        SPQuery queryDA = new SPQuery();

                        if (bPieza == false)
                        {
                            queryDA.Query = string.Concat("<Where><Eq><FieldRef Name='Ciclo_x0020_Promocional' LookupId='TRUE'/>", "<Value Type='Lookup'>", idCicloPromocional, "</Value></Eq></Where><OrderBy>  <FieldRef Name='ID' Ascending='False'/></OrderBy>");
                        }
                        else
                        {
                            queryDA.Query = string.Concat("<Where><Eq><FieldRef Name='Ciclo_x0020_Promcional_x0020__x0' LookupId='TRUE'/>", "<Value Type='Lookup'>", idCicloPromocional, "</Value></Eq></Where><OrderBy>  <FieldRef Name='ID' Ascending='False'/></OrderBy>");
                        }

                        
                        SPListItemCollection itemColl = null;
                        itemColl = lBitacora.GetItems(queryDA);



                        if (itemColl.Count > 0)
                        {
                            int i = 2;
                            foreach (SPListItem itmTarea in itemColl)
                            {

                                if (itmTarea["Estado"].ToString() != "")
                                {

                                    strAdjuntos = "";

                                    if (bPieza == true || (bPieza == false && itmTarea["Tipo Tarea"].ToString() == "Inicio")) 
                                    {
                                        Int32 iConfiguracionProceso = 0;
                                        SPListItem itmConfiguracionProceso;
                                        SPList lConfiguracionProceso = web.Lists["Configuración Proceso"];
                                        if (itmTarea["Configuracion Tarea"] is null) { iConfiguracionProceso = 0; } else { iConfiguracionProceso = Convert.ToInt32(itmTarea["Configuracion Tarea"].ToString().Split(';')[0]); };
                                        itmConfiguracionProceso = lConfiguracionProceso.GetItemById(iConfiguracionProceso);
                                        //if (itmTarea["Etapa"] is null) { strEtapaTarea = ""; } else { strEtapaTarea = itmTarea["Etapa"].ToString() + " - " + itmTarea["Configuracion Tarea"].ToString().Split('#')[1]; };
                                        strEtapaTarea = itmTarea.Title.ToString();
                                        if (itmTarea["Estado"] is null) { strEstadoTarea = ""; } else { strEstadoTarea = itmTarea["Estado"].ToString(); };
                                        if (itmTarea["Fecha Inicio"] is null) { strFechaInicio = ""; } else { strFechaInicio = Convert.ToDateTime(itmTarea["Fecha Inicio"].ToString()).ToShortDateString(); };
                                        if (itmTarea["Fecha de Fin"] is null) { strFechaFin = ""; } else { strFechaFin = Convert.ToDateTime(itmTarea["Fecha de Fin"].ToString()).ToShortDateString(); };
                                        //if (itmTarea["Asignado"] is null) { strCorrector = ""; } else { strCorrector = itmTarea["Asignado"].ToString().Split('#')[1]; };
                                        if (itmTarea["Asignado"] is null) { strCorrector = ""; }
                                        else
                                        {
                                            String strResponsable = "";
                                            try
                                            {
                                                string fieldValue = itmTarea["Asignado"].ToString();
                                                SPFieldUserValueCollection users = new SPFieldUserValueCollection(itmTarea.Web, fieldValue);
                                                foreach (SPFieldUserValue uv in users)
                                                {
                                                    if (uv != null)
                                                    {
                                                        if (uv.User != null)
                                                        {
                                                            SPUser user = uv.User;
                                                            if (strResponsable != "")
                                                            {
                                                                strResponsable = strResponsable + "; " + user.Name.ToString();
                                                            }
                                                            else
                                                            {
                                                                strResponsable = user.Name.ToString();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (web.Groups[uv.LookupValue] != null)
                                                            {
                                                                SPGroup sGroup = web.Groups[uv.LookupValue];
                                                                if (strResponsable != "")
                                                                {
                                                                    strResponsable = strResponsable + "; " + sGroup.Name.ToString();
                                                                }
                                                                else
                                                                {
                                                                    strResponsable = sGroup.Name.ToString();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            catch { }

                                            strCorrector = strResponsable;
                                        };
                                        if (itmTarea["Comentarios"] is null) { strDatosAprobacion = "Sin Comentarios."; } else { strDatosAprobacion = itmTarea["Comentarios"].ToString(); };
                                        if (itmTarea["Attachments"].ToString() == "True")
                                        {
                                            SPAttachmentCollection attachments = itmTarea.Attachments;
                                            foreach (string fileName in itmTarea.Attachments)
                                            {
                                                SPFile file = itmTarea.ParentList.ParentWeb.GetFile(
                                                itmTarea.Attachments.UrlPrefix + fileName);
                                                strAdjuntos = strAdjuntos + @"<a href=' " + itmTarea.Attachments.UrlPrefix + fileName + "'>" + file.Name.ToString() + "</a><br/>";
                                            }
                                        }
                                        else
                                        {
                                            strAdjuntos = "Sin documentos adjuntos.";
                                        }


//                                        if (itmConfiguracionProceso["Tarea Resumen"].ToString() == "True")
                                            if ("False" == "True")
                                            {
                                            TableHeaderRow tblRowCabecera = new TableHeaderRow();
                                            tblRowCabecera.BackColor = System.Drawing.Color.FromName("#144733");
                                            TableCell tblCellCabeceraEtapa = new TableCell();
                                            TableCell tblCellCabeceraFechaInicio = new TableCell();
                                            TableCell tblCellCabeceraAsignado = new TableCell();

                                            tblCellCabeceraAsignado.ColumnSpan = 3;
                                            tblCellCabeceraAsignado.Text = "Iniciador";
                                            tblCellCabeceraAsignado.ForeColor = System.Drawing.Color.White;
                                            tblCellCabeceraFechaInicio.ColumnSpan = 2;
                                            tblCellCabeceraFechaInicio.Text = "Fecha de Inicio";
                                            tblCellCabeceraFechaInicio.ForeColor = System.Drawing.Color.White;
                                            TableCell tblCellCabecerAdjuntos = new TableCell();



                                            tblRowCabecera.Cells.Add(tblCellCabeceraAsignado);
                                            tblRowCabecera.Cells.Add(tblCellCabeceraFechaInicio);


                                            TableRow tblRowFila1 = new TableRow();
                                            TableCell tblCellFechaInicio = new TableCell();
                                            TableCell tblCellAsignado = new TableCell();


                                            tblCellFechaInicio.Text = strFechaInicio;
                                            tblCellFechaInicio.ColumnSpan = 2;
                                            tblCellAsignado.ColumnSpan = 3;
                                            tblCellAsignado.Text = strCorrector;
                                            tblRowFila1.Cells.Add(tblCellAsignado);
                                            tblRowFila1.Cells.Add(tblCellFechaInicio);





                                            tblHistorialTareas.Rows.Add(tblRowCabecera);
                                            tblHistorialTareas.Rows.Add(tblRowFila1);
                                        }
                                        else
                                        {
                                            TableHeaderRow tblRowCabecera = new TableHeaderRow();
                                            if (i % 2 == 0)
                                            {
                                                tblRowCabecera.BackColor = System.Drawing.Color.FromName("#009933");
                                            }
                                            else
                                            {
                                                tblRowCabecera.BackColor = System.Drawing.Color.CadetBlue;
                                            }
                                            TableCell tblCellCabeceraEtapa = new TableCell();
                                            TableCell tblCellCabeceraEstado = new TableCell();
                                            TableCell tblCellCabeceraFechaInicio = new TableCell();
                                            TableCell tblCellCabeceraFechaFin = new TableCell();
                                            TableCell tblCellCabeceraAsignado = new TableCell();

                                            tblCellCabeceraEtapa.Text = "Tarea";
                                            tblCellCabeceraEtapa.ForeColor = System.Drawing.Color.White;
                                            tblCellCabeceraEstado.Text = "Estado";
                                            tblCellCabeceraEstado.ForeColor = System.Drawing.Color.White;
                                            tblCellCabeceraAsignado.Text = "Usuario";
                                            tblCellCabeceraAsignado.ForeColor = System.Drawing.Color.White;
                                            tblCellCabeceraFechaInicio.Text = "Fecha de Inicio de tarea";
                                            tblCellCabeceraFechaInicio.ForeColor = System.Drawing.Color.White;
                                            tblCellCabeceraFechaFin.Text = "Fecha de cumplimentado";
                                            tblCellCabeceraFechaFin.ForeColor = System.Drawing.Color.White;
                                            tblRowCabecera.Cells.Add(tblCellCabeceraEtapa);
                                            tblRowCabecera.Cells.Add(tblCellCabeceraEstado);
                                            tblRowCabecera.Cells.Add(tblCellCabeceraAsignado);
                                            tblRowCabecera.Cells.Add(tblCellCabeceraFechaInicio);
                                            tblRowCabecera.Cells.Add(tblCellCabeceraFechaFin);


                                            TableRow tblRowFila1 = new TableRow();
                                            TableCell tblCellEtapa = new TableCell();
                                            TableCell tblCellEstado = new TableCell();
                                            TableCell tblCellFechaInicio = new TableCell();
                                            TableCell tblCellFechaFin = new TableCell();
                                            TableCell tblCellAsignado = new TableCell();

                                            tblCellEtapa.Text = strEtapaTarea;
                                            tblCellEstado.Text = strEstadoTarea;
                                            tblCellFechaInicio.Text = strFechaInicio;
                                            tblCellFechaFin.Text = strFechaFin;
                                            tblCellAsignado.Text = strCorrector;
                                            tblRowFila1.Cells.Add(tblCellEtapa);
                                            tblRowFila1.Cells.Add(tblCellEstado);
                                            tblRowFila1.Cells.Add(tblCellAsignado);
                                            tblRowFila1.Cells.Add(tblCellFechaInicio);
                                            tblRowFila1.Cells.Add(tblCellFechaFin);

                                            TableRow tblRowCabeceraFila2 = new TableRow();
                                            if (i % 2 == 0)
                                            {
                                                tblRowCabeceraFila2.BackColor = System.Drawing.Color.FromName("#009933");
                                            }
                                            else
                                            {
                                                tblRowCabeceraFila2.BackColor = System.Drawing.Color.CadetBlue;
                                            }



                                            TableRow tblRowFila2 = new TableRow();
                                            TableCell tblCellComentarios = new TableCell();
                                            TableCell tblCellAdjuntos = new TableCell();

                                            if (strOrigen == "T")
                                            {
                                                tblCellComentarios.ColumnSpan = 3;
                                                tblCellComentarios.Text = strDatosAprobacion;
                                                tblRowFila2.Cells.Add(tblCellComentarios);
                                                tblCellAdjuntos.ColumnSpan = 2;
                                            }
                                            else
                                            {
                                                tblCellAdjuntos.ColumnSpan = 5;
                                            }

                                            tblCellAdjuntos.Text = strAdjuntos;
                                            tblRowFila2.Cells.Add(tblCellAdjuntos);


                                            tblHistorialTareas.Rows.Add(tblRowCabecera);
                                            tblHistorialTareas.Rows.Add(tblRowFila1);
                                            tblHistorialTareas.Rows.Add(tblRowCabeceraFila2);
                                            tblHistorialTareas.Rows.Add(tblRowFila2);


                                            i = i + 1;
                                        }

                                    }

                                }

                            }
                        }
                        else
                        {
                            pnlBitacoraDocumentoHistoria.Visible = false;

                        }
                    }
                }
            });
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Boolean bProcesar = true;
            Int32 idDocument = Convert.ToInt32(Request["ID"]);

            if (IdTareaBitacora.Value.ToString() == "" || IdTareaBitacora.Value.ToString() == "0")
            {
                lblMensajeError.Text = "Se debe seleccionar la tarea a aprobar.";
                lblMensajeError.Visible = true;
                //txtUsuario.Focus();
                bProcesar = false;
            }


            if (bProcesar == true)
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.RootWeb)
                    {
                        SPList lBitacora = web.Lists["Bitácora Tareas"];
                        SPListItem itmBitacora = lBitacora.GetItemById(Convert.ToInt32(IdTareaBitacora.Value.ToString()));
                        itmBitacora["Comentarios"] = txtDatosAprobacion.Text.ToString();
                        itmBitacora.Update();
                        AdjuntarDocumentoTarea(Convert.ToInt32(IdTareaBitacora.Value.ToString()));
                    }

                }

                CargarTareaEdicion(Convert.ToInt32(IdTareaBitacora.Value.ToString()));
            }
            CargarTareasCumplidas(idDocument,true);

            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/Procesando.aspx?ID=" + Request["ID"] + "&IDPieza=" + Request["IDPieza"] + "&Pg=TP");
        }
        protected void AdjuntarDocumentoTarea(Int32 iTarea)
        {
            try
            {
                if (FileAdjuntoTarea.FileName != "")
                {
                    SPList lstList;
                    lstList = SPContext.Current.Web.Lists["Bitácora Tareas"];
                    SPListItem itmAdjunto;
                    itmAdjunto = lstList.GetItemById(Convert.ToInt32(IdTareaBitacora.Value.ToString()));
                    itmAdjunto.Attachments.Add(FileAdjuntoTarea.FileName, FileAdjuntoTarea.FileBytes);
                    itmAdjunto.UpdateOverwriteVersion();
                }
            }
            catch (Exception ex)
            {
                //Errores.Text = Errores.Text + " - Error Adjuntar: " + ex.Message;
            }
        }
        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            Boolean bProcesar = true;
            Int32 idDocument = Convert.ToInt32(Request["ID"]);

            if (IdTareaBitacora.Value.ToString() == "" || IdTareaBitacora.Value.ToString() == "0")
            {
                lblMensajeError.Text = "Se debe seleccionar la tarea a aprobar.";
                lblMensajeError.Visible = true;
                bProcesar = false;
            }

            if (AdjuntoObligatorio.Value == "SI")
            {
                if (gridAdjuntoTarea.Rows.Count == 0)
                {
                    lblMensajeError.Text = "Se debe adjuntar el archivo correspondiente antes de aprobar esta tarea.";
                    lblMensajeError.Visible = true;
                    //txtUsuario.Focus();
                    bProcesar = false;
                }

            }

            if (bProcesar == true)
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.RootWeb)
                    {
                        SPList lBitacora = web.Lists["Bitácora Tareas"];
                        SPListItem itmBitacora = lBitacora.GetItemById(Convert.ToInt32(IdTareaBitacora.Value.ToString()));
                        itmBitacora["Comentarios"] = txtDatosAprobacion.Text.ToString();
                        itmBitacora["Asignado"] = SPContext.Current.Web.CurrentUser;
                        itmBitacora["Estado"] = "Completado";
                        if (ddlTareaSiguiente.SelectedItem != null) { 
                        if (ddlTareaSiguiente.SelectedItem.Value != "0")
                            {
                                itmBitacora["Tarea Siguiente"] = ddlTareaSiguiente.SelectedItem.Value;
                            }
                        }

                        itmBitacora.Update();                      
                    }

                }

                Int32 idCicloPromocionalPieza = 0;
                if (Request["IDPieza"] != null)
                {
                    idCicloPromocionalPieza = Convert.ToInt32(Request["IDPieza"]);
                }

                CargarTareaEdicion(Convert.ToInt32(IdTareaBitacora.Value.ToString()));
                CargarTareasCumplidas(idDocument, true);
                Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/Procesando.aspx?ID=" + Request["ID"] + "&IDPieza=" + Request["IDPieza"] + "&Pg=TP");
            }
        }
        protected void ddlSeleccioneTarea_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlSeleccioneTarea.SelectedItem.Value != "0")
            {

                CargarTareaEdicion(Convert.ToInt32(ddlSeleccioneTarea.SelectedItem.Value));
            }
            else
            {
                tblDatosTareaActual.Visible = false;
                tblDatosAprobacionTareaActual.Visible = false;
                //tblAdjuntarDocumento.Visible = false;
                tblDatosAprobacion.Visible = false;
                IdTareaBitacora.Value = "0";
            }

            Int32 idDocument = 0;
            idDocument = Convert.ToInt32(Request["ID"]);
            CargarTareasCumplidas(idDocument, true);

        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Int32 idCicloPromocional = 0;
            idCicloPromocional = Convert.ToInt32(Request["ID"]);
            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalPiezas.aspx?ID=" + idCicloPromocional + "&Origen=E");
        }

        protected void btnAdjuntar_Click(object sender, EventArgs e)
        {
            AdjuntarDocumento();
            //listarAdjuntos("Documentos - Materiales", Convert.ToInt32(iMaterial.Value));
        }

        protected void AdjuntarDocumento()
        {
            try
            {
                if (filUploadAdjunto.FileName != "")
                {
                    SPList lstList;
                    lstList = SPContext.Current.Web.Lists["Piezas"];
                    SPListItem itmPieza;
                    itmPieza = lstList.GetItemById(Convert.ToInt32(iMaterial.Value.ToString()));
                    SPFolder stDocumentosMateriales= SPContext.Current.Web.Folders["Documentos%20%20Materiales"];
                    // Prepare to upload
                    Boolean replaceExistingFiles = true;
                    SPFile spfile = stDocumentosMateriales.Files.Add(filUploadAdjunto.FileName, filUploadAdjunto.FileBytes, replaceExistingFiles);
                    
                    
                    stDocumentosMateriales.Update();
                    spfile.Item["Title"] = filUploadAdjunto.FileName;
                    spfile.Item["Material Asociado"] = itmPieza.ID;
                    spfile.Item["Estado"] = "Borrador";

                    spfile.Item.Update();

                    itmPieza["Estado Documento"] = "Borrador";
                    itmPieza.UpdateOverwriteVersion();

                    Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/Procesando.aspx?ID=" + Request["ID"] + "&IDPieza=" + Request["IDPieza"] + "&Pg=TP");
                }
            }
            catch (Exception ex)
            {
                //Errores.Text = Errores.Text + " - Error Adjuntar: " + ex.Message;
            }
        }

        protected void listarAdjuntos(String strLista, Int32 idElemento)
        {
            try
            {

                //cellAdjuntosAvisoPago.Visible = true;
                gridAdjuntoMaterial.DataSource = ObtenerArchivoBiblioteca(strLista, idElemento);
                gridAdjuntoMaterial.DataBind();

            }
            catch (Exception ex)
            {
                //Errores.Text = Errores.Text + " - Error Listar Adjuntos: " + ex.Message;
            }
        }

        protected void listarAdjuntosTarea(String strLista, Int32 idElemento)
        {
            try
            {

                //cellAdjuntosAvisoPago.Visible = true;
                gridAdjuntoTarea.DataSource = getAttachmentsData(strLista, idElemento);
                gridAdjuntoTarea.DataBind();

            }
            catch (Exception ex)
            {
                //Errores.Text = Errores.Text + " - Error Listar Adjuntos: " + ex.Message;
            }
        }
        public List<AttachmentsDataMaterial> ObtenerArchivoBiblioteca(String strLista, Int32 iElemento)
        {
            List<AttachmentsDataMaterial> AttachmentsData = new List<AttachmentsDataMaterial>();

            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                {
                    using (SPWeb currentWeb = site.OpenWeb(webId))

                    {
                        SPList lst = currentWeb.Lists[strLista];
                        SPQuery queryDA = new SPQuery();
                        
                        queryDA.Query = string.Concat("<Where><Eq><FieldRef Name='Material_x0020_Asociado' LookupId='TRUE'/>", "<Value Type='Lookup'>", iElemento, "</Value></Eq></Where>");
                        
                        SPListItemCollection item = lst.GetItems(queryDA);

                        foreach (SPListItem fileName in item)
                        {
                            bTieneDocumento.Value = "SI";
                            AttachmentsData.Add(new AttachmentsDataMaterial()
                            {
                                Id = fileName.ID.ToString(),
                                Estado = fileName["Estado"].ToString(),
                                Title = fileName["Title"].ToString(),
                                AttachmentTitle = fileName["Title"].ToString(), // file.Name.ToString(),
                            AttachmentURL = currentWeb.Url + "/" + fileName.Url.ToString()
                            });
                            strEstadoDocumento.Value = fileName["Estado"].ToString();
                        }

                        
                    }
                }
            });
            return AttachmentsData;
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

        protected void AdjuntosTareasGridView_RowCommand(object sender, GridViewCommandEventArgs e)
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
                listarAdjuntosTarea("Piezas", Convert.ToInt32(IdTareaBitacora.Value.ToString()));
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
                    SPList lCicloPromocionalMaterial = web.Lists["Piezas"];
                    SPListItem itmCicloPromocionalPieza = lCicloPromocionalMaterial.GetItemById(Convert.ToInt32(iMaterial.Value.ToString()));
                    itmCicloPromocionalPieza["Diseño"] = SPContext.Current.Web.CurrentUser;
                    itmCicloPromocionalPieza.Update();

                    SPList lCicloPromocional = web.Lists["Procesamiento"];

                    SPListItem itmCicloPromocional = lCicloPromocional.AddItem();
                    itmCicloPromocional["Title"] = iCicloPromocional.Value.ToString();
                    itmCicloPromocional["Acción"] = "Iniciar Ciclo Aprobación";
                    itmCicloPromocional["Identificador"] = iMaterial.Value.ToString();

                    if (ddlReinicio.SelectedItem != null)
                    {
                        itmCicloPromocional["Sector"] = ddlReinicio.SelectedItem.Value.ToString();
                    }

                    itmCicloPromocional.Update();

                }
            }

            if (bProcesado == true)
            {

                Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/Procesando.aspx?ID=" + Request["ID"] + "&IDPieza=" + Request["IDPieza"] + "&Pg=TP");
            }
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            Boolean bProcesar = true;
            Int32 idDocument = Convert.ToInt32(Request["ID"]);
            if (IdTareaBitacora.Value.ToString() == "" || IdTareaBitacora.Value.ToString() == "0")
            {
                lblMensajeError.Text = "Se debe seleccionar la tarea a rechazar.";
                lblMensajeError.Visible = true;
                //txtUsuario.Focus();
                bProcesar = false;
            }
            if (txtDatosAprobacion.Text.ToString() != "")
            {
                if (bProcesar == true) { 
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.RootWeb)
                    {

                        SPList lBitacora = web.Lists["Bitácora Tareas"];
                        SPListItem itmBitacora = lBitacora.GetItemById(Convert.ToInt32(IdTareaBitacora.Value.ToString()));
                        //itmBitacora["Comentario de Revisión / Aprobación"] = txtDatosAprobacion.Text.ToString();
                        itmBitacora["Comentarios"] = txtDatosAprobacion.Text.ToString();
                        itmBitacora["Asignado"] = SPContext.Current.Web.CurrentUser;
                        itmBitacora["Estado"] = "Correcciones Pendientes";

                        if (ddlTareaSiguiente.SelectedItem.Value != "0")
                        {
                            itmBitacora["Tarea Siguiente"] = ddlTareaSiguiente.SelectedItem.Value;
                        }

                            itmBitacora.Update();
                    }

                }
                }

                Int32 idCicloPromocionalPieza = 0;
                if (Request["IDPieza"] != null)
                {
                    idCicloPromocionalPieza = Convert.ToInt32(Request["IDPieza"]);

                }

                CargarTareaEdicion(Convert.ToInt32(IdTareaBitacora.Value.ToString()));
                CargarTareasCumplidas(idDocument, true);
                Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/Procesando.aspx?ID=" + Request["ID"] + "&IDPieza=" + Request["IDPieza"] + "&Pg=TP");
            }
            else
            {
                lblMensajeError.Text = "Se debe indicar correcciones al documento.";
                lblMensajeError.Visible = true;
                txtDatosAprobacion.Focus();
            }
        }

        protected void btnAnularProceso_Click(object sender, EventArgs e)
        {
            Int32 idCicloPromocionalPieza = 0;
            if (Request["IDPieza"] != null)
            {
                idCicloPromocionalPieza = Convert.ToInt32(Request["IDPieza"]);

            }

            SPList lstList;
            lstList = SPContext.Current.Web.Lists["Piezas"];
            SPListItem itmPieza;
            itmPieza = lstList.GetItemById(Convert.ToInt32(idCicloPromocionalPieza));
            itmPieza["Estado Documento"] = "Anulado";
            itmPieza.UpdateOverwriteVersion();
            SPList lBitacora = SPContext.Current.Web.Lists["Bitácora Tareas"];

            SPQuery queryDA = new SPQuery();
            queryDA.Query = string.Concat("<Where><And><Eq><FieldRef Name='Ciclo_x0020_Promcional_x0020__x0' LookupId='TRUE'/>", "<Value Type='Lookup'>", idCicloPromocionalPieza, "</Value></Eq><Eq><FieldRef Name='Estado'/>", "<Value Type='String'>Pendiente</Value></Eq></And></Where>");
            SPListItemCollection itemColl = null;
            itemColl = lBitacora.GetItems(queryDA);
            if (itemColl.Count > 0)
            {
                foreach (SPListItem itmTarea in itemColl)
                {
                    itmTarea["Estado"] = "Anulado";
                    itmTarea["Procesado"] = "SI";
                    itmTarea.UpdateOverwriteVersion();
                    
                }

                }
            }
    }
}
