//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebFormRail
{
    public class WebFormRailException : Exception
    {
        public WebFormRailException(string message) : base(message)
        {
        }


        public WebFormRailException(string message, Exception innerException) : base(message, innerException)
        {
        }


        public WebFormRailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
