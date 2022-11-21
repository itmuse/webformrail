//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//=============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace WebFormRail
{
    public class OfflineHttpResponse : WebFormRailHttpResponse
    {
        private readonly HttpCookieCollection _cookies;
        private readonly StringBuilder _output = new StringBuilder();
        private string _redirectedUrl = null;
        private int _statusCode = 0;
        private string _contentType = "text/html";

        public OfflineHttpResponse()
        {
            _cookies = new HttpCookieCollection();
        }

        public override HttpCookieCollection Cookies
        {
            get { return _cookies; }
        }

        public override void Write(string s)
        {
            _output.Append(s);
        }

        internal override string Output
        {
            get { return _output.ToString(); }
        }

        internal string RedirectedUrl
        {
            get { return _redirectedUrl; }
        }

        public override int StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        public override string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }

        public override void End()
        {
            throw new RedirectPageException(null);
        }

        public override void Redirect(string url)
        {
            if (url.StartsWith("~"))
                url = url.Substring(1);

            if (url.StartsWith("/"))
                _redirectedUrl = url;
            else
            {
                _redirectedUrl = WebAppContext.Request.FilePath.Substring(0, WebAppContext.Request.FilePath.LastIndexOf('/') + 1) + url;
            }
        }

        public override void BinaryWrite(byte[] bytes)
        {
        }

        public override void AppendHeader(string header, string value)
        {
        }

        public override void DisableCaching()
        {
        }

        public override void SetETag(string eTag)
        {
        }

        public override void ClearHeaders()
        {
        }
    }
}