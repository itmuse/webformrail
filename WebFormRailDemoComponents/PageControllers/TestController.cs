//======================================================================
//
//        Copyright (C) 2007-2008 IFCA SOFTWARE    
//        All rights reserved
//
//        filename :TestController
//        description :
//
//        created by hyson at  2008-5-27 16:05:40
//
//======================================================================
using System;
using System.Collections.Generic;
using System.Text;
using WebFormRail;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;

namespace WebFormRailDemoComponents
{
    //[Layout("MasterPageForList")]
    class TestController : WebFormController
    {
        public void Index(int index)
        {
            ViewData["Hello"] = "Hello Worlds!!";
            string formData = PostData.Get<string>("dcText$_$tbxName");
            ViewData["Form"] = formData;
            View.Title = "demo";
            List<string> list = new List<string>();
            list.Add("aa");
            list.Add("bb");
            list.Add("cc");
            list.Add("d");
            list.Add("e");
            list.Add("f");
            ViewData["List"] = list;

            DataTable dataTable = new DataTable("demo");
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Name",typeof(string));
            dataTable.Columns.Add("Sex",typeof(string));

            for (int i = 0; i < index; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["ID"] = i;
                dr["Name"] = "WebFormRail" + ((i + 7) * 11).ToString();
                dr["Sex"] = i % 7 == 0 ? "Male" : "Femal";
                dataTable.Rows.Add(dr);
            }
            DataGrid dataGrid = View.FindControl<DataGrid>("dtlDemo");
            dataGrid.DataSource = dataTable;
            dataGrid.DataBind();
            View.FindControl<TextBox>("tbxName").Text = "hello hyson";
        }
        public void Test()
        {
            ViewData["Hello"] = "hyson ,you are very good!";
            View.Title = "test title";
            //View.Load+=new EventHandler(View_Load);
        }
        [View("List")]
        public void List([GetOrPost("id")] Employee employee)
        {
            if (employee == null)
                employee = new Employee();

            if (IsPost())
            {
                new DataService().Save(employee);
            }
            ViewData["Employee"] = employee;
            foreach (KeyValuePair<string, ControllerCollection> kvp in WebAppContext.ControllerActionCollection.AllControllers)
            {
                foreach (KeyValuePair<string, ActionCollection> str in kvp.Value.ActionCollection)
                {
                    Response.Write(String.Format("<a href=\"{0}/{1}/{2}.aspx\">{3}</a><br/>", Request.ApplicationPath, kvp.Key, str.Value.ActionName, kvp.Key + "->" + str.Value.ActionName));
                }
            }
            View.Title = "Title has changed";
            //Button button = View.FindControl("submit") as Button;
            //button.Text = "button text has changed";
            View.FindControl<Button>("submit").Text = "button text is changed";
            View.FindControl<Button>("submit").Click += new EventHandler(TestController_Click);
            View.FindControl<TextBox>("id").Text = "hellos";
        }

        void TestController_Click(object sender, EventArgs e)
        {
            View.FindControl<Button>("submit").Text = "button clicked";
        }

        [View("List")]
        public void List2(int id)
        {
            Employee employee = new DataService().LoadEmployee(id);
            ViewData["employee"] = employee;
        }
        [View("List")]
        public void List3(string form1)
        {
            //Employee employee = new DataService().LoadEmployee(id);
            //ViewData["employee"] = employee;
            Response.Write(form1);
        }

        [View("Login")]
        public void login(string aa,string bb,string dd)
        {
            List<string> list = new List<string>();
            list.Add(aa);
            list.Add(bb);
            list.Add(dd);
            list.Add("d");
            list.Add("e");
            list.Add("f");
            ViewData["List"] = list;

        }

    }
}
