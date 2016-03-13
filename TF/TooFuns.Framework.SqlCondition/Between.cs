using System;
using System.Text;
using TooFuns.Framework.Data;
namespace TooFuns.Framework.SqlCondition
{
	public class Between : BaseCondition
	{
		private object value1;
		private object value2;
		internal Between(string name, object value1, object value2) : base(name)
		{
			this.value1 = value1;
			this.value2 = value2;
		}
		public override string ToString(Command command)
		{
			string text = "P" + command.Parameters.Count;
			string text2 = "P" + (command.Parameters.Count + 1);
			Parameter parameter = command.CreateParameter();
			parameter.ParameterName = text;
			parameter.Value = this.value1;
			Parameter parameter2 = command.CreateParameter();
			parameter2.ParameterName = text2;
			parameter2.Value = this.value2;
			command.Parameters.Add(parameter);
			command.Parameters.Add(parameter2);
			return string.Format("{0}{1}{2} BETWEEN {3}{4} AND {3}{5}", new object[]
			{
				command.Connection.SpecialStart,
				this.columnName,
				command.Connection.SpecialEnd,
				command.Connection.ParameterFlag,
				text,
				text2
			});
		}
		public override void ToString(Command command, StringBuilder builder)
		{
			string text = "P" + command.Parameters.Count;
			string text2 = "P" + (command.Parameters.Count + 1);
			Parameter parameter = command.CreateParameter();
			parameter.ParameterName = text;
			parameter.Value = this.value1;
			Parameter parameter2 = command.CreateParameter();
			parameter2.ParameterName = text2;
			parameter2.Value = this.value2;
			command.Parameters.Add(parameter);
			command.Parameters.Add(parameter2);
			builder.Append(command.Connection.SpecialStart);
			builder.Append(this.columnName);
			builder.Append(command.Connection.SpecialEnd);
			builder.Append(" BETWEEN ");
			builder.Append(command.Connection.ParameterFlag);
			builder.Append(text);
			builder.Append(" AND ");
			builder.Append(command.Connection.ParameterFlag);
			builder.Append(text2);
		}
	}
}
