using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
namespace TooFuns.Framework.Data
{
	public abstract class Command
	{
		protected DbCommand command;
		private Connection connection;
		private Transaction transaction;
		protected ParameterCollection parameters;
		[DefaultValue(""), RefreshProperties(RefreshProperties.All)]
		public string CommandText
		{
			get
			{
				return this.command.CommandText;
			}
			set
			{
				this.command.CommandText = value;
			}
		}
		public int CommandTimeout
		{
			get
			{
				return this.command.CommandTimeout;
			}
			set
			{
				this.command.CommandTimeout = value;
			}
		}
		[RefreshProperties(RefreshProperties.All)]
		public CommandType CommandType
		{
			get
			{
				return this.command.CommandType;
			}
			set
			{
				this.command.CommandType = value;
			}
		}
		[Browsable(false), DefaultValue(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Connection Connection
		{
			get
			{
				return this.connection;
			}
			set
			{
				this.connection = value;
				this.command.Connection = this.connection.InnerConnection;
			}
		}
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ParameterCollection Parameters
		{
			get
			{
				return this.parameters;
			}
		}
		[Browsable(false), DefaultValue(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Transaction Transaction
		{
			get
			{
				return this.transaction;
			}
			set
			{
				this.transaction = value;
				this.command.Transaction = this.transaction.InnerTransaction;
			}
		}
		internal Command()
		{
			this.command = this.GetCommand();
			this.parameters = new ParameterCollection(this.command.Parameters);
		}
		protected abstract DbCommand GetCommand();
		internal Command(DbCommand command, Connection connection)
		{
			this.command = command;
			this.connection = connection;
			command.Connection = connection.InnerConnection;
			this.parameters = new ParameterCollection(command.Parameters);
		}
		public void Cancel()
		{
			this.command.Cancel();
		}
		public abstract Parameter CreateParameter();
		public abstract Parameter CreateParameter(string parameterName, object value);
		public int ExecuteNonQuery()
		{
			return this.command.ExecuteNonQuery();
		}
		public Task<int> ExecuteNonQueryAsync()
		{
			return this.command.ExecuteNonQueryAsync();
		}
		public virtual Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
		{
			return this.command.ExecuteNonQueryAsync(cancellationToken);
		}
		public DataReader ExecuteReader()
		{
			return new DataReader(this.command.ExecuteReader());
		}
		public DataReader ExecuteReader(CommandBehavior behavior)
		{
			return new DataReader(this.command.ExecuteReader(behavior));
		}
		public Task<DbDataReader> ExecuteReaderAsync()
		{
			return this.command.ExecuteReaderAsync();
		}
		public Task<DbDataReader> ExecuteReaderAsync(CancellationToken cancellationToken)
		{
			return this.command.ExecuteReaderAsync(cancellationToken);
		}
		public Task<DbDataReader> ExecuteReaderAsync(CommandBehavior behavior)
		{
			return this.command.ExecuteReaderAsync(behavior);
		}
		public async Task<DataReader> ExecuteReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
		{
			Task<DbDataReader> reader = null;
			await Task.Run(delegate
			{
				reader = this.command.ExecuteReaderAsync(behavior, cancellationToken);
			});
			return new DataReader(reader.Result);
		}
		public object ExecuteScalar()
		{
			return this.command.ExecuteScalar();
		}
		public Task<object> ExecuteScalarAsync()
		{
			return this.command.ExecuteScalarAsync();
		}
		public Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
		{
			return this.command.ExecuteScalarAsync(cancellationToken);
		}
		public void Prepare()
		{
			this.command.Prepare();
		}
	}
}
