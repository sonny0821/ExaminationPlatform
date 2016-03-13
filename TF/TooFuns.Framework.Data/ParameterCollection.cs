using System;
using System.ComponentModel;
using System.Data.Common;
namespace TooFuns.Framework.Data
{
	public class ParameterCollection
	{
		private DbParameterCollection parameterCollection;
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Count
		{
			get
			{
				return this.parameterCollection.Count;
			}
		}
		internal ParameterCollection(DbParameterCollection parameterCollection)
		{
			this.parameterCollection = parameterCollection;
		}
		public int Add(Parameter parameter)
		{
			return this.parameterCollection.Add(parameter.InnerParameter);
		}
		public void Clear()
		{
			this.parameterCollection.Clear();
		}
		public bool Contains(string value)
		{
			return this.parameterCollection.Contains(value);
		}
		public void CopyTo(Array array, int index)
		{
			this.parameterCollection.CopyTo(array, index);
		}
		public int IndexOf(object value)
		{
			return this.parameterCollection.IndexOf(value);
		}
		public int IndexOf(string parameterName)
		{
			return this.parameterCollection.IndexOf(parameterName);
		}
		public void Insert(int index, object value)
		{
			this.parameterCollection.Insert(index, value);
		}
		public void Remove(object value)
		{
			this.parameterCollection.Remove(value);
		}
		public void RemoveAt(int index)
		{
			this.parameterCollection.RemoveAt(index);
		}
		public void RemoveAt(string parameterName)
		{
			this.parameterCollection.RemoveAt(parameterName);
		}
	}
}
