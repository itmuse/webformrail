//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================
using System;
using System.Collections.Generic;
using System.Reflection;

namespace WebFormRail
{
    internal class WebFormControllerClass
    {
        private class ActionMethod
        {
            public MethodInfo Method;
            public string ControllerName;

            public ActionMethod(MethodInfo method, string url)
            {
                Method = method;
                ControllerName = url;
            }
        }

        private readonly Type _classType;
        private readonly List<MethodInfo> _setupMethods = new List<MethodInfo>();
        private readonly List<MethodInfo> _teardownMethods = new List<MethodInfo>();
        private readonly Dictionary<string, MethodInfo> _publicMethods = new Dictionary<string, MethodInfo>(StringComparer.InvariantCultureIgnoreCase);
        //private readonly Dictionary<string, AjaxMethod> _ajaxMethods = new Dictionary<string, AjaxMethod>();

        private readonly string _controllerName;

        public WebFormControllerClass(Type classType)
        {
            _classType = classType;

            UrlAttribute[] urlAttributes = (UrlAttribute[])_classType.GetCustomAttributes(typeof(UrlAttribute), true);

            if (urlAttributes.Length > 0)
            {
                _controllerName = urlAttributes[0].Path;
            }
            else
            {
                _controllerName = _classType.Name.Replace('_', '/');
            }

            Type currentClassType = _classType;
            Stack<Type> pageTypeStack = new Stack<Type>();

            while (currentClassType != typeof(WebFormController) && currentClassType != null)
            {
                pageTypeStack.Push(currentClassType);

                currentClassType = currentClassType.BaseType;
            }

            while (pageTypeStack.Count > 0)
            {
                currentClassType = pageTypeStack.Pop();

                MethodInfo[] methods = currentClassType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);

                foreach (MethodInfo methodInfo in methods)
                {
                    if (methodInfo.IsSpecialName)
                        continue;

                    if (methodInfo.IsDefined(typeof(SetupAttribute), true))
                    {
                        _setupMethods.Add(methodInfo);
                    }
                    else if (methodInfo.IsDefined(typeof(TeardownAttribute), true))
                    {
                        _teardownMethods.Add(methodInfo);
                    }
                    //else if (methodInfo.IsDefined(typeof(AjaxAttribute), true))
                    //{
                    //    AjaxAttribute ajaxAttribute = (AjaxAttribute)methodInfo.GetCustomAttributes(typeof(AjaxAttribute), false)[0];

                    //    AjaxMethods[methodInfo.Name] = new AjaxMethod(methodInfo, this, ajaxAttribute.JavaScriptAlias, ajaxAttribute.UseFormData);
                    //}
                    else if (methodInfo.IsPublic)
                    {
                        _publicMethods[methodInfo.Name] = methodInfo;
                        WebAppContext.ControllerActionCollection.Add(classType.Name,classType, methodInfo.Name,methodInfo);

                        /*
                                                if (_publicMethods.ContainsKey(methodInfo.Name))
                                                {
                                                    if (_publicMethods[methodInfo.Name].IsPrivate && methodInfo.IsPublic)
                                                        _publicMethods[methodInfo.Name] = methodInfo;
                                                    else
                                                        throw new Exception("More than one method " + methodInfo.Name + "() was found with the same visibility");
                                                }
                                                else
                                                {
                                                    _publicMethods.Add(methodInfo.Name, methodInfo);
                                                }
                        */
                    }
                }
            }
        }

        //internal Dictionary<string, AjaxMethod> AjaxMethods
        //{
        //    get { return _ajaxMethods; }
        //}

        internal string ControllerName
        {
            get { return _controllerName; }
        }

        public WebFormController CreateWebFormControllerObject()
        {
            return (WebFormController)Activator.CreateInstance(_classType);
        }

        public bool Run(WebFormController pageObject, string actionName)
        {
            //foreach (AjaxMethod ajaxMethod in _ajaxMethods.Values)
            //    WebAppContext.AjaxMethods[ajaxMethod.MethodInfo.Name] = ajaxMethod;

            //if (WebAppContext.AjaxMethods.ContainsKey(actionName))
            //{
            //    AjaxMethod ajaxMethod = WebAppContext.AjaxMethods[actionName];

            //    if (ajaxMethod.MethodInfo.IsStatic)
            //        pageObject = null;

            //    WebAppContext.Response.ContentType = "application/json";
            //    WebAppContext.Response.Write(WebAppHelper.RunAjaxMethod(ajaxMethod.MethodInfo, pageObject));

            //    return false;
            //}

            try
            {
                foreach (MethodInfo method in _setupMethods)
                    //method.Invoke(pageObject, WebAppHelper.CreateParameters(method));
                    pageObject.CurrentMethod.Enqueue(method);

                if (_publicMethods.ContainsKey(actionName))
                {
                    MethodInfo method = _publicMethods[actionName];

                    //method.Invoke(pageObject, WebAppHelper.CreateParameters(method));
                    //bind current method to view load event
                    pageObject.CurrentMethod.Enqueue(method);

                }

                foreach (MethodInfo method in _teardownMethods)
                    //method.Invoke(pageObject, WebAppHelper.CreateParameters(method));
                    pageObject.CurrentMethod.Enqueue(method);
            }
            catch (TargetInvocationException ex)
            {
                FieldInfo remoteStackTrace = typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);

                remoteStackTrace.SetValue(ex.InnerException, ex.InnerException.StackTrace + "\r\n");

                throw ex.InnerException;
            }

            return true;
        }
    }
}
