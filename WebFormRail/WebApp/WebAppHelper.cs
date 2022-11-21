//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Caching;

namespace WebFormRail
{
    internal static class WebAppHelper
    {
        private static readonly object _templateCacheLock = new object();
        private static readonly string _etagSignature = DateTime.Now.Ticks.ToString("X16");
        private static char[] spliter = new char[] { '/' };

        private static bool isExistAction = true;


        private static string BuildETag(string assemblyName, string resource)
        {
            return assemblyName.GetHashCode().ToString("X8") + resource.GetHashCode().ToString("X8") + ":" + _etagSignature;
        }

        private static void SendResource(string assemblyName, string resourceKey , string contentType)
        {
            assemblyName = WebFormRailUtil.DecodeFromURL(assemblyName);
            resourceKey = WebFormRailUtil.DecodeFromURL(resourceKey);

            string eTag = BuildETag(assemblyName, resourceKey);

            if (WebAppContext.Request.CheckETag(eTag))
            {
                WebAppContext.Response.SetETag(eTag);
                WebAppContext.Response.StatusCode = 304;
            }
            else
            {
                Assembly assembly = Assembly.Load(assemblyName);

                WebAppContext.Response.ContentType = contentType;
                WebAppContext.Response.SetETag(eTag);
                WebAppContext.Response.BinaryWrite(WebFormRailUtil.GetResourceData(assembly, resourceKey));
            }
        }
        
        public static bool IsExistAction
        {
            get { return isExistAction; }
            set { isExistAction = value; }
        }
        public static string[] ExtractPageUrl()
        {
            string url = WebAppContext.Request.AppRelativeCurrentExecutionFilePath;
            
            url = url.Substring(2);

            if (url.ToLower().EndsWith(".aspx"))
            {
                url = url.Substring(0, url.Length - 5);
            }
            string defaultAction = WebAppConfig.DefaultAction;
            if (!IsExistAction)
            {
                url = url.Insert(url.IndexOf(spliter[0]) + 1, string.Format("{0}{1}", defaultAction, spliter[0]));
            }


            string[] routeValue = url.Split(spliter, StringSplitOptions.RemoveEmptyEntries);

            return routeValue;

        }

