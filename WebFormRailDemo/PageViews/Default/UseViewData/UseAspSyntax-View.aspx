<%@ Page Language="C#" MasterPageFile="~/PageViews/Default/MasterPageMain.master" Title="完全支持像asp.net mvc的asp.net语法" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphFull" Runat="Server">
<h5>
说明：<br />
在aspx页面上，与asp.net mvc 一样，可以完全的使用asp.net的语法来写。
</h5>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeft" Runat="Server">
<UC:DemoControl ID="ControllerList" ControllerName="UseViewData" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRight" Runat="Server">
<div class="Content">
<%if(GetViewData<bool>("Show")){%>
<h1>这里显示内容</h1>
<%} %>
<%if (GetViewData<bool>("Show"))
  {
      Write("<h1>这里显示内容</h1>");
  }%>
<%for (int i = 0; i < GetViewData<int>("Loop"); i++)
  { %>
loop<%=i%><br />
<%} %>
</div>
</asp:Content>

