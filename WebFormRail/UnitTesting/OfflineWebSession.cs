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
    public class ContextCreatedEventArgs : EventArgs
    {
        public ContextCreatedEventArgs(OfflineWebSession webSession, WebFormRailHttpContext httpContext)
        {
            _webSession = webSession;
            _httpContext = httpContext;
        }

        private OfflineWebSession _webSession;
        private WebFormRailHttpContext _httpContext;

        public OfflineWebSession WebSession
        {
            get { return _webSession; }
            set { _webSession = value; }
        }

        public WebFormRailHttpContext HttpContext
        {
            get { return _httpContext; }
            set { _httpContext = value; }
        }
    }


    public class OfflineWebSession
    {
        private bool _isNewSession = true;
        private readonly string _sessionID;

        private readonly Dictionary<string, object> _sessionData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        private readonly NameValueCollection _postData = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);

        //private View _view;

        private readonly string _rootPath;
        private string _redirectedPage;
        private string _currentPage;
        private bool _followRedirects = false;

        public event EventHandler<ContextCreatedEventArgs> ContextCreated;

        public OfflineWebSession(string rootPath)
        {
            _rootPath = rootPath;
            _sessionID = Guid.NewGuid().ToString();
        }

        public void Reset()
        {
            _sessionData.Clear();
        }

        public static void ResetSession()
        {
        }

        internal object GetSessionData(string key)
        {
            if (_sessionData.ContainsKey(key))
                return _sessionData[key];
            else 
                return null;
        }

        internal void SetSessionData(string key, object value)
        {
            _sessionData[key] = value;
        }

        public void ClearSessionData()
        {
            _sessionData.Clear();
        }

        //private string RunPage(string url, bool post)
        //{
        //    for (; ; )
        //    {
        //        _redirectedPage = null;
        //        _currentPage = url;

        //        WebFormRailHttpContext.CreatePageContext(this, post ? "POST" : "GET", url, post ? _postData : null);

        //        if (ContextCreated != null)
        //            ContextCreated(this, new ContextCreatedEventArgs(this, WebFormRailHttpContext.Current));

        //        WebFormRailPageHandler pageHandler = new WebFormRailPageHandler();

        //        pageHandler.ProcessRequest(WebFormRailHttpContext.Current);

        //        _isNewSession = false;

        //        _view = WebFormRailHttpContext.Current.Response.RenderedView;
        //        _redirectedPage = ((OfflineHttpResponse)WebFormRailHttpContext.Current.Response).RedirectedUrl;

        //        PostData.Clear();

        //        if (!_followRedirects || _redirectedPage == null)
        //            return WebFormRailHttpContext.Current.Response.Output;

        //        post = false;
        //        url = _redirectedPage;
        //    }
        //}

        public bool FollowRedirects
        {
            get { return _followRedirects; }
            set { _followRedirects = value; }
        }

        //public string PageGet(string url)
        //{
        //    return RunPage(url, false);
        //}

        //public string PagePost(string url)
        //{
        //    return RunPage(url, true);
        //}

        //public View View
        //{
        //    get { return _view; }
        //}

        //public ViewDataCollection ViewData
        //{
        //    get { return _view.ViewData.Variables; }
        //}

        public bool IsNewSession
        {
            get { return _isNewSession; }
        }

        public string RootPath
        {
            get { return _rootPath; }
        }

        public string SessionID
        {
            get { return _sessionID; }
        }

        public string RedirectedPage
        {
            get { return _redirectedPage; }
        }

        public string CurrentPage
        {
            get { return _currentPage; }
        }

        public NameValueCollection PostData
        {
            get { return _postData; }
        }

        public void PushButton(string buttonName)
        {
            PostData[buttonName] = "*";
        }
    }
}
