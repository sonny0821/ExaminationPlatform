using System;
namespace TooFuns.Framework
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class PrimaryKeyAttribute : Attribute
	{
		private string[] values;
		public string[] Values
		{
			get
			{
				return this.values;
			}
		}
		public PrimaryKeyAttribute(params string[] values)
		{
			this.values = values;
		}
	}
}
