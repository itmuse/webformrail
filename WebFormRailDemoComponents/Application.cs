//======================================================================
//
//        Copyright (C) 2007-2008 IFCA SOFTWARE    
//        All rights reserved
//
//        filename :Application
//        description :
//
//        created by hyson at  2008-5-27 14:30:37
//
//======================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using WebFormRail;


namespace WebFormRailDemoComponents
{
    public static class Application
    {
        public delegate string CoolDelegate(int x);

        public static void Init()
        {
            WebAppConfig.PageNotFound += new Action<string>(WebAppConfig_PageNotFound);
            WebAppConfig.RegisterCustomObjectCreator(new DataObjectCreator());
            //WebAppConfig.ExceptionOccurred += new Action<Exception>(WebAppConfig_ExceptionOccurred);
        }

        static void WebAppConfig_ExceptionOccurred(Exception ex)
        {
            WebAppContext.Response.Write("<pre>");
            for (Exception currentException = ex; currentException != null;currentException=ex.InnerException )
                WebAppContext.Response.Write("<em><b>" + currentException.Message + "</b></em>");

            WebAppContext.Response.Write("</pre>");
        }

        static void WebAppConfig_PageNotFound(string filePath)
        {
            WebAppContext.Response.Write(filePath);
        }
    }
}
