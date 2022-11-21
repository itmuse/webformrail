//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;

namespace WebFormRail
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UrlAttribute : Attribute
    {
        private readonly string _path;

        public UrlAttribute(string path)
        {
            _path = path;

            if (_path.StartsWith("~/"))
                _path = _path.Substring(2);
            else if (_path.StartsWith("/"))
                _path = _path.Substring(1);
        }

        public string Path
        {
            get { return _path; }
        }
    }
}
