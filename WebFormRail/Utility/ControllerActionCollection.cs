//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace WebFormRail
{
    public class ControllerActionCollection
    {
        private readonly Dictionary<string, ControllerCollection> controllers = new Dictionary<string, ControllerCollection>(StringComparer.InvariantCultureIgnoreCase);

        public ControllerActionCollection()
        {
        }
        public void Add(string controllerName, Type controllerType, string actionName,MethodInfo actionInfo)
        {
            ControllerCollection controller;

            if (!controllers.TryGetValue(controllerName, out controller))
            {
                Dictionary<string, ActionCollection> action = new Dictionary<string, ActionCollection>(StringComparer.InvariantCultureIgnoreCase);
                action.Add(actionName, new ActionCollection(actionName, actionInfo));

                controller = new ControllerCollection(controllerName, controllerType, action);
                controllers.Add(controllerName, controller);
            }

            if (controller != null)
            {
                if (!controller.ActionCollection.ContainsKey(actionName))
                    controller.ActionCollection.Add(actionName,new ActionCollection(actionName,actionInfo));
            }
        }
        public Dictionary<string, ControllerCollection> AllControllers
        {
            get { return controllers; }
        }
        public Type GetControllerType(string controllerName)
        {
            ControllerCollection controller;
            if(!controllers.TryGetValue(controllerName,out controller))
                return null;
            else
                return controller.ControllType;
        }
        public Dictionary<string, ActionCollection> GetAction(string controllerName)
        {
            ControllerCollection controller;

            if (!controllers.TryGetValue(controllerName, out controller))
                return null;//new Dictionary<string, ActionCollection>();
            else
                return controller.ActionCollection;
        }
        public ActionCollection GetAction(string controllerName, string actionName)
        {
            Dictionary<string, ActionCollection> actions = GetAction(controllerName);

            ActionCollection action = null;
            if (actions != null && !actions.TryGetValue(actionName, out action))
                return null;
            else
                return action;
        }
        public MethodInfo GetActionInfo(string controllerName, string actionName)
        {
            ActionCollection action = GetAction(controllerName, actionName);
            if (action != null)
                return action.ActionInfo;
            else
                return null;
        }
        public bool ExistAction(string controllerName, string actionName)
        {
            return GetAction(controllerName, actionName) != null;
        }
        public bool ExistController(string controllerName)
        {
            return GetAction(controllerName).Count != 0;
        }
    }
    public class ControllerCollection
    {
        public ControllerCollection(string controllerName, Type controllerType, Dictionary<string, ActionCollection> action)
        {
            ControllerName = controllerName;
            ControllType = controllerType;
            ActionCollection = action;
        }
        private string controllerName;
        public string ControllerName
        {
            get { return controllerName; }
            set { controllerName = value; }
        }
        private Type controllerType;
        public Type ControllType
        {
            get { return controllerType; }
            set { controllerType = value; }
        }
        private Dictionary<string, ActionCollection> actionCollection;
        public Dictionary<string, ActionCollection> ActionCollection
        {
            get { return actionCollection; }
            set { actionCollection = value; }
        }
    }
    public class ActionCollection
    {
        public ActionCollection(string actionName, MethodInfo actionInfo)
        {
            ActionName = actionName;
            ActionInfo = actionInfo;
        }
        private string actionName;
        public string ActionName
        {
            get { return actionName; }
            set { actionName = value; }
        }
        private MethodInfo actionInfo;
        public MethodInfo ActionInfo
        {
            get { return actionInfo; }
            set { actionInfo = value; }
        }
    }
}
