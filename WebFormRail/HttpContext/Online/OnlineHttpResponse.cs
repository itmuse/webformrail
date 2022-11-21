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
using System.Web;

namespace WebFormRail
{
    public class OnlineHttpResponse : WebFormRailHttpResponse
    {
        private readonly HttpResponse _response;

        public OnlineHttpResponse(HttpResponse response)
        {
            _response = response;
        }

        public override HttpCookieCollection Cookies
        {
            get { return _response.Cookies; }
        }

        internal override string Output
        {
            get { throw new NotImplementedException(); }
        }

        public override int StatusCode
        {
            get { return _response.StatusCode; }
            set { _response.StatusCode = value; }
        }

        public override string ContentType
        {
            get { return _response.ContentType; }
            set { _response.ContentType = value; }
        }

        public override void Write(string s)
        {
            _response.Write(s);
        }

        public override void End()
        {
            _response.End();
        }

        public override void Redirect(string url)
        {
            _response.Redirect(url);
        }

        public override void BinaryWrite(byte[] bytes)
        {
            _response.BinaryWrite(bytes);
        }

        public override void AppendHeader(string header, string value)
        {
            _response.AppendHeader(header,value);
        }

        public override void DisableCaching()
        {
            _response.Cache.SetCacheability(HttpCacheability.NoCache);
            _response.Cache.SetNoServerCaching();
        }

        public override void SetETag(string eTag)
        {
            _response.Cache.SetCacheability(HttpCacheability.Public);
            _response.Cache.SetETag('"' + eTag + '"');
        }

        public override void ClearHeaders()
        {
            _response.ClearHeaders();
        }
    }
}