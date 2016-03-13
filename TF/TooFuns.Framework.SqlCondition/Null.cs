using System;
using System.Text;
using TooFuns.Framework.Data;
namespace TooFuns.Framework.SqlCondition
{
	public class Null : BaseCondition
	{
		internal Null(string columnName) : base(columnName)
		{
		}
		public override string ToString(Command command)
		{
			return string.Format("{0}{1}{2} IS NULL", command.Connection.SpecialStart, this.columnName, command.Connection.SpecialEnd);
		}
		public override void ToString(Command command, StringBuilder builder)
		{
			builder.Append(command.Connection.SpecialStart);
			builder.Append(this.columnName);
			builder.Append(command.Connection.SpecialEnd);
			builder.Append(" IS NULL ");
		}
	}
}
