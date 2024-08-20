<%@ Assembly Name="CiclosPromocionales, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d125eceac5c92719" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CicloPromocionalDetallePieza.aspx.cs" Inherits="CiclosPromocionales.Layouts.CiclosPromocionales.CicloPromocionalDetallePieza" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link href="CiclosPromocionales.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:Panel ID="pnlCabecera" runat="server" CssClass="pnlEdicion">
        <div class="divFormulario">
            <asp:Label runat="server" ID="lblTitulo" CssClass="lblTitulo"><strong>Título</strong></asp:Label><br />
            <SharePoint:InputFormTextBox runat="server" Width="80%" ID="txtTitulo" ReadOnly="true"></SharePoint:InputFormTextBox>
            
        </div>

    </asp:Panel>
    <br />
    <asp:Panel ID="pnlEdicionPieza" runat="server" CssClass="pnlEdicion">
        <table width="100%">
            <tr style="width:100%">
                <td rowspan="2" style="width:20%">
                    <asp:Label runat="server" ID="lblProductoPieza" CssClass="lblTitulo"><strong>Producto</strong></asp:Label><br />
                    <asp:ListBox ID="llbProducto" runat="server" SelectionMode="Multiple" Enabled="false" Width="95%"></asp:ListBox>
                </td>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblPieza" CssClass="lblTitulo"><strong>Material</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" ID="txtPieza" Enabled="false" Width="95%"></SharePoint:InputFormTextBox>
                </td>
                <td style="width:20%">
                    <asp:Label runat="server" ID="lblTipoPieza" CssClass="lblTitulo"><strong>Tipo Material</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" Enabled="false" ID="txtTipoMaterial" Width="90%"></SharePoint:InputFormTextBox>
                </td>
                <td style="width:20%">
                    <asp:Label runat="server" ID="lblCodigoSAP" CssClass="lblTitulo"><strong>Código SAP</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" Enabled="false" ID="txtCodigoSAP" Width="90%"></SharePoint:InputFormTextBox>
                </td>
            </tr>
            <tr style="width:100%">
                <td colspan="3">
                    <asp:Label runat="server" ID="lblComentarios" CssClass="lblTitulo"><strong>Comentarios</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" Enabled="false" ID="txtComentarios" TextMode="MultiLine" Rows="2" Width="95%"></SharePoint:InputFormTextBox>
                </td>
                <td style="width:20%">
                    <asp:Label runat="server" ID="lblCantidad" CssClass="lblTitulo"><strong>Cantidad</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" Enabled="false" ID="txtCantidad" Width="90%"></SharePoint:InputFormTextBox>
                </td>
                <td style="width:20%">
                    <asp:Label runat="server" ID="lblResponsable" CssClass="lblTitulo"><strong>Departamento Médico</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" Enabled="false" ID="txtDepartamentoMedico" Width="90%"></SharePoint:InputFormTextBox>
                </td>
            </tr>
            <tr style="width:100%">
                <td style="width:20%">
                    <asp:Label runat="server" ID="Label1" CssClass="lblTitulo"><strong>Alternativo 1</strong></asp:Label><br />
                    <asp:CheckBox ID="chbAlternativo1" runat="server" Checked="false"/>
                </td>
                <td colspan="3">
                    <asp:Label runat="server" ID="Label3" CssClass="lblTitulo"><strong>Alternativo 2</strong></asp:Label><br />
                    <asp:CheckBox ID="chbAlternativo2" runat="server" Checked="false"/>
                </td>
                <td style="width:20%">
                    <asp:Label runat="server" ID="Label8" CssClass="lblTitulo"><strong>Estado Cotización</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" Enabled="false" ID="txtEstadoCotizacion" Width="90%"></SharePoint:InputFormTextBox>
                </td>
                <td style="width:20%">
                    <asp:Label runat="server" ID="Label4" CssClass="lblTitulo"><strong>Estado Documento</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" Enabled="false" ID="txtEstadoDocumento" Width="90%"></SharePoint:InputFormTextBox>
                </td>
            </tr>
        </table>
