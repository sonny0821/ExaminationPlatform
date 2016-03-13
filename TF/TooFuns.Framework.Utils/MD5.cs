using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace TooFuns.Framework.Utils
{
	public class MD5
	{
		public static byte[] ToBytes(string str)
		{
			return System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(str));
		}
		public static byte[] ToBytes(byte[] bytes)
		{
			return System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
		}
		public static byte[] ToBytes(Stream inputStream)
		{
			return System.Security.Cryptography.MD5.Create().ComputeHash(inputStream);
		}
		public static string ToString(string str)
		{
			return Encoding.UTF8.GetString(System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(str)));
		}
		public static string ToString(byte[] bytes)
		{
			return Encoding.UTF8.GetString(System.Security.Cryptography.MD5.Create().ComputeHash(bytes));
		}
		public static string ToString(Stream inputStream)
		{
			return Encoding.UTF8.GetString(System.Security.Cryptography.MD5.Create().ComputeHash(inputStream));
		}
	}
}
