using System;
using System.Data;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CiclosPromocionales.Layouts.CiclosPromocionales
{
    public partial class TipoMaterialListado : LayoutsPageBase
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
                new DataColumn("TipoMaterial", typeof(string)),
                new DataColumn("Detalle", typeof(string)),
                new DataColumn("Estado", typeof(string)),
                new DataColumn("FechaInicio", typeof(DateTime)),

            });


            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                {
                    using (SPWeb web = site.OpenWeb(webId))
                    {

                        SPList lCicloPromocional = web.Lists["Tipo Pieza"];

                        SPQuery qryCiclos = new SPQuery();
                        String strQuery = "";
                        qryCiclos = new SPQuery(lCicloPromocional.Views["Todos los elementos"]);
                        String sOrden = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='{1}' /></OrderBy>", "Title", "True");
                        //strQuery = "<Eq><FieldRef Name='Estado' /><Value Type='Choice'>En Curso</Value></Eq>";


                        if (!string.IsNullOrEmpty(strQuery))
                        {
                            strQuery = "<Where>" + strQuery + "</Where>";
                        }
                        if (!string.IsNullOrEmpty(sOrden))
                        {
                            strQuery = strQuery + sOrden;
                        }

                        qryCiclos.Query = strQuery;
                        qryCiclos.RowLimit = 500;

                        SPListItemCollection colCiclos = null;
                        colCiclos = lCicloPromocional.GetItems(qryCiclos);


                        int i = 0;
                        foreach (SPListItem itmCiclo in colCiclos)
                        {
                            //int iCantidadMateriales = iObtenerTotalMateriales(itmCiclo.ID);
                            i++;


                            HyperLink aCicloPromocional = new HyperLink();
                            aCicloPromocional.Text = itmCiclo.Title.ToString();
                            aCicloPromocional.NavigateUrl = SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/TipoMaterialConfiguracion.aspx?ID=" + itmCiclo.ID.ToString() + "&Origen=E";

                            DataRow drRow = tempTbl.NewRow();
                            drRow["Numero"] = itmCiclo.ID.ToString(); // itmProyecto["Numero"].ToString();
                            drRow["Detalle"] = aCicloPromocional.NavigateUrl;
                            drRow["TipoMaterial"] = itmCiclo.Title;
                            drRow["Estado"] = itmCiclo["Estado"].ToString();
                            drRow["FechaInicio"] = Convert.ToDateTime(itmCiclo["Created"].ToString()).ToShortDateString();


                            tempTbl.Rows.Add(drRow);

                        }
                    }
                }
            });


            //jsonDataChart.Add(new DHX.Gantt.Web.Models.GanttContext { tasks = jsonDataProyecto, links = jsonLinkProyecto });

            //strChartData = JsonConvert.SerializeObject(jsonDataChart);

            //hdnChartData.Value = strChartData.Substring(1, strChartData.Length - 2);

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
            //if (e.Row.Cells[8].Text.ToString() != "" && e.Row.Cells[10].Text.ToString() != "&nbsp;")
            //{
            //    DateTime sFechaInicioReal = Convert.ToDateTime(e.Row.Cells[10].Text.ToString());
            //    if (sFechaInicioReal == Convert.ToDateTime("01-01-2000")) e.Row.Cells[10].ForeColor = System.Drawing.Color.White;
            //}

            //if (e.Row.Cells[3].Text.ToString() != "" && e.Row.Cells[3].Text.ToString() != "&nbsp;")
            //{
            //    e.Row.Cells[4].ToolTip = e.Row.Cells[3].Text.ToString();
            //}

        }

        protected void btnAltaCicloPromocional_Click(object sender, EventArgs e)
        {
            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/TipoMaterialConfiguracion.aspx?ID=0");

        }
    }
}
