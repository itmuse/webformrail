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
    public class JQueryAjaxProvider : IAjaxProvider
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

        public string GenerateJavascriptMethod(string url, string className, string methodName, string[] parameters, bool includeFormData)
        {
            StringBuilder output = new StringBuilder();

            if (className != null && className.Length > 0)
                output.Append(className + '.');

            output.Append(methodName);
            output.Append(" = function(");

            if (includeFormData)
            {
                output.Append("form,");
            }
            else
            {
                output.Append(String.Join(", ", parameters));

                if (parameters.Length > 0)
                    output.Append(',');
            }

            output.Append("__callback, __errorcallback");

            output.Append(")\r\n");
            output.Append("{ jQuery.ajax(\r\n");
            output.Append("       {\r\n");
            output.Append("         type: \"POST\",\r\n");
            output.Append("         url: \"" + url + "\",\r\n");

            if (includeFormData)
            {
                output.Append("         data: $(form).serialize() , ");
            }
            else
            {
                output.Append("         data: { ");

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (i > 0)
                        output.Append(',');

                    output.Append("\"" + parameters[i] + "\" : " + parameters[i]);
                }

                output.Append("},\r\n");
            }

            output.Append("         dataType: \"json\",\r\n");
            output.Append("         success: function (result) { if (__callback && !result.error) __callback(result.value); else if (result.error && __errorcallback) __errorcallback(result.error); },\r\n");
            output.Append("         error: function(xhr,errMsg,ex) { if (__errorcallback) __errorcallback(errMsg); }\r\n");
            output.Append("       });\r\n");
            output.Append("};\r\n");

            return output.ToString();
        }
    }
}
