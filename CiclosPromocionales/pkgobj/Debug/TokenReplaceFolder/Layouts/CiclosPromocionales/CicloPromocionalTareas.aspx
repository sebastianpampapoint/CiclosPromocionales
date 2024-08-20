<%@ Assembly Name="CiclosPromocionales, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d125eceac5c92719" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CicloPromocionalTareas.aspx.cs" Inherits="CiclosPromocionales.Layouts.CiclosPromocionales.CicloPromocionalTareas" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link href="CiclosPromocionales.css" rel="stylesheet" />
    <link href="http://balarshpdes01:1212/_layouts/15/CiclosPromocionales/CiclosPromocionales.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:Panel ID="pnlCabecera" runat="server" CssClass="pnlEdicion">
        <div class="divFormulario">
            <asp:Label runat="server" ID="lblTitulo" CssClass="lblTitulo"><strong>Ciclo</strong></asp:Label><br />
            <SharePoint:InputFormTextBox runat="server" Width="90%" ID="txtTitulo" ReadOnly="true"></SharePoint:InputFormTextBox>
            
        </div>
        <div class="divFormulario">
            <asp:Label runat="server" ID="lblDescripcion" CssClass="lblTitulo"><strong>Descripción</strong></asp:Label><br />
            <SharePoint:InputFormTextBox runat="server" Width="90%" TextMode="MultiLine" Rows="3" ID="txtDescripcion" ReadOnly="true"></SharePoint:InputFormTextBox>
        </div>
        <div class="divFormulario" id="divMaterial" runat="server">
            <table width="100%">
            <tr style="width:100%">
                <td colspan="3">
                    <asp:Label runat="server" ID="lblPieza" CssClass="lblTitulo"><strong>Material</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" ID="txtMaterial" Enabled="false" Width="95%"></SharePoint:InputFormTextBox>
                </td>
                <td style="width:20%">
                    <asp:Label runat="server" ID="lblTipoPieza" CssClass="lblTitulo"><strong>Tipo Material</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" Enabled="false" ID="txtTipoMaterial" Width="90%"></SharePoint:InputFormTextBox>
                </td>
                <td style="width:20%">
                    <asp:Label runat="server" ID="lblResponsable" CssClass="lblTitulo"><strong>Departamento Médico</strong></asp:Label><br />
                    <SharePoint:InputFormTextBox runat="server" Enabled="false" ID="txtDepartamentoMedico" Width="90%"></SharePoint:InputFormTextBox>
                </td>
            </tr>
        </table>
        </div>
         <div class="divFormulario">
                    <asp:Button ID="btnVolver" runat="server" Text="Volver a Materiales" CssClass="btnFormulario" OnClick="btnVolver_Click" />
                    </div>
    </asp:Panel>
   
    <br />
    <asp:Panel ID="pnlAdjuntar" runat="server" CssClass="pnlEdicion">
        <h3 runat="server" style="background-color:cadetblue;color:white" Font-Bold="true">&nbsp;&nbsp;Documento Diseño</h3>
        <asp:Table ID="tblAdjuntarDocumento" runat="server" Width="100%">
            <asp:TableHeaderRow BackColor="#009933" >
                
            </asp:TableHeaderRow>
            <asp:TableRow>
                
                <asp:TableCell>
                    <asp:gridview CssClass="ms-listviewtable"  id="gridAdjuntoMaterial" allowpaging="false" PagerSettings-Visible="false" autogeneratecolumns="false" runat="server" Width="100%" onrowcommand="AvisosPagoAdjuntosGridView_RowCommand" GridLines="None" >
                        <HeaderStyle ForeColor="DarkGray" Font-Bold="True" BackColor="#f3f3f3" Font-Names="Tahoma" HorizontalAlign="Left" Font-Size="8pt" />
                        <RowStyle Font-Size="8pt" Font-Names="Tahoma" BorderStyle="None" BorderWidth="0" BorderColor="White"  />
                        <AlternatingRowStyle CssClass="ms-alternating"/>
                            <columns>
                                <asp:boundfield datafield="Id" headertext="-" Visible="true" ><ItemStyle Font-Size="0" Width="5%" HorizontalAlign="Left" /></asp:boundfield>
                                <asp:HyperLinkField DataNavigateUrlFields="AttachmentURL" DataTextField="AttachmentTitle" headertext="Nombre" Visible="true" ><ItemStyle Width="69%" HorizontalAlign="Left" /></asp:HyperLinkField>
                                <asp:boundfield datafield="Estado" headertext="Estado" Visible="true" ><ItemStyle Width="20%" HorizontalAlign="Left" /></asp:boundfield>
                                <%--<asp:buttonfield ButtonType="Image" CommandName="EliminarAdjunto" DataTextField="Title" headertext=""  ImageUrl="../images/delitem.gif" ><ItemStyle Width="5%" HorizontalAlign="Left" /></asp:buttonfield>--%>
                                <asp:boundfield datafield="AttachmentTitle" headertext="" Visible="true" ><ItemStyle Width="1%" Font-Size="0" ForeColor="White" HorizontalAlign="Left" /></asp:boundfield>
                            </columns>
                        </asp:gridview>
                </asp:TableCell>
                <asp:TableCell  runat="server" Width="40%" BorderStyle="None">
                     <asp:Panel ID="pnlAdjuntarDisenio" runat="server">
                     <asp:FileUpload ID="filUploadAdjunto" runat="server" />
                     <asp:Button runat="server" Id="btnAdjuntar" Text="Cargar" OnClick="btnAdjuntar_Click"/>
                     <br />
                     <asp:Label ID="Label2" runat="server" Text="Seleccione el archivo a procesar y presione el botón Cargar"></asp:Label>
                     </asp:Panel>
                </asp:TableCell>
                </asp:TableRow>
        </asp:Table>  
                       <div class="divFormulario">
                    <asp:Button ID="btnIniciarProceso" runat="server" Text="Iniciar Proceso Aprobación" CssClass="btnFormulario" OnClick="btnIniciarProceso_Click" />
                     &nbsp;&nbsp;&nbsp;      <asp:Label ID="lblReinicio" runat="server" Text="Enviar documento a: " Visible="false"></asp:Label><asp:DropDownList ID="ddlReinicio" runat="server" AutoPostBack="false" Visible="false"></asp:DropDownList>
                    </div> 
                    <div class="divFormulario">
                    <asp:Button ID="btnAnularProceso" runat="server" Text="Anular Proceso Aprobación" CssClass="btnFormulario" OnClick="btnAnularProceso_Click" />
                    </div> 
         </asp:Panel>           

    <br />

    <asp:Panel ID="pnlBitacoraDocumentoActual" runat="server" CssClass="pnlEdicion">
        <h3 runat="server" style="background-color:cadetblue;color:white" Font-Bold="true">&nbsp;&nbsp;Tareas activas</h3>
        <asp:Label ID="Label1" runat="server" Text="Seleccione la tarea a procesar: "></asp:Label><asp:DropDownList ID="ddlSeleccioneTarea"  OnSelectedIndexChanged="ddlSeleccioneTarea_SelectedIndexChanged" runat="server" AutoPostBack="True"></asp:DropDownList>
        <asp:Table ID="tblDatosTareaActual" runat="server" Width="100%">
            <asp:TableHeaderRow BackColor="#009933">
                <asp:TableHeaderCell runat="server" ForeColor="White">Etapa</asp:TableHeaderCell>
                <asp:TableHeaderCell runat="server" ForeColor="White">Estado</asp:TableHeaderCell>
                <asp:TableHeaderCell runat="server" ForeColor="White">Corrector</asp:TableHeaderCell>
                <asp:TableHeaderCell runat="server" ForeColor="White">Fecha de Inicio de tarea</asp:TableHeaderCell>
                <asp:TableHeaderCell runat="server" ForeColor="White">Fecha de Vencimiento</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell runat="server" Width="40%" BorderStyle="None"><SharePoint:InputFormTextBox runat="server" BorderStyle="None" Width="100%" Enabled="false" Font-Bold="true" ID="txtEtapaTarea" ></SharePoint:InputFormTextBox></asp:TableCell>
                <asp:TableCell runat="server" Width="10%" BorderStyle="None"><SharePoint:InputFormTextBox runat="server" BorderStyle="None" Width="100%" Enabled="false" Font-Bold="true" ID="txtEstadoTarea" /></asp:TableCell>
                <asp:TableCell runat="server" Width="20%" BorderStyle="None"><SharePoint:InputFormTextBox runat="server" BorderStyle="None" Width="100%" Enabled="false" Font-Bold="true" ID="txtCorrector" /></asp:TableCell>
                <asp:TableCell runat="server" Width="15%" BorderStyle="None"><SharePoint:InputFormTextBox runat="server" BorderStyle="None" Width="100%" Enabled="false" Font-Bold="true" ID="txtFechaInicio" /></asp:TableCell>
                <asp:TableCell runat="server" Width="15%" BorderStyle="None"><SharePoint:InputFormTextBox runat="server" BorderStyle="None" Width="100%" Enabled="false" Font-Bold="true" ID="txtFechaFin"/></asp:TableCell>
            </asp:TableRow>
        </asp:Table>  
                <asp:Table ID="tblDatosAprobacionTareaActual" runat="server" Width="100%">
            <asp:TableHeaderRow BackColor="#009933">
                <asp:TableHeaderCell runat="server" HorizontalAlign="Left" ForeColor="White">Comentarios:</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell runat="server" Width="100%" BorderStyle="None"><SharePoint:InputFormTextBox runat="server" Width="99%" Enabled="true" ID="txtDatosAprobacion" TextMode="MultiLine" RichText="false" Rows="3" ></SharePoint:InputFormTextBox></asp:TableCell>
                </asp:TableRow>
                   
        </asp:Table>
        <asp:Table ID="Table1" runat="server" Width="100%">
            <asp:TableHeaderRow BackColor="#009933" >
                <asp:TableHeaderCell runat="server" HorizontalAlign="Left" ForeColor="White">Adjuntar Documento:</asp:TableHeaderCell>
                <asp:TableHeaderCell runat="server" HorizontalAlign="Left" ForeColor="White">Documentos Adjuntos:</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell runat="server" Width="40%" BorderStyle="None">
                    <asp:FileUpload ID="FileAdjuntoTarea" runat="server" />
                    <asp:Button runat="server" Id="btnAdjuntarTarea" Text="Adjuntar" OnClick="btnGuardar_Click"/>
                    <br />
                    <asp:Label ID="Label3" runat="server" Text="Seleccione el archivo a adjuntar y presione el botón Adjuntar"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:gridview CssClass="ms-listviewtable"  id="gridAdjuntoTarea" allowpaging="false" PagerSettings-Visible="false" autogeneratecolumns="false" runat="server" Width="100%" onrowcommand="AdjuntosTareasGridView_RowCommand" GridLines="None" >
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
                </asp:TableRow>
        </asp:Table>

        <asp:Table ID="tblDatosAprobacion" runat="server" Width="100%">
            <asp:TableRow><asp:TableCell>
                <asp:Button runat="server" Id="btnGuardar" Text="Guardar Borrador" Width="150px" OnClick="btnGuardar_Click"/>
                <asp:Button runat="server" Id="btnCompletar" Text="Completar Tarea" Width="150px" OnClick="btnAprobar_Click" />
                <asp:Button runat="server" Id="btnAprobar" Text="Aprobar" Width="150px" OnClick="btnAprobar_Click" />
                <asp:Button runat="server" Id="btnRechazar" Text="Correcciones" Width="150px" OnClick="btnRechazar_Click" />
                &nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblTareaSiguiente" runat="server" Text="Enviar documento a: "></asp:Label><asp:DropDownList ID="ddlTareaSiguiente" runat="server" AutoPostBack="false"></asp:DropDownList>
                
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table runat="server" ID="tblError">
            <asp:TableRow>
                <asp:TableCell><asp:Label runat="server" ID="lblMensajeError" ForeColor="Red" Visible="true"></asp:Label></asp:TableCell>
            </asp:TableRow>
        </asp:Table>

    </asp:Panel>

    <br />
    <asp:Panel ID="pnlBitacoraDocumentoHistoria" runat="server" CssClass="pnlEdicion">
        <h2 runat="server" style="background-color:cadetblue;color:white" Font-Bold="true">&nbsp;&nbsp;Historial de Tareas</h2>
        <asp:Table ID="tblHistorialTareas" runat="server" Width="100%"></asp:Table>
    </asp:Panel>
    <asp:HiddenField ID="iMaterial" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="iCicloPromocional" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="IdTareaBitacora" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="bTieneDocumento" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="AdjuntoObligatorio" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="strEstadoDocumento" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="strEstadoCotizacion" runat="server" EnableViewState="true" />
</asp:Content>


<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Ciclo Promocional - Panel de Tareas
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Ciclo Promocional - Panel de Tareas
</asp:Content>
