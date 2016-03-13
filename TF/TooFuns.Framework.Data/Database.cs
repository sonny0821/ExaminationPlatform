using System;
using System.Collections.Generic;
using System.Configuration;
namespace TooFuns.Framework.Data
{
	public abstract class Database
	{
        public string connectString = "";
        public string dbTypeName = "";

		private bool isDefault;
		protected string connectionString;
		private string name;
		private static List<Database> databaseList;
		private static Dictionary<string, Database> databases;
		public bool IsDefault
		{
			get
			{
				return this.isDefault;
			}
			internal set
			{
				this.isDefault = value;
			}
		}
		internal string ConnectionString
		{
			set
			{
				this.connectionString = value;
			}
		}
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
		public static List<Database> DatabaseList
		{
			get
			{
				if (Database.databaseList == null)
				{
					Database.initDb();
				}
				return Database.databaseList;
			}
		}
		public static Dictionary<string, Database> Databases
		{
			get
			{
				if (Database.databaseList == null)
				{
					Database.initDb();
				}
				return Database.databases;
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
		public Connection CreateConnection()
		{
			Connection connection = this.GetConnection();
			connection.Database = this;
			return connection;
		}
		protected abstract Connection GetConnection();
		private static void initDb()
		{
			Database.databases = new Dictionary<string, Database>();
			Database.databaseList = (List<Database>)ConfigurationManager.GetSection("csyweb/database");
			for (int i = 0; i < Database.databaseList.Count; i++)
			{
				Database database = Database.databaseList[i];
				Database.databases.Add(database.name, database);
			}
		}
	}
}
