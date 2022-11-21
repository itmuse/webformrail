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
    public interface IAjaxProvider
    {
        string GenerateJavascriptClassName(string className);
        string GenerateJavascriptMethod(string url, string className, string methodName, string[] parameters, bool includeFormData);
    }
}
