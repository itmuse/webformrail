//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;

namespace WebFormRail
{
    public class ClientDataAttribute : Attribute
    {
        private readonly string _name;
        private readonly bool _useGet;
        private readonly bool _usePost;

        protected ClientDataAttribute(string name, bool useGet, bool usePost)
        {
            _name = name;
            _useGet = useGet;
            _usePost = usePost;
        }

        public string Name
        {
            get { return _name; }
        }

        public bool UseGet
        {
            get { return _useGet; }
        }

        public bool UsePost
        {
            get { return _usePost; }
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PostAttribute : ClientDataAttribute
    {
        public PostAttribute(string parameterName) 
            : base(parameterName,false,true)
        {
        }

    }


}
