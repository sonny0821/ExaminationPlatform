using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Xml;
namespace TooFuns.Framework.Data
{
	public class DataConfigHandle : IConfigurationSectionHandler
	{
		public object Create(object parent, object configContext, XmlNode section)
		{
			List<Database> list = new List<Database>();
			Assembly assembly = Assembly.GetAssembly(typeof(Database));
			for (int i = 0; i < section.ChildNodes.Count; i++)
			{
				XmlNode xmlNode = section.ChildNodes[i];
				Database database = (Database)assembly.CreateInstance(xmlNode.Attributes["type"].Value);
				database.ConnectionString = xmlNode.Attributes["connectionString"].Value;
                database.connectString = xmlNode.Attributes["connectionString"].Value;
                database.dbTypeName = xmlNode.Attributes["type"].Value;
				database.Name = xmlNode.Attributes["name"].Value;
				XmlAttribute xmlAttribute = xmlNode.Attributes["default"];
				if (xmlAttribute != null && xmlAttribute.Value == "true")
				{
					database.IsDefault = true;
				}
				list.Add(database);
			}
			return list;
		}
	}
}
