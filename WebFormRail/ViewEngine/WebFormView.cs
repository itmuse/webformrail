//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;


namespace WebFormRail
{
    public class WebFormView : Page
    {
        private IHttpHandlerFactory pageHandlerFactory = ClassHelper.CreateInstance<PageHandlerFactory>();

        private readonly ViewData viewData;

        private WebFormRailHttpContext context;
        private string viewFilePath;
        private string layoutName,viewName;
        private string[] routeValue;

        public WebFormView():this(WebFormRailHttpContext.Current)
        {
        }
        public WebFormView(WebFormRailHttpContext context)
        {
            this.EnsureChildControls();
            this.context = context;
            viewData = new ViewData(this);
        }
        public WebFormView(WebFormRailHttpContext context, string viewFilePath):this(context)
        {
            this.viewFilePath = viewFilePath;
        }
        public string ViewName
        {
            get { return viewName; }
            set { viewName = value; }
        }
        public string LayoutName
        {
            get { return layoutName; }
            set { layoutName = value; }
        }
        public string ViewFilePath
        {
            get { return viewFilePath; }
            set { viewFilePath = value; }
        }
        public string[] RouteValue
        {
            get { return routeValue; }
            set { routeValue = value; }
        }
        private Dictionary<string, Control> controlDictonary = new Dictionary<string, Control>();
        public Dictionary<string, Control> ControlDictionary
        {
            get
            {
                if (controlDictonary.Count == 0)
                {
                    IterateThroughControl(this);
                }
                return controlDictonary;
            }
        }
        //===
        private void IterateThroughControl(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control != null && !string.IsNullOrEmpty(control.ID))
                    if (!controlDictonary.ContainsKey(control.ID))
                        controlDictonary.Add(control.ID, control);
                if (control.Controls.Count > 0)
                    IterateThroughControl(control);
            }

        }
        protected internal ViewDataCollection ViewData
        {
            get { return viewData.Variables; }
        }
        protected internal T GetViewData<T>(string key)
        {
            object value = ViewData[key];
            if (value == null)
                return default(T);
            else
            {
                try
                {
                    return (T)value;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        protected internal T Convert<T>(object obj)
        {
            try
            {
                return (T)WebFormRailUtil.ConvertType(obj, typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        protected internal void Write(object o)
        {
            Response.Write(o);
        }
        protected internal void Write(string s)
        {
            Response.Write(s);
        }

        #region RouteLink methods
        private readonly string urlBase = "<a href=\"{0}\" title=\"{1}\" {2}>{1}</a>";
        protected internal string RouteLink(string title, string addOnAttribute, string controller)
        {
            return RouteLink(title, addOnAttribute, controller, null);
        }
        protected internal string RouteLink(string title, string addOnAttribute, string controller, string action)
        {
            return RouteLink(title, addOnAttribute, controller, action, null);
        }
        protected internal string RouteLink(string title, string addOnAttribute, string controller, string action, object[] urlParametersValue)
        {
            return FormatLink(title, addOnAttribute, controller, action, true, urlParametersValue);
        }
        protected internal string RouteLink(string title, string addOnAttribute, string controller, string action, object parameterName, object parameterValue, params object[] moreParameters)
        {
            object[] parameters = new object[moreParameters.Length+2];
            parameters[0] = parameterName;
            parameters[1] = parameterValue;

            for (int i = 2; i < moreParameters.Length + 2; i++)
            {
                parameters[i]=moreParameters[i-2];
            }
            return FormatLink(title, addOnAttribute, controller, action, false, parameters);
        }
        private string FormatLink(string title, string addOnAttribute, string controller, string action, bool urlParameter, object[] parameters)
        {
            string url, returnLink;

            if (string.IsNullOrEmpty(controller))
                throw new ArgumentNullException(controller);
            if (!string.IsNullOrEmpty(action))
            {
                if (!WebAppContext.ControllerActionCollection.ExistAction(controller + "controller", action))
                    throw new ArgumentException(string.Format("{0} of {1} is not exist!", action, controller), string.Format("{0} and {1}", controller, action));

                url = string.Format("{0}/{1}/{2}", WebAppContext.Request.ApplicationPath, controller, action);
            }
            else
            {
                if (!WebAppContext.ControllerActionCollection.ExistController(controller + "controller"))
                    throw new ArgumentException(string.Format("{0} is not exist!", controller), controller);

                url = string.Format("{0}/{1}", WebAppContext.Request.ApplicationPath, controller);
            }


            if (parameters != null)
            {
                if (!urlParameter)
                {
                    string parameterList = FormatParameter(parameters);
                    url = string.Format("{0}.{1}?{2}", url, WebAppConfig.PageExtension, parameterList);
                }
                else
                {
                    if (parameters != null)
                    {
                        foreach (object obj in parameters)
                        {
                            url = string.Format("{0}/{1}", url, HttpUtility.UrlEncode(Convert<string>(obj)));
                        }
                    }
                    url = string.Format("{0}.{1}", url, WebAppConfig.PageExtension);
                }
            }
            else
            {
                url = string.Format("{0}.{1}", url, WebAppConfig.PageExtension);
            }
            if (string.IsNullOrEmpty(title))
            {
                if (!string.IsNullOrEmpty(action))
                    title = string.Format("{0}-{1}", controller, action);
                else
                    title = controller;
            }
            
            returnLink = string.Format(urlBase, url, title, addOnAttribute);

            return returnLink;
        }
        private string FormatParameter(object[] parameterList)
        {
            if (parameterList != null && parameterList.Length % 2 != 0)
                throw new ArgumentException("parameterList parameter invalid!", "parameterList");

            string TempParameterList = string.Empty;

            for (int i = 0; i < parameterList.Length; i += 2)
            {
                TempParameterList = string.Format("{0}{1}={2}&", TempParameterList, HttpUtility.UrlEncode(Convert<string>(parameterList[i])), HttpUtility.UrlEncode(Convert<string>(parameterList[i + 1])));
            }

            TempParameterList = TempParameterList.Substring(0, TempParameterList.Length - 1);

            return TempParameterList;
        }
        #endregion

        //===   
        public new Control FindControl(string id)
        {
            Control ctrl;
            if (!ControlDictionary.TryGetValue(id, out ctrl))
                return null;
            else
                return ctrl;
        }
        public T FindControl<T>(string id)
        {
            Object ctrl = FindControl(id);
            if (ctrl == null)
                return ClassHelper.CreateInstance<T>();
            else
                return (T)ctrl;
        }

        protected internal IHttpHandler Render()
        {
            string viewPath = null;
            if (!string.IsNullOrEmpty(ViewFilePath))
            {
                viewPath = ViewFilePath;
            }
            else
            {
                if (!string.IsNullOrEmpty(ViewName))
                {
                    if (ViewName.IndexOf("/") != -1)
                        viewPath = string.Format("{0}/{1}/{2}-view.aspx", context.Request.ApplicationPath, WebAppConfig.TemplatePath, ViewName);
                    else
                        viewPath = string.Format("{0}/{1}/{2}/{3}-view.aspx", context.Request.ApplicationPath, WebAppConfig.TemplatePath, RouteValue[0], ViewName);
                }
                else
                {
                    viewPath = string.Format("{0}/{1}/{2}/{3}-view.aspx", context.Request.ApplicationPath, WebAppConfig.TemplatePath, RouteValue[0], WebAppConfig.DefaultAction);
                }
            }
            try
            {
                string viewPhysicalPath = context.Server.MapPath(viewPath);
                IHttpHandler handler = pageHandlerFactory.GetHandler(context.PrimalContext, context.Request.RequestType, viewPath, viewPhysicalPath);
                return handler;
            }
            catch (HttpException ex)
            {
                throw new HttpException(ex.Message);
            }
        }
    }
}
