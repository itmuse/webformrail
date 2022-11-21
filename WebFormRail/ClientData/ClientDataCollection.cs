//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace WebFormRail
{
    public class ClientDataCollection
    {
        private readonly bool _post;

        public ClientDataCollection(bool post)
        {
            _post = post;
        }

        private NameValueCollection Data
        {
            get
            {
                if (_post)
                    return WebAppContext.Request.Form;
                else
                    return WebAppContext.Request.QueryString;
            }
        }

        public string[] Variables
        {
            get { return Data.AllKeys; }
        }

        public bool Has(string name)
        {
            return Data[name] != null;
        }

        public string Get(string name)
        {
            return Data[name];
        }

        public string Get(string name, string defaultValue)
        {
            return Data[name] ?? defaultValue;
        }

        public object Get(string name, Type t)
        {
            if (typeof(Array).IsAssignableFrom(t))
            {
                ArrayList valueList = (ArrayList)typeof(NameObjectCollectionBase).InvokeMember("BaseGet", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, Data, new object[] { name });

                Type elementType = t.GetElementType();

                Array array = Array.CreateInstance(elementType, valueList.Count);

                for (int i = 0; i < valueList.Count; i++)
                {
                    array.SetValue(WebFormRailUtil.ConvertString((string) valueList[i], elementType), i);
                }

                return array;
            }

            return WebFormRailUtil.ConvertString(Get(name), t);
        }

        public string this[string name]
        {
            get { return Data[name]; }
        }

        public T Get<T>(string name)
        {
            return Get(name, default(T));
        }

        public T Get<T>(string name, T defaultValue)
        {
            if (!Has(name))
                return defaultValue;

            return (T)Get(name, typeof(T));
        }

    }

 
}
