//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================
using System;
using System.Web;
using System.Web.UI;

namespace WebFormRail
{
    public class ViewData
    {
        private readonly ViewDataCollection _vars;
        private readonly System.Web.UI.ControlCollection _controls;
        private readonly WebFormView _view;

        internal ViewData(WebFormView view)
        {
            _view = view;
            _vars = new ViewDataCollection();
            _controls = new System.Web.UI.ControlCollection(View.Page);
        }

        public ViewDataCollection Variables
        {
            get { return _vars; }
        }

        public System.Web.UI.ControlCollection Controls
        {
            get { return _controls; }
        }

        public WebFormView View
        {
            get { return _view; }
        }
    }
}
