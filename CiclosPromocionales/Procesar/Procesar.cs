using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using OfficeOpenXml;
using System.IO;
using System.Xml;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.DirectoryServices.AccountManagement;
using Microsoft.SharePoint.Administration;
using System.Net.Mail;
using System.Net;

namespace CiclosPromocionales.Procesar
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class Procesar : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            if (properties.List.Title == "Procesamiento")
            {
                Int32 idSolicitud = 0;
                String strAccion = "";

                idSolicitud = Convert.ToInt32(properties.ListItem["Identificador"].ToString());
                strAccion = properties.ListItem["Acción"].ToString();

                if (strAccion == "Alta Cotización")
                {
                    vEnviarCotizacion(properties, idSolicitud);
                }

                if (strAccion == "Iniciar Ciclo Promocional")
                {
                    vIniciarProcesoAlta(properties, idSolicitud);
                }

                if (strAccion == "Iniciar Ciclo Aprobación")
                {
                    vIniciarProcesoAprobacion(properties, idSolicitud);
                }

                if (strAccion == "Envío Proveedor")
                {
                    vEnviarProveedor(properties, properties.ListItem["Identificador"].ToString());
                }

                if (strAccion == "Descarga Datos")
                {
                    vEnviarDatos(properties, idSolicitud);
                }
            }

            if (properties.List.Title == "Bitácora Tareas")
            {
                Boolean bEstoyTareaEspera = false;
                Int32 iConfiguracionProceso;
                SPList lConfiguracionProceso = properties.Web.Lists["Configuración Proceso"];
                if (properties.ListItem["Configuracion Tarea"] is null) { iConfiguracionProceso = 0; } else { iConfiguracionProceso = Convert.ToInt32(properties.ListItem["Configuracion Tarea"].ToString().Split(';')[0]); };
                SPListItem itmConfiguracionProceso = lConfiguracionProceso.GetItemById(iConfiguracionProceso);

                string strCiclo, strIdCiclo, strTipoCircuito;
                strCiclo = properties.ListItem["Ciclo Promocional"].ToString().Split('#')[1].ToString();
                strIdCiclo = properties.ListItem["Ciclo Promocional"].ToString().Split(';')[0].ToString();
                SPList lSolicitud = properties.Web.Lists["Ciclo Promocional"];
                SPListItem itmDocumento = lSolicitud.GetItemById(Convert.ToInt32(strIdCiclo));


                string strMaterialAsociado = "", strIdMaterial = "";
                if (properties.ListItem["Ciclo Promcional - Material"] != null)
                {
                    strMaterialAsociado = properties.ListItem["Ciclo Promcional - Material"].ToString().Split('#')[1].ToString();
                    strIdMaterial = properties.ListItem["Ciclo Promcional - Material"].ToString().Split(';')[0].ToString();
                }


                if (Convert.ToBoolean(itmConfiguracionProceso["Tarea Aprobación"].ToString()) == true)
                {
                    SPList lst = properties.Web.Lists["Documentos - Materiales"];
                    SPQuery queryDA = new SPQuery();

                    queryDA.Query = string.Concat("<Where><Eq><FieldRef Name='Material_x0020_Asociado' LookupId='TRUE'/>", "<Value Type='Lookup'>", strIdMaterial, "</Value></Eq></Where>");

                    SPListItemCollection item = lst.GetItems(queryDA);

                    foreach (SPListItem fileName in item)
                    {
                        fileName["Estado"] = "Aprobado";
                        fileName.UpdateOverwriteVersion();
                    }
                }
                else {


                    StringBuilder strCuerpoAnuncio = new StringBuilder();
                    String strCabeceraMail = "";
                    string strLinkPaginaTarea = "";
                    strCuerpoAnuncio = strCuerpoAnuncio.Append("</tr>");
                    string strResponsable = "";
                    string strCopiaMail = "";
                    string strMensajeReinicio = "";

                    if (strMaterialAsociado != "")
                    {
                        strLinkPaginaTarea = properties.WebUrl + "/_layouts/15/CiclosPromocionales/CicloPromocionalTareas.aspx?ID=" + strIdCiclo + "&IDPieza=" + strIdMaterial;
                        strCabeceraMail = "Se le ha asignado la tarea " + properties.ListItem.Title.ToString() + " para el Material " + strMaterialAsociado + ".";

                    }
                    else
                    {
                        strLinkPaginaTarea = properties.WebUrl + "/_layouts/15/CiclosPromocionales/CicloPromocionalPiezas.aspx?ID=" + strIdCiclo + "&Origen=T";
                        strCabeceraMail = "Se le ha asignado la tarea " + properties.ListItem.Title.ToString() + " para el Ciclo " + itmDocumento.Title.ToString() + ".";
                    }

                    strCuerpoAnuncio = strCuerpoAnuncio.Append("<b>Ciclo:</b> " + itmDocumento.Title.ToString() + "<br /><br />");
                    if (strMaterialAsociado != "")
                    {
                        SPList lMateriales = properties.Web.Lists["Piezas"];
                        SPListItem itmMaterial = lMateriales.GetItemById(Convert.ToInt32(strIdMaterial));
                        strCuerpoAnuncio = strCuerpoAnuncio.Append("<b>Material:</b> " + strMaterialAsociado + "<br /><br />");
                        strCuerpoAnuncio = strCuerpoAnuncio.Append("<b>Tipo Material:</b> " + itmMaterial["Tipo Pieza"].ToString().Split('#')[1].ToString() + "<br /><br />");
                        strCuerpoAnuncio = strCuerpoAnuncio.Append("<b>Departamento Médico:</b> " + itmMaterial["DM"].ToString() + "<br /><br />");
                    }
                    strCuerpoAnuncio = strCuerpoAnuncio.Append("<b>Fecha de Vencimiento de la tarea:</b> " + Convert.ToDateTime(properties.ListItem["Fecha de Fin"].ToString()).ToShortDateString() + "<br /><br />");
                    strCuerpoAnuncio = strCuerpoAnuncio.Append("Para continuar con el proceso, ingrese a la tarea para completarla: " + @"<a href='" + strLinkPaginaTarea + "'>" + properties.ListItem.Title.ToString() + "</a><br/>");

                    string fieldValue = properties.ListItem["Asignado"].ToString();

                    SPFieldUserValueCollection users = new SPFieldUserValueCollection(properties.ListItem.Web, fieldValue);

                    foreach (SPFieldUserValue uv in users)
                    {
                        if (uv.User != null)
                        {
                            SPUser user = uv.User;
                            strResponsable = strResponsable + " " + user.Email.ToString() + ",";
                        }
                        else
                        {
                            SPGroup sGroup = properties.Web.Groups[uv.LookupValue];
                            foreach (SPUser user in sGroup.Users)
                            {
                                if (user.IsDomainGroup == true)
                                {
                                    ArrayList ADMembers = GetADGroupUsers(user.Name.ToString());
                                    foreach (string userName in ADMembers)
                                    {
                                        strResponsable = strResponsable + " " + userName + ",";
                                    }
                                }
                                else
                                {

                                    strResponsable = strResponsable + " " + user.Email.ToString() + ",";
                                }
                            }

                        }

                        // Process user
                    }


                    //AgregarLogMails(properties, properties.ListItem.Title.ToString() + " - " + strTipoCircuito + " - " + strSolicitudAsociada, strResponsable, strCabeceraMail.ToString());

                    string emailBody = " ";
                    emailBody = emailBody + "</tr></table>";
                    StringDictionary headers = new StringDictionary();
                    headers.Add("to", strResponsable);// sDevolverMailUsuario(strResponsable, properties));
                    headers.Add("from", properties.Web.Title.ToString() + "<sharepoint@baliarda.com.ar>");
                    if (strCopiaMail != "") { headers.Add("cc", strCopiaMail); }
                    headers.Add("subject", properties.ListItem.Title.ToString() + " - " + strCiclo);
                    headers.Add("content-type", "text/html");
                    //SPUtility.SendEmail(properties.Web, headers, strCabeceraMail + "<br /><br />" + strCuerpoAnuncio.ToString() + emailBody);
                    emailBody = "";

                    strResponsable = strResponsable.Substring(0, strResponsable.Length - 1);

                    SPWebApplication webApp = properties.Web.Site.WebApplication;
                    string smtpServerAddress = webApp.OutboundMailServiceInstance.Server.Address;
                    string fromAddress = webApp.OutboundMailSenderAddress;
                    System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain);

                    MailMessage email = new MailMessage();
                    email.From = new MailAddress(fromAddress, "Ciclos Promocionales - Sharepoint");
                    email.To.Add(strResponsable);

                    email.Subject = properties.ListItem.Title.ToString() + " - " + strCiclo;
                    email.Body = strCuerpoAnuncio.ToString();
                    email.IsBodyHtml = true;

                    // Set up the mail server and sent the email
                    SmtpClient mailServer = new SmtpClient(smtpServerAddress);
                    mailServer.Credentials = CredentialCache.DefaultNetworkCredentials;
                    mailServer.Send(email);

                }




            }
        }

        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            Boolean bProcesar = false;
            if (properties.List.Title == "Bitácora Tareas")
            {
                String sEstado = properties.ListItem["Estado"].ToString();
                String sProcesado = properties.ListItem["Procesado"].ToString();
                if (sEstado != "Pendiente" && sProcesado == "NO")
                {
                    bProcesar = bProcesarTarea(properties);
                    if (bProcesar == true)
                    {
                        properties.ListItem["Procesado"] = "SI";
                        properties.ListItem["Fecha de Fin"] = DateTime.Now;
                        properties.ListItem.UpdateOverwriteVersion();
                    }
                }

            }
        }

        private void vIniciarProcesoAlta(SPItemEventProperties properties, Int32 idCicloPromocional)
        {
            String strNombreCiclo = "";
            Boolean bDepartamentoMedico = false;
            using (SPWeb web = properties.Site.RootWeb)
            {

                SPList lCicloPromocional = web.Lists["Ciclo Promocional"];
                SPList lBitacora = web.Lists["Bitácora Tareas"];
                SPList lConfiguracionProceso = web.Lists["Configuración Proceso"];
                SPListItem itmCicloPromocional = lCicloPromocional.GetItemById(idCicloPromocional);

                if (itmCicloPromocional != null)
                {
                    strNombreCiclo = itmCicloPromocional.Title.ToString();

                }

                SPList lCicloPromocionalPiezas = web.Lists["Piezas"];
                SPQuery qryTareas = new SPQuery();
                String strQuery = "";
                qryTareas = new SPQuery(lCicloPromocionalPiezas.Views["Todos los elementos"]);
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

                if (lCicloPromocionalPiezas.GetItems(qryTareas).Count != 0)
                {
                    SPListItemCollection itemColl = null;
                    itemColl = lCicloPromocionalPiezas.GetItems(qryTareas);
                    foreach (SPListItem itmCicloPromocionalPieza in itemColl)
                    {
                        Int32 iTipoPieza = 0;
                        String strDM = "NO";
                        SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmCicloPromocionalPieza["Tipo Pieza"] as String);
                        iTipoPieza = lkvTipoPieza.LookupId;
                        strDM = itmCicloPromocionalPieza["DM"].ToString();
                        if (strDM == "SI")
                        {
                            SPQuery queryDM = new SPQuery();
                            queryDM.Query = string.Concat("<Where><And><Eq><FieldRef Name='Tarea_x0020_Inicial_x0020_Etapa' /><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='Etapa'/><Value Type='Choice'>Alta Ciclo - Departamento Médico</Value></Eq></And></Where>");
                            queryDM.RowLimit = 1;
                            queryDM.ViewFields = "";
                            SPListItemCollection itemsDM = lConfiguracionProceso.GetItems(queryDM);
                            SPListItem itmTareaDM = itemsDM[0];

                            SPListItem itmTareaBitacoraDM = lBitacora.AddItem();
                            itmTareaBitacoraDM = lBitacora.AddItem();
                            itmTareaBitacoraDM["Title"] = itmTareaDM.Title.ToString();
                            itmTareaBitacoraDM["Ciclo_x0020_Promocional"] = idCicloPromocional;
                            itmTareaBitacoraDM["Ciclo Promcional - Material"] = itmCicloPromocionalPieza.ID;
                            itmTareaBitacoraDM["Asignado"] = itmTareaDM["Usuario Asignado"];
                            itmTareaBitacoraDM["Tipo Tarea"] = itmTareaDM["Tipo Tarea"];
                            itmTareaBitacoraDM["Configuracion Tarea"] = itmTareaDM.ID.ToString();
                            itmTareaBitacoraDM["Fecha de Fin"] = Funciones_Comunes.dtFechaVencimiento(Convert.ToInt32(itmTareaDM["Días Vencimiento"].ToString()));
                            itmTareaBitacoraDM["Ver"] = itmCicloPromocional["Ver"];
                            //itmTareaBitacora["Sector"] = itmTarea["Sector"].ToString().Split('#')[1].ToString();
                            itmTareaBitacoraDM.Update();
                        }
                    }
                }

                SPQuery query = new SPQuery();

                query.Query = string.Concat("<Where><And><Eq><FieldRef Name='Tarea_x0020_Inicial_x0020_Etapa' /><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='Etapa'/><Value Type='Choice'>Alta Ciclo - Diseño</Value></Eq></And></Where>");
                query.RowLimit = 1;
                query.ViewFields = "";
                SPListItemCollection items = lConfiguracionProceso.GetItems(query);
                SPListItem itmTarea = items[0];

                SPListItem itmTareaBitacora = lBitacora.AddItem();
                itmTareaBitacora["Title"] = itmTarea.Title.ToString();
                itmTareaBitacora["Ciclo_x0020_Promocional"] = idCicloPromocional;
                itmTareaBitacora["Asignado"] = itmTarea["Usuario Asignado"];
                itmTareaBitacora["Configuracion Tarea"] = itmTarea.ID.ToString();
                itmTareaBitacora["Tipo Tarea"] = itmTarea["Tipo Tarea"];
                itmTareaBitacora["Fecha de Fin"] = Funciones_Comunes.dtFechaVencimiento(Convert.ToInt32(itmTarea["Días Vencimiento"].ToString()));
                itmTareaBitacora["Ver"] = itmCicloPromocional["Ver"];
                //itmTareaBitacora["Sector"] = itmTarea["Sector"].ToString().Split('#')[1].ToString();
                itmTareaBitacora.Update();


                if (bDepartamentoMedico == true)
                {

                }


                itmCicloPromocional = lCicloPromocional.GetItemById(idCicloPromocional);
                itmCicloPromocional["Estado"] = "En Curso";
                itmCicloPromocional.UpdateOverwriteVersion();

            }
        }

        private void vIniciarProcesoAprobacion(SPItemEventProperties properties, Int32 idPiezaCicloPromocional)
        {
            String strNombreCiclo = "";
            Boolean bDepartamentoMedico = false;
            using (SPWeb web = properties.Site.RootWeb)
            {

                SPList lCicloPromocionalPiezas = web.Lists["Piezas"];
                SPListItem itmCicloPromocionalPieza = lCicloPromocionalPiezas.GetItemById(idPiezaCicloPromocional);

                Int32 iTipoPieza = 0;
                Int32 iUsuarioDM = 0;
                String strDM = "NO";
                String strSector = properties.ListItem["Sector"].ToString();
                SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmCicloPromocionalPieza["Tipo Pieza"] as String);
                iTipoPieza = lkvTipoPieza.LookupId;
                strDM = itmCicloPromocionalPieza["DM"].ToString();
                if (strDM == "SI" && strSector == "Departamento Médico")
                {
                    bDepartamentoMedico = true;
                    SPFieldLookupValue lkvUsuarioDM = new SPFieldLookupValue(itmCicloPromocionalPieza["Departamento Médico"] as String);
                    iUsuarioDM = lkvUsuarioDM.LookupId;
                }

                SPFieldLookupValue lkvCicloPromocional = new SPFieldLookupValue(itmCicloPromocionalPieza["Ciclo"] as String);
                Int32 iCicloPromocional = 0;
                iCicloPromocional = lkvCicloPromocional.LookupId;

                SPList lCicloPromocional = web.Lists["Ciclo Promocional"];
                SPList lBitacora = web.Lists["Bitácora Tareas"];
                SPList lConfiguracionProceso = web.Lists["Configuración Proceso"];

                SPListItem itmCicloPromocional = lCicloPromocional.GetItemById(iCicloPromocional);

                if (itmCicloPromocional != null)
                {
                    strNombreCiclo = itmCicloPromocional.Title.ToString();
                }

                SPQuery query = new SPQuery();
                SPListItemCollection items;
                SPListItem itmTarea;
                SPListItem itmTareaBitacora;

                if (bDepartamentoMedico == true)
                {
                    query = new SPQuery();
                    query.Query = string.Concat("<Where><And><And><And><Eq><FieldRef Name='Tarea_x0020_Inicial_x0020_Etapa' /><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='Etapa'/><Value Type='Choice'>Aprobación Documento</Value></Eq></And><Eq><FieldRef Name='Sector'/><Value Type='Choice'>Departamento Médico</Value></Eq></And><Includes><FieldRef Name= 'Usuario_x0020_Asignado' LookupId ='TRUE'/><Value Type='Integer'>" + iUsuarioDM.ToString() + "</Value></Includes></And></Where>");
                    query.RowLimit = 1;
                    query.ViewFields = "";
                    items = lConfiguracionProceso.GetItems(query);
                    Boolean bTarea = false;

                    foreach (SPListItem itmTareaDM in items)
                    {
                        if (bTarea == false) {
                            itmTareaBitacora = lBitacora.AddItem();
                            itmTareaBitacora["Title"] = itmTareaDM.Title.ToString();
                            itmTareaBitacora["Ciclo_x0020_Promocional"] = iCicloPromocional;
                            itmTareaBitacora["Ciclo Promcional - Material"] = idPiezaCicloPromocional;
                            itmTareaBitacora["Asignado"] = itmTareaDM["Usuario Asignado"];
                            itmTareaBitacora["Tipo Tarea"] = itmTareaDM["Tipo Tarea"];
                            itmTareaBitacora["Configuracion Tarea"] = itmTareaDM.ID.ToString();
                            itmTareaBitacora["Fecha de Fin"] = Funciones_Comunes.dtFechaVencimiento(Convert.ToInt32(itmTareaDM["Días Vencimiento"].ToString()));
                            itmTareaBitacora["Ver"] = itmCicloPromocional["Ver"];
                            //itmTareaBitacora["Sector"] = itmTarea["Sector"].ToString().Split('#')[1].ToString();
                            itmTareaBitacora.Update();
                            bTarea = true;
                        }
                    }


                }
                else
                {

                    query.Query = string.Concat("<Where><And><And><Eq><FieldRef Name='Tarea_x0020_Inicial_x0020_Etapa' /><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='Etapa'/><Value Type='Choice'>Aprobación Documento</Value></Eq></And><Eq><FieldRef Name='Sector'/><Value Type='Choice'>", strSector, "</Value></Eq></And></Where>");
                    query.RowLimit = 1;
                    query.ViewFields = "";
                    items = lConfiguracionProceso.GetItems(query);
                    itmTarea = items[0];

                    itmTareaBitacora = lBitacora.AddItem();
                    itmTareaBitacora["Title"] = itmTarea.Title.ToString();
                    itmTareaBitacora["Ciclo_x0020_Promocional"] = iCicloPromocional;
                    itmTareaBitacora["Ciclo Promcional - Material"] = idPiezaCicloPromocional;
                    itmTareaBitacora["Asignado"] = itmTarea["Usuario Asignado"];
                    itmTareaBitacora["Tipo Tarea"] = itmTarea["Tipo Tarea"];
                    itmTareaBitacora["Configuracion Tarea"] = itmTarea.ID.ToString();
                    itmTareaBitacora["Fecha de Fin"] = Funciones_Comunes.dtFechaVencimiento(Convert.ToInt32(itmTarea["Días Vencimiento"].ToString()));
                    itmTareaBitacora["Ver"] = itmCicloPromocional["Ver"];
                    //itmTareaBitacora["Sector"] = itmTarea["Sector"].ToString().Split('#')[1].ToString();
                    itmTareaBitacora.Update();
                }

                SPList lst = web.Lists["Documentos - Materiales"];
                SPQuery queryDA = new SPQuery();

                queryDA.Query = string.Concat("<Where><Eq><FieldRef Name='Material_x0020_Asociado' LookupId='TRUE'/>", "<Value Type='Lookup'>", idPiezaCicloPromocional, "</Value></Eq></Where>");

                SPListItemCollection item = lst.GetItems(queryDA);

                foreach (SPListItem fileName in item)
                {
                    fileName["Estado"] = "En Curso";
                    fileName.UpdateOverwriteVersion();
                }

                itmCicloPromocionalPieza["Estado Documento"] = "En Curso";
                itmCicloPromocionalPieza["Correcciones"] = "NO";
                itmCicloPromocionalPieza.UpdateOverwriteVersion();


            }

            vProcesarTareaAltaDocumentos(properties, idPiezaCicloPromocional);
        }

        private void vEnviarCotizacion(SPItemEventProperties properties, Int32 idCicloPromocional)
        {
            String strNombreCiclo = "";
            String strMensajeCotizador = properties.ListItem["Mensaje"].ToString();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Cotización");
                worksheet.Column(1).Width = 50;
                worksheet.Column(2).Width = 50;
                worksheet.Column(3).Width = 50;
                worksheet.Column(4).Width = 50;
                int i = 1;
                using (SPWeb web = properties.Site.RootWeb)
                {

                    SPList lCicloPromocional = web.Lists["Ciclo Promocional"];
                    SPListItem itmCicloPromocional = lCicloPromocional.GetItemById(idCicloPromocional);

                    if (itmCicloPromocional != null)
                    {
                        strNombreCiclo = itmCicloPromocional.Title.ToString();
                        worksheet.Cells["A" + i.ToString()].Value = itmCicloPromocional.Title.ToString();
                        worksheet.Cells["A" + i.ToString()].Style.Font.Bold = true;
                        worksheet.Cells["A" + i.ToString()].Style.Font.Size = 16;

                    }

                    SPList lCicloPromocionalPiezas = web.Lists["Piezas"];
                    SPQuery qryTareas = new SPQuery();
                    String strQuery = "";
                    qryTareas = new SPQuery(lCicloPromocionalPiezas.Views["Todos los elementos"]);
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

                    if (lCicloPromocionalPiezas.GetItems(qryTareas).Count != 0)
                    {
                        SPListItemCollection itemColl = null;
                        itemColl = lCicloPromocionalPiezas.GetItems(qryTareas);
                        foreach (SPListItem itmCicloPromocionalPieza in itemColl)
                        {
                            i = i + 2;
                            Int32 iTipoPieza = 0;
                            SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmCicloPromocionalPieza["Tipo Pieza"] as String);
                            iTipoPieza = lkvTipoPieza.LookupId;
                            Boolean bAlternativo1 = false;
                            Boolean bAlternativo2 = false;
                            
                            String strAuxXML = "-";
                            String strAuxXMLAlternativo1 = "-";
                            String strAuxXMLAlternativo2 = "-";


                            if (itmCicloPromocionalPieza["Detalle"] != null)
                            {
                                strAuxXML = itmCicloPromocionalPieza["Detalle"].ToString();
                                if (itmCicloPromocionalPieza["Opción 2"] != null) {
                                    strAuxXMLAlternativo1 = itmCicloPromocionalPieza["Opción 2"].ToString();
                                    bAlternativo1 = true;
                                }
                                if (itmCicloPromocionalPieza["Opción 3"] != null)
                                {
                                    strAuxXMLAlternativo1 = itmCicloPromocionalPieza["Opción 3"].ToString();
                                    bAlternativo2 = true;
                                }

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


                                SPList lConfigTipoPieza = web.Lists["Configuración Tipo Pieza"];
                                SPQuery qryConfig = new SPQuery();
                                String strQueryConfig = "";
                                qryConfig = new SPQuery(lConfigTipoPieza.Views["Todos los elementos"]);
                                String sOrdenConfig = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='{1}' /></OrderBy>", "ID", "True");
                                strQueryConfig = "<Eq><FieldRef Name='Tipo_x0020_Pieza' LookupId='TRUE' /><Value Type='Lookup'>" + iTipoPieza.ToString() + "</Value></Eq>";


                                if (!string.IsNullOrEmpty(strQueryConfig))
                                {
                                    strQueryConfig = "<Where>" + strQueryConfig + "</Where>";
                                }
                                if (!string.IsNullOrEmpty(sOrdenConfig))
                                {
                                    strQueryConfig = strQueryConfig + sOrdenConfig;
                                }

                                qryConfig.Query = strQueryConfig;
                                qryConfig.RowLimit = 500;

                                if (lConfigTipoPieza.GetItems(qryConfig).Count != 0)
                                {
                                    worksheet.Cells["A" + i.ToString()].Value = "Producto";
                                    worksheet.Cells["A" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    worksheet.Cells["A" + i.ToString()].Style.Font.Bold = true;
                                    worksheet.Cells["B" + i.ToString()].Value = txtProducto;
                                    worksheet.Cells["B" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    worksheet.Cells["B" + i.ToString()].Style.Font.Bold = false;

                                    i = i + 1;

                                    worksheet.Cells["A" + i.ToString()].Value = "Material";
                                    worksheet.Cells["A" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    worksheet.Cells["A" + i.ToString()].Style.Font.Bold = true;
                                    //worksheet.Cells["A" + i.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                    worksheet.Cells["B" + i.ToString()].Value = "Detalle";
                                    worksheet.Cells["B" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    worksheet.Cells["B" + i.ToString()].Style.Font.Bold = true;
                                    //worksheet.Cells["B" + i.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                    if (bAlternativo1 == true) {
                                        worksheet.Cells["C" + i.ToString()].Value = "Alternativo1";
                                        worksheet.Cells["C" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        worksheet.Cells["C" + i.ToString()].Style.Font.Bold = true;
                                        //worksheet.Cells["C" + i.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                    }
                                    if (bAlternativo2 == true) {
                                        worksheet.Cells["D" + i.ToString()].Value = "Alternativo2";
                                        worksheet.Cells["D" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        worksheet.Cells["D" + i.ToString()].Style.Font.Bold = true;
                                        //worksheet.Cells["D" + i.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                    }
                                    i = i + 1;

                                    worksheet.Cells["A" + i.ToString()].Value = "Material";
                                    worksheet.Cells["A" + i.ToString()].Style.Font.Bold = true;
                                    worksheet.Cells["A" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    worksheet.Cells["B" + i.ToString()].Value = lkvTipoPieza.LookupValue.ToString();
                                    worksheet.Cells["B" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    if (bAlternativo1 == true)
                                    {
                                        worksheet.Cells["C" + i.ToString()].Value = itmCicloPromocionalPieza["Opción 2 Titulo"].ToString();
                                        worksheet.Cells["C" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    }
                                    if (bAlternativo2 == true)
                                    {
                                        worksheet.Cells["D" + i.ToString()].Value = itmCicloPromocionalPieza["Opción 3 Titulo"].ToString();
                                        worksheet.Cells["D" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    }
                                    i = i + 1;
                                    worksheet.Cells["A" + i.ToString()].Value = "Cantidad";
                                    worksheet.Cells["A" + i.ToString()].Style.Font.Bold = true;
                                    worksheet.Cells["A" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    worksheet.Cells["B" + i.ToString()].Value = itmCicloPromocionalPieza["Cantidad"].ToString();
                                    worksheet.Cells["B" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    if (bAlternativo1 == true)
                                    {
                                        worksheet.Cells["C" + i.ToString()].Value = itmCicloPromocionalPieza["Opción 2 Cantidad"].ToString();
                                        worksheet.Cells["C" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    }
                                    if (bAlternativo2 == true)
                                    {
                                        worksheet.Cells["D" + i.ToString()].Value = itmCicloPromocionalPieza["Opción 3 Cantidad"].ToString();
                                        worksheet.Cells["D" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    }
                                    i = i + 1;

                                    SPListItemCollection lColConfigTipoPieza = lConfigTipoPieza.GetItems(qryConfig);
                                    foreach (SPListItem itmConfig in lColConfigTipoPieza)
                                    {
                                        worksheet.Cells["A" + i.ToString()].Value = itmConfig.Title.ToString();
                                        worksheet.Cells["A" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        worksheet.Cells["B" + i.ToString()].Value = vCargarDetalle(strAuxXML, itmConfig.ID);
                                        worksheet.Cells["B" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        if (bAlternativo1 == true)
                                        {
                                            worksheet.Cells["C" + i.ToString()].Value = vCargarDetalle(strAuxXMLAlternativo1, itmConfig.ID);
                                            worksheet.Cells["C" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        }
                                        if (bAlternativo2 == true)
                                        {
                                            worksheet.Cells["D" + i.ToString()].Value = vCargarDetalle(strAuxXMLAlternativo2, itmConfig.ID);
                                            worksheet.Cells["D" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        }
                                        String txtValorDefault = itmConfig["Valor Default"].ToString();
                                        i = i + 1;

                                    }
                                }
                            }
                            else
                            {
                                worksheet.Cells["A" + i.ToString()].Value = "Falta cargar detalle para este material!";
                                i = i + 1;
                            }



                        }

                    }


                }


                if (strNombreCiclo != "")
                {
                    package.SaveAs(new FileInfo("C:\\SharePoint\\Cotización " + strNombreCiclo + ".xlsx"));

                    using (SPWeb web = properties.Site.RootWeb)
                    {

                        SPList lCicloPromocional = web.Lists["Ciclo Promocional"];
                        SPListItem itmCicloPromocional = lCicloPromocional.GetItemById(idCicloPromocional);
                        FileStream fs = File.OpenRead("C:\\SharePoint\\Cotización " + strNombreCiclo + ".xlsx");
                        //Stream fs = fileCotizacion
                        byte[] fileContents = new byte[fs.Length];
                        fs.Read(fileContents, 0, (int)fs.Length);
                        fs.Close();

                        SPFolder stDocumentosMateriales = web.Folders["Cotizaciones"];
                        // Prepare to upload
                        Boolean replaceExistingFiles = true;
                        SPFile spfile = stDocumentosMateriales.Files.Add("Cotización " + strNombreCiclo + ".xlsx", fileContents, replaceExistingFiles);


                        stDocumentosMateriales.Update();
                        spfile.Item["Title"] = "Cotización " + strNombreCiclo + ".xlsx";
                        spfile.Item["Ciclo Promocional"] = idCicloPromocional;

                        spfile.Item.Update();

                        //itmCicloPromocional.Attachments.Add("Cotización " + strNombreCiclo + ".xlsx", fileContents);
                        itmCicloPromocional["Cotización"] = "Enviada";
                        itmCicloPromocional.UpdateOverwriteVersion();


                        if (SPUtility.IsEmailServerSet(web))
                        {
                            StringBuilder strCuerpoAnuncio = new StringBuilder();
                            String strCabeceraMail = "";
                            String strCorreo = "";

                            SPList lMensajes = web.Lists["Configuración"];
                            SPListItemCollection sPListItemCollection = lMensajes.GetItems();
                            foreach (SPListItem sPListItem in sPListItemCollection)
                            {
                                if (sPListItem["Title"].ToString() == "Mail Cotizador")
                                    strCorreo = sPListItem["Valor"].ToString();
                            }


                            strCabeceraMail = "El documento para cotizar el ciclo " + strNombreCiclo + " se encuentra adjunto.";
                            strCuerpoAnuncio = strCuerpoAnuncio.Append("<b>Comentarios del Ciclo:</b> " + properties.ListItem["Mensaje"].ToString() + "<br />");


                            SPWebApplication webApp = web.Site.WebApplication;
                            string smtpServerAddress = webApp.OutboundMailServiceInstance.Server.Address;
                            string fromAddress = webApp.OutboundMailSenderAddress;

                            fs = File.OpenRead("C:\\SharePoint\\Cotización " + strNombreCiclo + ".xlsx");
                            fs.Position = 0;
                            // read from the start of what was written             

                            System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain);

                            MailMessage email = new MailMessage();
                            email.From = new MailAddress(fromAddress);
                            email.To.Add(strCorreo);

                            email.Attachments.Add(new Attachment(fs, "Cotización " + strNombreCiclo + ".xlsx", "text/txt"));
                            email.Subject = "Cotizar Ciclo Promocional - " + strNombreCiclo;
                            email.Body = strCabeceraMail + "<br /><br />" + strCuerpoAnuncio.ToString();
                            email.IsBodyHtml = true;
                            // Set up the mail server and sent the email
                            SmtpClient mailServer = new SmtpClient(smtpServerAddress);
                            mailServer.Credentials = CredentialCache.DefaultNetworkCredentials;
                            mailServer.Send(email);
                            fs.Close();
                        }

                    }

                }


            }
        }

        private void vEnviarDatos(SPItemEventProperties properties, Int32 idCicloPromocional)
        {
            String strNombreCiclo = "";
            String strMensajeCotizador = "";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Materiales");
                worksheet.Column(1).Width = 50;
                worksheet.Column(2).Width = 50;
                worksheet.Column(3).Width = 50;
                worksheet.Column(4).Width = 50;
                int i = 1;
                using (SPWeb web = properties.Site.RootWeb)
                {

                    SPList lCicloPromocional = web.Lists["Ciclo Promocional"];
                    SPListItem itmCicloPromocional = lCicloPromocional.GetItemById(idCicloPromocional);

                    if (itmCicloPromocional != null)
                    {
                        strNombreCiclo = itmCicloPromocional.Title.ToString();
                        worksheet.Cells["A" + i.ToString()].Value = itmCicloPromocional.Title.ToString();
                        worksheet.Cells["A" + i.ToString()].Style.Font.Bold = true;
                        worksheet.Cells["A" + i.ToString()].Style.Font.Size = 16;

                    }

                    SPList lCicloPromocionalPiezas = web.Lists["Piezas"];
                    SPQuery qryTareas = new SPQuery();
                    String strQuery = "";
                    qryTareas = new SPQuery(lCicloPromocionalPiezas.Views["Todos los elementos"]);
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


                    i = i + 2;
                    worksheet.Cells["A" + i.ToString()].Value = "Producto";
                    worksheet.Cells["A" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells["A" + i.ToString()].Style.Font.Bold = true;
                    worksheet.Cells["B" + i.ToString()].Value = "Material";
                    worksheet.Cells["B" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells["B" + i.ToString()].Style.Font.Bold = true;
                    worksheet.Cells["C" + i.ToString()].Value = "Tipo Material";
                    worksheet.Cells["C" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells["C" + i.ToString()].Style.Font.Bold = true;
                    worksheet.Cells["D" + i.ToString()].Value = "Código SAP";
                    worksheet.Cells["D" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells["D" + i.ToString()].Style.Font.Bold = true;
                    worksheet.Cells["E" + i.ToString()].Value = "Cantidad";
                    worksheet.Cells["E" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells["E" + i.ToString()].Style.Font.Bold = true;
                    worksheet.Cells["F" + i.ToString()].Value = "DM";
                    worksheet.Cells["F" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells["F" + i.ToString()].Style.Font.Bold = true;

                    if (lCicloPromocionalPiezas.GetItems(qryTareas).Count != 0)
                    {
                        SPListItemCollection itemColl = null;
                        itemColl = lCicloPromocionalPiezas.GetItems(qryTareas);
                        foreach (SPListItem itmCicloPromocionalPieza in itemColl)
                        {
                            i = i + 1;
                            Int32 iTipoPieza = 0;
                            SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmCicloPromocionalPieza["Tipo Pieza"] as String);
                            iTipoPieza = lkvTipoPieza.LookupId;
                            Boolean bAlternativo1 = false;
                            Boolean bAlternativo2 = false;

                            String strAuxXML = "-";
                            String strAuxXMLAlternativo1 = "-";
                            String strAuxXMLAlternativo2 = "-";


                                String txtProducto = "";
                            String txtCodigoSAP = "";
                            String txtCantidad = "";

                            if (itmCicloPromocionalPieza["Código SAP"] != null)
                            {
                                txtCodigoSAP = itmCicloPromocionalPieza["Código SAP"].ToString();
                            }
                            if (itmCicloPromocionalPieza["Cantidad"] != null)
                            {
                                txtCantidad = itmCicloPromocionalPieza["Cantidad"].ToString();
                            }

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

                                worksheet.Cells["A" + i.ToString()].Value = txtProducto;
                                worksheet.Cells["A" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                worksheet.Cells["A" + i.ToString()].Style.Font.Bold = false;
                                worksheet.Cells["B" + i.ToString()].Value = itmCicloPromocionalPieza.Title.ToString();
                                worksheet.Cells["B" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                worksheet.Cells["B" + i.ToString()].Style.Font.Bold = false;
                                worksheet.Cells["C" + i.ToString()].Value = lkvTipoPieza.LookupValue;
                                worksheet.Cells["C" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                worksheet.Cells["C" + i.ToString()].Style.Font.Bold = false;
                                worksheet.Cells["D" + i.ToString()].Value = txtCodigoSAP;
                                worksheet.Cells["D" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                worksheet.Cells["D" + i.ToString()].Style.Font.Bold = false;
                                worksheet.Cells["E" + i.ToString()].Value = txtCantidad;
                                worksheet.Cells["E" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                worksheet.Cells["E" + i.ToString()].Style.Font.Bold = false;
                                worksheet.Cells["F" + i.ToString()].Value = itmCicloPromocionalPieza["DM"].ToString();
                                worksheet.Cells["F" + i.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                worksheet.Cells["F" + i.ToString()].Style.Font.Bold = false;
                            }

                    }


                }


                if (strNombreCiclo != "")
                {
                    package.SaveAs(new FileInfo("C:\\SharePoint\\Datos " + strNombreCiclo + ".xlsx"));

                    using (SPWeb web = properties.Site.RootWeb)
                    {

                        SPList lCicloPromocional = web.Lists["Ciclo Promocional"];
                        SPListItem itmCicloPromocional = lCicloPromocional.GetItemById(idCicloPromocional);
                        FileStream fs = File.OpenRead("C:\\SharePoint\\Datos " + strNombreCiclo + ".xlsx");
                        //Stream fs = fileCotizacion
                        byte[] fileContents = new byte[fs.Length];
                        fs.Read(fileContents, 0, (int)fs.Length);
                        fs.Close();

                        if (SPUtility.IsEmailServerSet(web))
                        {
                            StringBuilder strCuerpoAnuncio = new StringBuilder();
                            String strCabeceraMail = "";
                            String strCorreo = "";

                            SPList lMensajes = web.Lists["Configuración"];
                            SPListItemCollection sPListItemCollection = lMensajes.GetItems();
                            foreach (SPListItem sPListItem in sPListItemCollection)
                            {
                                if (sPListItem["Title"].ToString() == "Mail Marketing")
                                    strCorreo = sPListItem["Valor"].ToString();
                            }


                            strCabeceraMail = "El detalle del ciclo " + strNombreCiclo + " se encuentra adjunto.";
                            

                            SPWebApplication webApp = web.Site.WebApplication;
                            string smtpServerAddress = webApp.OutboundMailServiceInstance.Server.Address;
                            string fromAddress = webApp.OutboundMailSenderAddress;

                            fs = File.OpenRead("C:\\SharePoint\\Datos " + strNombreCiclo + ".xlsx");
                            fs.Position = 0;
                            // read from the start of what was written             

                            System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain);

                            MailMessage email = new MailMessage();
                            email.From = new MailAddress(fromAddress);
                            email.To.Add(strCorreo);

                            email.Attachments.Add(new Attachment(fs, "Datos " + strNombreCiclo + ".xlsx", "text/txt"));
                            email.Subject = "Datos Ciclo Promocional - " + strNombreCiclo;
                            email.Body = strCabeceraMail + "<br /><br />" + strCuerpoAnuncio.ToString();
                            email.IsBodyHtml = true;
                            // Set up the mail server and sent the email
                            SmtpClient mailServer = new SmtpClient(smtpServerAddress);
                            mailServer.Credentials = CredentialCache.DefaultNetworkCredentials;
                            mailServer.Send(email);
                            fs.Close();
                        }

                    }

                }


            }
        }

        private void vEnviarProveedor(SPItemEventProperties properties, String idProceso) {

            String strMensajeCotizador = properties.ListItem["Mensaje"].ToString();

            using (SPWeb web = properties.Site.RootWeb)
            {
                SPList lCicloPromocionalPiezas = web.Lists["Procesar Proveedor"];
                SPListItem itmProcesamiento = lCicloPromocionalPiezas.GetItemById(Convert.ToInt32(idProceso));

                Int32 iProveedor = 0;
                String strCiclo = "";


                SPFieldLookupValue lkvProveedor = new SPFieldLookupValue(itmProcesamiento["Proveedor"] as String);
                iProveedor = lkvProveedor.LookupId;

                SPFieldLookupValue lkvCiclo = new SPFieldLookupValue(itmProcesamiento["Ciclo"] as String);
                strCiclo = lkvCiclo.LookupValue;


                SPList lProveedores = web.Lists["Proveedores"];
                SPListItem itmProveedor = lProveedores.GetItemById(iProveedor);

                String strCorreo = itmProveedor["Correo"].ToString();

                String strMensaje = "Entrega de originales - Materiales " + strCiclo;

                StringBuilder strCuerpoAnuncio = new StringBuilder();
                String strCabeceraMail = "";


                strCuerpoAnuncio.Append(properties.ListItem["Mensaje"].ToString());


                SPWebApplication webApp = web.Site.WebApplication;
                string smtpServerAddress = webApp.OutboundMailServiceInstance.Server.Address;
                string fromAddress = "diseno@baliarda.com.ar"; // webApp.OutboundMailSenderAddress;
                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain);

                MailMessage email = new MailMessage();
                email.From = new MailAddress(fromAddress, "Diseño Baliarda");
                email.To.Add(strCorreo);

                email.Subject = strMensaje.ToString();
                email.Body = strCuerpoAnuncio.ToString();
                email.IsBodyHtml = true;

                // Set up the mail server and sent the email
                SmtpClient mailServer = new SmtpClient(smtpServerAddress);
                mailServer.Credentials = CredentialCache.DefaultNetworkCredentials;
                mailServer.Send(email);

                SPFieldLookupValueCollection flPiezas = itmProcesamiento["Piezas"] as SPFieldLookupValueCollection;
                foreach (SPFieldLookupValue gwPieza in flPiezas)
                {
                    SPList lPiezas = web.Lists["Piezas"];
                    SPListItem itmPieza = lPiezas.GetItemById(gwPieza.LookupId);
                    itmPieza["Enviado Proveedor"] = "1";
                    itmPieza["Fecha Envío Proveedor"] = DateTime.Now;
                    itmPieza.Update();
                    vProcesarTareaDocumentoProveedor(properties, gwPieza.LookupId);
                }



            }




        }

        public Boolean bProcesarTarea(SPItemEventProperties properties)
        {

            Boolean bReiniciar = false;

            String sEstado = properties.ListItem["Estado"].ToString();

            Int32 iConfiguracionProceso = 0;
            Int32 iTareaSiguiente = 0;
            Int32 iCicloPromocional = 0;
            Int32 iCicloPromocionalMaterial = 0;
            Int32 iIteracion = 0;
            String strVer;

            if (properties.ListItem["Configuracion Tarea"] is null) { iConfiguracionProceso = 0; } else { iConfiguracionProceso = Convert.ToInt32(properties.ListItem["Configuracion Tarea"].ToString().Split(';')[0]); };
            if (properties.ListItem["Ciclo Promocional"] is null) { iCicloPromocional = 0; } else { iCicloPromocional = Convert.ToInt32(properties.ListItem["Ciclo Promocional"].ToString().Split(';')[0]); };
            if (properties.ListItem["Ciclo Promcional - Material"] is null) { iCicloPromocionalMaterial = 0; } else { iCicloPromocionalMaterial = Convert.ToInt32(properties.ListItem["Ciclo Promcional - Material"].ToString().Split(';')[0]); };
            if (properties.ListItem["Ver"] is null) { strVer = ""; } else { strVer = properties.ListItem["Ver"].ToString(); }



            SPList lConfiguracionProceso = properties.Web.Lists["Configuración Proceso"];
            SPList lBitacoraDocumento = properties.Web.Lists["Bitácora Tareas"];
            SPListItem itmConfiguracionProceso = lConfiguracionProceso.GetItemById(iConfiguracionProceso);


            if (sEstado == "Completado" || sEstado == "Correcciones Pendientes")
            {

                if (sEstado == "Correcciones Pendientes")
                {
                    SPList lCicloPromocionalPiezas = properties.Web.Lists["Piezas"];
                    SPListItem itmCicloPromocionalPieza = lCicloPromocionalPiezas.GetItemById(iCicloPromocionalMaterial);
                    itmCicloPromocionalPieza["Correcciones"] = "SI";
                    itmCicloPromocionalPieza.UpdateOverwriteVersion();
                }

                if (itmConfiguracionProceso["Tipo Tarea"].ToString() == "Inicio" && itmConfiguracionProceso["Sector"].ToString() == "Departamento Médico")
                {
                    SPList lCicloPromocionalPiezas = properties.Web.Lists["Piezas"];
                    SPListItem itmCicloPromocionalPieza = lCicloPromocionalPiezas.GetItemById(iCicloPromocionalMaterial);
                    itmCicloPromocionalPieza["Departamento Médico"] = properties.ListItem["Asignado"];
                    itmCicloPromocionalPieza.UpdateOverwriteVersion();
                }


                if (properties.ListItem["Tarea Siguiente"] is null)
                {
                    iTareaSiguiente = 0; // iTarea(iConfiguracionProceso, iCicloPromocional, properties);
                }
                else
                {
                    iTareaSiguiente = Convert.ToInt32(properties.ListItem["Tarea Siguiente"].ToString().Split(';')[0]);
                }

                if (iTareaSiguiente != 0)
                {
                    if (itmConfiguracionProceso["Tipo Tarea"].ToString() == "Inicio" && itmConfiguracionProceso["Sector"].ToString() == "Diseño")
                    {
                        // Busco las piezas en modo individual
                        SPListItem itmConfiguracionProcesoSiguiente = lConfiguracionProceso.GetItemById(iTareaSiguiente);
                        SPList lCicloPromocionalPiezas = properties.Web.Lists["Piezas"];
                        SPQuery qryTareas = new SPQuery();
                        String strQuery = "";
                        qryTareas = new SPQuery(lCicloPromocionalPiezas.Views["Todos los elementos"]);
                        String sOrden = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='{1}' /></OrderBy>", "ID", "False");
                        strQuery = "<Eq><FieldRef Name='Ciclo' LookupId='TRUE' /><Value Type='Lookup'>" + iCicloPromocional.ToString() + "</Value></Eq>";


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

                        if (lCicloPromocionalPiezas.GetItems(qryTareas).Count != 0)
                        {
                            SPListItemCollection itemColl = null;
                            itemColl = lCicloPromocionalPiezas.GetItems(qryTareas);
                            foreach (SPListItem itmCicloPromocionalPieza in itemColl)
                            {
                                Int32 iTipoPieza = 0;
                                String strDM = "NO";
                                SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmCicloPromocionalPieza["Tipo Pieza"] as String);
                                iTipoPieza = lkvTipoPieza.LookupId;
                                strDM = itmCicloPromocionalPieza["DM"].ToString();
                                if (strDM == "NO")
                                {
                                    String strEtapa = "";
                                    SPListItem itmTareaBitacora = lBitacoraDocumento.AddItem();
                                    itmTareaBitacora["Title"] = itmConfiguracionProcesoSiguiente.Title.ToString();
                                    itmTareaBitacora["Ciclo Promocional"] = iCicloPromocional;
                                    itmTareaBitacora["Ciclo Promcional - Material"] = itmCicloPromocionalPieza.ID;
                                    itmTareaBitacora["Asignado"] = itmConfiguracionProcesoSiguiente["Usuario Asignado"];
                                    itmTareaBitacora["Tipo Tarea"] = itmConfiguracionProcesoSiguiente["Tipo Tarea"];
                                    DateTime dAuxFechaVencimiento = dFechaVencimiento(Convert.ToInt32(itmConfiguracionProcesoSiguiente["Días Vencimiento"].ToString()), properties);
                                    itmTareaBitacora["Configuracion Tarea"] = itmConfiguracionProcesoSiguiente.ID.ToString();
                                    itmTareaBitacora["Fecha de Fin"] = dAuxFechaVencimiento;
                                    itmTareaBitacora["Fecha Vencimiento"] = dAuxFechaVencimiento;
                                    itmTareaBitacora["Ver"] = strVer;
                                    itmTareaBitacora.Update();
                                }
                            }
                        }



                    }
                    else
                    {

                        SPListItem itmConfiguracionProcesoSiguiente = lConfiguracionProceso.GetItemById(iTareaSiguiente);
                        String strCorrecciones = "NO";

                        if (itmConfiguracionProcesoSiguiente["Aplica Correcciones"].ToString() == "SI")
                        {
                            {
                                SPList lCicloPromocionalPiezas = properties.Web.Lists["Piezas"];
                                SPListItem itmCicloPromocionalPieza = lCicloPromocionalPiezas.GetItemById(iCicloPromocionalMaterial);
                                strCorrecciones = itmCicloPromocionalPieza["Correcciones"].ToString();
                            }
                        }

                        if (strCorrecciones == "NO")
                        {
                            String strEtapa = "";
                            SPListItem itmTareaBitacora = lBitacoraDocumento.AddItem();
                            itmTareaBitacora["Title"] = itmConfiguracionProcesoSiguiente.Title.ToString();
                            itmTareaBitacora["Ciclo Promocional"] = iCicloPromocional;
                            if (iCicloPromocionalMaterial != 0) itmTareaBitacora["Ciclo Promcional - Material"] = iCicloPromocionalMaterial;
                            itmTareaBitacora["Asignado"] = itmConfiguracionProcesoSiguiente["Usuario Asignado"];
                            itmTareaBitacora["Tipo Tarea"] = itmConfiguracionProcesoSiguiente["Tipo Tarea"];
                            DateTime dAuxFechaVencimiento = dFechaVencimiento(Convert.ToInt32(itmConfiguracionProcesoSiguiente["Días Vencimiento"].ToString()), properties);
                            itmTareaBitacora["Configuracion Tarea"] = itmConfiguracionProcesoSiguiente.ID.ToString();
                            itmTareaBitacora["Fecha de Fin"] = dAuxFechaVencimiento;
                            itmTareaBitacora["Fecha Vencimiento"] = dAuxFechaVencimiento;
                            itmTareaBitacora["Ver"] = strVer;
                            if (Convert.ToBoolean(itmConfiguracionProcesoSiguiente["Tarea Aprobación"].ToString()) == true)
                            {
                                itmTareaBitacora["Estado"] = "Completado";
                                itmTareaBitacora["Procesado"] = "SI";
                                itmTareaBitacora["Fecha de Fin"] = DateTime.Now;


                                if (iCicloPromocionalMaterial != 0)
                                {
                                    SPList lCicloPromocionalPiezas = properties.Web.Lists["Piezas"];
                                    SPListItem itmCicloPromocionalPieza = lCicloPromocionalPiezas.GetItemById(iCicloPromocionalMaterial);
                                    itmCicloPromocionalPieza["Estado Documento"] = "Aprobado";
                                    itmCicloPromocionalPieza.UpdateOverwriteVersion();
                                }

                                Int32 iTareaDocProveedor = 0;
                                if (itmConfiguracionProcesoSiguiente["Tarea Siguiente"] is null) { iTareaDocProveedor = 0; } else { iTareaDocProveedor = Convert.ToInt32(itmConfiguracionProcesoSiguiente["Tarea Siguiente"].ToString().Split(';')[0]); };

                                vProcesarTareaDocumentoProveedor(properties, iTareaDocProveedor, iCicloPromocional, iCicloPromocionalMaterial);


                            }
                            itmTareaBitacora.Update();
                        }
                        else
                        {
                            bReiniciar = true;
                        }
                    }
                }
            }


            if (bReiniciar == true)
            {
                // El documento fue rechazado
                string fieldValue = "";
                SPList lConfiguracion = properties.Web.Lists["Configuración Proceso"];
                SPQuery spqConfigDocumentos = new SPQuery();
                spqConfigDocumentos.Query = "<Where><Eq><FieldRef Name='Tarea_x0020_Rechazo' /><Value Type='Boolean'>1</Value></Eq></Where>";

                //query.Query = string.Concat("<Where><And><And><Eq><FieldRef Name='Tarea_x0020_Inicial_x0020_Etapa' /><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='Etapa'/><Value Type='Choice'>Aprobación Documento</Value></Eq></And><Eq><FieldRef Name='Sector'/><Value Type='Choice'>Departamento Médico</Value></Eq></And></Where>");

                SPListItemCollection colConfigDocumentos = lConfiguracion.GetItems(spqConfigDocumentos);
                if (colConfigDocumentos.Count > 0)
                {
                    fieldValue = colConfigDocumentos[0]["Usuario Asignado"].ToString();

                    SPListItem itmTareaBitacora;
                    SPListItem itmTarea = colConfigDocumentos[0];
                    //SPListItem itmCicloPromocional = lCicloPromocional.GetItemById(iCicloPromocional);

                    itmTareaBitacora = lBitacoraDocumento.AddItem();
                    itmTareaBitacora["Title"] = itmTarea.Title.ToString();
                    itmTareaBitacora["Ciclo_x0020_Promocional"] = iCicloPromocional;
                    itmTareaBitacora["Ciclo Promcional - Material"] = iCicloPromocionalMaterial;
                    itmTareaBitacora["Asignado"] = itmTarea["Usuario Asignado"];
                    itmTareaBitacora["Tipo Tarea"] = itmTarea["Tipo Tarea"];
                    itmTareaBitacora["Configuracion Tarea"] = itmTarea.ID.ToString();
                    itmTareaBitacora["Fecha de Fin"] = Funciones_Comunes.dtFechaVencimiento(Convert.ToInt32(itmTarea["Días Vencimiento"].ToString()));
                    //itmTareaBitacora["Ver"] = itmCicloPromocional["Ver"];
                    //itmTareaBitacora["Sector"] = itmTarea["Sector"].ToString().Split('#')[1].ToString();


                    if (iCicloPromocionalMaterial != 0)
                    {
                        SPList lCicloPromocionalPiezas = properties.Web.Lists["Piezas"];
                        SPListItem itmCicloPromocionalPieza = lCicloPromocionalPiezas.GetItemById(iCicloPromocionalMaterial);
                        fieldValue = itmCicloPromocionalPieza["Diseño"].ToString();
                        itmTareaBitacora["Asignado"] = itmCicloPromocionalPieza["Diseño"];
                    }

                    itmTareaBitacora.Update();

                }

                SPFieldUserValueCollection users = new SPFieldUserValueCollection(properties.ListItem.Web, fieldValue);
                string strResponsable = "";
                foreach (SPFieldUserValue uv in users)
                {
                    if (uv.User != null)
                    {
                        SPUser user = uv.User;
                        strResponsable = strResponsable + " " + user.Email.ToString() + ",";
                    }
                    else
                    {
                        SPGroup sGroup = properties.Web.Groups[uv.LookupValue];
                        foreach (SPUser user in sGroup.Users)
                        {
                            if (user.IsDomainGroup == true)
                            {
                                ArrayList ADMembers = GetADGroupUsers(user.Name.ToString());
                                foreach (string userName in ADMembers)
                                {
                                    strResponsable = strResponsable + " " + userName + ",";
                                }
                            }
                            else
                            {

                                strResponsable = strResponsable + " " + user.Email.ToString() + ",";
                            }
                        }


                    }

                    // Process user
                }

                //// Envío el correo
                //StringBuilder strCuerpoAnuncio = new StringBuilder();
                //String strCabeceraMail = "";
                //strCuerpoAnuncio = strCuerpoAnuncio.Append("</tr>");

                string strMaterialAsociado, strIdMaterial;
                strMaterialAsociado = properties.ListItem["Ciclo Promcional - Material"].ToString().Split('#')[1].ToString();
                strIdMaterial = properties.ListItem["Ciclo Promcional - Material"].ToString().Split(';')[0].ToString();


                //string strCicloPromocional, strIdCicloPromocional;
                //strCicloPromocional = properties.ListItem["Ciclo Promocional"].ToString().Split('#')[1].ToString();
                //strIdCicloPromocional = properties.ListItem["Ciclo Promocional"].ToString().Split(';')[0].ToString();
                //// /_layouts/15/CiclosPromocionales/CicloPromocionalTareas.aspx?ID=1&IDPieza=4
                //string strLinkPaginaTarea = properties.WebUrl + "/_layouts/15/CiclosPromocionales/CicloPromocionalTareas.aspx?ID=" + strIdCicloPromocional + "&IDPieza=" + strIdMaterial;
                //strCabeceraMail = "El documento para el material " + strMaterialAsociado + " fue rechazado en la tarea " + properties.ListItem.Title.ToString() + ".";
                //strCuerpoAnuncio = strCuerpoAnuncio.Append("<b>Comentarios del rechazo:</b> " + properties.ListItem["Comentarios"].ToString() + "<br />");
                //strCuerpoAnuncio = strCuerpoAnuncio.Append("Información del proceso: " + @"<a href='" + strLinkPaginaTarea + "'>" + strMaterialAsociado + "</a><br/>");

                //string emailBody = " ";
                //emailBody = emailBody + "</tr></table>";
                //StringDictionary headers = new StringDictionary();
                //headers.Add("to", strResponsable);// sDevolverMailUsuario(strResponsable, properties));
                //headers.Add("from", properties.Web.Title.ToString() + "<sharepoint@baliarda.com.ar>");
                //headers.Add("subject", "Documento Rechazado - " + strMaterialAsociado);
                //headers.Add("content-type", "text/html");
                //SPUtility.SendEmail(properties.Web, headers, strCabeceraMail + "<br /><br />" + strCuerpoAnuncio.ToString() + emailBody);
                //emailBody = "";

                SPList lst = properties.Web.Lists["Documentos - Materiales"];
                SPQuery queryDA = new SPQuery();

                queryDA.Query = string.Concat("<Where><Eq><FieldRef Name='Material_x0020_Asociado' LookupId='TRUE'/>", "<Value Type='Lookup'>", strIdMaterial, "</Value></Eq></Where>");

                SPListItemCollection item = lst.GetItems(queryDA);

                foreach (SPListItem fileName in item)
                {
                    fileName["Estado"] = "Correcciones Pendientes";
                    fileName.UpdateOverwriteVersion();
                }

                if (iCicloPromocional != 0)
                {
                    SPList lCicloPromocionalPiezas = properties.Web.Lists["Piezas"];
                    SPListItem itmCicloPromocionalPieza = lCicloPromocionalPiezas.GetItemById(iCicloPromocionalMaterial);
                    itmCicloPromocionalPieza["Estado Documento"] = "Correcciones Pendientes";
                    itmCicloPromocionalPieza.UpdateOverwriteVersion();
                }

            }




            return true;
        }


        private void vProcesarTareaDocumentoProveedor(SPItemEventProperties properties, Int32 iTareaDocProveedor, Int32 iCicloPromocional, Int32 iCicloPromocionalMaterial)
        {
            SPList lConfiguracionProceso = properties.Web.Lists["Configuración Proceso"];
            SPList lBitacoraDocumento = properties.Web.Lists["Bitácora Tareas"];
            SPListItem itmConfiguracionProcesoSiguiente = lConfiguracionProceso.GetItemById(iTareaDocProveedor);

            SPListItem itmTareaBitacora = lBitacoraDocumento.AddItem();
            itmTareaBitacora["Title"] = itmConfiguracionProcesoSiguiente.Title.ToString();
            itmTareaBitacora["Ciclo Promocional"] = iCicloPromocional;
            if (iCicloPromocionalMaterial != 0) itmTareaBitacora["Ciclo Promcional - Material"] = iCicloPromocionalMaterial;
            itmTareaBitacora["Asignado"] = itmConfiguracionProcesoSiguiente["Usuario Asignado"];
            itmTareaBitacora["Tipo Tarea"] = itmConfiguracionProcesoSiguiente["Tipo Tarea"];
            DateTime dAuxFechaVencimiento = dFechaVencimiento(Convert.ToInt32(itmConfiguracionProcesoSiguiente["Días Vencimiento"].ToString()), properties);
            itmTareaBitacora["Configuracion Tarea"] = itmConfiguracionProcesoSiguiente.ID.ToString();
            itmTareaBitacora["Fecha de Fin"] = dAuxFechaVencimiento;
            itmTareaBitacora["Fecha Vencimiento"] = dAuxFechaVencimiento;
            //itmTareaBitacora["Ver"] = strVer;
            itmTareaBitacora.Update();

        }
        private void vProcesarTareaAltaDocumentos(SPItemEventProperties properties, Int32 idCicloPromocional)
        {
            SPList lCicloPromocional = properties.Web.Lists["Ciclo Promocional"];
            SPList lBitacora = properties.Web.Lists["Bitácora Tareas"];
            SPList lConfiguracionProceso = properties.Web.Lists["Configuración Proceso"];


            SPQuery queryDA = new SPQuery();
            queryDA.Query = string.Concat("<Where><And><Eq><FieldRef Name='Ciclo_x0020_Promcional_x0020__x0' LookupId='TRUE'/><Value Type='Lookup'>", idCicloPromocional, "</Value></Eq><Eq><FieldRef Name='Estado'/><Value Type='String'>Pendiente</Value></Eq></And></Where>");

            //queryDA.Query = string.Concat("<Where><And><Eq><FieldRef Name='Solicitud_x0020_asociada' LookupId='TRUE'/>", "<Value Type='Lookup'>", idDocument, "</Value></Eq><Eq><FieldRef Name='Estado'/>", "<Value Type='String'>Pendiente</Value></Eq></And></Where><OrderBy><FieldRef Name='ID' Ascending='False'/></OrderBy>");

            SPListItemCollection itemColl = null;
            itemColl = lBitacora.GetItems(queryDA);
            if (itemColl.Count > 0)
            {


                foreach (SPListItem itmTarea in itemColl)
                {
                    Int32 iConfiguracionProceso;
                    SPListItem itmConfiguracionProceso;
                    if (itmTarea["Configuracion Tarea"] is null) { iConfiguracionProceso = 0; } else { iConfiguracionProceso = Convert.ToInt32(itmTarea["Configuracion Tarea"].ToString().Split(';')[0]); };
                    if (iConfiguracionProceso != 0)
                    {
                        itmConfiguracionProceso = lConfiguracionProceso.GetItemById(iConfiguracionProceso);
                        if (itmConfiguracionProceso["Completa Inicio Proceso"] is null == false)
                        {
                            if (itmConfiguracionProceso["Completa Inicio Proceso"].ToString() == "True")
                            {
                                SPList lst = properties.Web.Lists["Documentos - Materiales"];
                                SPQuery queryFile = new SPQuery();

                                queryFile.Query = string.Concat("<Where><Eq><FieldRef Name='Material_x0020_Asociado' LookupId='TRUE'/>", "<Value Type='Lookup'>", idCicloPromocional, "</Value></Eq></Where>");

                                SPListItemCollection item = lst.GetItems(queryFile);

                                foreach (SPListItem fileName in item)
                                {
                                    SPFile sArchivo = fileName.File;

                                    itmTarea.Attachments.Add(sArchivo.Name, sArchivo.OpenBinary());
 
                                }

                                itmTarea["Estado"] = "Completado";
                                itmTarea["Procesado"] = "SI";
                                itmTarea["Fecha de Fin"] = DateTime.Now;
                                itmTarea.UpdateOverwriteVersion();

                            }
                        }

                    }


                }
            }

        }

        private void vProcesarTareaDocumentoProveedor(SPItemEventProperties properties, Int32 idCicloPromocionalMaterial)
        {
            SPList lCicloPromocional = properties.Web.Lists["Ciclo Promocional"];
            SPList lBitacora = properties.Web.Lists["Bitácora Tareas"];
            SPList lConfiguracionProceso = properties.Web.Lists["Configuración Proceso"];


            SPQuery queryDA = new SPQuery();
            queryDA.Query = string.Concat("<Where><And><Eq><FieldRef Name='Ciclo_x0020_Promcional_x0020__x0' LookupId='TRUE'/><Value Type='Lookup'>", idCicloPromocionalMaterial, "</Value></Eq><Eq><FieldRef Name='Estado'/><Value Type='String'>Pendiente</Value></Eq></And></Where>");

            //queryDA.Query = string.Concat("<Where><And><Eq><FieldRef Name='Solicitud_x0020_asociada' LookupId='TRUE'/>", "<Value Type='Lookup'>", idDocument, "</Value></Eq><Eq><FieldRef Name='Estado'/>", "<Value Type='String'>Pendiente</Value></Eq></And></Where><OrderBy><FieldRef Name='ID' Ascending='False'/></OrderBy>");

            SPListItemCollection itemColl = null;
            itemColl = lBitacora.GetItems(queryDA);
            if (itemColl.Count > 0)
            {
                foreach (SPListItem itmTarea in itemColl)
                {
                    itmTarea["Estado"] = "Completado";
                    itmTarea["Procesado"] = "SI";
                    itmTarea["Fecha de Fin"] = DateTime.Now;
                    itmTarea.UpdateOverwriteVersion();
                }
            }
        }

        public Int32 iTarea(Int32 iConfiguracionProceso, Int32 iDocumento, SPItemEventProperties properties, String sPasaraDirector)
        {

            Int32 iTareaSiguiente = 0;
            Boolean bTareaSiguiente = false;
            Boolean sTareaUruguay = false;
            SPList lDocumentos = properties.Web.Lists["Documentos en Trabajo"];
            SPListItem itmDocumento = lDocumentos.GetItemById(iDocumento);
            String strCircuito = "";
            String sPais = "";
            Boolean bTareaUruguay = false;
            SPList lConfiguracionProceso = properties.Web.Lists["Configuracion Proceso Aprobacion"];
            SPList lBitacoraDocumento = properties.Web.Lists["Bitácora Documentos en Trabajo"];
            SPListItem itmConfiguracionProceso = lConfiguracionProceso.GetItemById(iConfiguracionProceso);
            SPListItem itmConfiguracionProcesoSiguiente;
            SPFieldLookupValueCollection itmCircuitos;

            strCircuito = itmDocumento.ContentType.Name.ToString();

            if (itmConfiguracionProceso["Tarea siguiente"] is null) { iTareaSiguiente = 0; } else { iTareaSiguiente = Convert.ToInt32(itmConfiguracionProceso["Tarea siguiente"].ToString().Split(';')[0]); };
            if (itmConfiguracionProceso["Tarea Uruguay"] is null) { sTareaUruguay = false; } else { sTareaUruguay = Convert.ToBoolean(itmConfiguracionProceso["Tarea Uruguay"].ToString()); };

            if (iTareaSiguiente != 0)
            {
                bTareaSiguiente = false;
                while (bTareaSiguiente == false && iTareaSiguiente != 0)
                {

                    itmConfiguracionProcesoSiguiente = lConfiguracionProceso.GetItemById(iTareaSiguiente);
                    itmCircuitos = new SPFieldLookupValueCollection(itmConfiguracionProcesoSiguiente["Circuito"].ToString());
                    foreach (SPFieldLookupValue value in itmCircuitos)
                    {
                        if (value.LookupValue == strCircuito)
                        {
                            bTareaSiguiente = true;
                        }
                    }

                    if (bTareaSiguiente == false)
                    {
                        if (itmConfiguracionProcesoSiguiente["Tarea siguiente"] is null) { iTareaSiguiente = 0; } else { iTareaSiguiente = Convert.ToInt32(itmConfiguracionProcesoSiguiente["Tarea siguiente"].ToString().Split(';')[0]); };
                    }
                    else
                    {
                        if (sPasaraDirector == "No")
                        {
                            sPasaraDirector = "NA";
                            if (itmConfiguracionProcesoSiguiente["Tarea siguiente"] is null) { iTareaSiguiente = 0; } else { iTareaSiguiente = Convert.ToInt32(itmConfiguracionProcesoSiguiente["Tarea siguiente"].ToString().Split(';')[0]); };
                            bTareaSiguiente = false;
                        }



                        if (sTareaUruguay == true)
                        {

                            if (itmDocumento["País"] != null)
                            {
                                sPais = itmDocumento["País"].ToString();
                            }
                            else
                            {
                                sPais = "No Aplica";
                            }

                            if (sPais != "No Aplica")
                            {
                                SPFieldMultiChoiceValue choices = new SPFieldMultiChoiceValue(itmDocumento["País"].ToString());

                                for (int i = 0; i < choices.Count; i++)
                                {
                                    sPais = choices[i]; ///here you need to write code for your form checkbox
                                    if (sPais == "Uruguay")
                                    {
                                        bTareaUruguay = true;
                                    }
                                }

                                if (bTareaUruguay == false)
                                {
                                    sTareaUruguay = false;
                                    if (itmConfiguracionProcesoSiguiente["Tarea siguiente"] is null) { iTareaSiguiente = 0; } else { iTareaSiguiente = Convert.ToInt32(itmConfiguracionProcesoSiguiente["Tarea siguiente"].ToString().Split(';')[0]); };
                                    bTareaSiguiente = false;
                                }


                            }





                        }
                    }



                }
            }
            return iTareaSiguiente;
        }

        public DateTime dFechaVencimiento(Int32 iDiasVencimiento, SPItemEventProperties properties)
        {

            Int32 iAuxDiasVencimiento = 0;
            DateTime dAuxFechaVencimiento = DateTime.Now;
            Boolean bEsFechaValida = false;

            while (iAuxDiasVencimiento < iDiasVencimiento)
            {
                dAuxFechaVencimiento = dAuxFechaVencimiento.AddDays(1);
                if (dAuxFechaVencimiento.DayOfWeek != DayOfWeek.Saturday && dAuxFechaVencimiento.DayOfWeek != DayOfWeek.Sunday && bEsFeriado(dAuxFechaVencimiento, properties) == false)
                {
                    iAuxDiasVencimiento = iAuxDiasVencimiento + 1;
                }
            }
            return dAuxFechaVencimiento;
        }

        public Boolean bEsFeriado(DateTime dFecha, SPItemEventProperties properties)
        {
            Boolean bAuxFeriado = false;
            using (SPWeb webSolicitudes = properties.Site.RootWeb)
            {

                SPList lCalendario = webSolicitudes.Lists["Calendario"];
                SPQuery qryCalendario = new SPQuery();
                String strQuery = "";
                strQuery = "<Where><Eq><FieldRef Name='Fecha' /><Value Type='DateTime'>" + dFecha.Date.ToString("yyyy-MM-ddTHH:mm:ssZ") + "</Value></Eq></Where>";
                qryCalendario.Query = strQuery;
                SPListItemCollection lstFeriado = lCalendario.GetItems(qryCalendario);
                if (lstFeriado.Count != 0)
                {
                    bAuxFeriado = true;
                }



            }


            return bAuxFeriado;
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

        public static string sDevolverDatosUsuario(string strUsuario, SPItemEventProperties properties)
        {
            string auxValor = "";
            auxValor = strUsuario.Split(';')[0].ToString();

            SPSite site = new SPSite(properties.Site.Url.ToString()); //.Settings.Default.UrlSite.ToString());
            SPWeb myweb = site.OpenWeb();
            SPUser sUsuario = myweb.AllUsers.GetByID(Convert.ToInt32(auxValor));

            auxValor = sUsuario.Name.ToString();

            return auxValor;
        }

        private ArrayList GetADGroupUsers(string groupName)
        {
            ArrayList userNames = new ArrayList();
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "Baliarda.com", "sharepointservice", "Shrp8451");
            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, groupName.Replace("Baliarda\\", "").ToString());

            if (group != null)
            {
                foreach (Principal p in group.GetMembers())
                {
                    UserPrincipal theUser = p as UserPrincipal;
                    if (theUser != null)
                    {
                        var user = UserPrincipal.FindByIdentity(ctx, p.SamAccountName);
                        if (user != null)
                        {
                            userNames.Add(user.EmailAddress);
                        }
                    }
                }

            }
            return userNames;

        }
    }
}