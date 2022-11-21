<%@ Page Language="C#" MasterPageFile="~/PageViews/Default/MasterPageMain.master" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphFull" Runat="Server">
<h5>
本示例演示了数据源绑定Web控件DataGrid。
</h5>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeft" Runat="Server">
<UC:DemoControl ID="ControllerList" ControllerName="UseViewData" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRight" Runat="Server">
<asp:DataGrid ID="dgdDemo" runat="server" />
</asp:Content>

