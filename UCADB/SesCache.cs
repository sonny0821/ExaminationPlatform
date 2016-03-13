using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace UCADB
{
    public class SesCache
    {
        ConnectionInstance cIsts;

        public SesCache(ConnectionInstance ists)
        {
            cIsts = ists;
        }

        //public object this[string field]
        //{
        //    get
        //    {
        //        if (cIsts.UserSession.ContainsKey(HttpContext.Current.Session.SessionID))
        //        {
        //            Dictionary<string, object> uSes = cIsts.UserSession[HttpContext.Current.Session.SessionID];
        //            if (uSes.ContainsKey(field))
        //            {
        //                return uSes[field];
        //            }
        //        }
        //        return null;
        //    }
        //    set
        //    {
        //        if (cIsts.UserSession.ContainsKey(HttpContext.Current.Session.SessionID))
        //        {
        //            Dictionary<string, object> uSes = cIsts.UserSession[HttpContext.Current.Session.SessionID];
        //            if (uSes.ContainsKey(field))
        //            {
        //                uSes[field] = value;
        //            }
        //            else
        //            {
        //                uSes.Add(field, value);
        //            }
        //        }
        //    }
        //}


        //public object SessionValueGet(string field, string SessionID)
        //{
        //    if (cIsts.UserSession.ContainsKey(SessionID))
        //    {
        //        Dictionary<string, object> uSes = cIsts.UserSession[SessionID];
        //        if (uSes.ContainsKey(field))
        //        {
        //            return uSes[field];
        //        }
        //    }
        //    return null;

        //}
            
               
    


        //public void SessionValueSet(string field, string SessionID,object value)
        //{

        //    if (cIsts.UserSession.ContainsKey(SessionID))
        //    {
        //        Dictionary<string, object> uSes = cIsts.UserSession[SessionID];
        //        if (uSes.ContainsKey(field))
        //        {
        //            uSes[field] = value;
        //        }
        //        else
        //        {
        //            uSes.Add(field, value);
        //        }
        //    }
        //}
         
                
    

 
        

    }
}
