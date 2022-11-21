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
using System.Web;

namespace WebFormRail
{
    public class UrlParameterCollection
    {
        private readonly Dictionary<string,List<string>> _parameters = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

        public UrlParameterCollection()
        {
        }

        public void Add(string name, string value)
        {
            List<string> values;

            if (!_parameters.TryGetValue(name, out values))
            {
                values = new List<string>();

                _parameters[name] = values;
            }

            if (value != null)
                values.Add(value);
        }

        public void Remove(string name)
        {
            _parameters.Remove(name);
        }

        public void Replace(string name, string value)
        {
            Remove(name);
            Add(name, value);
        }

        public void RemoveAll()
        {
            _parameters.Clear();
        }

        public UrlParameterCollection Clone()
        {
            UrlParameterCollection collection = new UrlParameterCollection();

            foreach (KeyValuePair<string,List<string>> entry in _parameters)
            {
                collection._parameters[entry.Key] = new List<string>(entry.Value);
            }

            return collection;
        }

        public UrlParameterCollection(NameValueCollection queryString)
        {
            MethodInfo baseGet = typeof(NameObjectCollectionBase).GetMethod("BaseGet", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[] { typeof(Int32) }, null);

            for (int i = 0; i < queryString.Count; i++)
            {
                string key = queryString.GetKey(i);

                ArrayList valueList = (ArrayList)baseGet.Invoke(queryString, new object[] { i });

                if (valueList.Count == 0)
                {
                    Add(key,null);
                }
                else
                {
                    foreach (string value in valueList)
                    {
                        Add(key,value);
                    }
                }
            }
        }

        public override string ToString()
        {
            string q = "";

            foreach (KeyValuePair<string,List<string>> entry in _parameters)
            {
                string key = entry.Key;

                if (key.Length > 0)
                    key = HttpUtility.UrlEncode(key) + '=';

                if (entry.Value.Count == 0)
                {
                    if (q.Length > 0)
                        q += '&';

                    q += key;
                }
                else
                {
                    foreach (string value in entry.Value)
                    {
                        if (q.Length > 0)
                            q += '&';

                        q += key + HttpUtility.UrlEncode(value);
                    }
                }
            }

            return q;
        }


    }
}