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
    public interface ITranslationProvider
    {
        string GetTranslation(string langCode, string viewName, string tagName);
        string[] GetAvailableLanguages();
    }
}
