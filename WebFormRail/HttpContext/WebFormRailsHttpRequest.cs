//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;

namespace WebFormRail
{
    public interface IHttpRequest
    {
        byte[] BinaryRead(int count);
        int[] MapImageCoordinates(string imageFieldName);
        string MapPath(string virtualPath);
        string MapPath(string virtualPath, string baseVirtualDir, bool allowCrossAppMapping);
        void SaveAs(string filename, bool includeHeaders);
        void ValidateInput();

        string[] AcceptTypes { get; }
        string AnonymousID { get; }
        string ApplicationPath { get; }
        string AppRelativeCurrentExecutionFilePath { get; }
        HttpBrowserCapabilities Browser { get; set; }
        HttpClientCertificate ClientCertificate { get; }
        Encoding ContentEncoding { get; set; }
        int ContentLength { get; }
        string ContentType { get; set; }
        HttpCookieCollection Cookies { get; }
        string CurrentExecutionFilePath { get; }
        string FilePath { get; }
        HttpFileCollection Files { get; }
        Stream Filter { get; set; }
        NameValueCollection Form { get; }
        NameValueCollection Headers { get; }
        string HttpMethod { get; }
        Stream InputStream { get; }
        bool IsAuthenticated { get; }
        bool IsLocal { get; }
        bool IsSecureConnection { get; }
        string this[string key] { get; }
        NameValueCollection Params { get; }
        string Path { get; }
        string PathInfo { get; }
        string PhysicalApplicationPath { get; }
        string PhysicalPath { get; }
        NameValueCollection QueryString { get; }
        string RawUrl { get; }
        string RequestType { get; set; }
        NameValueCollection ServerVariables { get; }
        int TotalBytes { get; }
        Uri Url { get; }
        Uri UrlReferrer { get; }
        string UserAgent { get; }
        string UserHostAddress { get; }
        string UserHostName { get; }
        string[] UserLanguages { get; }

        // WebFormRail.NET Specific

        bool CheckETag(string eTag);
    }

}