using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WebFormRail;
using System.Collections.Generic;


namespace WebFormRailDemoComponents
{
    /// <summary>
    /// ControlClassTemplate1 的摘要说明
    /// </summary>
    public class DemoControl : WebFormControl
    {
        //Here to define some object
        Literal ltlControlList;

        protected override void OnInit(EventArgs e)
        {
            if (SkinName == null)
            {
                ExternalSkinFileName = "democontrol.ascx";
            }
            else
            {
                ExternalSkinFileName = SkinName;
            }
            //===
            base.OnInit(e);
        }


        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataBind();

                foreach (KeyValuePair<string, ControllerCollection> kvp in WebAppContext.ControllerActionCollection.AllControllers)
                {
                    if (!string.IsNullOrEmpty(ControllerName) && kvp.Key != ControllerName) continue;
                    foreach (KeyValuePair<string, ActionCollection> str in kvp.Value.ActionCollection)
                    {
                        ltlControlList.Text += (String.Format("<a href=\"{0}/{1}/{2}.aspx\">{3}</a><br/>", WebAppContext.Request.ApplicationPath, kvp.Key.Replace("Controller", ""), str.Value.ActionName, kvp.Key.Replace("Controller", "") + "->" + str.Value.ActionName));
                    }
                }
            }
            //===
            base.OnLoad(e);
        }

        public override void DataBind()
        {
            base.DataBind();
            //===
            //TO DO : Here is your code.

        }

        protected override void AttachChildControls()
        {
            ///TO DO : Here to find some controls of page and instance the object.
            ltlControlList = FindControl<Literal>("ltlControlList");
        }
        //================================
        private string controllerName;
        public string ControllerName
        {
            set { controllerName = value.Replace("Controller",string.Empty); }
            get 
            {
                if (controllerName == null)
                    return null;
                else
                    return controllerName+"Controller";
            }
        }

    }
}
