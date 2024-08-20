<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CicloPromocionalPiezas.aspx.cs" Inherits="CiclosPromocionales.Layouts.CiclosPromocionales.CicloPromocionalPiezas" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link href="CiclosPromocionales.css" rel="stylesheet" />
    </asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:Panel ID="pnlCabecera" runat="server" CssClass="pnlEdicion">
        <div class="divFormulario">
            <table width="100%">
                <tr style="width:100%">
                    <td style="width:70%">
                        
                        <asp:Label runat="server" ID="lblTitulo" CssClass="lblTitulo"><strong>Título</strong></asp:Label><br />
                        <SharePoint:InputFormTextBox runat="server" Width="90%" ID="txtTitulo" ReadOnly="true"></SharePoint:InputFormTextBox>
                    </td>
                    <td style="width:30%">
                        <asp:Label runat="server" ID="lblCotiza" CssClass="lblTitulo"><strong>Cotiza</strong></asp:Label><br />
                        <SharePoint:InputFormTextBox runat="server" Width="90%" ID="txtCotiza" ReadOnly="true"></SharePoint:InputFormTextBox>
                    </td>
                </tr>
                <tr style="width:100%">
                    <td style="width:70%" rowspan="2">
                        <asp:Label runat="server" ID="lblDescripcion" CssClass="lblTitulo"><strong>Descripción</strong></asp:Label><br />
            <SharePoint:InputFormTextBox runat="server" Width="90%" TextMode="MultiLine" Rows="3" ID="txtDescripcion" ReadOnly="true"></SharePoint:InputFormTextBox>
        
                        </td>
                    <td style="width:30%">
                        <asp:Label runat="server" ID="lblEstadoCotiza" CssClass="lblTitulo"><strong>Estado Cotización</strong></asp:Label><br />
                        <SharePoint:InputFormTextBox runat="server" Width="90%" ID="txtEstadoCotización" ReadOnly="true"></SharePoint:InputFormTextBox>
                    </td>
                </tr>
                <tr style="width:100%">
                    <td style="width:30%">
                        <asp:Label runat="server" ID="lblLinkCotización" CssClass="lblTitulo"><strong>Cotización</strong></asp:Label><br />
                        <asp:HyperLink ID="hDocumento" runat="server"></asp:HyperLink>
                    </td>
                </tr>
            </table>
            
        </div>
        <div class="divFormulario">
            </div>
        <div class="divFormulario">
        <asp:Table ID="tblAdjuntarDocumento" runat="server" Width="100%">
            <asp:TableHeaderRow BackColor="#009933" >
                <asp:TableHeaderCell runat="server" ColumnSpan="2" HorizontalAlign="Left" ForeColor="White">Documentos Adjuntos</asp:TableHeaderCell>
                <%--<asp:TableHeaderCell runat="server" HorizontalAlign="Left" ForeColor="White"></asp:TableHeaderCell>--%>
            </asp:TableHeaderRow>
            <asp:TableRow>
                
                <asp:TableCell>
                    <asp:gridview CssClass="ms-listviewtable"  id="gridAdjuntoCiclo" allowpaging="false" PagerSettings-Visible="false" autogeneratecolumns="false" runat="server" Width="100%" onrowcommand="AvisosPagoAdjuntosGridView_RowCommand" GridLines="None" >
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
                <asp:TableRow>
                    <asp:TableCell>
                    <asp:Button runat="server" Id="btnGenerarExcel" Text="Generar Excel" CssClass="btnFormulario" OnClick="btnGenerarExcel_Click"/>    
                    </asp:TableCell>
                </asp:TableRow>
        </asp:Table>    
        </div>
    </asp:Panel>
   

    <br />
    <asp:Panel ID="pnlListaPiezas" runat="server" CssClass="pnlEdicion">
        <asp:gridview CssClass="ms-listviewtable"  id="PiezasGridView" allowpaging="True" PagerSettings-Visible="false" autogeneratecolumns="false" runat="server" onrowcommand="GridViewPiezas_RowCommand" OnRowDataBound="PiezasGridView_RowDataBound" GridLines="Both" Width="100%">
        <HeaderStyle ForeColor="DarkGray" Font-Bold="True" BackColor="#f3f3f3" Font-Names="Tahoma" HorizontalAlign="Left" Font-Size="10pt" />
        <RowStyle Font-Size="10pt" Font-Names="Tahoma" BorderStyle="Solid" BorderWidth="1" BorderColor="#339966"  />
        <AlternatingRowStyle CssClass="ms-alternating"/>
            <columns>
                <asp:boundfield datafield="ID" headertext="-" Visible="true" ItemStyle-ForeColor="White">
                    
                    <ItemStyle Width="1px" HorizontalAlign="Left">
                       
                    </ItemStyle>

                </asp:boundfield>
                <asp:buttonfield ButtonType="Image" CommandName="EditarProducto" DataTextField="ID" headertext=""  ImageUrl="../images/edititem.gif" ><ItemStyle Width="20px" HorizontalAlign="Center" /></asp:buttonfield>
                <asp:TemplateField HeaderText="Producto">
                    <ItemTemplate>
                        <%# RemoveCharacters(Eval("Producto_x003a_T_x00ed_tulo").ToString())%>
                    </ItemTemplate>
                    <ItemStyle Width="300px" /> 
                </asp:TemplateField>
                <asp:boundfield datafield="Title" headertext="Material" />
                <asp:boundfield datafield="Tipo_x0020_Pieza" headertext="Tipo Material"  />
                <asp:boundfield datafield="C_x00f3_digo_x0020_SAP" headertext="Código SAP" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" />
                <asp:boundfield datafield="Cantidad" headertext="Cantidad" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" />
                <asp:boundfield datafield="DM" headertext="DM" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:boundfield datafield="Estado" headertext="Estado Cotización" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" />
                <asp:boundfield datafield="Proveedor" headertext="Imprenta" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                <asp:boundfield datafield="Estado_x0020_Documento" headertext="Estado Diseño" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80px"/>
                
                <asp:buttonfield ButtonType="Image" CommandName="VerDetalle" DataTextField="ID" headertext=""  ImageUrl="../images/configcolumns.png" ><ItemStyle Width="20px" HorizontalAlign="Center" />
                    
                </asp:buttonfield>
                <asp:buttonfield ButtonType="Image" CommandName="VerTareas" DataTextField="ID" headertext=""  ImageUrl="../images/taskdone.gif" ><ItemStyle Width="20px" HorizontalAlign="Center" /></asp:buttonfield>
                <asp:boundfield datafield="Detalle" headertext="-">
                    <ItemStyle Width="1px" HorizontalAlign="Left" Font-Size="1">
                       
                    </ItemStyle>
                </asp:boundfield>
            </columns>
        </asp:gridview> 
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlEdicionPieza" runat="server" CssClass="pnlEdicion">
        <table width="100%">
            <tr style="width:100%">
                <td rowspan="4" style="width:20%;height:100px" >
                    <asp:Label runat="server" ID="lblProductoPieza" CssClass="lblTitulo"><strong>Producto</strong></asp:Label><br />
                    <asp:ListBox ID="llbProducto" runat="server" SelectionMode="Multiple" Width="95%" Style="height:90%;"></asp:ListBox>
                </td>
                <td rowspan="4" style="width:10%">
                    <asp:Label runat="server" ID="Label3" CssClass="lblTitulo"><strong></strong></asp:Label><br />
                   <asp:Button ID="btnAgregarProducto" runat="server" Text="Agregar -->" OnClick="btnAgregarProducto_Click" Width="75%" /><br /><br />
                    <asp:Button ID="btnSacarProducto" runat="server" Text="<-- Quitar" OnClick="btnSacarProducto_Click" Width="75%" />

                </td>
                <td rowspan="4" style="width:20%;height:100px">
                    <asp:Label runat="server" ID="Label1" CssClass="lblTitulo"><strong>Producto</strong></asp:Label><br />
                    <asp:ListBox ID="llbProductoSeleccionado" runat="server" SelectionMode="Multiple" Style="height:90%;" Width="95%"></asp:ListBox>
                </td>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblPieza" CssClass="lblTitulo"><strong>Material</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" ID="txtPieza" Width="95%"></SharePoint:InputFormTextBox>
                </td>
                </tr>
            <tr>
                <td style="width:20%">
                    <asp:Label runat="server" ID="lblTipoPieza" CssClass="lblTitulo"><strong>Tipo Material</strong></asp:Label><br />
                    <asp:DropDownList ID="ddlTipoPieza" runat="server" CssClass="ms-input" Width="95%" >
                        <asp:ListItem Text="<-- Seleccione -->" Value="0" Selected="True" />
                    </asp:DropDownList>
                </td>
                <td style="width:20%">
                    <asp:Label runat="server" ID="lblCodigoSAP" CssClass="lblTitulo"><strong>Código SAP</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" ID="txtCodigoSAP" Width="90%"></SharePoint:InputFormTextBox>
                </td>
            </tr>
            <tr style="width:100%">
                <td style="width:20%">
                    <asp:Label runat="server" ID="lblCantidad" CssClass="lblTitulo"><strong>Cantidad</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" ID="txtCantidad" Width="90%"></SharePoint:InputFormTextBox>
                </td>
                <td style="width:20%">
                    <asp:Label runat="server" ID="lblResponsable" CssClass="lblTitulo"><strong>Departamento Médico</strong></asp:Label><br />
                    <asp:DropDownList ID="ddlResponsable" runat="server" CssClass="ms-input" Width="95%">
                        <asp:ListItem Text="NO" Value="NO" Selected="True"  />
                        <asp:ListItem Text="SI" Value="SI"  />
                    </asp:DropDownList>
                </td>
                </tr>
            <tr style="width:100%">
                <td colspan="3">
                    <asp:Label runat="server" ID="lblComentarios" CssClass="lblTitulo"><strong>Comentarios</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="2" Width="95%"></SharePoint:InputFormTextBox>
                </td>
                
            </tr>
        </table>
        <div class="div-table">
            <div class="div-table-row" style="width:100%">
                <div class="div-table-col-2">
                    <asp:Button ID="btnGuardar" runat="server" Text="Agregar" CssClass="btnFormulario" OnClick="btnGuardar_Click" />
                    </div>
            <div class="div-table-col-2">
                    <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" CssClass="btnFormulario" OnClick="btnActualizar_Click" />
                    </div>
            <div class="div-table-col-2">
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btnFormulario" OnClick="btnEliminar_Click" />
                    </div>
                </div>
        </div>
    </asp:Panel>
    <br />

        <asp:Panel ID="pnlBotonera" runat="server" CssClass="pnlEdicion">
            <div class="div-table">
    <div class="div-table-row">
                <div class="div-table-col-2">
                    <asp:Button ID="btnIniciarProceso" runat="server" Text="Iniciar Proceso" CssClass="btnFormulario" OnClick="btnIniciarProceso_Click" />
                    </div>
                        <div class="div-table-col-2" style="align-content:flex-end">
                    <asp:Button ID="btnEnviarCotizar" runat="server" Text="Enviar a Cotizar" CssClass="btnFormulario" OnClick="btnEnviarCotizar_click"/>
                    </div>
        <div class="div-table-col-2" style="align-content:flex-end">
                    <asp:Button ID="btnPanelTareas" runat="server" Text="Panel de Tareas" CssClass="btnFormulario" OnClick="btnPanelTareas_Click"/>
                    </div>
        <div class="div-table-col-2" style="align-content:flex-end">
                    <asp:Button ID="btnEnviarProveedor" runat="server" Text="Envío a Proveedor" CssClass="btnFormulario" OnClick="btnEnviarProveedor_Click" />
                    </div>
            </div>
                </div>
            <asp:HiddenField ID="iMaterial" runat="server" EnableViewState="true" />
            <asp:HiddenField ID="iCicloPromocional" runat="server" EnableViewState="true" />
            <asp:HiddenField ID="sEstadoCotizacion" runat="server" EnableViewState="true" />
            <asp:HiddenField ID="hdnTipoPieza" runat="server" EnableViewState="true" />
    </asp:Panel>


    

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Ciclo Promocional - Materiales
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Ciclo Promocional - Materiales
</asp:Content>
