//=============================================================================
// ProMesh.NET - .NET Web Application Framework 
//
// Copyright (c) 2003-2007 Philippe Leybaert
//
//=============================================================================

using System;
using System.Collections.Generic;
using System.Web;

namespace WebFormRail
{
    public class HttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += PreRequestHandlerExecute;
        }

        private void PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (HttpContext.Current.Handler is WebFormRailHandler)
                WebFormRailHttpContext.CreatePageContext(HttpContext.Current);
            else
                WebFormRailHttpContext.CreateSessionContext(HttpContext.Current);
        }

        public void Dispose()
        {
        }
    }
}
