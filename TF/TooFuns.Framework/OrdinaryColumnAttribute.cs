using System;
namespace TooFuns.Framework
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class OrdinaryColumnAttribute : Attribute
	{
		private string[] values;
		public string[] Values
		{
			get
			{
				return this.values;
			}
		}
		public OrdinaryColumnAttribute(params string[] values)
		{
			this.values = values;
		}
	}
}
