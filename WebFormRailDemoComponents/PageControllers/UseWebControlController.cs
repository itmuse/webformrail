//======================================================================
//
//        Copyright (C) 2007-2008 IFCA SOFTWARE    
//        All rights reserved
//
//        filename :UseWebControlController
//        description :
//
//        created by hyson at  2008-7-18 16:28:59
//
//======================================================================
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using WebFormRail;
using System.Data;

namespace WebFormRailDemoComponents
{
    public class UseWebControlController : WebFormController
    {
        public void Index()
        {
            View.Title = "web控件示例";
            if(!IsPost())
                View.FindControl<Label>("lblText").Text = "Hello world!";
            View.FindControl<Button>("btnSubmit").Click += new EventHandler(UseWebControlController_Click);

            DataGrid dataGrid = View.FindControl<DataGrid>("ggdDataDemo");
            dataGrid.DataSource = GetDataTable();
            dataGrid.DataBind();
        }
        public void CreateWebControl()
        {
            Label contain = View.FindControl<Label>("lblText");
            contain.Controls.Clear();
            Button button = new Button();
            button.ID = "autoCreateButton";
            button.Text = "this button is created in behind code";
            contain.Controls.Add(button);

        }

        private void UseWebControlController_Click(object sender, EventArgs e)
        {
            View.FindControl<Label>("lblText").Text = "Button has been clicked!";
            if (string.IsNullOrEmpty(View.FindControl<TextBox>("tbxName").Text))
                View.FindControl<TextBox>("tbxName").Text = "TextBox value is empty!";
            else
                View.FindControl<TextBox>("tbxName").Text = "Hello!" + View.FindControl<TextBox>("tbxName").Text;
        }
        private DataTable GetDataTable()
        {
            DataTable dataTable = new DataTable("DemoTable");

            dataTable.Columns.Add(new DataColumn("ID", typeof(int)));
            dataTable.Columns.Add(new DataColumn("Name", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Date",typeof(DateTime)));

            for(int i=0;i<10;i++)
            {
                DataRow dr = dataTable.NewRow();

                dr["ID"] = i;
                dr["Name"] = string.Format("My name is :{0}",i);
                dr["Date"] = DateTime.Now.AddMonths(i);

                dataTable.Rows.Add(dr);
            }

            return dataTable;
        }
    }
   
}
