//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.SessionState;
using System.Diagnostics;
using System.Threading;


namespace WebFormRail
{
    public class WebFormRailHandler : IHttpHandler, IRequiresSessionState
    {
        #region IHttpHandler 成员

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            new RailsDispatcher().ProcessRequest(WebFormRailHttpContext.Current);
        }

        #endregion
    }

    public class RailsDispatcher
    {
        private IHttpHandlerFactory pageHandlerFactory = ClassHelper.CreateInstance<PageHandlerFactory>();

        public void ProcessRequest(WebFormRailHttpContext httpContext)
        {
            httpContext.WebFormRailHandler = this;

            //WebAppContext.Reset();

            Stopwatch stopWatchRun = new Stopwatch();
            Stopwatch stopWatchRender = new Stopwatch();

            stopWatchRun.Start();

            WebAppHelper.IsExistAction = true;//set default value
            string[] routeValue = WebAppHelper.ExtractPageUrl();
            string controllerName = String.Format("{0}{1}",routeValue[0],WebAppConfig.ControllerAgnomen);

            WebFormView view = null;

            try
            {
                stopWatchRun.Start();

                view = WebAppHelper.RunWebFormController(routeValue);

                stopWatchRun.Stop();

                //httpContext.Response.RenderedView = view;

                if (view != null)
                {
                    stopWatchRender.Start();

                    ((IHttpHandler)view).ProcessRequest(httpContext.PrimalContext);

                    stopWatchRender.Stop();

                    if (WebAppConfig.LoggingProvider != null)
                    {
                        WebAppConfig.LoggingProvider.LogPage(WebAppContext.Session.SessionId, controllerName, GetQueryString(),
                                                             WebAppContext.Session.LanguageCode, view.LayoutName,
                                                             view.ViewName, (int)stopWatchRun.ElapsedMilliseconds,
                                                             (int)stopWatchRender.ElapsedMilliseconds);
                    }

                    httpContext.Response.AppendHeader("X-Powered-By", "WebFormRail.NET v" + WebAppConfig.Version);

                    pageHandlerFactory.ReleaseHandler((IHttpHandler)view);
                }
            }
            catch (TemplateNotFoundException templateException)
            {
                if (!WebAppConfig.Fire_PageNotFound(controllerName))
                {
                    httpContext.Response.Write(templateException.Message);
                }
            }
            catch (RedirectPageException exception)
            {
                if (WebAppConfig.LoggingProvider != null && view != null)
                {
                    WebAppConfig.LoggingProvider.LogPage(WebAppContext.Session.SessionId, controllerName, GetQueryString(), WebAppContext.Session.LanguageCode, view.LayoutName, view.ViewName, (int)stopWatchRun.ElapsedMilliseconds, (int)stopWatchRender.ElapsedMilliseconds);
                }

                // NewUrl is null when Response.End() is called from an offline session
                if (exception.NewUrl != null)
                    WebFormRailHttpContext.Current.Response.Redirect(exception.NewUrl);
            }
            catch (ThreadAbortException)
            {
                if (WebAppConfig.LoggingProvider != null && view != null)
                {
                    WebAppConfig.LoggingProvider.LogPage(WebAppContext.Session.SessionId, controllerName, GetQueryString(), WebAppContext.Session.LanguageCode, view.LayoutName, view.ViewName, (int)stopWatchRun.ElapsedMilliseconds, (int)stopWatchRender.ElapsedMilliseconds);
                }

                throw; // Occurs when Response.End() is called, and is handled by the ASP.NET runtime
            }
            catch (Exception ex)
            {
                if (ex.InnerException is ThreadAbortException)
                    throw ex.InnerException;

                if (!WebAppConfig.Fire_ExceptionHandler(ex))
                {
                    httpContext.Response.Write("<pre>");

                    for (Exception currentException = ex; currentException != null; currentException = currentException.InnerException)
                    {
                        httpContext.Response.Write("<em><b>" + currentException.Message + "</b></em><br/>");

                        httpContext.Response.Write(currentException.StackTrace);
                        httpContext.Response.Write("<hr>");
                    }

                    httpContext.Response.Write("</pre>");
                }
            }
        }

        private static string GetQueryString()
        {
            string s = WebFormRailHttpContext.Current.Request.RawUrl;

            if (s.IndexOf("?") >= 0)
                return s.Substring(s.IndexOf("?") + 1);
            else
                return "";
        }

    }
}
