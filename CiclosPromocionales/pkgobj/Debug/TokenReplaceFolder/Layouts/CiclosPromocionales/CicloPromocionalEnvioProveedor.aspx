<%@ Assembly Name="CiclosPromocionales, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d125eceac5c92719" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CicloPromocionalEnvioProveedor.aspx.cs" Inherits="CiclosPromocionales.Layouts.CiclosPromocionales.CicloPromocionalEnvioProveedor" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link href="CiclosPromocionales.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
        <asp:Panel ID="pnlCabeceraDocumento" runat="server" CssClass="pnlEdicion">
        <h3 runat="server" style="background-color:cadetblue;color:white" Font-Bold="true">&nbsp;&nbsp;Datos de los Materiales a Enviar</h3>
        <div class="divFormulario">
            <asp:Label runat="server" ID="lblTitulo" CssClass="lblTitulo"><strong>Título</strong></asp:Label><br />
            <SharePoint:InputFormTextBox runat="server" Width="90%" ID="txtTitulo" ReadOnly="true"></SharePoint:InputFormTextBox>
            
        </div>
        <div class="divFormulario">
            <asp:Label runat="server" ID="lblDescripcion" CssClass="lblTitulo"><strong>Descripción</strong></asp:Label><br />
            <SharePoint:InputFormTextBox runat="server" Width="90%" TextMode="MultiLine" Rows="3" ID="txtDescripcion" ReadOnly="true"></SharePoint:InputFormTextBox>
        </div>
        <div class="divFormulario">
            <asp:Label runat="server" ID="Label7" CssClass="lblTitulo"><strong>Proveedor Seleccionado</strong></asp:Label><br />
                    <asp:DropDownList ID="ddlProveedor" AutoPostBack="true" runat="server" CssClass="ms-input" Width="50%" OnSelectedIndexChanged="ddlProveedor_SelectedIndexChanged" >
                        <asp:ListItem Text="<-- Seleccione -->" Value="0" Selected="True" />
                    </asp:DropDownList>
        </div>
        
        </asp:Panel>
    <br />
    <asp:Panel ID="pnlBitacoraSolicitudesHistoria" runat="server" CssClass="pnlEdicion">
        <asp:Panel ID="pnlListaPiezas" runat="server" CssClass="pnlEdicion">
        <asp:Label runat="server" ID="Label1" CssClass="lblTitulo"><strong>Piezas Pendientes de Envío</strong></asp:Label><br />
        <asp:gridview CssClass="ms-listviewtable"  id="PiezasGridView" allowpaging="True" PagerSettings-Visible="false" autogeneratecolumns="false" runat="server" GridLines="Both" Width="95%" AutoGenerateSelectButton="False">
        <HeaderStyle ForeColor="DarkGray" Font-Bold="True" BackColor="#f3f3f3" Font-Names="Tahoma" HorizontalAlign="Left" Font-Size="10pt" />
        <RowStyle Font-Size="10pt" Font-Names="Tahoma" BorderStyle="Solid" BorderWidth="1" BorderColor="#339966"  />
        <AlternatingRowStyle CssClass="ms-alternating"/>
            <columns>
                <asp:boundfield datafield="ID" headertext="-" Visible="true" ItemStyle-ForeColor="White"><ItemStyle Width="1px" HorizontalAlign="Left" />

                </asp:boundfield>
                <asp:TemplateField HeaderText="Producto">
                    <ItemTemplate>
                        <%# RemoveCharacters(Eval("Producto_x003a_T_x00ed_tulo").ToString())%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:boundfield datafield="Title" headertext="Material"  />
                <asp:boundfield datafield="Tipo_x0020_Pieza" headertext="Tipo Material"  />
                <asp:boundfield datafield="C_x00f3_digo_x0020_SAP" headertext="Código SAP"  />
                <asp:boundfield datafield="Cantidad" headertext="Cantidad"  />
                <asp:boundfield datafield="DM" headertext="DM"  />
                <asp:boundfield datafield="Estado" headertext="Estado"  />
                <asp:boundfield datafield="Estado_x0020_Documento" headertext="Documento"  />
                <asp:TemplateField HeaderText="Enviar"  >
                    <ItemTemplate >
                        <asp:CheckBox  runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                

            </columns>
        </asp:gridview> 
    </asp:Panel>
    <br />
        <asp:Panel ID="pnlListaPiezasEnviadas" runat="server" CssClass="pnlEdicion">
        <asp:Label runat="server" ID="Label2" CssClass="lblTitulo"><strong>Piezas Envíadas</strong></asp:Label><br />
        <asp:gridview CssClass="ms-listviewtable"  id="PiezasEnviadasView" allowpaging="True" PagerSettings-Visible="false" autogeneratecolumns="false" runat="server" GridLines="Both" Width="95%" AutoGenerateSelectButton="False">
        <HeaderStyle ForeColor="DarkGray" Font-Bold="True" BackColor="#f3f3f3" Font-Names="Tahoma" HorizontalAlign="Left" Font-Size="10pt" />
        <RowStyle Font-Size="10pt" Font-Names="Tahoma" BorderStyle="Solid" BorderWidth="1" BorderColor="#339966"  />
        <AlternatingRowStyle CssClass="ms-alternating"/>
            <columns>
                <asp:boundfield datafield="ID" headertext="-" Visible="true" ItemStyle-ForeColor="White"><ItemStyle Width="1px" HorizontalAlign="Left" />

                </asp:boundfield>
                <asp:TemplateField HeaderText="Producto">
                    <ItemTemplate>
                        <%# RemoveCharacters(Eval("Producto_x003a_T_x00ed_tulo").ToString())%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:boundfield datafield="Title" headertext="Material"  />
                <asp:boundfield datafield="Tipo_x0020_Pieza" headertext="Tipo Material"  />
                <asp:boundfield datafield="C_x00f3_digo_x0020_SAP" headertext="Código SAP"  />
                <asp:boundfield datafield="Cantidad" headertext="Cantidad"  />
                <asp:boundfield datafield="DM" headertext="DM"  />
                <asp:boundfield datafield="Estado" headertext="Estado"  />
                <asp:boundfield datafield="Estado_x0020_Documento" headertext="Documento"  />
                <asp:boundfield datafield="Fecha_x0020_Env_x00ed_o_x0020_Pr" headertext="Fecha Envío"  />
                
                

            </columns>
        </asp:gridview> 
    </asp:Panel>

    </asp:Panel>
    <br />
    <asp:Panel ID="pnlMensaje" runat="server" CssClass="pnlEdicion">
        <asp:Table ID="tblBotonera" runat="server" Width="100%">
                         <asp:TableRow>
                <asp:TableCell runat="server" Width="60%" BorderStyle="None" >
                    <asp:Button ID="btnVolver" runat="server" Text="Volver a Materiales" OnClick="btnVolver_Click" Width="150px"/>
                    <asp:Button ID="btnExportar" runat="server" Text="Enviar a Proveedor" OnClick="btnExportar_Click"  Width="150px"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>


    <asp:HiddenField ID="iCicloPromocional" runat="server" EnableViewState="true" />
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Ciclo Promocional - Envío a Proveedor
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Ciclo Promocional - Envío a Proveedor
</asp:Content>
