using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TooFuns.Framework.Data;
using TooFuns.Framework.SqlCondition;
namespace TooFuns.Framework.Access
{
	public abstract class AccessBase
	{
		protected Column Column(string columnName)
		{
			return new Column(columnName);
		}
		public Command CreateInsertCommand(Connection connection, EntityBase entity)
		{
			TableAttribute tableAttribute = (TableAttribute)entity.GetType().GetCustomAttributes(typeof(TableAttribute));
			ColumnAttribute columnAttribute = (ColumnAttribute)entity.GetType().GetCustomAttribute(typeof(ColumnAttribute));
			return this.CreateInsertCommand(connection, tableAttribute.Name, entity, columnAttribute.Values);
		}
		public Command CreateInsertCommand(Connection connection, EntityBase entity, params string[] columns)
		{
			TableAttribute tableAttribute = (TableAttribute)entity.GetType().GetCustomAttribute(typeof(TableAttribute));
			return this.CreateInsertCommand(connection, tableAttribute.Name, entity, columns);
		}
		private Command CreateInsertCommand(Connection connection, string tableName, EntityBase entity, string[] columns)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("INSERT INTO ");
			stringBuilder.Append(connection.SpecialStart);
			stringBuilder.Append(tableName);
			stringBuilder.Append(connection.SpecialEnd);
			stringBuilder.Append("(");
			stringBuilder.Append(connection.SpecialStart);
			stringBuilder.Append(columns[0]);
			stringBuilder.Append(connection.SpecialEnd);
			for (int i = 1; i < columns.Length; i++)
			{
				stringBuilder.Append(',');
				stringBuilder.Append(connection.SpecialStart);
				stringBuilder.Append(columns[i]);
				stringBuilder.Append(connection.SpecialEnd);
			}
			stringBuilder.Append(") VALUES (");
			stringBuilder.Append(connection.ParameterFlag);
			stringBuilder.Append(columns[0]);
			for (int i = 1; i < columns.Length; i++)
			{
				stringBuilder.Append(',');
				stringBuilder.Append(connection.ParameterFlag);
				stringBuilder.Append(columns[i]);
			}
			stringBuilder.Append(")");
			Command command = connection.CreateCommand();
			command.CommandText = stringBuilder.ToString();
			Type type = entity.GetType();
			for (int i = 0; i < columns.Length; i++)
			{
				PropertyInfo property = type.GetProperty(columns[i]);
				if (property == null)
				{
					throw new ArgumentException("Column name not exist.");
				}
				object value = property.GetValue(entity);
				Parameter parameter = command.CreateParameter();
				parameter.ParameterName = property.Name;
				if (value == null)
				{
					parameter.Value = DBNull.Value;
				}
				else
				{
					parameter.Value = value;
				}
				command.Parameters.Add(parameter);
			}
			return command;
		}
		public int Insert(Connection connection, EntityBase entity)
		{
			Command command = this.CreateInsertCommand(connection, entity);
			return command.ExecuteNonQuery();
		}
		public int Insert(Connection connection, EntityBase entity, params string[] columns)
		{
			Command command = this.CreateInsertCommand(connection, entity.GetType().Name, entity, columns);
			return command.ExecuteNonQuery();
		}
		public int Insert(Transaction transaction, EntityBase entity)
		{
			Command command = this.CreateInsertCommand(transaction.Connection, entity);
			command.Transaction = transaction;
			return command.ExecuteNonQuery();
		}
		public int Insert(Transaction transaction, EntityBase entity, params string[] columns)
		{
			Command command = this.CreateInsertCommand(transaction.Connection, entity.GetType().Name, entity, columns);
			command.Transaction = transaction;
			return command.ExecuteNonQuery();
		}
		public Command CreateUpdateCommand(Connection connection, EntityBase entity)
		{
			TableAttribute tableAttribute = (TableAttribute)entity.GetType().GetCustomAttribute(typeof(TableAttribute));
			OrdinaryColumnAttribute ordinaryColumnAttribute = (OrdinaryColumnAttribute)entity.GetType().GetCustomAttribute(typeof(OrdinaryColumnAttribute));
			return this.CreateUpdateCommand(connection, tableAttribute.Name, entity, ordinaryColumnAttribute.Values);
		}
		public Command CreateUpdateCommand(Connection connection, EntityBase entity, params string[] columns)
		{
			return this.CreateUpdateCommand(connection, entity.GetType().Name, entity, columns);
		}
		private Command CreateUpdateCommand(Connection connection, string tableName, EntityBase entity, string[] columns)
		{
			Type type = entity.GetType();
			PrimaryKeyAttribute primaryKeyAttribute = (PrimaryKeyAttribute)type.GetCustomAttribute(typeof(PrimaryKeyAttribute));
			string[] values = primaryKeyAttribute.Values;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE ");
			stringBuilder.Append(connection.SpecialStart);
			stringBuilder.Append(tableName);
			stringBuilder.Append(connection.SpecialEnd);
			stringBuilder.Append("SET ");
			stringBuilder.Append(connection.SpecialStart);
			stringBuilder.Append(columns[0]);
			stringBuilder.Append(connection.SpecialEnd);
			stringBuilder.Append("=@");
			stringBuilder.Append(columns[0]);
			for (int i = 1; i < columns.Length; i++)
			{
				stringBuilder.Append(',');
				stringBuilder.Append(columns[i]);
				stringBuilder.Append("=@");
				stringBuilder.Append(columns[i]);
			}
			stringBuilder.Append(" WHERE ");
			stringBuilder.Append(connection.SpecialStart);
			stringBuilder.Append(values[0]);
			stringBuilder.Append(connection.SpecialEnd);
			stringBuilder.Append("=@");
			stringBuilder.Append(values[0]);
			for (int i = 1; i < values.Length; i++)
			{
				stringBuilder.Append(" AND ");
				stringBuilder.Append(connection.SpecialStart);
				stringBuilder.Append(values[i]);
				stringBuilder.Append(connection.SpecialEnd);
				stringBuilder.Append("=@");
				stringBuilder.Append(values[i]);
			}
			Command command = connection.CreateCommand();
			command.CommandText = stringBuilder.ToString();
			for (int i = 0; i < columns.Length; i++)
			{
				PropertyInfo property = type.GetProperty(columns[i]);
				if (property == null)
				{
					throw new ArgumentException("Column name not exist.");
				}
				object value = property.GetValue(entity);
				Parameter parameter = command.CreateParameter();
				parameter.ParameterName = property.Name;
				if (value == null)
				{
					parameter.Value = DBNull.Value;
				}
				else
				{
					parameter.Value = value;
				}
				command.Parameters.Add(parameter);
			}
			for (int i = 0; i < values.Length; i++)
			{
				PropertyInfo property = type.GetProperty(values[i]);
				if (property == null)
				{
					throw new ArgumentException("Column name not exist.");
				}
				object value = property.GetValue(entity);
				Parameter parameter = command.CreateParameter();
				parameter.ParameterName = property.Name;
				if (value == null)
				{
					parameter.Value = DBNull.Value;
				}
				else
				{
					parameter.Value = value;
				}
				command.Parameters.Add(parameter);
			}
			return command;
		}
		public int Update(Connection connection, EntityBase entity)
		{
			TableAttribute tableAttribute = (TableAttribute)entity.GetType().GetCustomAttribute(typeof(TableAttribute));
			OrdinaryColumnAttribute ordinaryColumnAttribute = (OrdinaryColumnAttribute)entity.GetType().GetCustomAttribute(typeof(OrdinaryColumnAttribute));
			Command command = this.CreateUpdateCommand(connection, tableAttribute.Name, entity, ordinaryColumnAttribute.Values);
			return command.ExecuteNonQuery();
		}
		public int Update(Connection connection, EntityBase entity, params string[] columns)
		{
			Command command = this.CreateUpdateCommand(connection, entity.GetType().Name, entity, columns);
			return command.ExecuteNonQuery();
		}
		public int Update(Transaction transaction, EntityBase entity)
		{
			TableAttribute tableAttribute = (TableAttribute)entity.GetType().GetCustomAttribute(typeof(TableAttribute));
			OrdinaryColumnAttribute ordinaryColumnAttribute = (OrdinaryColumnAttribute)entity.GetType().GetCustomAttribute(typeof(OrdinaryColumnAttribute));
			Command command = this.CreateUpdateCommand(transaction.Connection, tableAttribute.Name, entity, ordinaryColumnAttribute.Values);
			command.Transaction = transaction;
			return command.ExecuteNonQuery();
		}
		public int Update(Transaction transaction, EntityBase entity, params string[] columns)
		{
			Command command = this.CreateUpdateCommand(transaction.Connection, entity.GetType().Name, entity, columns);
			command.Transaction = transaction;
			return command.ExecuteNonQuery();
		}
		public Command CreateDeleteCommand(Connection connection, EntityBase entity)
		{
			TableAttribute tableAttribute = (TableAttribute)entity.GetType().GetCustomAttribute(typeof(TableAttribute));
			return this.CreateDeleteCommand(connection, tableAttribute.Name, entity);
		}
		private Command CreateDeleteCommand(Connection connection, string tableName, EntityBase entity)
		{
			Type type = entity.GetType();
			PrimaryKeyAttribute primaryKeyAttribute = (PrimaryKeyAttribute)type.GetCustomAttribute(typeof(PrimaryKeyAttribute));
			string[] values = primaryKeyAttribute.Values;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DELETE FROM ");
			stringBuilder.Append(connection.SpecialStart);
			stringBuilder.Append(tableName);
			stringBuilder.Append(connection.SpecialEnd);
			stringBuilder.Append(" WHERE ");
			stringBuilder.Append(connection.SpecialStart);
			stringBuilder.Append(values[0]);
			stringBuilder.Append(connection.SpecialEnd);
			stringBuilder.Append("=");
			stringBuilder.Append(connection.ParameterFlag);
			stringBuilder.Append(values[0]);
			for (int i = 1; i < values.Length; i++)
			{
				stringBuilder.Append(" AND ");
				stringBuilder.Append(connection.SpecialStart);
				stringBuilder.Append(values[i]);
				stringBuilder.Append(connection.SpecialEnd);
				stringBuilder.Append("=");
				stringBuilder.Append(connection.ParameterFlag);
				stringBuilder.Append(values[i]);
			}
			Command command = connection.CreateCommand();
			command.CommandText = stringBuilder.ToString();
			for (int i = 0; i < values.Length; i++)
			{
				PropertyInfo property = type.GetProperty(values[i]);
				if (property == null)
				{
					throw new ArgumentException("Column name not exist.");
				}
				object value = property.GetValue(entity);
				Parameter parameter = command.CreateParameter();
				parameter.ParameterName = property.Name;
				if (value == null)
				{
					parameter.Value = DBNull.Value;
				}
				else
				{
					parameter.Value = value;
				}
				command.Parameters.Add(parameter);
			}
			return command;
		}
		private Command CreateDeleteCommand<T>(Connection connection, params BaseCondition[] conditions)
		{
			Type typeFromHandle = typeof(T);
			Command command = connection.CreateCommand();
			TableAttribute tableAttribute = (TableAttribute)typeFromHandle.GetCustomAttribute(typeof(TableAttribute));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DELETE FROM ");
			stringBuilder.Append(connection.SpecialStart);
			stringBuilder.Append(tableAttribute.Name);
			stringBuilder.Append(connection.SpecialEnd);
			stringBuilder.Append(" WHERE ");
			conditions[0].ToString(command, stringBuilder);
			for (int i = 1; i < conditions.Length; i++)
			{
				stringBuilder.Append(" AND ");
				conditions[i].ToString(command, stringBuilder);
			}
			command.CommandText = stringBuilder.ToString();
			return command;
		}
		public int Delete(Connection connection, EntityBase entity)
		{
			TableAttribute tableAttribute = (TableAttribute)entity.GetType().GetCustomAttribute(typeof(TableAttribute));
			Command command = this.CreateDeleteCommand(connection, tableAttribute.Name, entity);
			return command.ExecuteNonQuery();
		}
		public int Delete<T>(Connection connection, params BaseCondition[] conditions) where T : EntityBase
		{
			TableAttribute tableAttribute = (TableAttribute)typeof(T).GetCustomAttribute(typeof(TableAttribute));
			Command command = this.CreateDeleteCommand<T>(connection, conditions);
			return command.ExecuteNonQuery();
		}
		public int Delete(Transaction transaction, EntityBase entity)
		{
			TableAttribute tableAttribute = (TableAttribute)entity.GetType().GetCustomAttribute(typeof(TableAttribute));
			Command command = this.CreateDeleteCommand(transaction.Connection, tableAttribute.Name, entity);
			command.Transaction = transaction;
			return command.ExecuteNonQuery();
		}
		public int Delete<T>(Transaction transaction, params BaseCondition[] conditions) where T : EntityBase
		{
			TableAttribute tableAttribute = (TableAttribute)typeof(T).GetCustomAttribute(typeof(TableAttribute));
			Command command = this.CreateDeleteCommand<T>(transaction.Connection, conditions);
			command.Transaction = transaction;
			return command.ExecuteNonQuery();
		}
		public List<E> Select<E>(Connection connection) where E : EntityBase
		{
			return this.Select<E>(connection, OrderBy.None, new BaseCondition[0]);
		}
		public List<E> Select<E>(Connection connection, OrderBy orderBy) where E : EntityBase
		{
			return this.Select<E>(connection, orderBy, new BaseCondition[0]);
		}
		public List<E> Select<E>(Connection connection, params BaseCondition[] conditions) where E : EntityBase
		{
			return this.Select<E>(connection, OrderBy.None, conditions);
		}
		public List<E> Select<E>(Connection connection, OrderBy orderBy, params BaseCondition[] conditions) where E : EntityBase
		{
			Type typeFromHandle = typeof(E);
			Command command = connection.CreateCommand();
			ColumnAttribute columnAttribute = (ColumnAttribute)typeFromHandle.GetCustomAttribute(typeof(ColumnAttribute));
			string[] values = columnAttribute.Values;
			TableAttribute tableAttribute = (TableAttribute)typeFromHandle.GetCustomAttribute(typeof(TableAttribute));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ");
			stringBuilder.Append(connection.SpecialStart);
			stringBuilder.Append(values[0]);
			stringBuilder.Append(connection.SpecialEnd);
			for (int i = 1; i < values.Length; i++)
			{
				stringBuilder.Append(',');
				stringBuilder.Append(connection.SpecialStart);
				stringBuilder.Append(values[i]);
				stringBuilder.Append(connection.SpecialEnd);
			}
			stringBuilder.Append(" FROM ");
			stringBuilder.Append(connection.SpecialStart);
			stringBuilder.Append(tableAttribute.Name);
			stringBuilder.Append(connection.SpecialEnd);
			if (conditions != null && conditions.Length > 0)
			{
				stringBuilder.Append(" WHERE ");
				conditions[0].ToString(command, stringBuilder);
				for (int i = 1; i < conditions.Length; i++)
				{
					stringBuilder.Append(" AND ");
					conditions[i].ToString(command, stringBuilder);
				}
			}
			if (!orderBy.IsNull)
			{
				stringBuilder.Append(" ORDER BY ");
				stringBuilder.Append(orderBy.ToString());
			}
			command.CommandText = stringBuilder.ToString();
			return this.ReaderToEntity<E>(command.ExecuteReader());
		}
		public List<E> Select<E>(Connection connection, PageInfo pageInfo) where E : EntityBase
		{
			return this.Select<E>(connection, pageInfo, null);
		}
		public List<E> Select<E>(Connection connection, PageInfo pageInfo, params BaseCondition[] conditions) where E : EntityBase
		{
			Type typeFromHandle = typeof(E);
			Command command = connection.CreateCommand();
			ColumnAttribute columnAttribute = (ColumnAttribute)typeFromHandle.GetCustomAttribute(typeof(ColumnAttribute));
			string[] values = columnAttribute.Values;
			TableAttribute tableAttribute = (TableAttribute)typeFromHandle.GetCustomAttribute(typeof(TableAttribute));
			Command command2 = connection.CreateCommand();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT COUNT(*) Cnt FROM ");
			stringBuilder.Append(connection.SpecialStart);
			stringBuilder.Append(tableAttribute.Name);
			stringBuilder.Append(connection.SpecialEnd);
			if (conditions != null && conditions.Length > 0)
			{
				stringBuilder.Append(" WHERE ");
				conditions[0].ToString(command2, stringBuilder);
				for (int i = 1; i < conditions.Length; i++)
				{
					stringBuilder.Append(" AND ");
					conditions[i].ToString(command2, stringBuilder);
				}
			}
			command2.CommandText = stringBuilder.ToString();
			pageInfo.Total = (int)command2.ExecuteScalar();
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY ");
			if (!string.IsNullOrEmpty(pageInfo.TableName))
			{
				stringBuilder2.Append(connection.SpecialStart);
				stringBuilder2.Append(pageInfo.TableName);
				stringBuilder2.Append(connection.SpecialEnd);
				stringBuilder2.Append('.');
			}
			stringBuilder2.Append(connection.SpecialStart);
			stringBuilder2.Append(pageInfo.OrderBy);
			stringBuilder2.Append(connection.SpecialEnd);
			if (!pageInfo.Asc)
			{
				stringBuilder2.Append(" DESC");
			}
			stringBuilder2.Append(") RowNumber, ");
			stringBuilder2.Append(connection.SpecialStart);
			stringBuilder2.Append(values[0]);
			stringBuilder2.Append(connection.SpecialEnd);
			for (int i = 1; i < values.Length; i++)
			{
				stringBuilder2.Append(',');
				stringBuilder2.Append(connection.SpecialStart);
				stringBuilder2.Append(values[i]);
				stringBuilder2.Append(connection.SpecialEnd);
			}
			stringBuilder2.Append(" FROM ");
			stringBuilder2.Append(connection.SpecialStart);
			stringBuilder2.Append(tableAttribute.Name);
			stringBuilder2.Append(connection.SpecialEnd);
			if (conditions != null && conditions.Length > 0)
			{
				stringBuilder2.Append(" WHERE ");
				conditions[0].ToString(command, stringBuilder2);
				for (int i = 1; i < conditions.Length; i++)
				{
					stringBuilder2.Append(" AND ");
					conditions[i].ToString(command, stringBuilder2);
				}
			}
			stringBuilder2.Append(") T WHERE RowNumber BETWEEN ");
			stringBuilder2.Append(pageInfo.From);
			stringBuilder2.Append(" AND ");
			stringBuilder2.Append(pageInfo.To);
			command.CommandText = stringBuilder2.ToString();
			return this.ReaderToEntity<E>(command.ExecuteReader());
		}
		public List<E> Select<E>(Connection connection, string strSQL) where E : EntityBase
		{
			Command command = connection.CreateCommand();
			command.CommandText = strSQL;
			return this.ReaderToEntity<E>(command.ExecuteReader());
		}
		public List<E> Select<E>(Connection connection, string strSQL, PageInfo pageInfo, ParameterHandle addParameters) where E : EntityBase
		{
			string commandText = string.Format("SELECT COUNT(*) Cnt FROM ({0}) T", strSQL);
			Command command = connection.CreateCommand();
			command.CommandText = commandText;
			if (addParameters != null)
			{
				addParameters(command);
			}
			pageInfo.Total = (int)command.ExecuteScalar();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY ");
			if (!string.IsNullOrEmpty(pageInfo.TableName))
			{
				stringBuilder.Append(connection.SpecialStart);
				stringBuilder.Append(pageInfo.TableName);
				stringBuilder.Append(connection.SpecialEnd);
				stringBuilder.Append('.');
			}
			stringBuilder.Append(connection.SpecialStart);
			stringBuilder.Append(pageInfo.OrderBy);
			stringBuilder.Append(connection.SpecialEnd);
			if (!pageInfo.Asc)
			{
				stringBuilder.Append(" DESC");
			}
			stringBuilder.Append(") RowNumber,");
			stringBuilder.Append(strSQL.Trim().Substring(6));
			stringBuilder.Append(") T WHERE RowNumber BETWEEN ");
			stringBuilder.Append(pageInfo.From);
			stringBuilder.Append(" AND ");
			stringBuilder.Append(pageInfo.To);
			Command command2 = connection.CreateCommand();
			command2.CommandText = stringBuilder.ToString();
			if (addParameters != null)
			{
				addParameters(command2);
			}
			return this.ReaderToEntity<E>(command2.ExecuteReader());
		}
		public List<E> Select<E>(Connection connection, string strSQL, ParameterHandle addParameters) where E : EntityBase
		{
			Command command = connection.CreateCommand();
			command.CommandText = strSQL;
			if (addParameters != null)
			{
				addParameters(command);
			}
			return this.ReaderToEntity<E>(command.ExecuteReader());
		}
		public DataReader Select(Connection connection, string strSQL, ParameterHandle addParameters)
		{
			Command command = connection.CreateCommand();
			command.CommandText = strSQL;
			if (addParameters != null)
			{
				addParameters(command);
			}
			return command.ExecuteReader();
		}
		public DataReader Select(Connection connection, string strSQL, PageInfo pageInfo, ParameterHandle addParameters)
		{
			string commandText = string.Format("SELECT COUNT(*) Cnt FROM ({0}) T", strSQL);
			Command command = connection.CreateCommand();
			command.CommandText = commandText;
			if (addParameters != null)
			{
				addParameters(command);
			}
			pageInfo.Total = (int)command.ExecuteScalar();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY ");
			if (!string.IsNullOrEmpty(pageInfo.TableName))
			{
				stringBuilder.Append(connection.SpecialStart);
				stringBuilder.Append(pageInfo.TableName);
				stringBuilder.Append(connection.SpecialEnd);
				stringBuilder.Append('.');
			}
			stringBuilder.Append(connection.SpecialStart);
			stringBuilder.Append(pageInfo.OrderBy);
			stringBuilder.Append(connection.SpecialEnd);
			if (!pageInfo.Asc)
			{
				stringBuilder.Append(" DESC");
			}
			stringBuilder.Append(") RowNumber,");
			stringBuilder.Append(strSQL.Trim().Substring(6));
			stringBuilder.Append(") T WHERE RowNumber BETWEEN ");
			stringBuilder.Append(pageInfo.From);
			stringBuilder.Append(" AND ");
			stringBuilder.Append(pageInfo.To);
			Command command2 = connection.CreateCommand();
			command2.CommandText = stringBuilder.ToString();
			if (addParameters != null)
			{
				addParameters(command2);
			}
			return command2.ExecuteReader();
		}
		public List<E> Select<E>(Transaction transaction) where E : EntityBase
		{
			return this.Select<E>(transaction, OrderBy.None, new BaseCondition[0]);
		}
		public List<E> Select<E>(Transaction transaction, OrderBy orderBy) where E : EntityBase
		{
			return this.Select<E>(transaction, orderBy, new BaseCondition[0]);
		}
		public List<E> Select<E>(Transaction transaction, params BaseCondition[] conditions) where E : EntityBase
		{
			return this.Select<E>(transaction, OrderBy.None, conditions);
		}
		public List<E> Select<E>(Transaction transaction, OrderBy orderBy, params BaseCondition[] conditions) where E : EntityBase
		{
			Type typeFromHandle = typeof(E);
			Command command = transaction.Connection.CreateCommand();
			command.Transaction = transaction;
			ColumnAttribute columnAttribute = (ColumnAttribute)typeFromHandle.GetCustomAttribute(typeof(ColumnAttribute));
			string[] values = columnAttribute.Values;
			TableAttribute tableAttribute = (TableAttribute)typeFromHandle.GetCustomAttribute(typeof(TableAttribute));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ");
			stringBuilder.Append(transaction.Connection.SpecialStart);
			stringBuilder.Append(values[0]);
			stringBuilder.Append(transaction.Connection.SpecialEnd);
			for (int i = 1; i < values.Length; i++)
			{
				stringBuilder.Append(',');
				stringBuilder.Append(transaction.Connection.SpecialStart);
				stringBuilder.Append(values[i]);
				stringBuilder.Append(transaction.Connection.SpecialEnd);
			}
			stringBuilder.Append(" FROM ");
			stringBuilder.Append(transaction.Connection.SpecialStart);
			stringBuilder.Append(tableAttribute.Name);
			stringBuilder.Append(transaction.Connection.SpecialEnd);
			if (conditions != null && conditions.Length > 0)
			{
				stringBuilder.Append(" WHERE ");
				conditions[0].ToString(command, stringBuilder);
				for (int i = 1; i < conditions.Length; i++)
				{
					stringBuilder.Append(" AND ");
					conditions[i].ToString(command, stringBuilder);
				}
			}
			if (!orderBy.IsNull)
			{
				stringBuilder.Append(" ORDER BY ");
				stringBuilder.Append(orderBy.ToString());
			}
			command.CommandText = stringBuilder.ToString();
			return this.ReaderToEntity<E>(command.ExecuteReader());
		}
		public List<E> Select<E>(Transaction transaction, PageInfo pageInfo) where E : EntityBase
		{
			return this.Select<E>(transaction, pageInfo, null);
		}
		public List<E> Select<E>(Transaction transaction, PageInfo pageInfo, params BaseCondition[] conditions) where E : EntityBase
		{
			Type typeFromHandle = typeof(E);
			ColumnAttribute columnAttribute = (ColumnAttribute)typeFromHandle.GetCustomAttribute(typeof(ColumnAttribute));
			string[] values = columnAttribute.Values;
			TableAttribute tableAttribute = (TableAttribute)typeFromHandle.GetCustomAttribute(typeof(TableAttribute));
			Command command = transaction.Connection.CreateCommand();
			command.Transaction = transaction;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT COUNT(*) Cnt FROM ");
			stringBuilder.Append(transaction.Connection.SpecialStart);
			stringBuilder.Append(tableAttribute.Name);
			stringBuilder.Append(transaction.Connection.SpecialEnd);
			if (conditions != null && conditions.Length > 0)
			{
				stringBuilder.Append(" WHERE ");
				conditions[0].ToString(command, stringBuilder);
				for (int i = 1; i < conditions.Length; i++)
				{
					stringBuilder.Append(" AND ");
					conditions[i].ToString(command, stringBuilder);
				}
			}
			command.CommandText = stringBuilder.ToString();
			pageInfo.Total = (int)command.ExecuteScalar();
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY ID) RowNumber,");
			stringBuilder2.Append(transaction.Connection.SpecialStart);
			stringBuilder2.Append(values[0]);
			stringBuilder2.Append(transaction.Connection.SpecialEnd);
			for (int i = 1; i < values.Length; i++)
			{
				stringBuilder2.Append(',');
				stringBuilder2.Append(transaction.Connection.SpecialStart);
				stringBuilder2.Append(values[i]);
				stringBuilder2.Append(transaction.Connection.SpecialEnd);
			}
			stringBuilder2.Append(" FROM ");
			stringBuilder2.Append(transaction.Connection.SpecialStart);
			stringBuilder2.Append(tableAttribute.Name);
			stringBuilder2.Append(transaction.Connection.SpecialEnd);
			Command command2 = transaction.Connection.CreateCommand();
			command2.Transaction = transaction;
			if (conditions != null && conditions.Length > 0)
			{
				stringBuilder2.Append(" WHERE ");
				conditions[0].ToString(command2, stringBuilder2);
				for (int i = 1; i < conditions.Length; i++)
				{
					stringBuilder2.Append(" AND ");
					conditions[i].ToString(command2, stringBuilder2);
				}
			}
			stringBuilder2.Append(") T WHERE RowNumber BETWEEN ");
			stringBuilder2.Append(pageInfo.From);
			stringBuilder2.Append(" AND ");
			stringBuilder2.Append(pageInfo.To);
			command2.CommandText = stringBuilder2.ToString();
			return this.ReaderToEntity<E>(command2.ExecuteReader());
		}
		public List<E> Select<E>(Transaction transaction, string strSQL) where E : EntityBase
		{
			Command command = transaction.Connection.CreateCommand();
			command.CommandText = strSQL;
			command.Transaction = transaction;
			return this.ReaderToEntity<E>(command.ExecuteReader());
		}
		public List<E> Select<E>(Transaction transaction, string strSQL, ParameterHandle addParameters) where E : EntityBase
		{
			Command command = transaction.Connection.CreateCommand();
			command.Transaction = transaction;
			command.CommandText = strSQL;
			if (addParameters != null)
			{
				addParameters(command);
			}
			return this.ReaderToEntity<E>(command.ExecuteReader());
		}
		public List<E> Select<E>(Transaction transaction, string strSQL, PageInfo pageInfo, ParameterHandle addParameters) where E : EntityBase
		{
			string text = string.Format("SELECT COUNT(*) Cnt FROM ({0}) T", strSQL);
			Command command = transaction.Connection.CreateCommand();
			command.Transaction = transaction;
			pageInfo.Total = (int)command.ExecuteScalar();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY ");
			if (!string.IsNullOrEmpty(pageInfo.TableName))
			{
				stringBuilder.Append(transaction.Connection.SpecialStart);
				stringBuilder.Append(pageInfo.TableName);
				stringBuilder.Append(transaction.Connection.SpecialEnd);
				stringBuilder.Append('.');
			}
			stringBuilder.Append(transaction.Connection.SpecialStart);
			stringBuilder.Append(pageInfo.OrderBy);
			stringBuilder.Append(transaction.Connection.SpecialEnd);
			if (!pageInfo.Asc)
			{
				stringBuilder.Append(" DESC");
			}
			stringBuilder.Append(") RowNumber,");
			stringBuilder.Append(strSQL.Trim().Substring(6));
			stringBuilder.Append(") T WHERE RowNumber BETWEEN ");
			stringBuilder.Append(pageInfo.From);
			stringBuilder.Append(" AND ");
			stringBuilder.Append(pageInfo.To);
			Command command2 = transaction.Connection.CreateCommand();
			command2.CommandText = stringBuilder.ToString();
			command2.Transaction = transaction;
			if (addParameters != null)
			{
				addParameters(command2);
			}
			return this.ReaderToEntity<E>(command2.ExecuteReader());
		}
		public DataReader Select(Transaction transaction, string strSQL, ParameterHandle addParameters)
		{
			Command command = transaction.Connection.CreateCommand();
			command.Transaction = transaction;
			command.CommandText = strSQL;
			if (addParameters != null)
			{
				addParameters(command);
			}
			return command.ExecuteReader();
		}
		public DataReader Select(Transaction transaction, string strSQL, PageInfo pageInfo, ParameterHandle addParameters)
		{
			string text = string.Format("SELECT COUNT(*) Cnt FROM ({0}) T", strSQL);
			Command command = transaction.Connection.CreateCommand();
			command.Transaction = transaction;
			pageInfo.Total = (int)command.ExecuteScalar();
			strSQL = strSQL.Trim().Insert(6, " ROW_NUMBER() OVER(ORDER BY ID) RowNumber,");
			Command command2 = transaction.Connection.CreateCommand();
			command2.Transaction = transaction;
			command2.CommandText = string.Concat(new object[]
			{
				"SELECT * FROM (",
				strSQL,
				") T WHERE RowNumber BETWEEN ",
				pageInfo.From,
				" AND ",
				pageInfo.To
			});
			if (addParameters != null)
			{
				addParameters(command2);
			}
			return command2.ExecuteReader();
		}
		private List<E> ReaderToEntity<E>(DataReader reader) where E : EntityBase
		{
			List<E> list = new List<E>();
			Type typeFromHandle = typeof(E);
			PropertyInfo[] array = new PropertyInfo[reader.FieldCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = typeFromHandle.GetProperty(reader.GetName(i));
			}
			while (reader.Read())
			{
				E e = Activator.CreateInstance<E>();
				for (int i = 0; i < array.Length; i++)
				{
					object obj = reader[i];
					PropertyInfo propertyInfo = array[i];
					if (propertyInfo != null)
					{
						if (!DBNull.Value.Equals(obj))
						{
							propertyInfo.SetValue(e, obj);
						}
					}
				}
				list.Add(e);
			}
			reader.Close();
			return list;
		}
		public int ExecuteNonQuery(Connection connection, string strSQL)
		{
			return this.ExecuteNonQuery(connection, strSQL, null);
		}
		public int ExecuteNonQuery(Connection connection, string strSQL, ParameterHandle addParameters)
		{
			Command command = connection.CreateCommand();
			command.CommandText = strSQL;
			if (addParameters != null)
			{
				addParameters(command);
			}
			return command.ExecuteNonQuery();
		}
		public object ExecuteScalar(Connection connection, string strSQL)
		{
			return this.ExecuteScalar(connection, strSQL, null);
		}
		public object ExecuteScalar(Connection connection, string strSQL, ParameterHandle addParameters)
		{
			Command command = connection.CreateCommand();
			command.CommandText = strSQL;
			if (addParameters != null)
			{
				addParameters(command);
			}
			return command.ExecuteScalar();
		}
		public void ExecuteReader(Connection connection, string strSQL, DataReaderHandle readDatas)
		{
			this.ExecuteReader(connection, strSQL, null, readDatas);
		}
		public void ExecuteReader(Connection connection, string strSQL, ParameterHandle addParameters, DataReaderHandle readDatas)
		{
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
		public int ExecuteNonQuery(Transaction transaction, string strSQL)
		{
			return this.ExecuteNonQuery(transaction, strSQL, null);
		}
		public int ExecuteNonQuery(Transaction transaction, string strSQL, ParameterHandle addParameters)
		{
			Command command = transaction.Connection.CreateCommand();
			command.CommandText = strSQL;
			command.Transaction = transaction;
			if (addParameters != null)
			{
				addParameters(command);
			}
			return command.ExecuteNonQuery();
		}
		public object ExecuteScalar(Transaction transaction, string strSQL)
		{
			return this.ExecuteScalar(transaction, strSQL, null);
		}
		public object ExecuteScalar(Transaction transaction, string strSQL, ParameterHandle addParameters)
		{
			Command command = transaction.Connection.CreateCommand();
			command.CommandText = strSQL;
			command.Transaction = transaction;
			if (addParameters != null)
			{
				addParameters(command);
			}
			return command.ExecuteScalar();
		}
		public void ExecuteReader(Transaction transaction, string strSQL, DataReaderHandle readDatas)
		{
			this.ExecuteReader(transaction, strSQL, null, readDatas);
		}
		public void ExecuteReader(Transaction transaction, string strSQL, ParameterHandle addParameters, DataReaderHandle readDatas)
		{
			Command command = transaction.Connection.CreateCommand();
			command.CommandText = strSQL;
			command.Transaction = transaction;
			if (addParameters != null)
			{
				addParameters(command);
			}
			DataReader dataReader = command.ExecuteReader();
			readDatas(dataReader);
			dataReader.Close();
		}
		public Command CreateCommand(Connection connection, string strSQL)
		{
			return this.CreateCommand(connection, strSQL, null);
		}
		public Command CreateCommand(Connection connection, string strSQL, ParameterHandle addParameters)
		{
			Command command = connection.CreateCommand();
			command.CommandText = strSQL;
			if (addParameters != null)
			{
				addParameters(command);
			}
			return command;
		}
		public Command CreateCommand(Transaction transaction, string strSQL)
		{
			return this.CreateCommand(transaction, strSQL, null);
		}
		public Command CreateCommand(Transaction transaction, string strSQL, ParameterHandle addParameters)
		{
			Command command = transaction.Connection.CreateCommand();
			command.CommandText = strSQL;
			command.Transaction = transaction;
			if (addParameters != null)
			{
				addParameters(command);
			}
			return command;
		}
	}
}
