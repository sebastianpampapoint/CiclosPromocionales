<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TareasPendientes.aspx.cs" Inherits="CiclosPromocionales.Layouts.CiclosPromocionales.TareasPendientes" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:Panel ID="pnlTareasPendientes" runat="server"  CssClass="pnlEdicion">
        <asp:gridview CssClass="ms-listviewtable"  id="gwListaCiclos" allowpaging="True" PagerSettings-Visible="false" autogeneratecolumns="false" runat="server" GridLines="Both" Width="100%" BorderWidth="1" AllowSorting="true" OnSorting="gwListaCiclos_Sorting" OnRowDataBound="gwListaCiclos_RowDataBound">
        <HeaderStyle ForeColor="White" Font-Bold="True" BackColor="#339933" Font-Names="Tahoma" HorizontalAlign="Left" Font-Size="9pt" />
        <RowStyle Font-Size="9pt" Font-Names="Tahoma" BorderStyle="None" BorderWidth="1" BorderColor="Black"  />
        <AlternatingRowStyle CssClass="ms-alternating"/>
            <columns>
                <asp:HyperLinkField DataNavigateUrlFields="Detalle" Text="&lt;img src='/SiteAssets/dashboard.png' alt='alternate text' border='0'/&gt;" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10px" />
                <asp:boundfield datafield="Tarea" SortExpression="Numero" headertext="Tarea" HeaderStyle-HorizontalAlign="Center"  ItemStyle-Width="300px" />
                <asp:boundfield datafield="CicloPromocional" headertext="Ciclo" ItemStyle-Width="400px" SortExpression="CicloPromocional" HeaderStyle-HorizontalAlign="Center"  />
                <asp:boundfield datafield="Material" headertext="Material" ItemStyle-Width="400px" SortExpression="Material" HeaderStyle-HorizontalAlign="Center"  />
                <asp:boundfield datafield="Estado" headertext="Estado" ItemStyle-Width="100px" SortExpression="Estado" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                <asp:boundfield datafield="FechaInicio" SortExpression="FechaInicio" headertext="Alta" HtmlEncode="false" DataFormatString = "{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  />
                <asp:boundfield datafield="FechaVencimiento" SortExpression="FechaVencimiento" headertext="Vencimiento" HtmlEncode="false" DataFormatString = "{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  />
            </columns>
        </asp:gridview> 
    </asp:Panel>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Mis Tareas Pendientes
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Mis Tareas Pendientes
</asp:Content>
