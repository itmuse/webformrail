//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Reflection;

namespace WebFormRail
{

    /// <summary>
    /// The base class for ALL controls that must load their UI either through an external skin
    /// or via an in-page template.
    /// </summary>
    [ParseChildren(true), PersistChildren(false),]
    public abstract class WebFormControl : WebControl, INamingContainer
    {

        #region Composite Controls

        /// <exclude/>
        public override System.Web.UI.ControlCollection Controls
        {
            get
            {
                this.EnsureChildControls();
                return base.Controls;
            }
        }

        /// <exclude/>
        public override void DataBind()
        {
            this.EnsureChildControls();
        }

        #endregion

        #region External Skin

        /// <summary>
        /// Gets the name of the theme being used.
        /// </summary>
        protected virtual string ThemeName
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["UserTheme"] == null)
                        return "Default";
                    else
                        return (string)HttpContext.Current.Session["UserTheme"];
                }
                catch
                {
                    return "Default";
                }
            }
        }

        protected virtual string SkinFolder
        {
            get
            {
                return "~/ControlViews/" + ThemeName; //TODO Use correct skin folderSkins/
            }
        }

        private String SkinPath
        {
            get
            {
                return this.SkinFolder + ExternalSkinFileName;
            }
        }

        /// <summary>
        /// Gets the name of the skin file to load from
        /// </summary>
        protected virtual String ExternalSkinFileName
        {
            get
            {
                if (SkinName == null)
                    return CreateExternalSkinFileName(null);

                return SkinName;
            }
            set
            {
                SkinName = value;
            }
        }

        string skinName;
        public string SkinName
        {
            get
            {
                if (!string.IsNullOrEmpty(skinName) && !skinName.StartsWith("/"))
                {
                    skinName = "/" + skinName;
                }
                return skinName;
            }
            set
            {
                skinName = value;
            }
        }

        protected virtual string CreateExternalSkinFileName(string path)
        {
            return CreateExternalSkinFileName(path, "Skin-" + this.GetType().Name);
        }

        protected virtual string CreateExternalSkinFileName(string path, string name)
        {
            if (path != null && !path.EndsWith("/"))
            {
                path = path + "/";
            }

            return string.Format("{0}{1}.ascx", path, name);
        }


        private Boolean SkinFolderExists
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    String folderPath = context.Server.MapPath(this.SkinFolder);
                    return System.IO.Directory.Exists(folderPath);
                }
                return true;
            }
        }

        private Boolean SkinFileExists
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    String filePath = context.Server.MapPath(this.SkinPath);
                    return System.IO.File.Exists(filePath);
                }
                return true;
            }
        }

        private Boolean DefaultSkinFileExists
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    String filePath = context.Server.MapPath(this.DefaultSkinPath);
                    return System.IO.File.Exists(filePath);
                }
                return true;
            }
        }

        protected virtual string DefaultSkinPath
        {
            get
            {
                return "~/ControlViews/Default/" + ExternalSkinFileName;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The template used to override the default UI of the control.
        /// </summary>
        /// <remarks>
        /// All serverside controls that are in the default UI must exist and have the same ID's.
        /// </remarks>
        [
        Browsable(false),
        DefaultValue(null),
        Description("TODO SkinTemplate Description"),
        PersistenceMode(PersistenceMode.InnerProperty),
        ]
        public ITemplate SkinTemplate
        {
            get
            {
                return _skinTemplate;
            }
            set
            {
                _skinTemplate = value;
                ChildControlsCreated = false;
            }
        }
        private ITemplate _skinTemplate;

        #endregion
        /// <summary>
        /// No Start Span
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            //we don't need a span tag
        }

        /// <summary>
        /// No End Span
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            //we don't need a span tag
        }

        protected virtual string ControlText()
        {
            return null;
        }

        public override Page Page
        {
            get
            {
                //There is at least one control which causes Page to be null
                //this check should allow us to safely populate the page value

                //ReWritten for aspnet 2 support
                //if(base.Page == null && this.Context != null)
                //{
                //    base.Page = this.Context.Handler as System.Web.UI.Page;
                //}
                if (base.Page == null)
                {
                    base.Page = HttpContext.Current.Handler as System.Web.UI.Page;
                }

                return base.Page;
            }
            set
            {
                base.Page = value;
            }
        }


        /// <exclude/>
        public override System.Web.UI.Control FindControl(string id)
        {
            System.Web.UI.Control ctrl = base.FindControl(id);
            if (ctrl == null && this.Controls.Count == 1)
            {
                ctrl = this.Controls[0].FindControl(id);
            }
            return ctrl;
        }

        #region Extend method
        public virtual T FindControl<T>(string id)
        {
            Object ctrl = FindControl(id);
            if (ctrl == null)
            {
                return ClassHelper.CreateInstance<T>();
            }
            else
            {
                return (T)ctrl;
            }
        }
        public virtual void SetControlProperty(Control webControl, string propertyName, string value)
        {
            Type wctype = webControl.GetType();
            PropertyInfo property = wctype.GetProperty(propertyName);

            if (property != null)
            {
                property.SetValue(webControl, Convert.ChangeType(value, property.PropertyType), null);
            }
        }

        public virtual string GetControlValue(System.Web.UI.Control webControl)
        {
            Type wctype = webControl.GetType();

            PropertyInfo property = wctype.GetProperty("Text");

            if (property == null)
            {
                property = wctype.GetProperty("Value");
            }

            if (property == null)
            {
                property = wctype.GetProperty("SelectedValue");
            }

            if (property == null)
            {
                throw new Exception("Can not get value from " + webControl.ID + " control.");
            }

            return property.GetValue(webControl, null).ToString();
        }


        public virtual void SetControlValue(System.Web.UI.Control webControl, string value)
        {
            Type wctype = webControl.GetType();

            PropertyInfo property = wctype.GetProperty("Text");

            if (property == null)
            {
                property = wctype.GetProperty("Value");
            }
            if (property == null)
            {
                property = wctype.GetProperty("SelectedValue");
            }

            if (property == null)
            {
                throw new Exception("Borges framework can not set value for " + webControl.ID + " control.");
            }

            property.SetValue(webControl, value, null);
        }
        #endregion

        #region Load Control Methods

        /// <summary>
        /// First choice for skins. The value of Control() text will be interpreted as a skin. The 
        /// primary usage of this feature will be to serve skins from the database
        /// </summary>
        /// <returns></returns>
        protected virtual bool LoadTextBasedControl()
        {
            string text = ControlText();

            if (!string.IsNullOrEmpty(text))
            {
                System.Web.UI.Control skin = this.Page.ParseControl(text);
                skin.ID = "_";
                this.Controls.Add(skin);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads the skin file from the users current theme
        /// </summary>
        /// <returns></returns>
        protected virtual bool LoadThemedControl()
        {
            if (!string.IsNullOrEmpty(ThemeName) && SkinFolderExists)
            {
                if (SkinFileExists && this.Page != null)
                {
                    System.Web.UI.Control skin = this.Page.LoadControl(this.SkinPath);
                    skin.ID = "_";
                    this.Controls.Add(skin);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Load the skin as an inline template. By default, this will be the second option
        /// </summary>
        /// <returns></returns>
        protected virtual bool LoadSkinTemplate()
        {
            if (SkinTemplate != null)
            {
                SkinTemplate.InstantiateIn(this);
                return true;
            }

            return false;
        }

        /// <summary>
        /// By default, as a last ditch effort, we will try to load the skin file 
        /// from the default Theme
        /// </summary>
        /// <returns></returns>
        protected virtual bool LoadDefaultThemedControl()
        {
            if (this.Page != null && this.DefaultSkinFileExists)
            {
                System.Web.UI.Control defaultSkin = this.Page.LoadControl(this.DefaultSkinPath);
                defaultSkin.ID = "_";
                this.Controls.Add(defaultSkin);
                return true;
            }

            return false;
        }

        #endregion

        /// <exclude/>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            // 1) String Control
            Boolean _skinLoaded = LoadTextBasedControl();

            // 2) Inline Template
            if (!_skinLoaded)
            {
                _skinLoaded = LoadSkinTemplate();
            }

            // 3) Themed Control
            if (!_skinLoaded)
            {
                _skinLoaded = LoadThemedControl();
            }

            // 4) Default Control
            if (!_skinLoaded)
            {
                _skinLoaded = LoadDefaultThemedControl();
            }

            // 5) If none of the skin locations were successful, throw.
            if (!_skinLoaded)
            {
                throw new Exception("Critical error: The skinfile " + this.SkinPath + " could not be found. The skin must exist for [" + this.GetType().ToString() + "] control to render.");
            }

            if (_skinLoaded)
                AttachChildControls();
        }


        /// <summary>
        /// Override this method to attach templated or external skin controls to local references.
        /// </summary>
        /// <remarks>
        /// This will only be called if the non-default skin is used.
        /// </remarks>
        protected abstract void AttachChildControls();

        protected override void Render(HtmlTextWriter writer)
        {
            SourceMarker(true, writer);
            base.Render(writer);
            SourceMarker(false, writer);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        protected void SourceMarker(bool isStart, HtmlTextWriter writer)
        {

            if (isStart)
            {
                writer.WriteLine("<!-- Start: {0} -->", this.GetType());

                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(this.SkinPath)))
                    writer.WriteLine("<!-- Skin Path: {0} -->", this.SkinPath);
                else if (SkinTemplate != null)
                    writer.WriteLine("<!-- Inline Skin: {0} -->", true);
                else
                    writer.WriteLine("<!-- Skin Path: {0} -->", this.DefaultSkinPath);

            }
            else
            {
                writer.WriteLine("<!-- End: {0} -->", this.GetType());
            }
        }

        /// <summary>
        /// Identifies the control that fired posback.
        /// </summary>
        public System.Web.UI.Control GetPostBackControl()
        {
            return GetPostBackControl(this);
        }

        public System.Web.UI.Control GetPostBackControl(System.Web.UI.Control container)
        {
            // Note: this is an adapted eggheadcafe.com code.
            //
            System.Web.UI.Control control = null;

            string ctrlname = Page.Request.Params["__EVENTTARGET"];
            if (ctrlname != null && ctrlname != String.Empty)
            {
                string[] tokens = ctrlname.Split(new char[1] { ':' });
                if (tokens != null && tokens.GetLength(0) > 0)
                    ctrlname = tokens[(tokens.GetLength(0) - 1)];

                control = this.FindControl(ctrlname);
            }
            else
            {
                // If __EVENTTARGET is null, control is a button type and need to 
                // iterate over the form collection to find it
                //
                string ctrlStr = String.Empty;
                System.Web.UI.Control c = null;
                foreach (string ctl in Page.Request.Form)
                {

                    // Handle ImageButton controls
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        ctrlStr = ctl.Substring(0, (ctl.Length - 2));
                        c = this.FindControl(ctrlStr);
                    }
                    else
                    {
                        c = this.FindControl(ctl);
                    }
                    if (c is System.Web.UI.WebControls.Button ||
                        c is System.Web.UI.WebControls.ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }

            return control;
        }

    }
}

