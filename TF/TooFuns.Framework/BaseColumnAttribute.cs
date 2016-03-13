using System;
namespace TooFuns.Framework
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class BaseColumnAttribute : Attribute
	{
		private string[] values;
		public string[] Values
		{
			get
			{
				return this.values;
			}
		}
		public BaseColumnAttribute(params string[] values)
		{
			this.values = values;
		}
	}
}
