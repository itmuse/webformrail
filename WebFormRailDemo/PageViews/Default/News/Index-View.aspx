<%@ Page Language="C#" MasterPageFile="~/PageViews/Default/MasterPage.master" Title="无标题页" %>

<script runat="server">

</script>

<asp:Content ID="Content1" ContentPlaceHolderID="masterPage" Runat="Server">
<h1>News controller index view</h1>
    <h3><%=ViewData["Date"] %></h3>
    <h6><%=GetViewData<DateTime>("Date").Ticks %></h6>
</asp:Content>

