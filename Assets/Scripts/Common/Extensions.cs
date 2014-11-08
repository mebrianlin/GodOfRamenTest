using System.Collections.Generic;

namespace ExtensionMethods
{
	public static class Extensions
	{
		public static bool Empty<T>(this Queue<T> queue)
		{
			return queue.Count == 0;
		}
	}   
}
