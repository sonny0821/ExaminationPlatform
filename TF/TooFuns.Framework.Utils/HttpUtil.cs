using System;
using System.IO;
using System.Net;
using System.Text;
namespace TooFuns.Framework.Utils
{
	public class HttpUtil
	{
		public static string Get(string url)
		{
			return HttpUtil.request(url, string.Empty, "GET");
		}
		public static string Post(string url, string data)
		{
			return HttpUtil.request(url, data, "POST");
		}
		private static string request(string url, string data, string method)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.ContentType = "application/json; encoding=utf-8";
			httpWebRequest.KeepAlive = true;
			httpWebRequest.Method = method;
			if (method == "POST" && !string.IsNullOrEmpty(data))
			{
				Encoding uTF = Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(data);
				httpWebRequest.ContentLength = (long)bytes.Length;
				Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
			}
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			string result = streamReader.ReadToEnd();
			httpWebResponse.Close();
			return result;
		}
	}
}
