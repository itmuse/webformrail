<%@ Page Language="C#" MasterPageFile="~/PageViews/Default/MasterPageMain.master" Title="Untitled Page" %>

<script runat="server">

</script>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFull" Runat="Server">
<h6>
说明：<br />
本示例演示了如何使用ViewData数据集合。ViewData数据集合是试图层(View)和控制层(Controller)数据交互的数据集合。<br />
在View页面上调用ViewData数据的方式是：ViewData["key name"]，返回的类型是object类型。同时为了方便类型转换，提供了
GetViewData&lt;T&gt;(string id)的泛型方法，方法是封装了对取ViewData数据时进行了数据类型转换，调用方式是：GetViewData&lt;object type&gt;("key name")。
<br />
同时：为了方便进行数据类型转换，也提供另外一个泛型方法：
Convert&lt;T&gt;(object obj)，该方法是封装了对对象数据进行数据类型转换。
</h6>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphLeft" Runat="Server">
<UC:DemoControl ID="ControllerList" ControllerName="UseViewData" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphRight" Runat="Server">

当前系统时间（从ViewData["DateTime"]读取）:<b><%=ViewData["DateTime"] %></b><br />

<hr />
<ul>
<%foreach(string str in GetViewData<List<string>>("List")) {%>
<li><%=str %></li>
<%} %>
</ul>
<hr />
另外一种调用显示方式：
<ul>
<%foreach (string str in GetViewData<List<string>>("List"))
  {
      Write(String.Format("<li>{0}</li>", str));//Write方式是Response.Write()方式的封装实现，提供更简单的方式把内容写出到页面。
  }%>
</ul>
<hr />
<table border="1" width="80%">
<tr style="background-color:#eeefff">
<td>Id</td>
<td>Name</td>
<td>Birthday</td>
</tr>
<%--
GetViewData<T>(string id)方法是封装了对取ViewData数据时进行了数据类型转换；
Convert<T>(object obj)方法是封装了对对象数据进行数据类型转换；
 --%>
<%foreach (DataRow dr in GetViewData<DataTable>("Table").Rows)
  {%>
  <tr>
    <td><%=dr["Id"] %></td>
    <td><%=dr["Name"] %></td>
    <td><%=Convert<DateTime>(dr["Birthday"]).ToShortDateString() %></td>
  </tr>
    
<%}%>
</table>
</asp:Content>

