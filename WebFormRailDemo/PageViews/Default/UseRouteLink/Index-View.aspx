<%@ Page Language="C#" MasterPageFile="~/PageViews/Default/MasterPageMain.master" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphFull" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeft" Runat="Server">
<UC:DemoControl ID="ControllerList" ControllerName="UseRouteLink" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRight" Runat="Server">
<h3>Current action is : <%=ViewData["action"] %></h3>
<hr />
following using method RouteLink(string title,string addOnAttribute,string controller)<br /><br />

<%=RouteLink("Link without action","id=\"myId\" class=\"myClass\" style=\"background-color:red;\"","UseRouteLink") %><br /><br />

<%=RouteLink("Link without action", null, "UseRouteLink")%><br /><br />

<%=RouteLink(null,null,"UseRouteLink") %><br /><br />
<hr />
following using method RouteLink(string title,string addOnAttribute,string controller,string action)<br /><br />

<%=RouteLink("Link to ActionWithoutParameter", null, "UseRouteLink", "ActionWithoutParameter")%><br /><br />

<%=RouteLink(null, null, "UseRouteLink", "ActionWithoutParameter")%><br /><br />
<hr />
following using method RouteLink(string title,string addOnAttribute,string controller,string action,object[] urlParameters)<br /><br />

<%=RouteLink("Link to ActionWithParameter", null, "UseRouteLink", "ActionWithParameter",new object[]{"jack",25,DateTime.Now.AddYears(-25).ToShortDateString()})%><br /><br />
<%if (ViewData["UrlParameter"] != null)
  {
      foreach (string str in GetViewData<List<string>>("UrlParameter"))
      {
          Write(str+"<br/>");
      }
  }
%>
<hr />
following using method RouteLink(string title,string addOnAttribute,string controller,string action,object parameterName,object parameterValue,params object[] moreParameters)<br /><br />

<%=RouteLink("Link to ActionWithParameter", null, "UseRouteLink", "ActionWithParameter", "Name", "Jack", "Age", 25, "Birthday", DateTime.Now.AddYears(-25).ToShortDateString())%><br /><br />
</asp:Content>

