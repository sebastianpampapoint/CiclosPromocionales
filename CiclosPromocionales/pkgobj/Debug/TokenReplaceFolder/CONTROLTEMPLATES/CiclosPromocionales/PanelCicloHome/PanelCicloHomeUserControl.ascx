<%@ Assembly Name="CiclosPromocionales, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d125eceac5c92719" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PanelCicloHomeUserControl.ascx.cs" Inherits="CiclosPromocionales.PanelCicloHome.PanelCicloHomeUserControl" %>
    <link href="../_layouts/CiclosPromocionales/CiclosPromocionales.css" rel="stylesheet" />

<asp:Panel ID="pnlBotonera" runat="server"  CssClass="pnlEdicion">
    <asp:Button ID="btnAltaCicloPromocional" runat="server" Text="Nuevo Ciclo" OnClick="btnAltaCicloPromocional_Click" Width="180px"/>
</asp:Panel>
<br />
<%--<asp:Panel ID="pnlFiltros" runat="server"  CssClass="pnlEdicion">

</asp:Panel>--%>

<asp:Panel ID="pnlCiclosPromocionales" runat="server"  CssClass="pnlEdicion">
        <asp:gridview CssClass="ms-listviewtable"  id="gwListaCiclos" allowpaging="True" PagerSettings-Visible="false" autogeneratecolumns="false" runat="server" GridLines="Both" Width="100%" BorderWidth="1" AllowSorting="true" OnSorting="gwListaCiclos_Sorting" OnRowDataBound="gwListaCiclos_RowDataBound">
        <HeaderStyle ForeColor="White" Font-Bold="True" BackColor="#339933" Font-Names="Tahoma" HorizontalAlign="Left" Font-Size="9pt" />
        <RowStyle Font-Size="9pt" Font-Names="Tahoma" BorderStyle="None" BorderWidth="1" BorderColor="Black"  />
        <AlternatingRowStyle CssClass="ms-alternating"/>
            <columns>
                <asp:HyperLinkField DataNavigateUrlFields="Detalle" Text="&lt;img src='/SiteAssets/dashboard.png' alt='alternate text' border='0'/&gt;" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10px" />
                <asp:boundfield datafield="Numero" SortExpression="Numero" headertext="#" HeaderStyle-HorizontalAlign="Center"  ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center"  />
                <asp:boundfield datafield="CicloPromocional" headertext="Ciclo Promocional" ItemStyle-Width="400px" SortExpression="CicloPromocional" HeaderStyle-HorizontalAlign="Center"  />
                <asp:boundfield datafield="Materiales" headertext="Materiales" HtmlEncode="false" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="true"  />
                <asp:boundfield datafield="Estado" headertext="Estado" ItemStyle-Width="100px" SortExpression="Estado" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                <asp:ImageField DataImageUrlField="Semaforo" ItemStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                <asp:boundfield datafield="FechaInicio" SortExpression="FechaInicio" headertext="Alta" HtmlEncode="false" DataFormatString = "{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  />
                <asp:ImageField DataImageUrlField="Cotiza" headertext="Cotiza" ItemStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
            </columns>
        </asp:gridview> 
    </asp:Panel>
