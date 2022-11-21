//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WebFormRail
{
    public class TemplateNotFoundException : Exception
    {
        public string FileName;

        public TemplateNotFoundException(string fileName)
            : base("Template not found: " + Path.GetFileName(fileName))
        {
            FileName = fileName;
        }
    }
}
