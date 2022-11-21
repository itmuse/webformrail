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
    public interface ITemplateResolver
    {
        /// <summary>
        /// Converts a logical view name (without extension, in the format Path1/Path2/ViewName) to a physical filename
        /// </summary>
        /// <param name="virtualPath">The virtual path of the view (Path1/Path2/ViewName)</param>
        /// <param name="isLayout">true if the template is a layout template, false if it is a normal view template</param>
        /// <returns>The physical filename (full path) name of the template, or null to use WebFormRail.NET default resolver</returns>
        string ResolveTemplate(string virtualPath, bool isLayout);
    }
}
