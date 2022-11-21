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
    public interface ISessionDataProvider
    {
        int CreateSession(string httpReferer, string httpRemoteAddress, string httpUserAgent);
        void AssignVisitorToSession(int sessionId, int visitorId);
        void AssignUserToSession(int sessionId, int userId);
        int GetVisitorIdForSessionId(int sessionId);

        int CreateVisitor();
    }
}
