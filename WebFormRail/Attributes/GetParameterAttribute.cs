//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;

namespace WebFormRail
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class GetAttribute : ClientDataAttribute
    {
        public GetAttribute(string parameterName)
            : base(parameterName, true, false)
        {
        }

    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class GetOrPostAttribute : ClientDataAttribute
    {
        public GetOrPostAttribute(string parameterName)
            : base(parameterName, true, true)
        {
        }

    }
}
