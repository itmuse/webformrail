//======================================================================
//
//        Copyright (C) 2007-2008 IFCA SOFTWARE    
//        All rights reserved
//
//        filename :IndexController
//        description :
//
//        created by hyson at  2008-5-28 14:48:58
//
//======================================================================
using System;
using System.Collections.Generic;
using System.Text;
using WebFormRail;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace WebFormRailDemoComponents
{
    /// <summary>
    /// 
    /// </summary>
    public class IndexController : WebFormController
    {

        public void Index()
        {
            View.Title = "WebFormRail Central";
        }
        
    }
}
