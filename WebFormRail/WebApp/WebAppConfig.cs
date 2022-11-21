//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace WebFormRail
{
    public static class WebAppConfig
    {
        private static Type _sessionType = typeof(SessionBase);

        private static ITranslationProvider _translationProvider;
        private static ISessionDataProvider _sessionDataProvider = new MinimalSessionDataProvider();
        private static ILoggingProvider _loggingProvider;
        private static IAjaxProvider _ajaxProvider = new JQueryAjaxProvider();
        private static ITemplateResolver _templateResolver;

        private static Dictionary<string, WebFormControllerClass> _webFormControllerClasses = null;
        private static readonly List<Assembly> _registeredAssemblies = new List<Assembly>();
        private static readonly List<ICustomObjectCreator> _customObjectCreators = new List<ICustomObjectCreator>();

        private static string _templatePath = "pageviews";
        private static string _defaultLayout = "masterpage";
        private static string _defaultLanguage = "en";
        private static string _pageExtension = "aspx";
        private static bool   _useLanguagePath = false;
        private static bool   _forceLanguagePath = false;
        private static string _controllerAgnomen = "controller";
        private static string _defaultAction = "index";
        private static string _currentThemas = "default";

        private static readonly string _version;

        public static event Action<Exception> ExceptionOccurred;
        public static event Action<string> PageNotFound;
        public static event Action<SessionBase> SessionCreated;

        static WebAppConfig()
        {
            _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            string applicationClassName = null;

            foreach (string configKey in ConfigurationManager.AppSettings.Keys)
            {
                string configValue = ConfigurationManager.AppSettings[configKey];

                switch (configKey.ToLower())
                {
                    case "webformrail.defaultlayout":
                        DefaultLayout = configValue;
                        break;
                    case "webformrail.templatepath":
                        TemplatePath = configValue;
                        break;
                    case "webformrail.pageextension":
                        PageExtension = configValue.StartsWith(".") ? configValue.Substring(1) : configValue;
                        break;
                    case "webformrail.uselanguagepath":
                        UseLanguagePath = (configValue.ToLower() == "true");
                        break;
                    case "webformrail.forcelanguagepath":
                        ForceLanguagePath = (configValue.ToLower() == "true");
                        break;
                    case "webformrail.defaultlanguage":
                        DefaultLanguage = configValue.ToUpper();
                        break;
                    case "webformrail.applicationclass":
                        applicationClassName = configValue;
                        break;
                    case "webformrail.currentthemas":
                        CurrentThemas = configValue;
                        break;
                    case "webformrail.defaultaction":
                        DefaultAction = configValue;
                        break;
                }
            }

            if (applicationClassName == null)
                throw new Exception("No WebFormRail.ApplicationClass defined in web.config");

            Type appType = Type.GetType(applicationClassName, false);

            if (appType == null)
                throw new Exception("Application class {" + applicationClassName + "} could not be loaded");

            MethodInfo initMethod = appType.GetMethod("Init", new Type[0]);

            if (initMethod == null || !initMethod.IsStatic)
                throw new Exception("No \"public static void Init()\" method defined for class " + appType.Name);

            RegisterAssembly(appType.Assembly);

            initMethod.Invoke(null, null);

            LoadWebFormControllerClasses();
        }

        public static void Init()
        {
            // This method does nothing, but it will trigger the execution of the static constructor for WebApp
        }
        #region properties
        public static Type SessionType
        {
            get
            {
                return _sessionType;
            }
            set
            {
                if (!typeof(SessionBase).IsAssignableFrom(value))
                    throw new Exception("SessionType should be derived from SessionBase");

                _sessionType = value;
            }
        }

        public static ITranslationProvider TranslationProvider
        {
            get { return _translationProvider; }
            set { _translationProvider = value; }
        }

        public static ISessionDataProvider SessionDataProvider
        {
            get { return _sessionDataProvider; }
            set { _sessionDataProvider = value; }
        }

        public static ILoggingProvider LoggingProvider
        {
            get { return _loggingProvider; }
            set { _loggingProvider = value; }
        }

        public static IAjaxProvider AjaxProvider
        {
            get { return _ajaxProvider; }
            set { _ajaxProvider = value; }
        }

        public static ITemplateResolver TemplateResolver
        {
            get { return _templateResolver; }
            set { _templateResolver = value; }
        }

        internal static List<ICustomObjectCreator> CustomObjectCreators
        {
            get { return _customObjectCreators; }
        }

        public static string TemplatePath
        {
            get { return string.Format("{0}/{1}", _templatePath, CurrentThemas); }
            set { _templatePath = value; }
        }
        public static string CurrentThemas
        {
            get { return _currentThemas; }
            set { _currentThemas = value; }
        }

        public static string DefaultLayout
        {
            get { return _defaultLayout; }
            set { _defaultLayout = value; }
        }

        public static string DefaultLanguage
        {
            get { return _defaultLanguage; }
            set { _defaultLanguage = value; }
        }

        public static string PageExtension
        {
            get { return _pageExtension; }
            set { _pageExtension = value; }
        }

        public static bool UseLanguagePath
        {
            get { return _useLanguagePath; }
            set { _useLanguagePath = value; }
        }

        public static bool ForceLanguagePath
        {
            get { return _forceLanguagePath; }
            set { _forceLanguagePath = value; }
        }

        public static string Version
        {
            get { return _version; }
        }
        public static string DefaultAction
        {
            get { return _defaultAction; }
            set { _defaultAction = value; }
        }
        public static string ControllerAgnomen
        {
            get { return _controllerAgnomen; }
            set { _controllerAgnomen = value; }
        }
        #endregion

        public static void RegisterAssembly(Assembly assembly)
        {
            // We don't need to load anything, because if the assembly object is here, the assembly is automatically loaded

            if (_registeredAssemblies.Find(delegate(Assembly a) { return a.FullName == assembly.FullName; }) == null)
            {
                _registeredAssemblies.Add(assembly);
            }
        }

 
        public static void RegisterAssembly(string assemblyPath)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyPath);

            RegisterAssembly(assembly);
        }

        internal static WebFormControllerClass GetWebFormControllerClass(string url)
        {
            WebFormControllerClass webFormControllerClass;

            if (_webFormControllerClasses.TryGetValue(url, out webFormControllerClass))
                return webFormControllerClass;
            else
                return null;
        }

        private static void LoadWebFormControllerClasses()
        {
            _webFormControllerClasses = new Dictionary<string, WebFormControllerClass>(StringComparer.OrdinalIgnoreCase);

            List<Type> pageTypes = new List<Type>();

            foreach (Assembly assembly in _registeredAssemblies)
            {
                pageTypes.AddRange(WebFormRailUtil.FindCompatibleTypes(assembly, typeof(WebFormController)));
            }

            foreach (Type type in pageTypes)
            {
                WebFormControllerClass controllerClass = new WebFormControllerClass(type);

                _webFormControllerClasses.Add(controllerClass.ControllerName, controllerClass);
            }
        }

        public static void RegisterCustomObjectCreator(ICustomObjectCreator creator)
        {
            _customObjectCreators.Add(creator);
        }

        internal static bool Fire_ExceptionHandler(Exception ex)
        {
            if (ExceptionOccurred != null)
            {
                ExceptionOccurred(ex);

                return true;
            }

            return false;
        }

        internal static bool Fire_PageNotFound(string controllerName)
        {
            if (PageNotFound != null)
            {
                PageNotFound(controllerName);

                return true;
            }

            return false;
        }

        internal static bool Fire_SessionCreated(SessionBase session)
        {
            if (SessionCreated != null)
            {
                SessionCreated(session);

                return true;
            }

            return false;
        }
    }
}
