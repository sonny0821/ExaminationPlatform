using System;
using TooFuns.Framework.SqlCondition;
namespace TooFuns.Framework.Access
{
	public class Column
	{
		private string name;
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		public Column(string name)
		{
			this.name = name;
		}
		public override string ToString()
		{
			return this.name;
		}
		public BaseCondition Between(object value1, object value2)
		{
			return new Between(this.name, value1, value2);
		}
		public BaseCondition Contains(string value)
		{
			return new Contains(this.name, value);
		}
		public BaseCondition EndWith(string value)
		{
			return new EndWith(this.name, value);
		}
		public BaseCondition Equal(object value)
		{
			return new Equal(this.name, value);
		}
		public BaseCondition In(params object[] values)
		{
			return new In(this.name, values);
		}
		public BaseCondition LessOrEqual(object value)
		{
			return new LessOrEqual(this.name, value);
		}
		public BaseCondition LessThan(object value)
		{
			return new LessThan(this.name, value);
		}
		public BaseCondition MoreOrEqual(object value)
		{
			return new MoreOrEqual(this.name, value);
		}
		public BaseCondition MoreThan(object value)
		{
			return new MoreThan(this.name, value);
		}
		public BaseCondition NotEqual(object value)
		{
			return new NotEqual(this.name, value);
		}
		public BaseCondition StartWith(string value)
		{
			return new StartWith(this.name, value);
		}
		public BaseCondition IsNULL()
		{
			return new Null(this.name);
		}
		public BaseCondition IsNotNULL()
		{
			return new NotNull(this.name);
		}
	}
}
