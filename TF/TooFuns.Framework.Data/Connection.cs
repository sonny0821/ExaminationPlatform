using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
namespace TooFuns.Framework.Data
{
	public abstract class Connection : IDisposable
	{
		protected DbConnection connection;
		private Transaction transaction;
		private Database database;
		internal DbConnection InnerConnection
		{
			get
			{
				return this.connection;
			}
			set
			{
				this.connection = value;
			}
		}
		[DefaultValue(""), RefreshProperties(RefreshProperties.All), SettingsBindable(true)]
		public string ConnectionString
		{
			get
			{
				return this.connection.ConnectionString;
			}
			set
			{
				this.connection.ConnectionString = value;
			}
		}
		public virtual int ConnectionTimeout
		{
			get
			{
				return this.connection.ConnectionTimeout;
			}
		}
		[Browsable(false)]
		public ConnectionState State
		{
			get
			{
				return this.connection.State;
			}
		}
		public Transaction Transaction
		{
			get
			{
				return this.transaction;
			}
		}
		public Database Database
		{
			get
			{
				return this.database;
			}
			internal set
			{
				this.database = value;
			}
		}
		public abstract char SpecialStart
		{
			get;
		}
		public abstract char SpecialEnd
		{
			get;
		}
		public abstract char ParameterFlag
		{
			get;
		}
		public abstract string NotEqualOperator
		{
			get;
		}
		internal Connection(DbConnection connection)
		{
			this.connection = connection;
		}
		public Transaction BeginTransaction()
		{
			this.transaction = new Transaction(this.connection.BeginTransaction(), this);
			return this.transaction;
		}
		public Transaction BeginTransaction(IsolationLevel isolationLevel)
		{
			return new Transaction(this.connection.BeginTransaction(isolationLevel), this);
		}
		public void Close()
		{
			this.connection.Close();
		}
		public abstract Command CreateCommand();
		public void Open()
		{
			this.connection.Open();
		}
		public Task OpenAsync()
		{
			return this.connection.OpenAsync();
		}
		public virtual Task OpenAsync(CancellationToken cancellationToken)
		{
			return this.connection.OpenAsync(cancellationToken);
		}
		public void Dispose()
		{
			this.connection.Dispose();
		}
	}
}
