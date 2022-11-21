using System;
using System.Collections.Generic;
using System.Text;
using WebFormRail;
namespace WebFormRailDemoComponents.PageControllers
{
    public class NewsController : WebFormController
    {
        public void Index()
        {
            ViewData["Date"] = DateTime.Now;
        }
    }
}
