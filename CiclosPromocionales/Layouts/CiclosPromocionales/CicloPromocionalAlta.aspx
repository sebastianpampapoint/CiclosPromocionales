<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CicloPromocionalAlta.aspx.cs" Inherits="CiclosPromocionales.Layouts.CiclosPromocionales.CicloPromocionalAlta" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link href="CiclosPromocionales.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:Panel ID="pnlCabeceraAlta" runat="server" CssClass="pnlEdicion">
        <div class="divFormulario">
            <asp:Label runat="server" ID="lblTitulo" CssClass="lblTitulo"><strong>Título</strong></asp:Label><br />
            <SharePoint:InputFormTextBox runat="server" Width="80%" ID="txtTitulo"></SharePoint:InputFormTextBox>
            
        </div>
        <div class="divFormulario">
            <asp:Label runat="server" ID="lblDescripcion" CssClass="lblTitulo"><strong>Descripción</strong></asp:Label><br />
            <SharePoint:InputFormTextBox runat="server" Width="80%" TextMode="MultiLine" Rows="3" ID="txtDescripcion"></SharePoint:InputFormTextBox>
        </div>
        <div class="divFormulario">
            <asp:Label runat="server" ID="Label1" CssClass="lblTitulo"><strong>Cotiza?</strong></asp:Label><br />
            <asp:DropDownList ID="ddlCotiza" runat="server">
                <asp:ListItem Selected="True" Value="SI" Text="SI"></asp:ListItem>
                <asp:ListItem Value="NO" Text="NO"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlBotonera" runat="server" CssClass="pnlEdicion">
        <div>
            <asp:Button ID="btnGuardar" runat="server" Text="Siguiente" Class="btnFormulario" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnFormulario" OnClick="btnCancelar_Click" />
        </div>
    </asp:Panel>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Ciclo Promocional - Alta
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Ciclo Promocional - Alta
</asp:Content>
