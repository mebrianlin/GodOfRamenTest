using System.Collections.Generic;

namespace ExtensionMethods
{
	public static class Extensions
	{

		public static bool Empty<T>(this Queue<T> queue)
		{
			return queue.Count == 0;
		}

		public static bool Empty<K, V>(this Dictionary<K, V> container)
		{
			return container.Count == 0;
		}
	}   
}
