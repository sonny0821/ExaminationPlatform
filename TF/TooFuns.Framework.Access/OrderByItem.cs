using System;
namespace TooFuns.Framework.Access
{
	public class OrderByItem
	{
		private OrderBy orderBy;
		private string columnName;
		private string tableName;
		private bool desc;
		public string ColumnName
		{
			get
			{
				return this.columnName;
			}
			set
			{
				this.columnName = value;
			}
		}
		public OrderByItem(OrderBy orderBy, string columnName)
		{
			this.columnName = columnName;
			this.orderBy = orderBy;
		}
		public OrderByItem(OrderBy orderBy, string columnName, string tableName)
		{
			this.columnName = columnName;
			this.orderBy = orderBy;
			this.tableName = tableName;
		}
		public OrderBy Desc()
		{
			this.desc = true;
			return this.orderBy;
		}
		public OrderBy Asc()
		{
			this.desc = false;
			return this.orderBy;
		}
		public override string ToString()
		{
			string result;
			if (this.desc)
			{
				if (string.IsNullOrEmpty(this.tableName))
				{
					result = this.columnName + " DESC";
				}
				else
				{
					result = this.tableName + "." + this.columnName + " DESC";
				}
			}
			else
			{
				if (string.IsNullOrEmpty(this.tableName))
				{
					result = this.columnName;
				}
				else
				{
					result = this.tableName + "." + this.columnName;
				}
			}
			return result;
		}
	}
}
