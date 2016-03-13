using System;
namespace TooFuns.Framework
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class TableAttribute : Attribute
	{
		private string name;
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		public TableAttribute(string name)
		{
			this.name = name;
		}
	}
}
