//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;

namespace WebFormRail
{
    public class ViewAttribute : Attribute
    {
        private readonly string _viewName;

        public ViewAttribute(string viewName)
        {
            _viewName = viewName;

            if (_viewName.StartsWith("~/"))
                _viewName = _viewName.Substring(2);
            else if (_viewName.StartsWith("/"))
                _viewName = _viewName.Substring(1);
        }

        public string ViewName
        {
            get { return _viewName; }
        }
    }
}
