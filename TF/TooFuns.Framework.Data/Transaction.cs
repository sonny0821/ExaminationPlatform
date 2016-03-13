using System;
using System.Data.Common;
namespace TooFuns.Framework.Data
{
	public class Transaction
	{
		private Connection connection;
		private DbTransaction transaction;
		public DbTransaction InnerTransaction
		{
			get
			{
				return this.transaction;
			}
		}
		public Connection Connection
		{
			get
			{
				return this.connection;
			}
		}
		internal Transaction(DbTransaction transaction, Connection connection)
		{
			this.transaction = transaction;
			this.connection = connection;
		}
		public void Commit()
		{
			this.transaction.Commit();
		}
		public void Dispose()
		{
			this.transaction.Dispose();
		}
		public void Rollback()
		{
			this.transaction.Rollback();
		}
	}
}
