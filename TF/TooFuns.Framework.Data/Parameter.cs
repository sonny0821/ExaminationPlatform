using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
namespace TooFuns.Framework.Data
{
	public abstract class Parameter
	{
		protected string parameterName;
		protected DbParameter parameter;
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), RefreshProperties(RefreshProperties.All)]
		public DbType DbType
		{
			get
			{
				return this.parameter.DbType;
			}
			set
			{
				this.parameter.DbType = value;
			}
		}
		[RefreshProperties(RefreshProperties.All)]
		public ParameterDirection Direction
		{
			get
			{
				return this.parameter.Direction;
			}
			set
			{
				this.parameter.Direction = value;
			}
		}
		[DefaultValue("")]
		public string ParameterName
		{
			get
			{
				return this.parameterName;
			}
			set
			{
				this.parameterName = value;
				this.SetParameterName(value);
			}
		}
		public int Size
		{
			get
			{
				return this.parameter.Size;
			}
			set
			{
				this.parameter.Size = value;
			}
		}
		[DefaultValue(""), RefreshProperties(RefreshProperties.All)]
		public object Value
		{
			get
			{
				return this.parameter.Value;
			}
			set
			{
				this.parameter.Value = value;
			}
		}
		internal DbParameter InnerParameter
		{
			get
			{
				return this.parameter;
			}
		}
		internal Parameter()
		{
			this.parameter = this.CreateParameter();
		}
		internal Parameter(string parameterName, object value)
		{
			this.parameterName = parameterName;
			this.parameter = this.CreateParameter(parameterName, value);
		}
		internal Parameter(DbParameter parameter)
		{
			this.parameter = parameter;
		}
		protected abstract DbParameter CreateParameter();
		protected abstract DbParameter CreateParameter(string parameterName, object value);
		protected abstract void SetParameterName(string parameterName);
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void ResetDbType()
		{
			this.parameter.ResetDbType();
		}
	}
}
