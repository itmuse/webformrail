using System;
using System.Collections.Generic;
using System.Text;
using WebFormRail;

namespace WebFormRailDemoComponents
{
    public class UseRouteLinkController : RouteLinkBase
    {
        public void Index()
        {
            ViewData["Action"] = "Index";
        }
        public void ActionWithoutParameter()
        {
            ViewData["Action"] = "ActionWithoutParameter";
        }
        public void ActionWithParameter(string name,int age,DateTime birthday)
        {
            ViewData["Action"] = "ActionWithParameter";

            List<string> list = new List<string>();

            list.Add(string.Format("Name: {0}", name));
            list.Add(string.Format("Age: {0}", age));
            list.Add(string.Format("Birthday: {0}", birthday));

            ViewData["UrlParameter"] = list;
        }
        [Teardown]
        private void AddonTitle()
        {
            View.Title += "_____Add some words in here";
        }
    }
    public class RouteLinkBase : WebFormController
    {
        [Setup]
        private void SetTitle()
        {
            View.Title = "Use RouteLink example";
        }
    }
}
