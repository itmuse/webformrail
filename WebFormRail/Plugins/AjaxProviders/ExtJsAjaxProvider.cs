//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace WebFormRail
{
    public class ExtJsAjaxProvider : IAjaxProvider
    {
        public string GenerateJavascriptClassName(string className)
        {
            return "Ext.namespace('" + className + "');";
        }

        public string GenerateJavascriptMethod(string url, string className, string methodName, string[] parameters, bool includeFormData)
        {
            StringBuilder output = new StringBuilder();

            if (className != null && className.Length > 0)
                output.Append(className + '.');

            output.Append(methodName);
            output.Append(" = function(");
            output.Append(String.Join(", ", parameters));

            if (parameters.Length > 0)
                output.Append(',');

            output.Append("__callback, __errorcallback");

            output.Append(")\r\n");
            output.Append("{ Ext.Ajax.request(\r\n");
            output.Append("       {\r\n");
            output.Append("         method: \"POST\",\r\n");
            output.Append("         url: \"" + url + "\",\r\n");
            output.Append("         params: { ");

            for (int i = 0; i < parameters.Length; i++)
            {
                if (i > 0)
                    output.Append(',');

                output.Append("\"" + parameters[i] + "\" : " + parameters[i]);
            }

            output.Append("},\r\n");
            output.Append("         success: function (xhr) { var result = eval('result=' + xhr.responseText); if (__callback && !result.error) __callback(result.value); else if (result.error && __errorcallback) __errorcallback(result.error.Message); },\r\n");
            output.Append("         failure: function(xhr) { if (__errorcallback) __errorcallback(xhr.statusText); }\r\n");
            output.Append("       });\r\n");
            output.Append("};\r\n");

            return output.ToString();
        }

    }
}
