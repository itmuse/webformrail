//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;

namespace WebFormRail
{
    public class LayoutAttribute : Attribute
    {
        private readonly string _layoutName;

        public LayoutAttribute(string layoutName)
        {
            _layoutName = String.Format("../{0}.master",layoutName);
        }

        public string LayoutName
        {
            get { return _layoutName; }
        }
    }
}
