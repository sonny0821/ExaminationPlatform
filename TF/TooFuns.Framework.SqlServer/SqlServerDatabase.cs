using System;
using TooFuns.Framework.Data;
namespace TooFuns.Framework.SqlServer
{
	public class SqlServerDatabase : Database
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
		protected override Connection GetConnection()
		{
			return new SqlServerConnection(this.connectionString);
		}
	}
}
