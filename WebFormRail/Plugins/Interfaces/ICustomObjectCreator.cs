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
    public interface ICustomObjectCreator
    {
        bool TryConvert(string value, Type objectType, out object obj);
    }
}
