//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace WebFormRail
{
    public class Url
    {
        private string _baseUrl;
        private readonly UrlParameterCollection _parameters;

        public Url(string baseUrl)
        {
            _baseUrl = baseUrl;
            _parameters = new UrlParameterCollection();
        }

        public Url(string baseUrl, NameValueCollection queryString)
        {
            _baseUrl = baseUrl;
            _parameters = new UrlParameterCollection(queryString);
        }

        private Url(Url source)
        {
            _baseUrl = source._baseUrl;
            _parameters = source._parameters.Clone();
        }

        public Url Clone()
        {
            return new Url(this);
        }

        public UrlParameterCollection Parameters
        {
            get { return _parameters; }
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }

        public static Url FromCurrent()
        {
            return new Url(WebAppContext.Request.Path, WebAppContext.Request.QueryString);
        }

        public override string ToString()
        {
            string queryString = _parameters.ToString();

            if (queryString.Length > 0)
                return BaseUrl + "?" + queryString;
            else
                return BaseUrl;
        }

    }
}
