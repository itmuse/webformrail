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
    internal static class AjaxHelper
    {
        private class JSONError
        {
            public JSONError(string error)
            {
                this.error = error;
            }

            public string error;
        }

        private class JSONReturnValue
        {
            public JSONReturnValue(object value)
            {
                this.value = value;
            }

            public object value;
           
        }

        public static string GenerateJSONError(string message)
        {
            JSONError error = new JSONError(message);

            return JSONSerializer.ToJSON(error);
        }

        public static string GenerateJSONReturnValue(object value)
        {
            JSONReturnValue returnValue = new JSONReturnValue(value);

            return JSONSerializer.ToJSON(returnValue);
        }

    }
}
