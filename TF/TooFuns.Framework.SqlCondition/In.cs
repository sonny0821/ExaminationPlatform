using System;
using System.Text;
using TooFuns.Framework.Data;
namespace TooFuns.Framework.SqlCondition
{
	public class In : BaseCondition
	{
		private object[] values;
		internal In(string columnName, object[] values) : base(columnName)
		{
			this.values = values;
		}
		public override string ToString(Command command)
		{
			string[] array = new string[this.values.Length];
			for (int i = 0; i < this.values.Length; i++)
			{
				string text = "P" + (command.Parameters.Count + i);
				Parameter parameter = command.CreateParameter();
				parameter.ParameterName = text;
				parameter.Value = this.values[i];
				command.Parameters.Add(parameter);
				array[i] = text;
			}
			return string.Format("{0}{1}{2} IN ({3}{4})", new object[]
			{
				command.Connection.SpecialStart,
				this.columnName,
				command.Connection.SpecialEnd,
				command.Connection.ParameterFlag,
				string.Join("," + command.Connection.ParameterFlag, array)
			});
		}
		public override void ToString(Command command, StringBuilder builder)
		{
			string[] array = new string[this.values.Length];
			for (int i = 0; i < this.values.Length; i++)
			{
				string text = "P" + (command.Parameters.Count + i);
				Parameter parameter = command.CreateParameter();
				parameter.ParameterName = text;
				parameter.Value = this.values[i];
				command.Parameters.Add(parameter);
				array[i] = text;
			}
			builder.Append(command.Connection.SpecialStart);
			builder.Append(this.columnName);
			builder.Append(command.Connection.SpecialEnd);
			builder.Append(" IN (");
			builder.Append(command.Connection.ParameterFlag);
			builder.Append(string.Join("," + command.Connection.ParameterFlag, array));
			builder.Append(")");
		}
	}
}
