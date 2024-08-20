using System;
using System.Data;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CiclosPromocionales.Layouts.CiclosPromocionales
{
    public partial class TareasPendientes : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {



                //gwListaCiclos.Columns[3].Visible = true;
                gwListaCiclos.DataSource = ArmarListaTipos();
                gwListaCiclos.PageSize = 80;
                gwListaCiclos.DataBind();
                //gwListaCiclos.Columns[3].Visible = false;
                gwListaCiclos.Font.Size = 1;
            }


        }

        

        protected DataTable ArmarListaTipos()
        {
            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;
            DateTime dMenorFechaInicio = DateTime.Now;
            DateTime dMayorFechaFin = DateTime.Now;

            // GridView Proyectos
            DataTable tempTbl = new DataTable();
            tempTbl.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Numero", typeof(string)),
                new DataColumn("CicloPromocional", typeof(string)),
                new DataColumn("Tarea", typeof(string)),
                new DataColumn("Material", typeof(string)),
                new DataColumn("Detalle", typeof(string)),
                new DataColumn("Estado", typeof(string)),
                new DataColumn("FechaInicio", typeof(DateTime)),
                new DataColumn("FechaVencimiento", typeof(DateTime)),

            });

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lCicloPromocional = web.Lists["Ciclo Promocional"];
                    SPList lBitacora = web.Lists["Bitácora Tareas"];
                    //SPListItem itmDocumento = lCicloPromocional.GetItemById(idCicloPromocional);
                    String strOrigen = "T";
                    strOrigen = Request["Origen"];

                    SPQuery queryDA = new SPQuery();
                    queryDA.Query = string.Concat("<Where><And><Or><Membership Type='CurrentUserGroups'><FieldRef Name='Asignado'/></Membership><Eq> <FieldRef Name='Asignado'></FieldRef><Value Type='Integer'><UserID Type='Integer'/></Value></Eq></Or><Eq><FieldRef Name='Estado'/><Value Type='String'>Pendiente</Value></Eq></And></Where>");
                    
                    //queryDA.Query = string.Concat("<Where><And><Eq><FieldRef Name='Solicitud_x0020_asociada' LookupId='TRUE'/>", "<Value Type='Lookup'>", idDocument, "</Value></Eq><Eq><FieldRef Name='Estado'/>", "<Value Type='String'>Pendiente</Value></Eq></And></Where><OrderBy><FieldRef Name='ID' Ascending='False'/></OrderBy>");

                    SPListItemCollection itemColl = null;
                    itemColl = lBitacora.GetItems(queryDA);
                    if (itemColl.Count > 0)
                    {

                        foreach (SPListItem itmTarea in itemColl)
                        {
                            

                            string strCicloPromocional = "-";
                            string strMaterial = "-";
                            Int32 iCicloPromocional = 0;
                            Int32 iMaterial = 0;

                            if (itmTarea["Ciclo Promocional"] != null) {
                                SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmTarea["Ciclo Promocional"] as String);
                                strCicloPromocional = lkvTipoPieza.LookupValue;
                                iCicloPromocional = lkvTipoPieza.LookupId;
                            }

                            if (itmTarea["Ciclo Promcional - Material"] != null)
                            {
                                SPFieldLookupValue lkvTipoPieza = new SPFieldLookupValue(itmTarea["Ciclo Promcional - Material"] as String);
                                strMaterial = lkvTipoPieza.LookupValue;
                                iMaterial = lkvTipoPieza.LookupId;
                            }

                            HyperLink aCicloPromocional = new HyperLink();
                            aCicloPromocional.Text = itmTarea.Title.ToString();
                            aCicloPromocional.NavigateUrl = SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/Procesando.aspx?ID=" + iCicloPromocional.ToString() +  "&IDPieza=" + iMaterial.ToString() + "&Pg=TP";

                            DataRow drRow = tempTbl.NewRow();
                            drRow["Numero"] = itmTarea.ID.ToString(); // itmProyecto["Numero"].ToString();
                            drRow["Detalle"] = aCicloPromocional.NavigateUrl;
                            drRow["Tarea"] = itmTarea.Title;
                            
                            drRow["CicloPromocional"] = strCicloPromocional;
                            drRow["Material"] = strMaterial;
                            drRow["Estado"] = itmTarea["Estado"].ToString();
                            drRow["FechaInicio"] = Convert.ToDateTime(itmTarea["Created"].ToString()).ToShortDateString();
                            drRow["FechaVencimiento"] = Convert.ToDateTime(itmTarea["Fecha de Fin"].ToString()).ToShortDateString();


                            tempTbl.Rows.Add(drRow);
                        }
                    }


                }
            }

            return tempTbl;



        }

        protected void gwListaCiclos_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtrslt = ArmarListaTipos();
            if (dtrslt.Rows.Count > 0)
            {
                if (Convert.ToString(ViewState["sortdr"]) == "Asc")
                {
                    dtrslt.DefaultView.Sort = e.SortExpression + " Desc";
                    ViewState["sortdr"] = "Desc";
                }
                else
                {
                    dtrslt.DefaultView.Sort = e.SortExpression + " Asc";
                    ViewState["sortdr"] = "Asc";
                }
                gwListaCiclos.DataSource = dtrslt;
                gwListaCiclos.DataBind();


            }

        }

        protected void gwListaCiclos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells[6].Text.ToString() != "" && e.Row.Cells[6].Text.ToString() != "&nbsp;")
            {
                DateTime sFechaInicioReal = Convert.ToDateTime(e.Row.Cells[6].Text.ToString());
                if (sFechaInicioReal < DateTime.Now) e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
            }

            //if (e.Row.Cells[3].Text.ToString() != "" && e.Row.Cells[3].Text.ToString() != "&nbsp;")
            //{
            //    e.Row.Cells[4].ToolTip = e.Row.Cells[3].Text.ToString();
            //}

        }

    }
}