        public static WebFormView RunWebFormController(string[] routeValue)
        {
            WebFormView view;
            string controllerName = string.Format("{0}{1}",routeValue[0],WebAppConfig.ControllerAgnomen);

            if (controllerName.StartsWith("_ajax_/"))
            {
                WebAppContext.Response.ContentType = "application/json";
                WebAppContext.Response.Write(RunAjaxMethod(controllerName.Substring(7)));

                return null;
            }

            if (controllerName.StartsWith("_js_/"))
            {
                string[] parts = controllerName.Substring(5).Split('/');

                SendResource(parts[0], parts[1], "text/javascript");

                return null;
            }

            if (controllerName.StartsWith("_gif_/"))
            {
                string[] parts = controllerName.Substring(6).Split('/');

                SendResource(parts[0], parts[1], "image/gif");

                return null;
            }

            if (controllerName.StartsWith("_jpg_/"))
            {
                string[] parts = controllerName.Substring(6).Split('/');

                SendResource(parts[0], parts[1], "image/jpeg");

                return null;
            }

            WebFormControllerClass webFormControllerClass = WebAppConfig.GetWebFormControllerClass(controllerName);

            if (webFormControllerClass != null)
            {
                WebAppContext.Response.DisableCaching();

                string actionName = WebAppConfig.DefaultAction;

                if (routeValue.Length > 1)
                {
                    //if the action name from url is not exist then needing format the url
                    if (!WebAppContext.ControllerActionCollection.ExistAction(String.Format("{0}{1}",routeValue[0],WebAppConfig.ControllerAgnomen),routeValue[1]))
                        IsExistAction = false;
                    else
                        actionName = routeValue[1];
                }

                WebFormController webFormController = webFormControllerClass.CreateWebFormControllerObject();

                if (webFormControllerClass.Run(webFormController, actionName))
                {
                    view = webFormController.View;
                    return view;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                string filePath;
                if (routeValue.Length > 1)
                    filePath = string.Format("{0}/{1}/{2}/{3}-view.aspx", WebAppContext.Request.ApplicationPath, WebAppConfig.TemplatePath, routeValue[0], routeValue[1]);//string.Format("/{0}/{1}-view.aspx", routeValue[0], routeValue[1]);
                else
                    filePath = string.Format("{0}/{1}/{2}-view.aspx", WebAppContext.Request.ApplicationPath, WebAppConfig.TemplatePath, routeValue[0]);
                WebFormView noneControlerView = new WebFormView(WebAppContext.HttpContext, filePath);
                view = (WebFormView)noneControlerView.Render();
            }

            return view;
        }

        public static string GetTranslation(string viewName, string tag)
        {
            if (WebAppConfig.TranslationProvider != null)
                return WebAppConfig.TranslationProvider.GetTranslation(WebAppContext.Session.LanguageCode, viewName, tag);
            else 
                return null;
        }

        internal static string GetClientValue(ClientDataAttribute attribute)
        {
            if (attribute.UseGet && WebAppContext.GetData.Has(attribute.Name))
                return WebAppContext.GetData.Get(attribute.Name);
            else if (attribute.UsePost && WebAppContext.PostData.Has(attribute.Name))
                return WebAppContext.PostData.Get(attribute.Name);

            return null;
        }

        internal static object GetClientValue(ClientDataAttribute attribute, Type type)
        {
            if (attribute.UseGet && WebAppContext.GetData.Has(attribute.Name))
                return WebAppContext.GetData.Get(attribute.Name,type);
            else if (attribute.UsePost && WebAppContext.PostData.Has(attribute.Name))
                return WebAppContext.PostData.Get(attribute.Name,type);

            return null;
        }

        internal static string InsertLanguageInURL(string languageID)
        {
            string urlPath = WebFormRailHttpContext.Current.Request.RawUrl;
            string appPath = WebFormRailHttpContext.Current.Request.ApplicationPath;

            if (urlPath.StartsWith(appPath))
                urlPath = urlPath.Substring(appPath.Length);

            if (!urlPath.StartsWith("/"))
                urlPath = "/" + urlPath;

            if (!appPath.EndsWith("/"))
                appPath += "/";

            return appPath + languageID + urlPath;
        }

        internal static string GetLanguageFromURL()
        {
            if (!WebAppConfig.UseLanguagePath)
                return "";

            string urlPath = SessionBase.Request.FilePath;
            string appPath = SessionBase.Request.ApplicationPath;

            if (urlPath.StartsWith(appPath))
                urlPath = urlPath.Substring(appPath.Length);

            int idx1 = urlPath.LastIndexOf('/');
			
            if (idx1 > 0)
            {
                int idx2 = urlPath.Substring(0,idx1).LastIndexOf('/');

                if (idx2 >= 0)
                    return urlPath.Substring(idx2+1,idx1-idx2-1).ToUpper();
                else
                    return urlPath.Substring(0,idx1);
            }

            return "";
        }

        internal static SessionBase CreateSessionObject()
        {
            SessionBase newSession = (SessionBase) Activator.CreateInstance(WebAppConfig.SessionType);

            WebAppConfig.Fire_SessionCreated(newSession);

            return newSession;
        }


        internal static object[] CreateParameters(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();

            object[] parameterValues = new object[parameters.Length];

            string[] routeValue = ExtractPageUrl();

            int i = 0;
            foreach (ParameterInfo parameter in parameters)
            {
                ClientDataAttribute[] attributes = (ClientDataAttribute[]) parameter.GetCustomAttributes(typeof(ClientDataAttribute), true);

                ClientDataAttribute mapAttribute = null;

                object clientValue = null;

                if ((i + 2) < routeValue.Length)
                {
                    clientValue = WebFormRailUtil.ConvertString(routeValue[i + 2],parameter.ParameterType);
                }
                if (clientValue == null)
                {
                    if (attributes.Length > 0)
                    {
                        mapAttribute = attributes[0];
                    }
                    else
                    {
                        mapAttribute = new GetOrPostAttribute(parameter.Name);
                    }
                    clientValue = GetClientValue(mapAttribute, parameter.ParameterType);
                }

                parameterValues[i++] = clientValue;
            }

            return parameterValues;
        }

        private static string RunAjaxMethod(string controllerName)
        {
            string[] parts = controllerName.Split('/');

            if (parts.Length != 3)
            {
                throw new WebFormRailException("Unrecognized Ajax URL");
            }

            string assemblyName = WebFormRailUtil.DecodeFromURL(parts[0]);
            string className = WebFormRailUtil.DecodeFromURL(parts[1]);
            string methodName = WebFormRailUtil.DecodeFromURL(parts[2]);

            Type type = Type.GetType(className + ", " + assemblyName);

            if (type == null)
            {
                return AjaxHelper.GenerateJSONError("Unknown class " + className + " in assembly " + assemblyName);
            }

            MethodInfo method = type.GetMethod(methodName);

            if (method == null)
            {
                return AjaxHelper.GenerateJSONError("Unknown method " + methodName + " in class " + className);
            }

            if (!method.IsDefined(typeof(AjaxAttribute), false))
            {
                return AjaxHelper.GenerateJSONError("Method " + methodName + " is not an Ajax method");
            }

            object obj = null;

            if (!method.IsStatic)
                obj = Activator.CreateInstance(type);

            return RunAjaxMethod(method, obj);
        }

        internal static string RunAjaxMethod(MethodInfo ajaxMethod, object obj)
        {
            try
            {
                object result = ajaxMethod.Invoke(ajaxMethod.IsStatic ? null : obj, CreateParameters(ajaxMethod));

                return AjaxHelper.GenerateJSONReturnValue(result);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    FieldInfo remoteStackTrace = typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);

                    remoteStackTrace.SetValue(ex.InnerException, ex.InnerException.StackTrace + "\r\n");

                    return AjaxHelper.GenerateJSONError(ex.InnerException.Message);
                }
                else
                {
                    return AjaxHelper.GenerateJSONError(ex.Message);
                }
            }
        }
    }
}
