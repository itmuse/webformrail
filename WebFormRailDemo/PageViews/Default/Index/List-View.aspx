<%@ Page Language="C#" MasterPageFile="../MasterPage.master" Title="Untitled Page" %>
<script runat="server">
    protected void Page_Load(object o, EventArgs e)
    {
        tbxName.Text = "hello";
    }
    //protected override void OnPreInit(EventArgs e)
    //{
    //    this.MasterPageFile = "~/Views/MasterPageForList.master";
    //    base.OnPreInit(e);
    //}
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="masterPage" Runat="Server">
    <pre>
    <em><b>this page is using masterpage</b></em>
    </pre>
    <asp:TextBox ID="tbxName" runat="server" Text="textbox" /><br />
    <asp:Button ID="btnSubmit" runat="server" Text="submit" />
</asp:Content>

