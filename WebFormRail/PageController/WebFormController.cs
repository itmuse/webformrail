//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace WebFormRail
{
    public abstract class WebFormController
    {
        protected static WebFormRailHttpResponse Response { get { return WebAppContext.Response; } }
        protected static IHttpRequest Request { get { return WebAppContext.Request; } }
        protected static IHttpServerUtility Server { get { return WebAppContext.Server; } }
        protected static SessionBase Session { get { return WebAppContext.Session; } }
        protected static ClientDataCollection GetData { get { return WebAppContext.GetData; } }
        protected static ClientDataCollection PostData { get { return WebAppContext.PostData; } }

        private WebFormView webFormView = new WebFormView();

        protected WebFormController()
        {
            WebAppContext.CurrentWebFormController = this;

            LayoutAttribute layoutAttribute = null;
            ViewAttribute viewAttribute = null;

            if (GetType().IsDefined(typeof(LayoutAttribute), true))
                layoutAttribute = (LayoutAttribute)GetType().GetCustomAttributes(typeof(LayoutAttribute), true)[0];

            if (GetType().IsDefined(typeof(ViewAttribute), true))
                viewAttribute = (ViewAttribute)GetType().GetCustomAttributes(typeof(ViewAttribute), true)[0];

            if (layoutAttribute != null)
                webFormView.LayoutName = layoutAttribute.LayoutName;

            if (viewAttribute != null)
                webFormView.ViewName = viewAttribute.ViewName;

            string[] routeValue = WebAppHelper.ExtractPageUrl();

            //Extract view name of action
            if(routeValue.Length > 1)
            {
                MethodInfo method = WebAppContext.ControllerActionCollection.GetActionInfo(String.Format("{0}{1}",routeValue[0],WebAppConfig.ControllerAgnomen), routeValue[1]);
                if (method != null && method.IsDefined(typeof(ViewAttribute), true))
                {
                    ViewAttribute viewattribute = (ViewAttribute)method.GetCustomAttributes(typeof(ViewAttribute), true)[0];
                    webFormView.ViewName = viewattribute.ViewName;
                }
            }

            webFormView.RouteValue = routeValue;

            view = (WebFormView)webFormView.Render();
            
            view.PreInit += new EventHandler(view_PreInit);
            view.Load += new EventHandler(view_Load);

            MapClientData();
        }

        private void view_PreInit(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(webFormView.LayoutName))
                view.MasterPageFile = webFormView.LayoutName;
        }
        private void view_Load(object sender, EventArgs e)
        {
            if (CurrentMethod.Count != 0)
            {
                while (CurrentMethod.Count > 0)
                {
                    MethodInfo method = CurrentMethod.Dequeue();
                    if(method!=null)
                        method.Invoke(this, WebAppHelper.CreateParameters(method));
                }
            }
        }
        private void MapClientData()
        {
            Type type = GetType();

            MemberInfo[] members = type.GetMembers(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            foreach (MemberInfo member in members)
            {
                if (member is MethodInfo)
                    continue;

                if (member.IsDefined(typeof(ClientDataAttribute), true))
                {
                    ClientDataAttribute attribute = (ClientDataAttribute)member.GetCustomAttributes(typeof(ClientDataAttribute), true)[0];


                    if (member is FieldInfo)
                    {
                        object value = WebAppHelper.GetClientValue(attribute, ((FieldInfo)member).FieldType);

                        ((FieldInfo)member).SetValue(this, value);
                    }
                    else if (member is PropertyInfo)
                    {
                        object value = WebAppHelper.GetClientValue(attribute, ((PropertyInfo)member).PropertyType);

                        ((PropertyInfo)member).SetValue(this, value, null);
                    }
                }
            }
        }

        private WebFormView view;
        public WebFormView View
        {
            get { return view; }
        }

        public ViewDataCollection ViewData
        {
            get { return View.ViewData; }
        }

        private Queue<MethodInfo> currentMethod = new Queue<MethodInfo>();
        public Queue<MethodInfo> CurrentMethod
        {
            get { return currentMethod; }
            set { currentMethod = value; }
        }

        public string RequestMethod
        {
            get { return Request.ServerVariables["REQUEST_METHOD"].ToUpper(); }
        }

        public bool IsPost()
        {
            return RequestMethod == "POST";
        }

        public bool IsPost(string buttonName)
        {
            return IsPost() && (WebAppContext.PostData[buttonName] != null && WebAppContext.PostData[buttonName].Length > 0);
        }
        public string Url
        {
            get { return Request.Path; }
        }

        public string UrlWithParameters
        {
            get { return Request.RawUrl; }
        }
        public void Redirect(string newUrl)
        {
            Response.Redirect(newUrl);
        }

        public void RedirectWithoutParameters()
        {
            Response.Redirect(WebAppContext.Request.Path);
        }

        public void RedirectWithParameters()
        {
            Response.Redirect(WebAppContext.Request.RawUrl);
        }
    }
}
