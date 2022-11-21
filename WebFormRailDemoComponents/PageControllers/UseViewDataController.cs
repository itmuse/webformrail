using System;
using System.Collections.Generic;
using System.Text;
using WebFormRail;
using System.Data;
using System.Web.UI.WebControls;

namespace WebFormRailDemoComponents
{
    public class UseViewDataController : WebFormController
    {
        public void Index()
        {
            View.Title = "使用ViewData示例";//改变页面title

            ViewData["DateTime"] = DateTime.Now;//设置当期系统时间到ViewData集合里

            List<string> list = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(String.Format("List-{0}", i));
            }
            ViewData["List"] = list;//设置泛型list到ViewData集合里

            DataTable dataTable = new DataTable("DemoTable");
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name",typeof(string));
            dataTable.Columns.Add("Birthday", typeof(DateTime));
            for (int i = 100; i < 300; i+=7)
            {
                DataRow dr = dataTable.NewRow();

                dr["Id"] = i;
                dr["Name"] = String.Format("Name {0} of members", i);
                dr["Birthday"] = DateTime.Now.AddDays(-i);

                dataTable.Rows.Add(dr);
            }
            ViewData["Table"] = dataTable;


        }
        /// <summary>
        /// 
        /// </summary>
        [View("DataGrid")]
        public void UseDataGrid()
        {
            DataTable dataTable = new DataTable("DemoTable");
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Birthday", typeof(DateTime));
            for (int i = 100; i < 300; i += 7)
            {
                DataRow dr = dataTable.NewRow();

                dr["Id"] = i;
                dr["Name"] = String.Format("Name {0} of members", i);
                dr["Birthday"] = DateTime.Now.AddDays(-i);

                dataTable.Rows.Add(dr);
            }

            DataGrid dataGrid = View.FindControl<DataGrid>("dgdDemo");
            dataGrid.DataSource = dataTable;
            dataGrid.DataBind();
        }
        [View("UseAspSyntax")]
        public void UseAspSyntax()
        {
            ViewData["Show"] = true;
            ViewData["Loop"] = 10;
        }
    }
}
