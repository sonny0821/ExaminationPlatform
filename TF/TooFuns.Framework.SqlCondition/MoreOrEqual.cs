using System;
using System.Text;
using TooFuns.Framework.Data;
namespace TooFuns.Framework.SqlCondition
{
	public class MoreOrEqual : BaseCondition
	{
		private object value;
		internal MoreOrEqual(string columnName, object value) : base(columnName)
		{
			this.value = value;
		}
		public override string ToString(Command command)
		{
			string text = "P" + command.Parameters.Count;
			Parameter parameter = command.CreateParameter();
			parameter.ParameterName = text;
			parameter.Value = this.value;
			command.Parameters.Add(parameter);
			return string.Format("{0}{1}{2} >= {3}{4}", new object[]
			{
				command.Connection.SpecialStart,
				this.columnName,
				command.Connection.SpecialEnd,
				command.Connection.ParameterFlag,
				text
			});
		}
		public override void ToString(Command command, StringBuilder builder)
		{
			string parameterName = "P" + command.Parameters.Count;
			Parameter parameter = command.CreateParameter();
			parameter.ParameterName = parameterName;
			parameter.Value = this.value;
			command.Parameters.Add(parameter);
			builder.Append(command.Connection.SpecialStart);
			builder.Append(this.columnName);
			builder.Append(command.Connection.SpecialEnd);
			builder.Append(" >= ");
			builder.Append(command.Connection.ParameterFlag);
			builder.Append(parameterName);
		}
	}
}
