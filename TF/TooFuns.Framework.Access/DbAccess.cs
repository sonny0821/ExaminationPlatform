using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TooFuns.Framework.Data;
using TooFuns.Framework.SqlCondition;
namespace TooFuns.Framework.Access
{
	public abstract class DbAccess : AccessBase, IDisposable
	{
		private Database db;
		private Connection connection;
		private Transaction transaction;
		public abstract Database getDatabase();
		public void BeginTransaction()
		{
			this.transaction = this.connection.BeginTransaction();
		}
		public void Rollback()
		{
			if (this.transaction != null)
			{
				this.transaction.Rollback();
			}
		}
		public void Commit()
		{
			if (this.transaction != null)
			{
				this.transaction.Commit();
			}
		}
		public void Open()
		{
			if (this.connection != null)
			{
				this.connection.Open();
			}
		}
		public void Close()
		{
			if (this.connection != null && (this.connection.State == ConnectionState.Open || this.connection.State == ConnectionState.Broken))
			{
				this.connection.Close();
			}
		}
		public DbAccess()
		{
			this.db = this.getDatabase();
		}
		public DbAccess(bool mutiCommand)
		{
			this.db = this.getDatabase();
			if (mutiCommand)
			{
				this.connection = this.db.CreateConnection();
			}
		}
		public int Insert(EntityBase entity)
		{
			int result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Insert(connection, entity);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			if (this.transaction == null)
			{
				result = base.Insert(this.connection, entity);
			}
			else
			{
				result = base.Insert(this.transaction, entity);
			}
			return result;
		}
		public int Insert(EntityBase entity, params string[] columns)
		{
			int result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Insert(connection, entity, columns);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			if (this.transaction == null)
			{
				result = base.Insert(this.connection, entity, columns);
			}
			else
			{
				result = base.Insert(this.transaction, entity, columns);
			}
			return result;
		}
		public int Update(EntityBase entity)
		{
			int result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Update(connection, entity);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			if (this.transaction == null)
			{
				result = base.Update(this.connection, entity);
			}
			else
			{
				result = base.Update(this.transaction, entity);
			}
			return result;
		}
		public int Update(EntityBase entity, params string[] columns)
		{
			int result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Update(connection, entity, columns);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			if (this.transaction == null)
			{
				result = base.Update(this.connection, entity, columns);
			}
			else
			{
				result = base.Update(this.transaction, entity, columns);
			}
			return result;
		}
		public int Delete(EntityBase entity)
		{
			int result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Delete(connection, entity);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					catch (Exception ex2)
					{
						throw ex2;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			if (this.transaction == null)
			{
				result = base.Delete(this.connection, entity);
			}
			else
			{
				result = base.Delete(this.transaction, entity);
			}
			return result;
		}
		public int Delete<T>(params BaseCondition[] conditions) where T : EntityBase
		{
			int result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Delete<T>(connection, conditions);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					catch (Exception ex2)
					{
						throw ex2;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			if (this.transaction == null)
			{
				result = base.Delete<T>(this.connection, conditions);
			}
			else
			{
				result = base.Delete<T>(this.transaction, conditions);
			}
			return result;
		}
		public List<E> Select<E>() where E : EntityBase
		{
			return this.Select<E>(OrderBy.None);
		}
		public List<E> Select<E>(OrderBy orderBy) where E : EntityBase
		{
			List<E> result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Select<E>(connection, orderBy, new BaseCondition[0]);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			try
			{
				result = base.Select<E>(this.connection, new BaseCondition[0]);
			}
			catch (DbException ex)
			{
				throw ex;
			}
			return result;
		}
		public List<E> Select<E>(params BaseCondition[] conditions) where E : EntityBase
		{
			return this.Select<E>(OrderBy.None, conditions);
		}
		public List<E> Select<E>(OrderBy orderBy, params BaseCondition[] conditions) where E : EntityBase
		{
			List<E> result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Select<E>(connection, orderBy, conditions);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			try
			{
				result = base.Select<E>(this.connection, orderBy, conditions);
			}
			catch (DbException ex)
			{
				throw ex;
			}
			return result;
		}
		public List<E> Select<E>(PageInfo pageInfo) where E : EntityBase
		{
			List<E> result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Select<E>(connection, pageInfo, null);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			try
			{
				result = base.Select<E>(this.connection, pageInfo, null);
			}
			catch (DbException ex)
			{
				throw ex;
			}
			return result;
		}
		public List<E> Select<E>(PageInfo pageInfo, params BaseCondition[] conditions) where E : EntityBase
		{
			List<E> result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Select<E>(connection, pageInfo, conditions);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			try
			{
				result = base.Select<E>(this.connection, pageInfo, conditions);
			}
			catch (DbException ex)
			{
				throw ex;
			}
			return result;
		}
		public List<E> Select<E>(string strSQL) where E : EntityBase
		{
			List<E> result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Select<E>(connection, strSQL);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			try
			{
				result = base.Select<E>(this.connection, strSQL);
			}
			catch (DbException ex)
			{
				throw ex;
			}
			return result;
		}
		public List<E> Select<E>(string strSQL, ParameterHandle addParameters) where E : EntityBase
		{
			List<E> result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Select<E>(connection, strSQL, addParameters);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			try
			{
				if (this.transaction != null)
				{
					result = base.Select<E>(this.transaction, strSQL, addParameters);
				}
				else
				{
					result = base.Select<E>(this.connection, strSQL, addParameters);
				}
			}
			catch (DbException ex)
			{
				throw ex;
			}
			return result;
		}
		public List<E> Select<E>(string strSQL, PageInfo pageInfo, ParameterHandle addParameters) where E : EntityBase
		{
			List<E> result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Select<E>(connection, strSQL, pageInfo, addParameters);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			try
			{
				if (this.transaction != null)
				{
					result = base.Select<E>(this.transaction, strSQL, pageInfo, addParameters);
				}
				else
				{
					result = base.Select<E>(this.connection, strSQL, pageInfo, addParameters);
				}
			}
			catch (DbException ex)
			{
				throw ex;
			}
			return result;
		}
		public List<E> Select<E>(string strSQL, PageInfo pageInfo) where E : EntityBase
		{
			List<E> result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.Select<E>(connection, strSQL, pageInfo, null);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			try
			{
				if (this.transaction != null)
				{
					result = base.Select<E>(this.transaction, strSQL, pageInfo, null);
				}
				else
				{
					result = base.Select<E>(this.connection, strSQL, pageInfo, null);
				}
			}
			catch (DbException ex)
			{
				throw ex;
			}
			return result;
		}
		public void Select(string strSQL, ParameterHandle addParameters, DataReaderHandle readDatas)
		{
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						DataReader dataReader = base.Select(connection, strSQL, addParameters);
						if (readDatas == null)
						{
							throw new ArgumentNullException("readDatas handel can't be null");
						}
						readDatas(dataReader);
						dataReader.Close();
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			else
			{
				try
				{
					DataReader dataReader;
					if (this.transaction != null)
					{
						dataReader = base.Select(this.transaction, strSQL, addParameters);
					}
					else
					{
						dataReader = base.Select(this.connection, strSQL, addParameters);
					}
					if (readDatas == null)
					{
						throw new ArgumentNullException("readDatas handel can't be null");
					}
					readDatas(dataReader);
					dataReader.Close();
				}
				catch (DbException ex)
				{
					throw ex;
				}
			}
		}
		public void Select(string strSQL, PageInfo pageInfo, ParameterHandle addParameters, DataReaderHandle readDatas)
		{
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						DataReader dataReader = base.Select(connection, strSQL, pageInfo, addParameters);
						if (readDatas == null)
						{
							throw new ArgumentNullException("readDatas handel can't be null");
						}
						readDatas(dataReader);
						dataReader.Close();
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
			}
			else
			{
				try
				{
					DataReader dataReader;
					if (this.transaction != null)
					{
						dataReader = base.Select(this.transaction, strSQL, pageInfo, addParameters);
					}
					else
					{
						dataReader = base.Select(this.connection, strSQL, pageInfo, addParameters);
					}
					if (readDatas == null)
					{
						throw new ArgumentNullException("readDatas handel can't be null");
					}
					readDatas(dataReader);
					dataReader.Close();
				}
				catch (DbException ex)
				{
					throw ex;
				}
			}
		}
		public int ExecuteNonQuery(string strSQL)
		{
			int result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.ExecuteNonQuery(connection, strSQL, null);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						if (connection.State == ConnectionState.Open)
						{
							connection.Close();
						}
					}
				}
			}
			if (this.transaction != null)
			{
				result = base.ExecuteNonQuery(this.transaction, strSQL, null);
			}
			else
			{
				result = base.ExecuteNonQuery(this.connection, strSQL, null);
			}
			return result;
		}
		public int ExecuteNonQuery(string strSQL, ParameterHandle addParameters)
		{
			Command command;
			int result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						command = connection.CreateCommand();
						command.CommandText = strSQL;
						if (addParameters != null)
						{
							addParameters(command);
						}
						result = command.ExecuteNonQuery();
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						if (connection.State == ConnectionState.Open)
						{
							connection.Close();
						}
					}
				}
			}
			command = this.connection.CreateCommand();
			command.CommandText = strSQL;
			if (this.transaction != null)
			{
				command.Transaction = this.transaction;
			}
			if (addParameters != null)
			{
				addParameters(command);
			}
			result = command.ExecuteNonQuery();
			return result;
		}
		public object ExecuteScalar(string strSQL)
		{
			object result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						result = base.ExecuteScalar(connection, strSQL, null);
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						if (connection.State == ConnectionState.Open)
						{
							connection.Close();
						}
					}
				}
			}
			if (this.transaction != null)
			{
				result = base.ExecuteScalar(this.transaction, strSQL, null);
			}
			else
			{
				result = base.ExecuteScalar(this.connection, strSQL, null);
			}
			return result;
		}
		public object ExecuteScalar(string strSQL, ParameterHandle addParameters)
		{
			Command command;
			object result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						command = connection.CreateCommand();
						command.CommandText = strSQL;
						if (addParameters != null)
						{
							addParameters(command);
						}
						result = command.ExecuteScalar();
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						if (connection.State == ConnectionState.Open)
						{
							connection.Close();
						}
					}
				}
			}
			command = this.connection.CreateCommand();
			command.CommandText = strSQL;
			if (this.transaction != null)
			{
				command.Transaction = this.transaction;
			}
			if (addParameters != null)
			{
				addParameters(command);
			}
			result = command.ExecuteScalar();
			return result;
		}
		public void ExecuteReader(string strSQL, DataReaderHandle readDatas)
		{
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						base.ExecuteReader(connection, strSQL, null, readDatas);
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						if (connection.State == ConnectionState.Open)
						{
							connection.Close();
						}
					}
				}
			}
			else
			{
				if (this.transaction != null)
				{
					base.ExecuteReader(this.transaction, strSQL, null, readDatas);
				}
				else
				{
					base.ExecuteReader(this.connection, strSQL, null, readDatas);
				}
			}
		}
		public void ExecuteReader(string strSQL, ParameterHandle addParameters, DataReaderHandle readDatas)
		{
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						connection.Open();
						Command command = connection.CreateCommand();
						command.CommandText = strSQL;
						if (addParameters != null)
						{
							addParameters(command);
						}
						DataReader dataReader = command.ExecuteReader();
						readDatas(dataReader);
						dataReader.Close();
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						if (connection.State == ConnectionState.Open)
						{
							connection.Close();
						}
					}
				}
			}
			else
			{
				Command command = this.connection.CreateCommand();
				command.CommandText = strSQL;
				if (this.transaction != null)
				{
					command.Transaction = this.transaction;
				}
				if (addParameters != null)
				{
					addParameters(command);
				}
				DataReader dataReader = command.ExecuteReader();
				readDatas(dataReader);
				dataReader.Close();
			}
		}
		public Command CreateCommand(string strSQL)
		{
			return base.CreateCommand(this.connection, strSQL, null);
		}
		public Command CreateCommand(string strSQL, ParameterHandle addParameters)
		{
			Command command;
			Command result;
			if (this.connection == null)
			{
				using (Connection connection = this.db.CreateConnection())
				{
					try
					{
						command = connection.CreateCommand();
						command.CommandText = strSQL;
						if (addParameters != null)
						{
							addParameters(command);
						}
						result = command;
						return result;
					}
					catch (DbException ex)
					{
						throw ex;
					}
					finally
					{
						if (connection.State == ConnectionState.Open)
						{
							connection.Close();
						}
					}
				}
			}
			command = this.connection.CreateCommand();
			command.CommandText = strSQL;
			if (this.transaction != null)
			{
				command.Transaction = this.transaction;
			}
			if (addParameters != null)
			{
				addParameters(command);
			}
			result = command;
			return result;
		}
		public void Dispose()
		{
			if (this.connection != null)
			{
				this.connection.Dispose();
			}
		}
		public T CreateAccess<T>() where T : DbAccess
		{
			T t = Activator.CreateInstance<T>();
			if (this.connection != null)
			{
				t.connection = this.connection;
				t.transaction = this.transaction;
			}
			return t;
		}
	}
}