<div class="divFormulario">
        <asp:Table ID="tblAdjuntarDocumento" runat="server" Width="100%">
            <asp:TableHeaderRow BackColor="#009933" >
                <asp:TableHeaderCell runat="server" ColumnSpan="2" HorizontalAlign="Left" ForeColor="White">Documentos Adjuntos</asp:TableHeaderCell>
                <%--<asp:TableHeaderCell runat="server" HorizontalAlign="Left" ForeColor="White"></asp:TableHeaderCell>--%>
            </asp:TableHeaderRow>
            <asp:TableRow>
                
                <asp:TableCell>
                    <asp:gridview CssClass="ms-listviewtable"  id="gridAdjuntoMaterial" allowpaging="false" PagerSettings-Visible="false" autogeneratecolumns="false" runat="server" Width="100%" onrowcommand="AvisosPagoAdjuntosGridView_RowCommand" GridLines="None" >
                        <HeaderStyle ForeColor="DarkGray" Font-Bold="True" BackColor="#f3f3f3" Font-Names="Tahoma" HorizontalAlign="Left" Font-Size="8pt" />
                        <RowStyle Font-Size="8pt" Font-Names="Tahoma" BorderStyle="None" BorderWidth="0" BorderColor="White"  />
                        <AlternatingRowStyle CssClass="ms-alternating"/>
                            <columns>
                            <asp:HyperLinkField DataNavigateUrlFields="AttachmentURL" DataTextField="AttachmentTitle" headertext="Nombre" Visible="true" ><ItemStyle Width="89%" HorizontalAlign="Left" /></asp:HyperLinkField>
                            <asp:buttonfield ButtonType="Image" CommandName="VerAdjunto" DataTextField="AttachmentURL" headertext=""  ImageUrl="../images/open.gif" Visible="false"><ItemStyle Width="5%" HorizontalAlign="Left" /></asp:buttonfield>
                            <asp:buttonfield ButtonType="Image" CommandName="EliminarAdjunto" DataTextField="Title" headertext=""  ImageUrl="../images/delitem.gif" ><ItemStyle Width="5%" HorizontalAlign="Left" /></asp:buttonfield>
                            <asp:boundfield datafield="AttachmentTitle" headertext="" Visible="true" ><ItemStyle Width="1%" Font-Size="0" ForeColor="White" HorizontalAlign="Left" /></asp:boundfield>
                            </columns>
                        </asp:gridview>
                </asp:TableCell>
                <asp:TableCell  runat="server" Width="40%" BorderStyle="None">
                     <asp:Panel ID="pnlAdjuntar" runat="server">
        
                        
                    <asp:FileUpload ID="filUploadAdjunto" runat="server" />
                    <asp:Button runat="server" Id="btnAdjuntar" Text="Adjuntar" OnClick="btnAdjuntar_Click"/>
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="Seleccione el archivo a adjuntar y presione el botón Adjuntar"></asp:Label>
                </asp:Panel>
                         </asp:TableCell>
                </asp:TableRow>
        </asp:Table>    
        </div>
    </asp:Panel>
    
    <br />
    <asp:Panel ID="pnlOpciones" runat="server" CssClass="pnlEdicion" >
    <asp:Table ID="tblCheckList" runat="server" Width="100%" ViewStateMode="Enabled" EnableViewState="true" >
                <asp:TableHeaderRow BackColor="#008080" ForeColor="White">
                    <asp:TableHeaderCell Width="10%">-</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="30%">Detalle</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="30%">Alternativo 1</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="30%">Alternativo 2</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlCotizacion" runat="server" CssClass="pnlEdicion">
        <table width="100%">
            <tr style="width:100%">
                <td style="width:20%">
                    <asp:Label runat="server" ID="Label5" CssClass="lblTitulo"><strong>Estado Cotización</strong></asp:Label><br />
                    <asp:DropDownList ID="ddlResultado" runat="server" CssClass="ms-input" Width="95%" >
                        <asp:ListItem Text="<-- Seleccione -->" Value="0" Selected="True" />
                        <asp:ListItem Text="Aprobado" Value="Aprobado" Selected="False" />
                        <asp:ListItem Text="Rechazado" Value="Rechazado" Selected="False" />
                    </asp:DropDownList>
                </td>
                <td style="width:20%">
                    <asp:Label runat="server" ID="Label6" CssClass="lblTitulo"><strong>Opción Seleccionada</strong></asp:Label><br />
                    <asp:DropDownList ID="ddlOpcionSeleccionada" runat="server" CssClass="ms-input" Width="95%" >
                        <asp:ListItem Text="<-- Seleccione -->" Value="0" Selected="True" />
                        <asp:ListItem Text="Detalle Original" Value="Detalle"  />
                        <asp:ListItem Text="Alternativo 1" Value="Alternativo 1"  />
                        <asp:ListItem Text="Alternativo 2" Value="Alternativo 2"  />
                    </asp:DropDownList>
                </td>
                <td style="width:60%">
                    <asp:Label runat="server" ID="Label7" CssClass="lblTitulo"><strong>Proveedor Seleccionado</strong></asp:Label><br />
                    <asp:DropDownList ID="ddlProveedor" runat="server" CssClass="ms-input" Width="95%" >
                        <asp:ListItem Text="<-- Seleccione -->" Value="0" Selected="True" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <br />
    <asp:Panel ID="pnlBotonera" runat="server" CssClass="pnlEdicion">
        <div class="div-table">
    <div class="div-table-row"  style="width:100%">
                <div class="div-table-col-2">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnFormulario" OnClick="btnGuardar_Click" />
                    </div>
        <div class="div-table-col-2">
                    <asp:Button ID="btnGuardarDetalle" runat="server" Text="Guardar Detalle Como..." CssClass="btnFormulario" onclick="btnGuardarDetalle_Click" />
                    </div>
        <div class="div-table-col-2" style="align-content:flex-end">
                    <asp:Button ID="btnPanelTareas" runat="server" Text="Panel de Tareas" CssClass="btnFormulario" OnClick="btnPanelTareas_Click"/>
                    </div>
        
                    <div class="div-table-col-2">
                    <asp:Button ID="btnVolver" runat="server" Text="Volver a Materiales" CssClass="btnFormulario" OnClick="btnVolver_Click" />
                    </div>
        </div>
            </div>
    </asp:Panel>
    <asp:HiddenField ID="iMaterial" runat="server" EnableViewState="true" />
            <asp:HiddenField ID="iCicloPromocional" runat="server" EnableViewState="true" />

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Detalle Material Promocional
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Detalle Material Promocional
</asp:Content>
