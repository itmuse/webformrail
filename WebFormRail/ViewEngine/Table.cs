//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace WebFormRail
{
	public class ViewDataCollection : Table.Row
	{
	}

	public class Table : List<Table.Row>
	{
		public int AddRow(Row rec)
		{
			Add(rec);

			return Count-1;
		}

        public Row AddRow(object rec)
        {
            return base[AddRow(new Row(rec))];
        }

		public Row AddRow()
		{
			return base[ AddRow(new Row()) ];
		}

		public Row LastRow
		{
			get
			{
				if (Count > 0)
					return base[Count-1];
				else
					return null;
			}
		}

		public class Row : IDictionary<string,object>
		{
            private readonly Dictionary<string, object> _dic = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
		    private object _dataObject;
	    
			public Row()
			{
			}

		    public Row(object dataObject)
		    {
		        _dataObject = dataObject;
		    }

		    public object DataObject
		    {
		        get { return _dataObject; }
		    }

		    public Table AddTable(string name)
			{
				Table table = new Table();

				this[name] = table;

				return table;
			}
			
			public void Merge(Row row)
			{
				foreach (string s in row.Keys)
					this[s] = row[s];
			}

            public Row Clone()
            {
                Row newRow = (Row) Activator.CreateInstance(GetType());

                foreach (string key in Keys)
                    newRow[key] = this[key];

                return newRow;
            }

            public bool HasProperty(string key)
            {
                if (_dataObject == null)
                    return false;

                return _dataObject.GetType().GetProperty(key,BindingFlags.Instance|BindingFlags.Public|BindingFlags.FlattenHierarchy) != null;
            }

		    public bool ContainsKey(string key)
		    {
		        return _dic.ContainsKey(key) || HasProperty(key);
		    }

		    public void Add(string key, object value)
		    {
		        _dic.Add(key,value);
		    }

		    public bool Remove(string key)
		    {
		        return _dic.Remove(key);
		    }

		    public bool TryGetValue(string key, out object value)
		    {
		        if (_dic.TryGetValue(key, out value))
		            return true;

                value = GetDataObjectProperty(key);

                return true;
		    }

		    private object GetDataObjectProperty(string key)
		    {
                if (_dataObject == null)
                    return null;

		        MemberInfo[] members = _dataObject.GetType().GetMember(key);

                if (members.Length > 0)
                {
                    MemberInfo member = members[0];

                    if (members.Length > 1) // CoolStorage, ActiveRecord and Dynamic Proxy frameworks sometimes return > 1 member
                    {
                        foreach (MemberInfo mi in members)
                            if (mi.DeclaringType == _dataObject.GetType())
                                member = mi;
                    }

                    if (member is PropertyInfo)
                    {
                        return ((PropertyInfo)member).GetValue(_dataObject, null);
                    }

                    if (member is FieldInfo)
                    {
                        return ((FieldInfo)member).GetValue(_dataObject);
                    }
                }

		        return null;
		    }

		    public object this[string key]
		    {
                get
                {
                    object value;

                    if (TryGetValue(key, out value))
                        return value;

                    return null;
                }
		        set
		        {
		            _dic[key] = value;
		        }
		    }

		    public ICollection<string> Keys
		    {
		        get { return _dic.Keys; }
		    }

		    public ICollection<object> Values
		    {
		        get { return _dic.Values; }
		    }

		    void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
		    {
                ((ICollection<KeyValuePair<string, object>>)_dic).Add(item);
		    }

		    public void Clear()
		    {
		        _dic.Clear();

		        _dataObject = null;
		    }

		    bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
		    {
		        return ((ICollection<KeyValuePair<string, object>>) _dic).Contains(item);
		    }

		    void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		    {
		        ((ICollection<KeyValuePair<string, object>>)_dic).CopyTo(array,arrayIndex);
		    }

		    bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
		    {
		        return ((ICollection<KeyValuePair<string, object>>) _dic).Remove(item);
		    }

		    public int Count
		    {
                get { return _dic.Count; }
		    }

		    bool ICollection<KeyValuePair<string, object>>.IsReadOnly
		    {
                get { return ((ICollection<KeyValuePair<string, object>>) _dic).IsReadOnly; }
		    }

		    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
		    {
		        throw new NotImplementedException();
		    }

		    public IEnumerator GetEnumerator()
		    {
		        return ((IEnumerable<KeyValuePair<string, object>>) this).GetEnumerator();
		    }
		}
	}
}
