using System;
namespace TooFuns.Framework
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class ColumnAttribute : Attribute
	{
		private string[] values;
		public string[] Values
		{
			get
			{
				return this.values;
			}
		}
		public ColumnAttribute(params string[] values)
		{
			this.values = values;
		}
	}
}
