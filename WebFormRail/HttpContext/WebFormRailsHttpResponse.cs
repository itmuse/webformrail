//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Collections.Generic;
using System.Web;

namespace WebFormRail
{
    public abstract class WebFormRailHttpResponse
    {
        //private View _renderedView;

        public abstract HttpCookieCollection Cookies { get; }
        public abstract void Write(string s);
        internal abstract string Output { get; }
        public abstract void End();
        public abstract void Redirect(string url);
        public abstract int StatusCode { get; set; }
        public abstract string ContentType { get; set; }
        public abstract void BinaryWrite(byte[] bytes);
        public abstract void AppendHeader(string header, string value);
        public abstract void ClearHeaders();

        public abstract void DisableCaching();
        public abstract void SetETag(string eTag);

        //internal View RenderedView
        //{
        //    get { return _renderedView; }
        //    set { _renderedView = value; }
        //}

    }
}