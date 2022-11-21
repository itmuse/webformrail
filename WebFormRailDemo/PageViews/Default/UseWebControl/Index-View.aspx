<%@ Page Language="C#" MasterPageFile="~/PageViews/Default/MasterPageMain.master" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphFull" Runat="Server">
<h5>
说明：<br />
WebFormRail完整地支持WEB控件的运用。在Controller的c#代码里，通过对象View可以直接访问aspx页面的控件，包括设置页面标题（title）等。<br />
要访问aspx页面的某个控件，可以这样的方式：View.FindControl&lt;web control type&gt;("web control id")。如：
<code>
if(!IsPost())
    View.FindControl&lt;Label&gt;("lblText").Text = "Hello world!";
View.FindControl&lt;Button&gt;("btnSubmit").Click += new EventHandler(UseWebControlController_Click);

DataGrid dataGrid = View.FindControl&lt;DataGrid&gt;("ggdDataDemo");
dataGrid.DataSource = GetDataTable();
dataGrid.DataBind();
</code>
</h5>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeft" Runat="Server">
<UC:DemoControl ID="ControllerList" ControllerName="UseWebControl" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRight" Runat="Server">
<asp:Label ID="lblText" runat="server" />
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" /><br />
    <asp:TextBox ID="tbxName" runat="server" />
    <hr />
    <asp:DataGrid ID="ggdDataDemo" runat="server" ></asp:DataGrid>
</asp:Content>

