<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmarEnvioProveedor.aspx.cs" Inherits="CiclosPromocionales.Layouts.CiclosPromocionales.ConfirmarEnvioProveedor" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link href="CiclosPromocionales.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
        <asp:Panel ID="pnlMensaje" runat="server" CssClass="pnlEdicion">
        <div class="divFormulario">
            <asp:Label runat="server" ID="Label1" CssClass="lblTitulo"><strong>Mensaje al Proveedor</strong></asp:Label><br />
            <SharePoint:InputFormTextBox runat="server" Width="90%" TextMode="MultiLine" Rows="25" ID="txtMensaje" ReadOnly="false" RichTextMode="FullHtml" ></SharePoint:InputFormTextBox>
        </div>
        <br />
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
    <asp:HiddenField ID="iProceso" runat="server" EnableViewState="true" />
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Confirmar Envío Proveedor
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Confirmar Envío Proveedor
</asp:Content>
