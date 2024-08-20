<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cotizar.aspx.cs" Inherits="CiclosPromocionales.Layouts.CiclosPromocionales.Cotizar" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link href="CiclosPromocionales.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <asp:Panel ID="pnlCabeceraDocumento" runat="server" CssClass="pnlEdicion">
        <h3 runat="server" style="background-color:cadetblue;color:white" Font-Bold="true">&nbsp;&nbsp;Datos de los Materiales a Cotizar</h3>
        <div class="divFormulario">
            <asp:Label runat="server" ID="lblTitulo" CssClass="lblTitulo"><strong>Título</strong></asp:Label><br />
            <SharePoint:InputFormTextBox runat="server" Width="90%" ID="txtTitulo" ReadOnly="true"></SharePoint:InputFormTextBox>
            
        </div>
        <div class="divFormulario">
            <asp:Label runat="server" ID="lblDescripcion" CssClass="lblTitulo"><strong>Descripción</strong></asp:Label><br />
            <SharePoint:InputFormTextBox runat="server" Width="90%" TextMode="MultiLine" Rows="3" ID="txtDescripcion" ReadOnly="true"></SharePoint:InputFormTextBox>
        </div>
        <div class="divFormulario">
            <asp:Label runat="server" ID="Label1" CssClass="lblTitulo"><strong>Mensaje al Cotizador</strong></asp:Label><br />
            <SharePoint:InputFormTextBox runat="server" Width="90%" TextMode="MultiLine" Rows="3" ID="txtMensaje" ReadOnly="false"></SharePoint:InputFormTextBox>
        </div>
        <asp:Table ID="tblBotonera" runat="server" Width="100%">
                         <asp:TableRow>
                <asp:TableCell runat="server" Width="60%" BorderStyle="None" >
                    <asp:Button ID="btnVolver" runat="server" Text="Volver a Materiales" OnClick="btnVolver_Click" Width="150px"/>
                    <asp:Button ID="btnExportar" runat="server" Text="Enviar a Cotizar" OnClick="btnExportar_Click" Width="150px"/>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell><asp:Label runat="server" ID="lblMensajeError" ForeColor="Red" Visible="true"></asp:Label></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        </asp:Panel>
    <br />
    <asp:Panel ID="pnlBitacoraSolicitudesHistoria" runat="server" CssClass="pnlEdicion">
        <asp:Table ID="tblMateriales" runat="server" Width="100%"></asp:Table>
    </asp:Panel>



    <asp:HiddenField ID="iCicloPromocional" runat="server" EnableViewState="true" />

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Enviar a Cotizar
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Enviar a Cotizar
</asp:Content>
