//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Collections.Generic;
using System.Web.Caching;

namespace WebFormRail
{
	public static class WebAppContext
	{
	    private static readonly ClientDataCollection _getParameters = new ClientDataCollection(false);
        private static readonly ClientDataCollection _postParameters = new ClientDataCollection(true);

        private static ControllerActionCollection _controllerActionCollection = new ControllerActionCollection();

	    public static SessionBase Session
        {
            get { return WebFormRailHttpContext.Current.SessionObject; }
        }
        public static WebFormRailHttpContext HttpContext
        {
            get { return WebFormRailHttpContext.Current;}
        }
		public static WebFormRailHttpResponse Response
        {
            get { return WebFormRailHttpContext.Current.Response;}
        }
		public static IHttpRequest Request
        {
            get { return WebFormRailHttpContext.Current.Request;}
        }
		public static IHttpServerUtility Server
        {
            get { return WebFormRailHttpContext.Current.Server;}
        }
		public static Cache WebCache
        {
            get { return WebFormRailHttpContext.Current.Cache;}
        }
        public static ClientDataCollection GetData
        {
            get { return _getParameters;}
        }
        public static ClientDataCollection PostData
        {
            get { return _postParameters;}
        }


        public static WebFormController CurrentWebFormController
        {
            get { return (WebFormController)HttpContext.Items["CurrentWebFormController"]; }
            internal set { HttpContext.Items["CurrentWebFormController"] = value; }
        }
        public static ControllerActionCollection ControllerActionCollection
        {
            get { return _controllerActionCollection; }
        }

	    public static bool Offline
	    {
            get { return (WebFormRailHttpContext.Current is OfflineHttpContext); }
	    }
	}
}
