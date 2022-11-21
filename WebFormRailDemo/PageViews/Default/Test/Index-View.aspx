<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
protected void Page_Load(object o, EventArgs e)
{
    //myHolder.Controls.Add(ParseControl("<asp:Button id='demo' runat='server' text='dddddd'/>"));

    //Response.Write(this.ViewData["Hello"]);
}
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="test2" runat="server" Text="submit" />
        <asp:PlaceHolder ID="myHolder" runat="server" />
        <UC:DemoControl ID="dcText" runat="server" />
        <asp:TextBox ID="tbxName" runat="server" />
        <br />
        <%=ViewData["Form"]%><br />
        <%=ViewData["Hello"]%> I love you!!<br />
        
        <%if (ViewData["List"] != null)
          {
              System.Collections.Generic.List<string> prvList = (System.Collections.Generic.List<string>)ViewData["List"];
              foreach (string str in prvList)
              { %>
              
                <%=str%><br />
                
               <%}
          }%>
          <asp:DataGrid id="dtlDemo"
           BorderColor="black"
           BorderWidth="1"
           CellPadding="3"
           AutoGenerateColumns="true"
           runat="server">

         <HeaderStyle BackColor="#00aaaa">
         </HeaderStyle> 
 
      </asp:DataGrid>

    </div>
    </form>
</body>
</html>
