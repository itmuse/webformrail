<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    WebFormRailDemoComponents.Employee employee;
    protected void Page_Load(object sender, EventArgs e)
    {
        employee = GetViewData<WebFormRailDemoComponents.Employee>("Employee");
        //submit.Text = "button has changed";
        Response.Write("Text from page load ");
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%if (employee != null && employee.Department!=null)
      { %>
        Name:<%=employee.Name%><br />
        Salary:<%=employee.Salary%><br />
        EmployeeID:<%=employee.EmployeeID%><br />
        DepartmentID:<%=employee.DepartmentID%><br />
        Department.Branch:<%=employee.Department.Branch%><br />
        Department.BranchID:<%=employee.Department.BranchID%><br />
        Department.Name:<%=employee.Department.Name%><br />
        <%} %>
        <hr />
        <asp:TextBox ID="id" runat="server" />
        <asp:Button ID="submit" runat="server" Text="Submit" />
    </div>
    </form>
</body>
</html>
