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
	public class SessionBase 
	{
		private readonly IHttpSessionState _httpSession = null;

		private int    _sessionID = 0;
		private int    _visitorID = 0;
		private int    _userID    = 0;
		private string _languageCode = null;

		public static WebFormRailHttpResponse      Response    { get { return WebAppContext.Response; } }
		public static IHttpRequest       Request     { get { return WebAppContext.Request;  } }
		public static IHttpServerUtility Server      { get { return WebAppContext.Server;   } }

		public static SessionBase CurrentSession
		{
			get { return WebAppContext.Session; }
		}

		public SessionBase()
		{
            _httpSession = WebFormRailHttpContext.Current.Session;

            SessionManager.CreateSessionProperties(this);

		    this["_START_TIME_"] = this["_START_TIME_"] ?? DateTime.Now;

			if (IsNewSession)
			{
				SessionId = CreateSessionDataObject();

				if (VisitorID <= 0)
					VisitorID = CreateVisitor();

                WebAppConfig.SessionDataProvider.AssignVisitorToSession(SessionId,VisitorID);
			}

			DetermineLanguage();
		}

	    private void DetermineLanguage()
	    {
	        string urlLanguage = WebAppHelper.GetLanguageFromURL();

	        string lang = null;

	        if (urlLanguage.Length > 0)
	        {
	            lang = urlLanguage;

	            //TODO: check if language is defined. If it isn't, set urlLanguage = ""
	        }
	        else
	        {
	            string langParam = WebAppContext.GetData["lang"];
			
	            if (langParam != null && langParam.Length > 0)
	            {
	                lang = langParam;
	            }
	        }

	        if (WebAppConfig.ForceLanguagePath && urlLanguage.Length == 0)
	        {
	            if (lang == null || lang.Length < 2)
	                lang = WebAppConfig.DefaultLanguage;

	            Response.Redirect(WebAppHelper.InsertLanguageInURL(lang.ToLower()));
	        }
	        else if (lang != null)
	        {
	            LanguageCode = lang;
	        }
	    }

	    public object this[string key]
		{
			get { return _httpSession[key]; }
            set { _httpSession[key] = value; }
		}

		public bool IsNewSession
		{
			get { return _httpSession.IsNewSession; }
		}

		public string InternalSessionID
		{
			get { return _httpSession.SessionID; }
		}

		private static void SetCookie(string cookieName, string cookieValue , bool permanent)
		{
            if (Response.Cookies[cookieName] != null)
			    Response.Cookies[cookieName].Value = cookieValue;
            else 
                Response.Cookies.Add(new HttpCookie(cookieName,cookieValue));
			
			if (permanent)
				Response.Cookies[cookieName].Expires = DateTime.Now.AddYears(10);
		}

		private static string GetCookie(string cookieName)
		{
			HttpCookie httpCookie = Request.Cookies[cookieName];

			if (httpCookie == null)
				return null;
            else
                return httpCookie.Value;
		}

		public int SessionId
		{
			get
			{
				if (_sessionID <= 0)
				{
					if (this["_SESSIONID_"] is int)
						_sessionID = (int) this["_SESSIONID_"];
				}

				return _sessionID;
			}
			set
			{
				this["_SESSIONID_"] = value;

				_sessionID = value;
			}

		}

		public string LanguageCode
		{
			get
			{
				if (_languageCode == null)
				{
					_languageCode = GetCookie("LANG");

					if (_languageCode == null || _languageCode.Length < 2)
					{
                        return (string) this["_LANG_"] ?? WebAppConfig.DefaultLanguage;
					}
				}

				return _languageCode;
			}
			set
			{
				this["_LANG_"] = value;

				SetCookie("LANG" , value , true);

				_languageCode = value;
			}
		}

		public int VisitorID
		{
			get
			{
				if (_visitorID <= 0)
				{
					_visitorID = int.Parse(GetCookie("VISITORID") ?? "0");

					if (_visitorID <= 0)
					{
						if (this["_VISITORID_"] is int)
							_visitorID = (int) this["_VISITORID_"];
					}
				}

				return _visitorID;
			}
			set
			{
				this["_VISITORID_"] = value;

				SetCookie("VISITORID" , value.ToString() , true);

                WebAppConfig.SessionDataProvider.AssignVisitorToSession(SessionId,VisitorID);

				_visitorID = value;
			}
		}

		public int UserID
		{
			get
			{
				if (_userID <= 0)
				{
					if (this["_USER_ID_"] is int)
						_userID = (int) this["_USER_ID_"];
				}

				return _userID;
			}
			set
			{
				this["_USER_ID_"] = value;

				if (value != 0)
                    WebAppConfig.SessionDataProvider.AssignUserToSession(SessionId,UserID);

				_userID = value;
			}
		}

		public DateTime LogonTime
		{
			get { return (DateTime) this["_START_TIME_"]; }
		}

		private static int CreateSessionDataObject()
		{
            return WebAppConfig.SessionDataProvider.CreateSession(Request.UrlReferrer == null ? "" : Request.UrlReferrer.OriginalString, Request.UserHostAddress, Request.UserAgent);
		}

		private static int CreateVisitor()
		{
		    return WebAppConfig.SessionDataProvider.CreateVisitor();
		}
	}
}
