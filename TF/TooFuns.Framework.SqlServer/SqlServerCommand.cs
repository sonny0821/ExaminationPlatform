using System;
using System.Data.Common;
using System.Data.SqlClient;
using TooFuns.Framework.Data;
namespace TooFuns.Framework.SqlServer
{
	public class SqlServerCommand : Command
	{
		internal SqlServerCommand()
		{
		}
		internal SqlServerCommand(DbCommand command, Connection connection) : base(command, connection)
		{
		}
		protected override DbCommand GetCommand()
		{
			return new SqlCommand();
		}
		public override Parameter CreateParameter()
		{
			return new SqlServerParameter(this.command.CreateParameter());
		}
		public override Parameter CreateParameter(string parameterName, object value)
		{
			return new SqlServerParameter(new SqlParameter(parameterName, value));
		}
	}
}
