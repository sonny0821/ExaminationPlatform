using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UCADB
{
    public class DBConnHandle : IConfigurationSectionHandler
    {



        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            List<ConnectionConfig> list = new List<ConnectionConfig>();
         
            for (int i = 0; i < section.ChildNodes.Count; i++)
            {
                ConnectionConfig cc = new ConnectionConfig();

                XmlNode xmlNode = section.ChildNodes[i];
                cc.dbType = xmlNode.Attributes["type"].Value;
                cc.connectionString = xmlNode.Attributes["connectionString"].Value;
               
                cc.connName = xmlNode.Attributes["name"].Value;

                list.Add(cc);
            }
            return list;
        }
    }
}
