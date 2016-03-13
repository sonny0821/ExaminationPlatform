using System;
namespace TooFuns.Framework
{
	public class PageInfo
	{
		private int total;
		private int from;
		private int to;
		private int count;
		private int current;
		private int totalPage;
		private string orderBy;
		private bool asc;
		private string tableName;
		internal int From
		{
			get
			{
				return this.from;
			}
		}
		internal int To
		{
			get
			{
				return this.to;
			}
		}
		public int TotalPage
		{
			get
			{
				return this.totalPage;
			}
		}
		public int Count
		{
			get
			{
				return this.count;
			}
		}
		public int Current
		{
			get
			{
				return this.current;
			}
			set
			{
				this.current = value;
			}
		}
		public int Total
		{
			get
			{
				return this.total;
			}
			internal set
			{
				this.total = value;
				this.totalPage = (this.total - 1) / this.count + 1;
				if (this.current > this.totalPage || this.current == 0)
				{
					this.current = this.totalPage;
					this.from = (this.current - 1) * this.count + 1;
					this.to = this.total;
				}
				else
				{
					this.from = (this.current - 1) * this.count + 1;
					this.to = this.from + this.count - 1;
				}
			}
		}
		internal string OrderBy
		{
			get
			{
				return this.orderBy;
			}
		}
		internal string TableName
		{
			get
			{
				return this.tableName;
			}
		}
		internal bool Asc
		{
			get
			{
				return this.asc;
			}
		}
		public PageInfo(int page, int count, string orderBy, string tableName, bool asc)
		{
			this.count = count;
			this.current = page;
			this.orderBy = orderBy;
			this.asc = asc;
			this.tableName = tableName;
		}
		public PageInfo(int page, int count, string orderBy, string tableName) : this(page, count, orderBy, tableName, true)
		{
		}
		public PageInfo(int page, int count, string orderBy, bool asc)
		{
			this.count = count;
			this.current = page;
			this.orderBy = orderBy;
			this.asc = asc;
		}
		public PageInfo(int page, int count, string orderBy) : this(page, count, orderBy, true)
		{
		}
	}
}
