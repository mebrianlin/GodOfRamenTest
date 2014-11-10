using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

        public static GameObject[] FindObjectsWithTagInChildren(this GameObject obj, string tag)
		{
            return obj.transform.Cast<Transform>()
                .Select(x => x.gameObject)
                .Where(x => x.tag == tag)
                .ToArray();
		}

		public static GameObject FindObjectWithTagInChildren(this GameObject obj, string tag)
		{
			GameObject[] array = obj.transform.Cast<Transform>()
				.Select(x => x.gameObject)
				.Where(x => x.tag == tag)
				.ToArray();
			if (array.Length == 0)
				return null;
			else
				return array[0];

		}
	}   
}
