using System;
using System.Data.SqlClient;
using TooFuns.Framework.Data;
namespace TooFuns.Framework.SqlServer
{
	public class SqlServerConnection : Connection
	{
		public override char SpecialStart
		{
			get
			{
				return '[';
			}
		}
		public override char SpecialEnd
		{
			get
			{
				return ']';
			}
		}
		public override char ParameterFlag
		{
			get
			{
				return '@';
			}
		}
		public override string NotEqualOperator
		{
			get
			{
				return "!=";
			}
		}
		internal SqlServerConnection(string connectionString) : base(new SqlConnection(connectionString))
		{
		}
		public override Command CreateCommand()
		{
			return new SqlServerCommand
			{
				Connection = this
			};
		}
	}
}
