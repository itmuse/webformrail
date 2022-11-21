//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace WebFormRail
{
    /// <summary>
    /// A general purpose class with static utility functions.
    /// </summary>
    public class WebFormRailUtil
    {
        /// <summary>
        /// Finds all types derived from the given type, limiting the search to the given assembly
        /// </summary>
        /// <param name="baseType">The base type or interface to use for finding types</param>
        /// <param name="assembly">The assembly to look into</param>
        /// <returns>An array of all types found in the given assembly which are either derived from the given type, or implement the given interface</returns>
        internal static Type[] FindCompatibleTypes(Assembly assembly, Type baseType)
        {
            List<Type> types = new List<Type>();

            foreach (Type type in assembly.GetTypes())
            {
                if (type != baseType && baseType.IsAssignableFrom(type))
                    types.Add(type);
            }

            return types.ToArray();
        }


        public static T ConvertString<T>(string stringValue)
        {
            object o = ConvertString(stringValue, typeof (T));

            if (o == null)
                return default(T);
            else
                return (T) o;
        }

        public static object ConvertString(string stringValue, Type targetType)
        {
            if (stringValue == null)
                return null;

            if (targetType == typeof(string))
                return stringValue;

            object value = stringValue;

            if (IsNullable(targetType))
            {
                if (stringValue.Trim().Length == 0)
                    return null;

                targetType = GetRealType(targetType);
            }

            if (targetType != typeof(string))
            {
                if (targetType == typeof(double) || targetType == typeof(float))
                {
                    double doubleValue;

                    if (!double.TryParse(stringValue.Replace(',', '.'), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out doubleValue))
                        value = null;
                    else
                        value = doubleValue;
                }
                else if (targetType == typeof(decimal))
                {
                    decimal decimalValue;

                    if (!decimal.TryParse(stringValue.Replace(',', '.'), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out decimalValue))
                        value = null;
                    else
                        value = decimalValue;
                }
                else if (targetType == typeof(Int32) || targetType == typeof(Int16) || targetType == typeof(Int64) || targetType == typeof(SByte) || targetType.IsEnum)
                {
                    long longValue;

                    if (!long.TryParse(stringValue, out longValue))
                        value = null;
                    else
                        value = longValue;
                }
                else if (targetType == typeof(UInt32) || targetType == typeof(UInt16) || targetType == typeof(UInt64) || targetType == typeof(Byte))
                {
                    ulong longValue;

                    if (!ulong.TryParse(stringValue, out longValue))
                        value = null;
                    else
                        value = longValue;
                }
                else if (targetType == typeof(DateTime))
                {
                    DateTime dateTime;

                    if (!DateTime.TryParseExact(stringValue, new string[] { "yyyyMMdd", "yyyy-MM-dd", "yyyy.MM.dd", "yyyy/MM/dd", "yyyy-M-d", "yyyy.M.d", "yyyy/M/d" }, null, DateTimeStyles.NoCurrentDateDefault, out dateTime))
                        value = null;
                    else
                        value = dateTime;
                }
                else if (targetType == typeof(bool))
                {
                    value = (stringValue == "1" || stringValue.ToUpper() == "Y" || stringValue.ToUpper() == "YES" || stringValue.ToUpper() == "T" || stringValue.ToUpper() == "TRUE");
                }
                else
                {
                    value = null;

                    foreach (ICustomObjectCreator objectCreator in WebAppConfig.CustomObjectCreators)
                    {
                        if (objectCreator.TryConvert(stringValue, targetType, out value))
                            return value;
                    }
                }
            }

            if (value == null)
                return null;

            if (targetType.IsValueType)
            {
                if (!targetType.IsGenericType)
                {
                    if (targetType.IsEnum)
                        return Enum.ToObject(targetType, value);
                    else
                        return Convert.ChangeType(value, targetType);
                }

                if (targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type sourceType = value.GetType();

                    Type underlyingType = targetType.GetGenericArguments()[0];

                    if (sourceType == underlyingType)
                        return value;

                    if (underlyingType.IsEnum)
                    {
                        return Enum.ToObject(underlyingType, value);
                    }
                    else
                    {
                        return Convert.ChangeType(value, underlyingType);
                    }
                }
            }

            return value;
        }

        public static object ConvertType(object value, Type targetType)
        {
            if (value == null)
                return null;

            if (value.GetType() == targetType)
                return value;

            if (targetType.IsValueType)
            {
                if (!targetType.IsGenericType)
                {
                    if (targetType.IsEnum)
                        return Enum.ToObject(targetType, value);
                    else
                        return Convert.ChangeType(value, targetType);
                }

                if (targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type underlyingType = targetType.GetGenericArguments()[0];

                    return ConvertType(value, underlyingType);
                }
            }

            if (targetType.IsAssignableFrom(value.GetType()))
                return value;
            else
                return Convert.ChangeType(value, targetType);
        }

        public static bool IsNullable(Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        internal static Type GetRealType(Type type)
        {
            if (IsNullable(type))
                return type.GetGenericArguments()[0];

            return type;
        }

        internal static object GetObjectProperty(object item,string property)
        {
            if (item == null)
                return null;

            int dotIdx = property.IndexOf('.');

            if (dotIdx > 0)
            {
                object obj = GetObjectProperty(item,property.Substring(0,dotIdx));

                return GetObjectProperty(obj,property.Substring(dotIdx+1));
            }

            if (item is Table.Row)
            {
                Table.Row dic = (Table.Row)item;

                return dic[property];
            }

            PropertyInfo propInfo = null;
            Type objectType = item.GetType();

            while (propInfo == null && objectType != null)
            {
                propInfo = objectType.GetProperty(property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                objectType = objectType.BaseType;
            }

            if (propInfo != null)
                return propInfo.GetValue(item, null);

            FieldInfo fieldInfo = item.GetType().GetField(property, BindingFlags.Public | BindingFlags.Instance);

            if (fieldInfo != null)
                return fieldInfo.GetValue(item);

            propInfo = item.GetType().GetProperty("Item", new Type[] { typeof(string) });

            if (propInfo != null)
                return propInfo.GetValue(item, new object[] { property });

            return null;
        }

        internal static byte[] GetResourceData(Assembly assembly , string resourceName)
        {
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                byte[] data = new byte[stream.Length];

                stream.Read(data, 0, (int)stream.Length);

                return data;

                //return Encoding.UTF8.GetString(data);
            }
        }

        public static string TranslateAbsolutePath(string path)
        {
            if (path.StartsWith("/"))
                return path;

            if (path.StartsWith("~/"))
            {
                string appPath = WebAppContext.Request.ApplicationPath;

                if (!appPath.EndsWith("/"))
                    appPath += "/";

                return appPath + path.Substring(2);
            }

            throw new Exception("Invalid path passed to Util.MakeAbsolutePath()");
        }

        public static string CreateResourceUrl(Assembly assembly, string resourceName, string contentType)
        {
            string prefix;

            switch (contentType)
            {
                case "image/jpeg"      : prefix = "_jpg_"; break;
                case "image/gif"       : prefix = "_gif_"; break;
                case "text/javascript" : prefix = "_js_"; break;
                case "text/css"        : prefix = "_css_"; break;
                default: return null;
            }

            return TranslateAbsolutePath("~/" + prefix + '/' + EncodeToURL(assembly.GetName().Name) + '/' + EncodeToURL(resourceName) + '.' + WebAppConfig.PageExtension);
        }

        public static string EncodeToURL(string s)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s)).Replace('+', '_').Replace('/', '-').Replace('=','$');
        }

        public static string DecodeFromURL(string url)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(url.Replace('_', '+').Replace('-', '/').Replace('$','=')));
        }

    }
}
