using System;
using System.Collections.Generic;
namespace TooFuns.Framework.Access
{
	public class OrderBy
	{
		public static readonly OrderBy None;
		private bool isNull;
		private List<OrderByItem> list;
		public bool IsNull
		{
			get
			{
				return this.isNull;
			}
		}
		static OrderBy()
		{
			OrderBy.None = new OrderBy();
		}
		public OrderBy(string columnName)
		{
			this.list = new List<OrderByItem>();
			this.list.Add(new OrderByItem(this, columnName));
			this.isNull = false;
		}
		public OrderBy(string columnName, bool desc)
		{
			this.list = new List<OrderByItem>();
			OrderByItem orderByItem = new OrderByItem(this, columnName);
			if (desc)
			{
				orderByItem.Desc();
			}
			this.list.Add(orderByItem);
			this.isNull = false;
		}
		public OrderBy(string columnName, string tableName)
		{
			this.list = new List<OrderByItem>();
			this.list.Add(new OrderByItem(this, columnName, tableName));
			this.isNull = false;
		}
		public OrderBy(string columnName, string tableName, bool desc)
		{
			this.list = new List<OrderByItem>();
			OrderByItem orderByItem = new OrderByItem(this, columnName, tableName);
			if (desc)
			{
				orderByItem.Desc();
			}
			this.list.Add(orderByItem);
			this.isNull = false;
		}
		private OrderBy()
		{
			this.isNull = true;
		}
		public OrderByItem Order(string columnName)
		{
			OrderByItem orderByItem = new OrderByItem(this, columnName);
			this.list.Add(orderByItem);
			return orderByItem;
		}
		public OrderBy Order(string columnName, bool asc)
		{
			OrderByItem item = new OrderByItem(this, columnName);
			this.list.Add(item);
			return this;
		}
		public override string ToString()
		{
			string result;
			if (this.isNull)
			{
				result = "";
			}
			else
			{
				result = string.Join<OrderByItem>(",", this.list);
			}
			return result;
		}
	}
}
