//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;

namespace WebFormRail
{
    public abstract class WebFormRailHttpContext
    {
        [ThreadStatic]
        private static WebFormRailHttpContext _current;

        private RailsDispatcher _webFormRailsHandler;
        private SessionBase _session;

        public abstract Exception[] AllErors { get; }
        public abstract Cache Cache { get; }
        public abstract IHttpRequest Request { get; }
        public abstract WebFormRailHttpResponse Response { get; }
        public abstract IHttpServerUtility Server { get; }
        public abstract IHttpSessionState Session { get; }
        public abstract IDictionary Items { get; }
        public abstract IPrincipal User { get; set; }
        public abstract HttpContext PrimalContext { get;}

        public static WebFormRailHttpContext Current
        {
            get
            {
                if (HttpContext.Current != null)
                    return (WebFormRailHttpContext) HttpContext.Current.Items["_CurrentContext"];
                else
                    return _current;
            }
            private set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Items["_CurrentContext"] = value;
                else
                    _current = value;
            }
        }

        internal RailsDispatcher WebFormRailHandler
        {
            get { return _webFormRailsHandler; }
            set { _webFormRailsHandler = value; }
        }

        internal SessionBase SessionObject
        {
            get { return _session; }
            set { _session = value; }
        }

        public static void CreatePageContext(HttpContext httpContext)
        {
            WebAppConfig.Init();

            WebFormRailHttpContext context = new OnlineHttpContext(httpContext);

            Current = context;

            if (httpContext.Session != null)
                context.SessionObject = WebAppHelper.CreateSessionObject();
        }

        public static void CreateSessionContext(HttpContext httpContext)
        {
            WebAppConfig.Init();

            WebFormRailHttpContext context = new OnlineHttpContext(httpContext);

            Current = context;

            if (httpContext.Session != null)
                context.SessionObject = WebAppHelper.CreateSessionObject();
        }

        public static void CreatePageContext(OfflineWebSession webSession, string method, string url, NameValueCollection postData)
        {
            WebAppConfig.Init();

            Current = new OfflineHttpContext(webSession, method, url, postData);

            Current.SessionObject = WebAppHelper.CreateSessionObject();
        }

        public static void CreateSessionContext(OfflineWebSession webSession, string method, string url, NameValueCollection postData)
        {
            WebAppConfig.Init();

            Current = new OfflineHttpContext(webSession, method, url, postData);

            Current.SessionObject = WebAppHelper.CreateSessionObject();
        }

    }
}