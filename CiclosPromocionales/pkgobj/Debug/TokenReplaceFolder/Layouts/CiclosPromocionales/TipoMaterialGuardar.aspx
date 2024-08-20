<%@ Assembly Name="CiclosPromocionales, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d125eceac5c92719" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TipoMaterialGuardar.aspx.cs" Inherits="CiclosPromocionales.Layouts.CiclosPromocionales.TipoMaterialGuardar" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link href="CiclosPromocionales.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
        <asp:Panel ID="pnlCabecera" runat="server" CssClass="pnlEdicion">
        <div class="divFormulario">
            <asp:Label runat="server" ID="lblTitulo" CssClass="lblTitulo"><strong>Título</strong></asp:Label><br />
            <SharePoint:InputFormTextBox runat="server" Width="80%" ID="txtTitulo"></SharePoint:InputFormTextBox>
            
        </div>

    </asp:Panel>
        <asp:Panel ID="pnlOpciones" runat="server" CssClass="pnlEdicion">
    <asp:Table ID="tblCheckList" runat="server" Width="100%" ViewStateMode="Enabled" EnableViewState="true">
                <asp:TableHeaderRow BackColor="#008080" ForeColor="White">
                    <asp:TableHeaderCell Width="10%">-</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="30%">Detalle</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlBotonera" runat="server" CssClass="pnlEdicion">
        <div class="div-table">
    <div class="div-table-row"  style="width:100%">
                <div class="div-table-col-2">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnFormulario" OnClick="btnGuardar_Click" />
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
Guardar Tipo Material 
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Guardar Tipo Material 
</asp:Content>
