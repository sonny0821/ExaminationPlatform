using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
namespace TooFuns.Framework.Data
{
	public class DataReader
	{
		private DbDataReader reader;
		public int Depth
		{
			get
			{
				return this.reader.Depth;
			}
		}
		public int FieldCount
		{
			get
			{
				return this.reader.FieldCount;
			}
		}
		public bool HasRows
		{
			get
			{
				return this.reader.HasRows;
			}
		}
		public bool IsClosed
		{
			get
			{
				return this.reader.IsClosed;
			}
		}
		public int RecordsAffected
		{
			get
			{
				return this.reader.RecordsAffected;
			}
		}
		public virtual int VisibleFieldCount
		{
			get
			{
				return this.reader.VisibleFieldCount;
			}
		}
		public object this[int ordinal]
		{
			get
			{
				return this.reader[ordinal];
			}
		}
		public object this[string name]
		{
			get
			{
				return this.reader[name];
			}
		}
		internal DataReader(DbDataReader reader)
		{
			this.reader = reader;
		}
		public void Close()
		{
			this.reader.Close();
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Dispose()
		{
			this.reader.Dispose();
			this.reader = null;
		}
		public bool GetBoolean(int ordinal)
		{
			return this.reader.GetBoolean(ordinal);
		}
		public byte GetByte(int ordinal)
		{
			return this.reader.GetByte(ordinal);
		}
		public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
		{
			return this.reader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
		}
		public char GetChar(int ordinal)
		{
			return this.reader.GetChar(ordinal);
		}
		public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
		{
			return this.reader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		public DbDataReader GetData(int ordinal)
		{
			return this.reader.GetData(ordinal);
		}
		public string GetDataTypeName(int ordinal)
		{
			return this.reader.GetDataTypeName(ordinal);
		}
		public DateTime GetDateTime(int ordinal)
		{
			return this.reader.GetDateTime(ordinal);
		}
		public decimal GetDecimal(int ordinal)
		{
			return this.reader.GetDecimal(ordinal);
		}
		public double GetDouble(int ordinal)
		{
			return this.reader.GetDouble(ordinal);
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		public IEnumerator GetEnumerator()
		{
			return this.reader.GetEnumerator();
		}
		public Type GetFieldType(int ordinal)
		{
			return this.reader.GetFieldType(ordinal);
		}
		public virtual T GetFieldValue<T>(int ordinal)
		{
			return this.reader.GetFieldValue<T>(ordinal);
		}
		public Task<T> GetFieldValueAsync<T>(int ordinal)
		{
			return this.reader.GetFieldValueAsync<T>(ordinal);
		}
		public virtual Task<T> GetFieldValueAsync<T>(int ordinal, CancellationToken cancellationToken)
		{
			return this.reader.GetFieldValueAsync<T>(ordinal, cancellationToken);
		}
		public float GetFloat(int ordinal)
		{
			return this.reader.GetFloat(ordinal);
		}
		public Guid GetGuid(int ordinal)
		{
			return this.reader.GetGuid(ordinal);
		}
		public short GetInt16(int ordinal)
		{
			return this.reader.GetInt16(ordinal);
		}
		public int GetInt32(int ordinal)
		{
			return this.reader.GetInt32(ordinal);
		}
		public long GetInt64(int ordinal)
		{
			return this.reader.GetInt64(ordinal);
		}
		public string GetName(int ordinal)
		{
			return this.reader.GetName(ordinal);
		}
		public int GetOrdinal(string name)
		{
			return this.reader.GetOrdinal(name);
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual Type GetProviderSpecificFieldType(int ordinal)
		{
			return this.reader.GetProviderSpecificFieldType(ordinal);
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual object GetProviderSpecificValue(int ordinal)
		{
			return this.reader.GetProviderSpecificValue(ordinal);
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual int GetProviderSpecificValues(object[] values)
		{
			return this.reader.GetProviderSpecificValues(values);
		}
		public virtual Stream GetStream(int ordinal)
		{
			return this.reader.GetStream(ordinal);
		}
		public string GetString(int ordinal)
		{
			return this.reader.GetString(ordinal);
		}
		public virtual TextReader GetTextReader(int ordinal)
		{
			return this.reader.GetTextReader(ordinal);
		}
		public object GetValue(int ordinal)
		{
			return this.reader.GetValue(ordinal);
		}
		public int GetValues(object[] values)
		{
			return this.reader.GetValues(values);
		}
		public bool IsDBNull(int ordinal)
		{
			return this.reader.IsDBNull(ordinal);
		}
		public Task<bool> IsDBNullAsync(int ordinal)
		{
			return this.reader.IsDBNullAsync(ordinal);
		}
		public virtual Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken)
		{
			return this.reader.IsDBNullAsync(ordinal, cancellationToken);
		}
		public bool NextResult()
		{
			return this.reader.NextResult();
		}
		public Task<bool> NextResultAsync()
		{
			return this.reader.NextResultAsync();
		}
		public virtual Task<bool> NextResultAsync(CancellationToken cancellationToken)
		{
			return this.reader.NextResultAsync(cancellationToken);
		}
		public bool Read()
		{
			return this.reader.Read();
		}
		public Task<bool> ReadAsync()
		{
			return this.reader.ReadAsync();
		}
		public virtual Task<bool> ReadAsync(CancellationToken cancellationToken)
		{
			return this.reader.ReadAsync(cancellationToken);
		}
	}
}
