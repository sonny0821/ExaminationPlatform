using System;
using System.Data.Common;
using System.Data.SqlClient;
using TooFuns.Framework.Data;
namespace TooFuns.Framework.SqlServer
{
	public class SqlServerParameter : Parameter
	{
		internal SqlServerParameter()
		{
		}
		internal SqlServerParameter(string parameterName, object value) : base(parameterName, value)
		{
		}
		internal SqlServerParameter(DbParameter parameter) : base(parameter)
		{
		}
		protected override DbParameter CreateParameter()
		{
			return new SqlParameter();
		}
		protected override DbParameter CreateParameter(string parameterName, object value)
		{
			return new SqlParameter("@" + parameterName, value);
		}
		protected override void SetParameterName(string parameterName)
		{
			this.parameter.ParameterName = "@" + parameterName;
		}
	}
}
