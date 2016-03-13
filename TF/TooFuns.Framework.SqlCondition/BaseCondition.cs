using System;
using System.Text;
using TooFuns.Framework.Data;
namespace TooFuns.Framework.SqlCondition
{
	public abstract class BaseCondition
	{
		protected string columnName;
		public string ColumnName
		{
			get
			{
				return this.columnName;
			}
		}
		public BaseCondition(string columnName)
		{
			this.columnName = columnName;
		}
		public abstract string ToString(Command command);
		public abstract void ToString(Command command, StringBuilder builder);
	}
}
