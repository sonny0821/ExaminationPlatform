using System;
using System.Collections.Generic;
namespace TooFuns.Framework.Utils
{
	public class RandomUtil
	{
		public static void Random<T>(List<T> items)
		{
			RandomUtil.Random<T>(items, 1);
		}
		public static void Random<T>(List<T> items, int deep)
		{
			Random random = new Random();
			for (int i = 0; i < deep; i++)
			{
				for (int j = items.Count - 1; j > 1; j--)
				{
					int index = random.Next(j);
					T value = items[j];
					items[j] = items[index];
					items[index] = value;
				}
				for (int j = 0; j < items.Count - 1; j++)
				{
					int index = random.Next(j + 1, items.Count);
					T value = items[j];
					items[j] = items[index];
					items[index] = value;
				}
			}
		}
	}
}
