//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================
//
// Original MootoolsAjaxProvider class created by Edwin De Jonge
//

using System;
using System.Collections.Generic;
using System.Text;

namespace WebFormRail
{
    public class MootoolsAjaxProvider : IAjaxProvider
    {
        public string GenerateJavascriptClassName(string className)
        {
            StringBuilder output = new StringBuilder();

            string[] parts = className.Split('.');

            string runningPart = "";

            foreach (string part in parts)
            {
                runningPart += part;

                output.Append("if (typeof " + runningPart + " == \"undefined\") " + runningPart + " = {};\r\n");

                runningPart += '.';
            }

            return output.ToString();
        }

        public string GenerateJavascriptMethod(string url, string className, string methodName, string[] parameters, bool includePostData)
        {
            StringBuilder output = new StringBuilder();

            output.Append(className + '.');
            output.Append(methodName);
            output.Append(" = function(");
            output.Append(String.Join(", ", parameters));

            if (parameters.Length > 0)
                output.Append(", ");

            output.Append("__callback, __errorcallback");

            output.Append("){\r\n");
            output.Append("  var data = { ");

            for (int i = 0; i < parameters.Length; i++)
            {
                if (i > 0)
                    output.Append(',');

                output.Append("\"" + parameters[i] + "\" : " + parameters[i]);
            }
            output.Append("};\r\n");
            output.Append("  new Ajax( \"" + url + "\", {\r\n");
            output.Append("         onSuccess: function (txt, xml) { txt = eval('('+txt+')'); if (__callback) __callback(txt.value);},\r\n");
            output.Append("         onException: function(e, type, msg) { if (__errorcallback) __errorcallback(msg); }\r\n");
            output.Append("       }).request(data);\r\n");
            output.Append("};\r\n");

            return output.ToString();
        }
    }
}
