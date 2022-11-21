//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Collections.Generic;

namespace WebFormRail
{
    public interface ILoggingProvider
    {
        int LogPage(int sessionId, string controllerName, string queryString, string languageCode, string layoutName, string viewName , int milliSecondsRun, int milliSecondsRender);
    }
}
