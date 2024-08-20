using Microsoft.SharePoint;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CiclosPromocionales.PanelCicloHome
{
    public partial class PanelCicloHomeUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                

                //gwListaCiclos.Columns[3].Visible = true;
                gwListaCiclos.DataSource = ArmarListaCiclos();
                gwListaCiclos.PageSize = 80;
                gwListaCiclos.DataBind();
                //gwListaCiclos.Columns[3].Visible = false;
                gwListaCiclos.Font.Size = 1;
            }


        }

        protected DataTable ArmarListaCiclos()
        {
            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;
            DateTime dMenorFechaInicio = DateTime.Now;
            DateTime dMayorFechaFin = DateTime.Now;
            Int32 iRangoHastaVerde = 5;
            Int32 iRangoHastaAmarillo = 30;

            // GridView Proyectos
            DataTable tempTbl = new DataTable();
            tempTbl.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Numero", typeof(string)),
                new DataColumn("CicloPromocional", typeof(string)),
                new DataColumn("Detalle", typeof(string)),
                new DataColumn("Materiales", typeof(string)),
                new DataColumn("Estado", typeof(string)),
                new DataColumn("Semaforo", typeof(string)),
                new DataColumn("FechaInicio", typeof(DateTime)),
                new DataColumn("Cotiza", typeof(string)),

            });

            
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                {
                    using (SPWeb web = site.OpenWeb(webId))
                    {

                        SPList lCicloPromocional = web.Lists["Ciclo Promocional"];

                        SPQuery qryCiclos = new SPQuery();
                        String strQuery = "";
                        qryCiclos = new SPQuery(lCicloPromocional.Views["Todos los elementos"]);
                        String sOrden = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='{1}' /></OrderBy>", "ID", "True");
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
                            int iCantidadMateriales = iObtenerTotalMateriales(itmCiclo.ID);
                            i++;


                            HyperLink aCicloPromocional = new HyperLink();
                            aCicloPromocional.Text = itmCiclo.Title.ToString();
                            aCicloPromocional.NavigateUrl = SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalPiezas.aspx?ID=" + itmCiclo.ID.ToString() + "&Origen=E";
                            
                            Image imgEnero = new Image() { ImageUrl = "../../../Fuentes/vacio.jpg" };
                            Image imgFebrero = new Image() { ImageUrl = "../../../Fuentes/vacio.jpg" };
                            Image imgMarzo = new Image() { ImageUrl = "../../../Fuentes/vacio.jpg" };
                            Image imgAbril = new Image() { ImageUrl = "../../../Fuentes/vacio.jpg" };
                            Image imgMayo = new Image() { ImageUrl = "../../../Fuentes/vacio.jpg" };
                            Image imgJunio = new Image() { ImageUrl = "../../../Fuentes/vacio.jpg" };
                            Image imgJulio = new Image() { ImageUrl = "../../../Fuentes/vacio.jpg" };
                            Image imgAgosto = new Image() { ImageUrl = "../../../Fuentes/vacio.jpg" };
                            Image imgSeptiembre = new Image() { ImageUrl = "../../../Fuentes/vacio.jpg" };
                            Image imgOctubre = new Image() { ImageUrl = "../../../Fuentes/vacio.jpg" };
                            Image imgNoviembre = new Image() { ImageUrl = "../../../Fuentes/vacio.jpg" };
                            Image imgDiciembre = new Image() { ImageUrl = "../../../Fuentes/vacio.jpg" };

                            
                            DataRow drRow = tempTbl.NewRow();
                            drRow["Numero"] = itmCiclo.ID.ToString(); // itmProyecto["Numero"].ToString();
                            drRow["CicloPromocional"] = itmCiclo.Title;
                            drRow["Detalle"] = aCicloPromocional.NavigateUrl;
                            drRow["Materiales"] = iCantidadMateriales.ToString();
                            drRow["Estado"] = itmCiclo["Estado"].ToString();
                            drRow["Semaforo"] = ""; // imgSemaforo.ImageUrl;
                            drRow["FechaInicio"] = Convert.ToDateTime(itmCiclo["Created"].ToString()).ToShortDateString();
                            drRow["Cotiza"] = imgDiciembre.ImageUrl;

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

        private int iObtenerTotalMateriales(Int32 idCicloPromocional) {
            int iAuxResultado = 0;
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
                    iAuxResultado = lCicloPromocional.GetItems(qryTareas).Count;
                     
                    
                }
            }

            return iAuxResultado;


        }


        protected void gwListaCiclos_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtrslt = ArmarListaCiclos();
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
            Response.Redirect(SPContext.Current.Site.Url + "/_layouts/15/CiclosPromocionales/CicloPromocionalAlta.aspx");

        }
    }
}
