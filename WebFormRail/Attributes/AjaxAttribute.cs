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
    [AttributeUsage(AttributeTargets.Method)]
    public class AjaxAttribute : Attribute
    {
        private readonly string _javaScriptAlias;
        private readonly bool _useFormData = false;

        public AjaxAttribute()
        {
        }

        public AjaxAttribute(string javascriptAlias)
        {
            _javaScriptAlias = javascriptAlias;
        }

        public AjaxAttribute(bool useFormData)
        {
            _useFormData = useFormData;
        }

        public AjaxAttribute(bool useFormData, string javascriptAlias)
        {
            _useFormData = useFormData;
            _javaScriptAlias = javascriptAlias;
        }

        public string JavaScriptAlias
        {
            get { return _javaScriptAlias; }
        }

        public bool UseFormData
        {
            get { return _useFormData; }
        }
    }

    
}
