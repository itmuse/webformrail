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
    internal class RedirectPageException : WebFormRailException
    {
        private readonly string _newUrl;

        public RedirectPageException(string newUrl) : base(newUrl)
        {
            _newUrl = newUrl;
        }

        public string NewUrl
        {
            get { return _newUrl; }
        }
    }
}
