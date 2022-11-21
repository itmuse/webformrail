<%@ Page Language="C#" MasterPageFile="~/PageViews/Default/MasterPageMain.master" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphFull" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeft" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRight" Runat="Server">
<div class="Content">
The datetime value is set in Setup method:<%=ViewData["Setup"] %><br />
The string value is set in Teardown mehtod:<%=ViewData["teardown"] %>
<br />
<br />
</div>
</asp:Content>

