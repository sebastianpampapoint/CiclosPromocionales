using System;
using System.Web.UI.WebControls;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CiclosPromocionales.Layouts.CiclosPromocionales
{
    public partial class TipoMaterialConfiguracion : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 idTipoPieza = 0;
            idTipoPieza = Convert.ToInt32(Request["ID"]);

            if (idTipoPieza != 0)
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.RootWeb)
                    {
                        SPList lTipoPieza = web.Lists["Tipo Pieza"];
                        SPListItem itmTipoPieza = lTipoPieza.GetItemById(idTipoPieza);
                        txtTitulo.Text = itmTipoPieza["Title"].ToString();
                        iTipoPieza.Value = idTipoPieza.ToString();
                    }
                }
                vArmarPanelDetalle(idTipoPieza, true);
            }
            else
            {
                vArmarPanelDetalle(idTipoPieza, true);
                iTipoPieza.Value = "0";
            }

        }

        public void vArmarPanelDetalle(Int32 iTipoPieza, Boolean bValorDefault)
        {
            Int32 iFila = 1;

            TableRow tblRowFila1 = new TableRow();
            TableCell tblCellCabeceraCampo = new TableCell();
            TableCell tblCellDetalle = new TableCell();
            TableCell tblCellEliminar = new TableCell();
            TableCell tblCellAlternativa2 = new TableCell();
            tblRowFila1 = new TableRow();

            if (iTipoPieza != 0) { 
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
                            tblCellDetalle = new TableCell();
                            tblCellEliminar = new TableCell();

                            tblRowFila1.Cells.Add(tblCellCabeceraCampo);
                            tblRowFila1.Cells.Add(tblCellDetalle);
                            tblRowFila1.Cells.Add(tblCellEliminar);


                            TextBox txtCampo = new TextBox();
                            txtCampo.ID = "ID_" + itmConfig.ID.ToString();
                            txtCampo.Width = 350;
                            txtCampo.Text = itmConfig.Title.ToString();
                            tblRowFila1.Cells[0].Controls.Add(txtCampo);

                            if (bValorDefault == true)
                            {
                                TextBox txtNotas = new TextBox();
                                txtNotas.ID = "Valor_D_" + itmConfig.ID.ToString();
                                txtNotas.Width = 350;
                                txtNotas.Text = itmConfig["Valor Default"].ToString();
                                tblRowFila1.Cells[1].Controls.Add(txtNotas);
                            }
                            CheckBox checkBox = new CheckBox();
                            tblRowFila1.Cells[2].Controls.Add(checkBox);

                            tblCheckList.Rows.Add(tblRowFila1);
                        }

                    }
                }
                }

            }

            while (iFila <= 25)
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
                tblCellDetalle = new TableCell();
                tblCellEliminar = new TableCell();

                tblRowFila1.Cells.Add(tblCellCabeceraCampo);
                tblRowFila1.Cells.Add(tblCellDetalle);
                tblRowFila1.Cells.Add(tblCellEliminar);


                TextBox txtCampo = new TextBox();
                txtCampo.ID = "IDF"+ iFila.ToString();
                txtCampo.Width = 350;
                txtCampo.Text = "";
                tblRowFila1.Cells[0].Controls.Add(txtCampo);

                TextBox txtNotas = new TextBox();
                txtNotas.ID = "Valor_FD_" + iFila.ToString();
                txtNotas.Width = 350;
                txtNotas.Text = "";
                tblRowFila1.Cells[1].Controls.Add(txtNotas);
                
                CheckBox checkBox = new CheckBox();
                tblRowFila1.Cells[2].Controls.Add(checkBox);

                tblCheckList.Rows.Add(tblRowFila1);
            }

        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (iTipoPieza.Value == "0")
            {
                vAltaTipoMaterial();
            }
            else {
                vActualizarTipoMaterial();
            }

            
        }

        protected void vActualizarTipoMaterial()
        {
            Int32 itmTipoMaterial = Convert.ToInt32(iTipoPieza.Value);
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList lTipoPieza = web.Lists["Tipo Pieza"];

                    SPListItem itmTipoPieza = lTipoPieza.GetItemById(itmTipoMaterial);
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

                            TextBox txtCampo = tblRowTarea.Cells[0].Controls[0] as TextBox;
                            if (txtCampo.Text.ToString().Trim() != "")
                            {
                                
                                if (txtCampo.ID.ToString().Substring(0, 3) == "ID_") {

                                    Int32 iConfigTipoPieza = 0;
                                    iConfigTipoPieza = Convert.ToInt32(txtCampo.ID.ToString().Split('_')[1].ToString());


                                    SPListItem itmConfigTipoPieza = lConfigTipoPieza.GetItemById(iConfigTipoPieza);

                                    itmConfigTipoPieza["Title"] = txtCampo.Text.ToString();
                                    itmConfigTipoPieza["Tipo Pieza"] = itmTipoMaterial;
                                    TextBox txtDetalle = tblRowTarea.Cells[1].Controls[0] as TextBox;
                                    itmConfigTipoPieza["Valor Default"] = txtDetalle.Text.ToString();
                                    itmConfigTipoPieza.Update();
                                }

                            
                                else {
                                    SPListItem itmConfigTipoPieza = lConfigTipoPieza.AddItem();

                                    itmConfigTipoPieza["Title"] = txtCampo.Text.ToString();
                                    itmConfigTipoPieza["Tipo Pieza"] = itmTipoMaterial;
                                    TextBox txtDetalle = tblRowTarea.Cells[1].Controls[0] as TextBox;
                                    itmConfigTipoPieza["Valor Default"] = txtDetalle.Text.ToString();
                                    itmConfigTipoPieza.Update();
                                }
                            }

                                
                        }
                        i = i + 1;
                    }



                }
            }
        }

        protected void vAltaTipoMaterial()
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
                            TextBox txtCampo = tblRowTarea.Cells[0].Controls[0] as TextBox;
                            if (txtCampo.Text.ToString().Trim() != "")
                            {
                                SPList lConfigTipoPieza = web.Lists["Configuración Tipo Pieza"];

                                SPListItem itmConfigTipoPieza = lConfigTipoPieza.AddItem();

                                itmConfigTipoPieza["Title"] = txtCampo.Text.ToString();
                                itmConfigTipoPieza["Tipo Pieza"] = itmTipoMaterial;
                                TextBox txtDetalle = tblRowTarea.Cells[1].Controls[0] as TextBox;
                                itmConfigTipoPieza["Valor Default"] = txtDetalle.Text.ToString();
                                itmConfigTipoPieza.Update();
                            }
                        }
                        i = i + 1;
                    }



                }
            }
        }

    }
}
